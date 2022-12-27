using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using NetCoreApiRest.Utils;
using WSTraceIT.Models.ModelsSQL;
using WSTraceIT.InterfacesSQL;

namespace WSTraceIT.Interfaces
{
    public class NotificacionesPush
    {
        public string ServerKey { get; set; }
        public string SenderID { get; set; }
        public string ReceiverId { get; set; }
        public string ImageURI { get; set; }
        public int DaysLeft { get; set; }
        public string FamiliaModelo { get; set; }
        public string TipoGarantia { get; set; }
        public string urlFoto { get; set; }
        public int idioma { get; set; }
		private LoggerD4 log = new LoggerD4("NotificacionesPush");

		public NotificacionesPush()
        {
            ServerKey = ConfigurationSite._cofiguration["Firebase:ServerKey"];
            SenderID = ConfigurationSite._cofiguration["Firebase:SenderID"];

            UserMobileSQL sql = new UserMobileSQL();
            List<NotificationData> datas = sql.consultaNotificacionesGarantia();
            foreach(NotificationData data in datas){
                TipoGarantia = data.TipoGarantia;
                FamiliaModelo = data.Modelo;
                ReceiverId = data.TokenPush;
                urlFoto = ConfigurationSite._cofiguration["Paths:urlImagesFamily"] + data.ImagenUrl;
                DaysLeft = data.dias;
                idioma = data.idioma;
                enviarNotificacion();
            }
        }

        public void enviarNotificacion()
        {
            log.trace("enviarNotificacion");
            WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
            tRequest.Method = "post";
            //serverKey - Key from Firebase cloud messaging server  
            tRequest.Headers.Add(string.Format("Authorization: key={0}", ServerKey));
            //Sender Id - From firebase project setting  
            tRequest.Headers.Add(string.Format("Sender: id={0}", SenderID));
            tRequest.ContentType = "application/json";
            string msg1 = idioma == 0 ?
                    ConfigurationSite._cofiguration["MessageNotification:MessageEsp1"] :
                    ConfigurationSite._cofiguration["MessageNotification:MessageEng1"];
            string msg2 = idioma == 0 ?
                    ConfigurationSite._cofiguration["MessageNotification:MessageEsp2"] :
                    ConfigurationSite._cofiguration["MessageNotification:MessageEng2"];
            string ttl = idioma == 0 ?
                    ConfigurationSite._cofiguration["MessageNotification:TitleEsp"] :
                    ConfigurationSite._cofiguration["MessageNotification:TitleEng"];
            var payload = new
            {
                to = ReceiverId,
                collapse_key = "type_a",
                notification = new
                {
                    body = msg1 + FamiliaModelo + msg2,
                    title = ttl
                },
                data = new
                {
                    model = FamiliaModelo,
                    url = ImageURI,
                    days = DaysLeft,
                    type = TipoGarantia
                },
            };

            string postbody = JsonConvert.SerializeObject(payload).ToString();
            Byte[] byteArray = Encoding.UTF8.GetBytes(postbody);
            tRequest.ContentLength = byteArray.Length;
            using (Stream dataStream = tRequest.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
                using (WebResponse tResponse = tRequest.GetResponse())
                {
                    using (Stream dataStreamResponse = tResponse.GetResponseStream())
                    {
                        if (dataStreamResponse != null) using (StreamReader tReader = new StreamReader(dataStreamResponse))
                            {
                                String sResponseFromServer = tReader.ReadToEnd();
                                //result.Response = sResponseFromServer;
                            }
                    }
                }
            }
        }
    }
}
