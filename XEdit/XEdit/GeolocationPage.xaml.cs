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
    public partial class GeolocationPage : ContentPage
    {
        public GeolocationViewModel Model { get; set; }
        public GeolocationPage()
        {
            InitializeComponent();
            Title = Resource.GEOTitle;
            Model = new GeolocationViewModel();
            this.BindingContext = Model;
        }

        private void Entry_Completed(object sender, EventArgs e)
        {
            Model.UpdateState();
        }
    }
}