using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace XEdit
{
    [ContentProperty("Name")]
    public sealed class Localizer : IMarkupExtension
    {
        public string Name { get; set; }
        public object ProvideValue(IServiceProvider provider)
        {
            return Resource.ResourceManager.GetString(Name, Resource.Culture);
        }
    }
    [ContentProperty("Name")]
    public sealed class Stylizator : IMarkupExtension
    {
        public string Name { get; set; }
        public object ProvideValue(IServiceProvider provider)
        {
            return App.Current.Resources[Name];
        }
    }
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainMenu();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
