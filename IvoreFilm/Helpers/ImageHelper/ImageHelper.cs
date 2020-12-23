using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mime;

namespace IvoreFilm.Helpers.ImageHelper
{
    public class ImageHelper : IImageHelper
    {
        public string SaveImage(string imageUrl,string type)
        {
            string filename = (type == "Movie") ? "Thumbnails/movies/" : "Thumbnails/series/";
            WebClient client = new WebClient();
            Stream stream = client.OpenRead(imageUrl);
            Bitmap bitmap;  bitmap = new Bitmap(stream);

            if (bitmap != null)
            {
                string[] args = imageUrl.Split("/");
                filename = filename + args[^1] ;
                bitmap.Save(filename, ImageFormat.Jpeg);
                stream.Flush();
                stream.Close();
                client.Dispose();
                return filename;
            }

            
            return null;
        }
    }
}