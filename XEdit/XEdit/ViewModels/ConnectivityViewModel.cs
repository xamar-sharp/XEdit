using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Xamarin.Essentials;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using System.Runtime.CompilerServices;
namespace XEdit.ViewModels
{
    public sealed class ConnectivityViewModel : INotifyPropertyChanged
    {
        private static readonly IDictionary<NetworkAccess, (string, Color)> _networkMap;
        static ConnectivityViewModel()
        {
            _networkMap = new Dictionary<NetworkAccess, (string, Color)>(4)
            {
                [NetworkAccess.None] = (Resource.NetNone, Color.Red),
                [NetworkAccess.Internet] = (Resource.NetActive, Color.Green),
                [NetworkAccess.Local] = (Resource.NetLocal, Color.GreenYellow),
                [NetworkAccess.ConstrainedInternet] = (Resource.NetConstrained, Color.OrangeRed)
            };
        }

        public ConnectivityViewModel(Page page)
        {
            _mainPage = page;
            GetNetworkCommand = new Command((obj) =>
            {
                var tuple = _networkMap[Connectivity.NetworkAccess];
                Device.BeginInvokeOnMainThread(() =>
                {
                    (obj as Label).Text = tuple.Item1;
                    NetworkState = tuple.Item2;
                });
            });
            GetProfilesCommand = new Command((obj) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    (obj as Label).Text = string.Join(",", Array.ConvertAll(Connectivity.ConnectionProfiles.ToArray(), profile => profile.ToString()));
                });
            });
            NotifyCommand = new Command((obj) =>
            {
                Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            });
        }

        private void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                await _mainPage.DisplayAlert(Resource.NetStatus, e.NetworkAccess.ToString() + "$" + string.Join(",", Array.ConvertAll(e.ConnectionProfiles.ToArray(), profile => profile.ToString())), Resource.Ok);
            });
        }
        private readonly Page _mainPage;
        public ICommand GetNetworkCommand { get; set; }
        public ICommand GetProfilesCommand { get; set; }
        public ICommand NotifyCommand { get; set; }
        private Color _networkState = _networkMap[Connectivity.NetworkAccess].Item2;
        public Color NetworkState
        {
            get => _networkState;
            set
            {
                _networkState = value;
                OnPropertyChanged();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = default)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
