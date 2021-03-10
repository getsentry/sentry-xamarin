using Android.Graphics;
using Sample.Xamarin.Core.Interfaces;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Sample.Xamarin.Droid.Tools
{
    internal class ImageResizer : IImageResizer
    {
        public async Task<string> ResizeImage(string imagePath, int maxSize, string filename)
            => await Task.Run(() =>
            {
                var option = new BitmapFactory.Options()
                {
                    InSampleSize = 1
                };

                var originalImage = BitmapFactory.DecodeFile(imagePath, option);
                var scaledSize = GetMeasure(originalImage.Width, originalImage.Height, maxSize);
                var resizedImage = Bitmap.CreateScaledBitmap(originalImage, scaledSize.Item1, scaledSize.Item2, false);

                var path = System.IO.Path.Combine(FileSystem.CacheDirectory, filename);
                using (var f = System.IO.File.Create(path))
                {
                    resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 35, f);
                }
                if (resizedImage != originalImage)
                {
                    originalImage.Recycle();
                }
                resizedImage.Recycle();
                return path;
            });

        private (int, int) GetMeasure(int width, int height, int newMax)
        {
            double ratio;
            if (width > newMax)
            {
                ratio = (double)newMax / width;
                height = (int)(height * ratio);
                width = (int)(width * ratio);
            }
            if (height > newMax)
            {
                ratio = (double)newMax / height;
                width = (int)(width * ratio);
                height = (int)(height * ratio);
            }
            return (width, height);
        }
    }
}