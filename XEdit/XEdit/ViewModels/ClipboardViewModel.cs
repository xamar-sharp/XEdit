using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.ComponentModel;
using Xamarin.Essentials;
using System.Windows.Input;
using Xamarin.Forms;
using System.Runtime.CompilerServices;

namespace XEdit.ViewModels
{
    public sealed class ClipboardViewModel : INotifyPropertyChanged
    {
        public ClipboardViewModel()
        {
            GetTextCommand = new Command(async (obj) =>
            {
                Value =await Clipboard.GetTextAsync();
            });
            SetTextCommand = new Command(async (obj) =>
            {
                await Clipboard.SetTextAsync(Value);
            }, (obj) => !string.IsNullOrEmpty(Value));
            HasTextCommand = new Command((obj) =>
            {
                Value = Clipboard.HasText.ToString();
            });
        }
        public ICommand GetTextCommand { get; set; }
        public ICommand SetTextCommand { get; set; }
        public ICommand HasTextCommand { get; set; }
        private string _value = String.Empty;
        public string Value { get => _value; set { _value = value; OnPropertyChanged(); } }
        public event PropertyChangedEventHandler PropertyChanged;
        public void UpdateState()
        {
            (SetTextCommand as Command).ChangeCanExecute();
        }
        public void OnPropertyChanged([CallerMemberName] string name = default)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
