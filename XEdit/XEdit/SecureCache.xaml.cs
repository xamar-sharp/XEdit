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
    public partial class SecureCache : ContentPage
    {
        public SecureStorageViewModel Model { get; set; }
        public SecureCache()
        {
            InitializeComponent();
            Title = Resource.SecureTitle;
            Model = new SecureStorageViewModel();
            this.BindingContext = Model;
        }

        private void Entry_Completed(object sender, EventArgs e)
        {
            Model.UpdateState();
        }
    }
}