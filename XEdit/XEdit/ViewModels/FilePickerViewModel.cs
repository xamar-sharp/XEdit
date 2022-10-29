using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Xamarin.Essentials;
using System.Windows.Input;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Linq;
using System.IO;
using System.Runtime.CompilerServices;
namespace XEdit.ViewModels
{
    public sealed class FileModel
    {
        public byte[] Content { get; set; }
        public int UniqueHash { get; set; }
        public string Extension { get; set; }
    }
    public static class StringExtensions
    {
        private static readonly IList<string> _validExtensions;
        static StringExtensions()
        {
            _validExtensions = new List<string>(4)
            {
                ".jpg",".png",".svg",".webp",".bmp"
            };
        }
        public static bool IsImage(this string fileName)
        {
            return _validExtensions.Contains(Path.GetExtension(fileName));
        }
    }
    public static class StreamExtensions
    {
        public static async Task<byte[]> GetData(this Stream stream)
        {
            byte[] buffer = new byte[stream.Length];
            await stream.ReadAsync(buffer, 0, buffer.Length);
            return buffer;
        }
    }
    public sealed class FilePickerViewModel : INotifyPropertyChanged
    {
        public FilePickerViewModel(ContentPage page,Image target)
        {
            _mainPage = page;
            VideoCommand = new Command(async () =>
            {
                var file = await MediaPicker.CaptureVideoAsync();
                FileModel model = new FileModel() { Extension = Path.GetExtension(file.FullPath), Content = await (await file.OpenReadAsync()).GetData(), UniqueHash = file.FullPath.GetHashCode() };
                string result;
                if (!await RestApi.SendFiles(new FileModel[] {model}))
                {
                    result = Resource.FailedSending;
                }
                else
                {
                    result = Resource.SuccessSending;
                }
                System.Threading.Thread.MemoryBarrier();
                Device.BeginInvokeOnMainThread(async () => await _mainPage.DisplayAlert(Resource.SendingStatus, result, Resource.Ok));
            });
            PhotoCommand = new Command(async () =>
            {
                var file = await MediaPicker.CapturePhotoAsync();
                FileModel model = new FileModel() { Extension = Path.GetExtension(file.FullPath), Content = await(await file.OpenReadAsync()).GetData(), UniqueHash = file.FullPath.GetHashCode() };
                string result;
                if (!await RestApi.SendFiles(new FileModel[] { model }))
                {
                    result = Resource.FailedSending;
                }
                else
                {
                    result = Resource.SuccessSending;
                }
                System.Threading.Thread.MemoryBarrier();
                Device.BeginInvokeOnMainThread(async () => await _mainPage.DisplayAlert(Resource.SendingStatus, result, Resource.Ok));
            });
            SendToApiCommand = new Command(async (obj) =>
            {
                var files = (await FilePicker.PickMultipleAsync()).ToArray();
                FileModel[] models = new FileModel[files.Length];
                for (int x = 0; x < models.Length; x++)
                {
                    models[x] = new FileModel() { Extension = Path.GetExtension(files[x].FullPath), Content = await (await files[x].OpenReadAsync()).GetData(), UniqueHash = files[x].FullPath.GetHashCode() };
                }
                string result;
                if (!await RestApi.SendFiles(models))
                {
                    result = Resource.FailedSending;
                }
                else
                {
                    result = Resource.SuccessSending;
                }
                System.Threading.Thread.MemoryBarrier();
                Device.BeginInvokeOnMainThread(async () => await _mainPage.DisplayAlert(Resource.SendingStatus, result, Resource.Ok));
            });
            VisualizeCommand = new Command(async (obj) =>
            {
                await Task.Run(async () =>
                {
                    FileResult[] els = (await FilePicker.PickMultipleAsync())?.Where(el => el.FileName.IsImage())?.ToArray();
                    var rowdef = new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) };
                    var def = new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) };
                    var floor = Math.Ceiling((double)els?.Length / 4);
                    if (els.Length > 0)
                    {
                        var child = ((_mainPage.Content as ScrollView).Content as StackLayout).Children;
                        if (child[child.Count - 1] is Grid)
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                child.Remove(child[child.Count - 1] as Grid);
                            });
                        }
                        Grid grid = new Grid()
                        {
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            ColumnDefinitions = { def, def, def, def }
                        };
                        for (int x = 0; x < floor; x++)
                        {
                            grid.RowDefinitions.Add(rowdef);
                        }
                        int row = 0;
                        int local = 0;
                        for (int x = 0; x < els.Length; x++, local++)
                        {
                            if (local > 3)
                            {
                                local = 0;
                                row++;
                            }
                            grid.Children.Add(new Image() { Aspect = Aspect.Fill, HeightRequest = 100, WidthRequest = 50, Source = ImageSource.FromFile(els[x].FullPath) }, local, row);
                        }
                        var diff = (grid.ColumnDefinitions.Count * grid.RowDefinitions.Count) - grid.Children.Count;
                        if (diff + grid.Children.Count > els.Length)
                        {
                            for (int x = 0; x < diff; x++)
                            {
                                grid.Children.Add(new Image() { Aspect = Aspect.Fill, HeightRequest = 100, WidthRequest = 50, Source = "filepicker.png" }, grid.ColumnDefinitions.Count - diff + x, grid.RowDefinitions.Count - 1);
                            }
                        }
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            child.Add(grid);
                        });
                    }
                });
            });
            SetImageCommand = new Command((obj) =>
            {
                target.Source = ImageSource.FromUri(new Uri("http://u1820450.plsk.regruhosting.ru/file/"+(obj as Entry).Text));
            }, (obj) => target != null);
        }
        public ICommand SendToApiCommand { get; set; }
        /// <summary>
        /// Warning!CPU-bound operation!
        /// </summary>
        public ICommand VisualizeCommand { get; set; }
        public ICommand SetImageCommand { get; set; }
        public ICommand PhotoCommand { get; set; }
        public ICommand VideoCommand { get; set; }
        private readonly ContentPage _mainPage;
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = default)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
