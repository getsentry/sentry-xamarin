using System.Threading.Tasks;

namespace Sample.Xamarin.Core.Interfaces
{
    public interface IImageResizer
    {
        Task<string> ResizeImage(string imagePath, int maxSize, string filename);
    }
}
