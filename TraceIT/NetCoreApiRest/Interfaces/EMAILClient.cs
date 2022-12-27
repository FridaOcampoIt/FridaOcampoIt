using NetCoreApiRest.Utils;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace WS.Interfaces
{
    public class EMAILClient
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool EnableSsl { get; set; }
        public string Usuario { get; set; }
        public string Contraseña { get; set; }
        public string Remitente { get; set; }
        public string NombreRemitente { get; set; }
		private LoggerD4 log = new LoggerD4("EMAILClient");

        public EMAILClient()
        {
            // Valores por defecto
            Host = ConfigurationSite._cofiguration["SMTP:Host"];
            Port = Convert.ToInt32(ConfigurationSite._cofiguration["SMTP:Puerto"]);
            EnableSsl = Convert.ToBoolean(ConfigurationSite._cofiguration["SMTP:EnableSsl"]);

            // Credenciales
            Usuario = ConfigurationSite._cofiguration["SMTP:User"];
            Contraseña = ConfigurationSite._cofiguration["SMTP:Password"];

            // Mensaje
            Remitente = ConfigurationSite._cofiguration["SMTP:User"];
            NombreRemitente = ConfigurationSite._cofiguration["SMTP:NombreRemitente"];
        }

		/// <summary>
		/// Envía un correo electrónico de manera masiva a los destinatarios indicados.
		/// </summary>
		/// <param name="Destinatarios">Arreglo con los destinatarios.</param>
		/// <param name="Mensaje">Cuerpo HTML del mensaje.</param>
		/// <param name="Asunto">Asunto del mensaje.</param>
		public void EnviarCorreo(String[] Destinatarios, String Mensaje, String Asunto = "", String CorreoReply = "")
		{
			
			//si esle mismo o no ?
			log.trace("executing EnviarCorreo()");
			if (Destinatarios.Count() > 0)
			{
				log.trace("There are "+ Destinatarios.Count() + " Destinatarios");
				using (MailMessage correo = new MailMessage())
				{
					log.trace("using MailMessage");
					correo.From = new MailAddress("admin@traceit.net", NombreRemitente);
					//correo.To.Add(new MailAddress(""));
					correo.Subject = Asunto;
					correo.Body = Mensaje;
					correo.IsBodyHtml = true;

					if (CorreoReply != "")
						correo.ReplyToList.Add(CorreoReply);

					//correo.To.Add(new MailAddress(Destinatarios[0]));

					for (int i = 0; i < Destinatarios.Length; i++)
					{
						correo.Bcc.Add(new MailAddress(Destinatarios[i]));
					}

					using (SmtpClient clienteSmtp = new SmtpClient())
					{
						log.trace("Configuring the SMTP client");
						// Configurar el cliente SMTP
						clienteSmtp.Port = Port;
						clienteSmtp.Host = Host;
						clienteSmtp.EnableSsl = EnableSsl;
						clienteSmtp.Timeout = 1000000;
						clienteSmtp.DeliveryMethod = SmtpDeliveryMethod.Network;
						clienteSmtp.UseDefaultCredentials = false;
						clienteSmtp.Credentials = new NetworkCredential(Usuario, Contraseña);
						//mail - co1nam040036.inbound.protection.outlook.com
						// Enviar correo
						log.trace("About to send the email");
						//log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(correo));
						try {
							clienteSmtp.Send(correo);
							log.trace("SMTP email send");
						} catch (SmtpException smtpEX) {
							log.error("SMTP Exception: " + smtpEX.Message);
							throw smtpEX;
						}
					}
				}
			}
		}

		/// <summary>
		/// Envía un correo electrónico de manera masiva a los destinatarios indicados.
		/// </summary>
		/// <param name="Destinatarios">Arreglo con los destinatarios.</param>
		/// <param name="Mensaje">Cuerpo HTML del mensaje.</param>
		/// <param name="PathTxt">Path del Txt que se adjuntara al correo.</param>
		/// <param name="Asunto">Asunto del mensaje.</param>
		public void EnviarCorreoImportacion(String[] Destinatarios, String Mensaje, String PathTxt, String Asunto = "")
		{
			if (Destinatarios.Count() > 0)
			{
				using (MailMessage correo = new MailMessage())
				{
					correo.From = new MailAddress(Remitente, NombreRemitente);
					correo.Subject = Asunto;
					correo.Body = Mensaje;
					correo.IsBodyHtml = true;

					correo.Attachments.Add(new Attachment(PathTxt));

					for (int i = 0; i < Destinatarios.Length; i++)
					{
						correo.Bcc.Add(new MailAddress(Destinatarios[i]));
					}

					using (SmtpClient clienteSmtp = new SmtpClient())
					{
						// Configurar el cliente SMTP
						clienteSmtp.Port = Port;
						clienteSmtp.Host = Host;
						clienteSmtp.EnableSsl = EnableSsl;
						clienteSmtp.Timeout = 10000;
						clienteSmtp.DeliveryMethod = SmtpDeliveryMethod.Network;
						clienteSmtp.UseDefaultCredentials = false;
						clienteSmtp.Credentials = new NetworkCredential(Usuario, Contraseña);

						// Enviar correo
						log.trace("About to send the email");
						log.trace(correo.Body);
						try
						{
							clienteSmtp.Send(correo);
							log.trace("SMTP email send");
						}
						catch (SmtpException smtpEX)
						{
							log.error("SMTP Exception: " + smtpEX.Message);
							throw smtpEX;
						}
					}
				}
			}
		}
	}
}