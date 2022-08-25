using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using System.Windows.Input;
using System.ComponentModel;
using Xamarin.Forms;
using System.Runtime.CompilerServices;
using System.Threading;
namespace XEdit.ViewModels
{
    public sealed class SecureStorageViewModel : INotifyPropertyChanged,IDisposable
    {
        public static SemaphoreSlim _slim;
        static SecureStorageViewModel()
        {
            _slim = new SemaphoreSlim(1, 1);
        }
        public SecureStorageViewModel()
        {
            GetCommand = new Command(async (obj) =>
            {
                await _slim.WaitAsync();
                try
                {
                    Value = await SecureStorage.GetAsync(Value);
                }
                finally
                {
                    _slim.Release(1);
                }
            }, (obj) => !string.IsNullOrEmpty(Value));
            SetCommand = new Command(async (obj) =>
            {
                string[] parts = Value.Split('#');
                if (parts.Length == 2)
                {
                    await SecureStorage.SetAsync(parts[0],parts[1]);
                }
            }, (obj) => !string.IsNullOrEmpty(Value));
            RemoveAllCommand = new Command((obj) =>
            {
                SecureStorage.RemoveAll();
            }, (obj) => !string.IsNullOrEmpty(Value));
            RemoveCommand = new Command((obj) =>
            {
                SecureStorage.Remove(Value);
            }, (obj) => !string.IsNullOrEmpty(Value));
        }
        public ICommand GetCommand { get; set; }
        public ICommand SetCommand { get; set; }
        public ICommand RemoveAllCommand { get; set; }
        public ICommand RemoveCommand { get; set; }
        private string _value = String.Empty;
        public string Value { get => _value; set { _value = value; OnPropertyChanged(); } }
        public event PropertyChangedEventHandler PropertyChanged;
        public void Dispose()
        {
            _slim.Dispose();
        }
        public void UpdateState()
        {
            (GetCommand as Command).ChangeCanExecute();
            (SetCommand as Command).ChangeCanExecute();
            (RemoveCommand as Command).ChangeCanExecute();
            (RemoveAllCommand as Command).ChangeCanExecute();
        }
        public void OnPropertyChanged([CallerMemberName] string name = default)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
