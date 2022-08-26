using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.ComponentModel;
using Xamarin.Forms;
using System.Runtime.CompilerServices;
using System.Threading;
namespace XEdit.ViewModels
{
    public sealed class TextToSpeechViewModel : INotifyPropertyChanged
    {
        public TextToSpeechViewModel()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var locales = await TextToSpeech.GetLocalesAsync();
                _current = locales.First(loc => loc.Language == Resource.Culture.Name.Split('-')[0]);
            });
            SpeakCommand = new Command(async (obj) =>
            {
                await TextToSpeech.SpeakAsync(Value, new SpeechOptions() { Locale = _current, Pitch = Convert.ToSingle(Pitch), Volume = Convert.ToSingle(Volume) });
            }, (obj) => _current != null && Value != String.Empty);
        }
        public ICommand SpeakCommand { get; set; }
        private Locale _current;
        private string _value = String.Empty;
        public string Value { get => _value; set { _value = value; OnPropertyChanged(); } }
        private double _pitch;
        public double Pitch { get => _pitch; set { _pitch = value; OnPropertyChanged(); } }
        private double _volume;
        public double Volume
        {
            get => _volume; set
            {
                _volume = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void UpdateState()
        {
            (SpeakCommand as Command).ChangeCanExecute();
        }
        public void OnPropertyChanged([CallerMemberName] string name = default)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}