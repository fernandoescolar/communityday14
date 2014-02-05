using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Mvc;
using CommunityDayProject.Models;

namespace CommunityDayProject.Controllers
{
    public class V4Controller : Controller
    {
        //
        // GET: /V4/
        public ActionResult Index(string id = ImageStore.DefaultImageUrl)
        {
            var grayScaleImage = HttpRuntime.Cache.Get(id) as Image;
            if (grayScaleImage == null)
            {
                grayScaleImage = ConvertImageToGrayScale(ImageStore.GetImageFromUrl(id));
                HttpRuntime.Cache.Insert(id, grayScaleImage); // .Insert() last one wins, .Add() first wins
            }

            var memoryStream = new MemoryStream();
            grayScaleImage.Save(memoryStream, ImageFormat.Jpeg);
            memoryStream.Position = 0; // reset the memory string position
            return new FileStreamResult(memoryStream, "image/jpeg");
        }

        private static Image ConvertImageToGrayScale(Image source)
        {
            // create a blank bitmap the same size as original
            var grayScaleBitmap = new Bitmap(source.Width, source.Height);

            // create the grayscale ColorMatrix
            var colorMatrix = new ColorMatrix(
               new float[][] 
                    {
                       new float[] {.3f, .3f, .3f, 0, 0},
                       new float[] {.59f, .59f, .59f, 0, 0},
                       new float[] {.11f, .11f, .11f, 0, 0},
                       new float[] {0, 0, 0, 1, 0},
                       new float[] {0, 0, 0, 0, 1}
                    });

            // create some image attributes
            var attributes = new ImageAttributes();

            // set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            // get a graphics object from the new image
            using (var g = Graphics.FromImage(grayScaleBitmap))
            {

                // draw the original image on the new image
                // using the grayscale color matrix
                g.DrawImage(source,
                   new Rectangle(
                  0, 0, source.Width, source.Height),
                   0, 0, source.Width, source.Height,
                   GraphicsUnit.Pixel, attributes);

            }

            return grayScaleBitmap;
        }
    }
}