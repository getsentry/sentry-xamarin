using Sample.Xamarin.Core.Interfaces;
using Sentry;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sample.Xamarin.Core.ViewModels
{
    public class ImageSelectorPageViewModel : NavigationService
    {
        public bool TaskCompleted { get; set; }
        public int MaxThreads { get; set; }
        public int MinThreads { get; set; }
        private int _selectedMaxThreads;
        public int SelectedMaxThreads 
        {
            get => _selectedMaxThreads;
            set
            {
                _selectedMaxThreads = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ImageSource> Images { get; set; }

        private List<string> _paths;

        private readonly IImageResizer _imageResizer;

        public ImageSelectorPageViewModel()
        {
            _imageResizer = DependencyService.Get<IImageResizer>();

            MaxThreads = Environment.ProcessorCount;
            SelectedMaxThreads = 1;
            MinThreads = 1;
            _paths = new List<string>();
            _ = _paths;
        }

        public async Task LoadImagesFromPath(List<string> paths)
        {
            Images = new ObservableCollection<ImageSource>();
            var throttler = new SemaphoreSlim(initialCount: MaxThreads);
            var tasks = paths.Select(async item =>
            {
                try
                {
                    await throttler.WaitAsync();
                    var resizedPath = await _imageResizer.ResizeImage(item, 1250, item + "_R");
                    Images.Add(ImageSource.FromFile(resizedPath));
                }
                catch (Exception ex)
                {
                    SentrySdk.CaptureException(ex);
                }
                finally
                {
                    throttler.Release();
                }
            });
            await Task.WhenAll(tasks);
            TaskCompleted = true;
        }
    }
}
