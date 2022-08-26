using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XEdit.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XEdit
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConnectivityPage : ContentPage
    {
        public ConnectivityViewModel Model { get; set; }
        public ConnectivityPage()
        {
            InitializeComponent();
            Title = Resource.NetTitle;
            Model = new ConnectivityViewModel(this);
            this.BindingContext = Model;
        }

        private void RadioButton_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            Model.NotifyCommand.Execute(null);
        }
    }
}