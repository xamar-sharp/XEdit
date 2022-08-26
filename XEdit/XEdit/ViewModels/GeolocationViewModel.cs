using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Xamarin.Essentials;
using System.Windows.Input;
using Xamarin.Forms;
using System.Runtime.CompilerServices;
using System.Linq;
namespace XEdit.ViewModels
{
    public sealed class GeolocationViewModel : INotifyPropertyChanged
    {
        public GeolocationViewModel()
        {
            GetCurrentCommand = new Command(async (obj) =>
            {
                try
                {
                    var location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(30)));
                    Value = $"{Resource.Latitude}:{location.Latitude}\n{Resource.Longitude}:{location.Longitude}\n{Resource.Altitude}:{location.Altitude}\n{Resource.Mock}:{location.IsFromMockProvider}";
                }
                catch (FeatureNotEnabledException) {
                    Value = Resource.EnableGPS;
                }
            });
            LastKnownCommand = new Command(async (obj) =>
            {
                try
                {
                    var location = await Geolocation.GetLastKnownLocationAsync();
                    Value = $"{Resource.Latitude}:{location.Latitude}\n{Resource.Longitude}:{location.Longitude}\n{Resource.Altitude}:{location.Altitude}\n{Resource.Mock}:{location.IsFromMockProvider}";
                }
                catch (FeatureNotEnabledException) {
                    Value = Resource.EnableGPS;
                }
            });
            CalculateCommand = new Command(async (obj) =>
            {
                try
                {
                    double[] values = Value.Split(':').Select(el => Convert.ToDouble(el)).ToArray();
                    if (values.Length == 3)
                    {
                        var location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(30)));
                        Value = $"{Location.CalculateDistance(new Location(values[0], values[1], values[2]), location, DistanceUnits.Kilometers) * 1000}m";
                    }
                    else
                    {
                        Value = Resource.InvalidUri;
                    }
                }
                catch (FeatureNotEnabledException)
                {
                    Value = Resource.EnableGPS;
                }
            }, (obj) => !string.IsNullOrEmpty(Value));
        }
        public ICommand LastKnownCommand { get; set; }
        public ICommand GetCurrentCommand { get; set; }
        public ICommand CalculateCommand { get; set; }
        private string _value = String.Empty;
        public string Value { get => _value; set { _value = value; OnPropertyChanged(); } }
        public event PropertyChangedEventHandler PropertyChanged;
        public void UpdateState()
        {
            (CalculateCommand as Command).ChangeCanExecute();
        }
        public void OnPropertyChanged([CallerMemberName] string name = default)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
