using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Mvc;
using CommunityDayProject.Models;

namespace CommunityDayProject.Controllers
{
    public class V1Controller : Controller
    {
        //
        // GET: /V1/
        public ActionResult Index(string id = ImageStore.DefaultImageUrl)
        {
            var grayScaleImage = ConvertImageToGrayScale(ImageStore.GetImageFromUrl(id));
            var memoryStream = new MemoryStream();

            grayScaleImage.Save(memoryStream, ImageFormat.Jpeg);
            memoryStream.Position = 0; // reset the memory string position
            return new FileStreamResult(memoryStream, "image/jpeg");
        }

        private static Image ConvertImageToGrayScale(Image source)
        {

            var originalBitmap = new Bitmap(source);

            // make an empty bitmap the same size as original
            var grayScaleBitmap = new Bitmap(originalBitmap.Width, originalBitmap.Height);

            for (var i = 0; i < originalBitmap.Width; i++)
            {
                for (var j = 0; j < originalBitmap.Height; j++)
                {
                    // get the pixel from the original image
                    var originalColor = originalBitmap.GetPixel(i, j);

                    // create the grayscale version of the pixel
                    var grayScale = (int)((originalColor.R * .3) + (originalColor.G * .59) + (originalColor.B * .11));

                    // create the color object
                    var newColor = Color.FromArgb(grayScale, grayScale, grayScale);

                    // set the new image's pixel to the grayscale version
                    grayScaleBitmap.SetPixel(i, j, newColor);
                }
            }

            return grayScaleBitmap;
        }
    }
}