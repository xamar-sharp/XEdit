using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XEdit
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Home : Shell
    {
        public Home()
        {
            InitializeComponent();
        }

        private async void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            try
            {
                await GoToAsync((sender as SearchBar).Text);
            }
            catch
            {
                (sender as SearchBar).Text = Resource.InvalidUri;
            }
        }
    }
}