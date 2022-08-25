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
    public partial class ClipboardPage : ContentPage
    {
        public ClipboardViewModel Model { get; set; }
        public ClipboardPage()
        {
            InitializeComponent();
            Title = Resource.ClipboardTitle;
            Model = new ClipboardViewModel();
            this.BindingContext = Model;
        }
        public void Entry_Completed(object sender,EventArgs empty)
        {
            Model.UpdateState();
        }
    }
}