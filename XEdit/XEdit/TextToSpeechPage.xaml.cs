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
    public partial class TextToSpeechPage : ContentPage
    {
        public TextToSpeechViewModel Model { get; set; }
        public TextToSpeechPage()
        {
            InitializeComponent();
            Title = Resource.TTSTitle;
            Model = new TextToSpeechViewModel();
            this.BindingContext = Model;
        }

        private void ent_Completed(object sender, EventArgs e)
        {
            Model.UpdateState();
        }

    }
}