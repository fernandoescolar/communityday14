using System.Drawing;
using System.Net;

namespace CommunityDayProject.Models
{
    public class ImageStore
    {
        public const string DefaultImageUrl =  @"http://farm4.static.flickr.com/3212/2770969139_e006393dd1_o.jpg";
        public static Image GetImageFromUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
                url = DefaultImageUrl;

            using (var wc = new WebClient())
            {
                var wp = new WebProxy { UseDefaultCredentials = true };
                wc.Proxy = wp;
                return Image.FromStream(wc.OpenRead(url));
            }
        }

    }
}