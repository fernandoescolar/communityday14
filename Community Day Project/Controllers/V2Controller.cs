using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Mvc;
using CommunityDayProject.Models;

namespace CommunityDayProject.Controllers
{
    public class V2Controller : Controller
    {
        //
        // GET: /V2/
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


            var sourceData = originalBitmap.LockBits(new Rectangle(0, 0, originalBitmap.Width, originalBitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
            var targetData = grayScaleBitmap.LockBits(new Rectangle(0, 0, grayScaleBitmap.Width, grayScaleBitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);


            var a = 0;
            unsafe
            {
                var sourcePointer = (byte*)sourceData.Scan0;
                var targetPointer = (byte*)targetData.Scan0;
                for (var i = 0; i < sourceData.Height; i++)
                {
                    for (var j = 0; j < sourceData.Width; j++)
                    {
                        a = (sourcePointer[0] + sourcePointer[1] + sourcePointer[2]) / 3;
                        targetPointer[0] = (byte)a;
                        targetPointer[1] = (byte)a;
                        targetPointer[2] = (byte)a;
                        targetPointer[3] = sourcePointer[3];

                        // 4 bytes per pixel
                        sourcePointer += 4;
                        targetPointer += 4;
                    }

                    // 4 bytes per pixel
                    sourcePointer += sourceData.Stride - (sourceData.Width * 4);
                    targetPointer += sourceData.Stride - (sourceData.Width * 4);
                }
            }
            grayScaleBitmap.UnlockBits(targetData);
            originalBitmap.UnlockBits(sourceData);

            return grayScaleBitmap;
        }
    }
}