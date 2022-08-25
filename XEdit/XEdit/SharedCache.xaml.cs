using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XEdit.ViewModels;
namespace XEdit
{
    public partial class SharedCache : ContentPage
    {
        public PreferencesViewModel Model { get; set; }
        public SharedCache()
        {
            InitializeComponent();
            Title = Resource.PrefTitle;
            Model = new PreferencesViewModel();
            this.BindingContext = Model;
        }

        private void Entry_Completed(object sender, EventArgs e)
        {
            Model.UpdateState();
        }
    }
}
