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
    public partial class FilePickerPage : ContentPage
    {
        public FilePickerViewModel Model { get; set; }
        public FilePickerPage()
        {
            InitializeComponent();
            Title = Resource.FPTitle;
            Model = new FilePickerViewModel(this,img);
            this.BindingContext = Model;
        }
    }
}