using System;
using System.Globalization
;
using Xamarin.Forms;
using Xamarin.Essentials;
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
    public sealed class LocaleConverter : IValueConverter
    {
        public object Convert(object path,Type targetType,object parameter,CultureInfo info)
        {
            if(path as Locale != null)
            {
                return (path as Locale).Name;
            }
            return Resource.NotFound;
        }
        public object ConvertBack(object path, Type targetType, object parameter, CultureInfo info)
        {
            return null;
        }
    }
    public partial class App : Application
    {
        public App()
        {
            Resource.Culture = CultureInfo.CurrentUICulture;
            InitializeComponent();
            Resources.Add("localeConv", new LocaleConverter());
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
