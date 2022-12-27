using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCoreApiRest.Utils;
using ZXing;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using Microsoft.SqlServer.Server;
using Microsoft.Extensions.Options;
using ZXing.Datamatrix.Encoder;
using ZXing.Rendering;
using System.Reflection.Emit;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;

namespace WSTraceIT.Interfaces
{
	/// <summary>
	/// Clase general para la generación de QR y DATAMATRIX
	/// Desarrollador: Iván Gutiérrez
	/// </summary>
	public class QRLabel
    {
        #region Properties
        public string code;
		public int Height;
		public int Width;
		public int Margin;
		public bool Image;

		private LoggerD4 log = new LoggerD4("QRLabel");
        #endregion

        #region Constructor
        public QRLabel()
        {
            this.code = String.Empty;
            this.Height = 50;
            this.Width = 50;
            this.Margin = 1;
            this.Image = false;
		}
		#endregion

		#region Public methods
		/// <summary>
		/// Método para generar un DataMatrix simple
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param></param>
		/// <returns>Convert.ToBase64String</returns>
		public string SimpleDataMatrix()
        {
            log.trace("GenerateDataMatrix");
			log.trace("GenerateDataMatrix: " + code);
			try
			{
				var writer = new BarcodeWriterPixelData
				{
					Format = BarcodeFormat.DATA_MATRIX,
					Options = new ZXing.Datamatrix.DatamatrixEncodingOptions
                    {
						GS1Format = true,
						SymbolShape = SymbolShapeHint.FORCE_SQUARE,
						Height = Height,
						Width = Width,
						PureBarcode = false,
						Margin = Margin
					}
				};

				// string to encode
				var barcode = writer.Write(code);

				return this.ProcessEncode(barcode);
			}
			catch (Exception ex)
			{
				log.error("There was an error to generate datamatrix code: " + ex.Message);
				throw new Exception("Error al generar datamatrix");
			}
        }

		/// <summary>
		/// Método para generar un código QR simple
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param></param>
		/// <returns>Convert.ToBase64String</returns>
		public string SimpleQRCode()
		{
			log.trace("GenerateDataMatrix");
			log.trace("GenerateDataMatrix: " + code);
			try
			{
				var writer = new BarcodeWriterPixelData
				{
					Format = BarcodeFormat.QR_CODE,
					Options = new ZXing.QrCode.QrCodeEncodingOptions
					{
						DisableECI = true,
						CharacterSet = "UTF-8",
						Height = Height,
						Width = Width,
						ErrorCorrection = ZXing.QrCode.Internal.ErrorCorrectionLevel.Q
					}
				};

				// string to encode
				var barcode = writer.Write(code);

				return this.ProcessEncode(barcode);

			}
			catch (Exception ex)
			{
				log.error("There was an error to generate datamatrix code: " + ex.Message);
				throw new Exception("Error al generar datamatrix");
			}
		}

		/// <summary>
		/// Método para procesar un PixelData a una imagen en base64
		/// Desarrollador: Iván Gutiérrez
		/// </summary>
		/// <param></param>
		/// <returns>Convert.ToBase64String</returns>
		private string ProcessEncode(PixelData barcode)
        {
			using (var bitmap = new Bitmap(barcode.Width, barcode.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb))
			using (var ms = new MemoryStream())
			{
				// lock the data area for fast access
				var bitmapData = bitmap.LockBits(new Rectangle(0, 0, barcode.Width, barcode.Height),
				   System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
				try
				{
					// we assume that the row stride of the bitmap is aligned to 4 byte multiplied by the width of the image
					System.Runtime.InteropServices.Marshal.Copy(barcode.Pixels, 0, bitmapData.Scan0,
					   barcode.Pixels.Length);
				}
				finally
				{
					bitmap.UnlockBits(bitmapData);
				}
				// save to stream as PNG
				bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
				return Convert.ToBase64String(ms.ToArray());
			}
		}
		#endregion
	}
}
