using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace Fishbot
{
	public class OCR
	{
		public static string RecognizeText(Bitmap image)
		{
			// Pełna ścieżka do folderu tessdata
			string tessDataPath = @"C:\Users\Jedrulcia\Desktop\Jedrzej\Programowanko\github\Fishbot\bin\Debug";

			// Sprawdzenie czy plik istnieje
			if (!System.IO.File.Exists(System.IO.Path.Combine(tessDataPath, "pol.traineddata")))
			{
				throw new Exception("Plik pol.traineddata nie został znaleziony w podanej ścieżce: " + tessDataPath);
			}

			using (var engine = new TesseractEngine(tessDataPath, "pol", EngineMode.Default))
			{
				using (var img = PixConverter.ToPix(image))
				{
					using (var page = engine.Process(img))
					{
						return page.GetText();
					}
				}
			}
		}
	}
}
