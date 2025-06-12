using ZXing;
using ZXing.Common;
using ZXing.CoreCompat.System.Drawing;

namespace AdminApi.helperFunctions
{
    public static class Code39Barcode
    {
		/*
			Takes in a fileName as string, barcodeString as string which is the data encoded in the barcode, a folder directory to save the image at as a string, barcode image width as int with standard value of 720, barcode image height as int with standard value of 250.
			Returns a file path to the generated barcode as a string.
		*/
        public static string GenerateBarcode(string fileName, string barcodeString, string saveDirectory, int width = 720, int height = 250)
        {
            Console.WriteLine("GenerateBarcode");
			if(!Directory.Exists(saveDirectory))
				throw new ArgumentException("Barcode directory not found");

            string filePath = Path.Combine(saveDirectory, fileName);
            Console.WriteLine("Before barcode writer");
			BarcodeWriter writer = new BarcodeWriter
			{
				Format = BarcodeFormat.CODE_39,
				Options = new EncodingOptions
				{
					Width = width,
					Height = height,
				}
			};
            Console.WriteLine("After barcode writer");

			var barcodeImage = writer.Write(barcodeString);
			using (var stream = new MemoryStream())
				barcodeImage.Save(filePath, System.Drawing.Imaging.ImageFormat.Png);
			
            Console.WriteLine("after stream");
			return filePath;
        }
    }
}
