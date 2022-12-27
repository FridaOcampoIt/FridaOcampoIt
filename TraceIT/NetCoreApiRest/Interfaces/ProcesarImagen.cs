using OfficeOpenXml;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using NetCoreApiRest.Utils;

namespace WSTraceIT.Interfaces
{
	public class ProcesarImagen
	{
		private LoggerD4 log = new LoggerD4("ProcesarImagen");
		/// <summary>
		/// Metodo para convertir un archivo base64 en imagen y guardarlo en el path especifico
		/// Desarrollador: David Martinez
		/// </summary>
		/// <param name="base64">Archivo codificado en base 64</param>
		/// <param name="nameImagen">Nombre de la imagen que recibira cuando se convierta en imagen</param>
		/// <param name="path">ubicacion de la carpeta donde se desea guardar la imagen</param>
		public void SaveImage(string base64, string nameImagen, string path)
		{
			log.trace("SaveImage " + path);
			log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(base64));
			try
			{
				if (File.Exists(path + nameImagen))
					File.Delete(path + nameImagen);

				base64 = base64
					.Replace("data:image/jpeg;base64,", "")
					.Replace("data:image/png;base64,", "")
					.Replace("data:image/jpg;base64,", "")
					.Replace("data:application/pdf;base64,", "")
					.Replace("data:application/octet-stream;base64,", "")
					.Replace("data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,", "");

				Byte[] bytes = Convert.FromBase64String(base64);
				File.WriteAllBytes(path + nameImagen, bytes);
			}
			catch (Exception ex)
			{
				log.error("There was an error 1 saving the image: " + ex.Message);
				throw new Exception("Error 1 al guardar la imagen");
			}
		}

        /// <summary>
		/// Metodo para convertir un archivo base64 en imagen disminuida y guardarlo en el path especifico
		/// Desarrollador: Oscar Ruesga
		/// </summary>
		/// <param name="base64">Archivo codificado en base 64</param>
		/// <param name="nameImagen">Nombre de la imagen que recibira cuando se convierta en imagen</param>
		/// <param name="path">ubicacion de la carpeta donde se desea guardar la imagen</param>
		public void SaveTmbn(string base64, string nameImagen, string path)
        {
            log.trace("SaveTmbn");
            log.trace(Newtonsoft.Json.JsonConvert.SerializeObject(base64));
            try
            {
                if (File.Exists(path + nameImagen))
                    File.Delete(path + nameImagen);

				log.trace("porcessing the base64 string");
                base64 = base64
                    .Replace("data:image/jpeg;base64,", "")
                    .Replace("data:image/png;base64,", "")
                    .Replace("data:image/jpg;base64,", "")
                    .Replace("data:application/pdf;base64,", "")
                    .Replace("data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,", "");
				log.trace("Converting base64");
                Byte[] bytes = Convert.FromBase64String(base64);
                using (MemoryStream ms = new MemoryStream(bytes))
                {
					log.trace("about to generate thumbnail");
                    Bitmap thumb = new Bitmap(150, 150);
					log.trace("Bitmap generated");
                    using (Image bmp = Image.FromStream(ms))
                    {
						log.trace("generating thumb from image");
                        using (Graphics g = Graphics.FromImage(thumb))
                        {
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
							log.trace("Interpolate");
                            g.CompositingQuality = CompositingQuality.HighQuality;
							log.trace("compositing");
                            g.SmoothingMode = SmoothingMode.HighQuality;
							log.trace("Smoothing");
                            g.DrawImage(bmp, 0, 0, 150, 150);
							log.trace("draw");
                        }
                    }
					log.trace("thumb saved - path: " + path + nameImagen);
					thumb.Save(path + nameImagen);
					log.trace("thumb saved");
                }
            }
            catch (Exception ex)
            {
				log.error("Exception at saving image: " + ex.Message);
				throw new Exception("Error 2 al guardar la imagen");
			}
        }

        /// <summary>
        /// Metodo para leer los datos de un excel y lo convierte en un DataTable
        /// Desarrollador: David Martinez
        /// </summary>
        /// <param name="oSheet">libro de excel</param>
        /// <returns></returns>
        public DataTable ConvertToDataTable(ExcelWorksheet oSheet)
		{
			log.trace("ConvertToDataTable");
			int totalRows = oSheet.Dimension.End.Row;
			int totalCols = oSheet.Dimension.End.Column;
			DataTable dt = new DataTable(oSheet.Name);
			DataRow dr = null;
			for (int i = 1; i <= totalRows; i++)
			{
				if (i > 1) dr = dt.Rows.Add();
				for (int j = 1; j <= totalCols; j++)
				{
					if (i == 1)
						dt.Columns.Add(oSheet.Cells[i, j].Value.ToString());
					else
						dr[j - 1] = (oSheet.Cells[i, j].Value != null) ? dr[j - 1] = oSheet.Cells[i, j].Value.ToString() : "";
				}
			}
			return dt;
		}
	}
}
