using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Xamarin.Essentials;
using System.Windows.Input;
using Xamarin.Forms;
using System.Runtime.CompilerServices;
namespace XEdit.ViewModels
{
    public sealed class PreferencesViewModel : INotifyPropertyChanged
    {
        public PreferencesViewModel()
        {
            GetCommand = new Command((obj) =>
            {
                Value = Preferences.Get(Value, Resource.NotFound);
            }, (obj) => !string.IsNullOrEmpty(Value));
            SetCommand = new Command((obj) =>
            {
                string[] parts = Value.Split('#');
                if (parts.Length == 2)
                {
                    Preferences.Set(parts[0], parts[1]);
                }
            }, (obj) => !string.IsNullOrEmpty(Value));
            ContainsKeyCommand = new Command((obj) =>
            {
                Value = (Preferences.ContainsKey(Value)).ToString();
            }, (obj) => !string.IsNullOrEmpty(Value));
            RemoveCommand = new Command((obj) =>
            {
                Preferences.Remove(Value);
            }, (obj) => !string.IsNullOrEmpty(Value));
            ClearCommand = new Command(obj =>
            {
                Preferences.Clear();
            }, (obj) => !string.IsNullOrEmpty(Value));
        }
        public ICommand GetCommand { get; set; }
        public ICommand SetCommand { get; set; }
        public ICommand ContainsKeyCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        private string _value = String.Empty;
        public string Value { get => _value; set { _value = value; OnPropertyChanged(); } }
        public event PropertyChangedEventHandler PropertyChanged;
        public void UpdateState()
        {
            (GetCommand as Command).ChangeCanExecute();
            (SetCommand as Command).ChangeCanExecute();
            (ContainsKeyCommand as Command).ChangeCanExecute();
            (RemoveCommand as Command).ChangeCanExecute();
            (ClearCommand as Command).ChangeCanExecute();
        }
        public void OnPropertyChanged([CallerMemberName] string name = default)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
