using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamOrchard.Services;
using XamOrchard.Views;

namespace XamOrchard
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            Xamarin.Forms.Device.SetFlags(new List<string>() { "StateTriggers_Experimental", "IndicatorView_Experimental", "CarouselView_Experimental", "MediaElement_Experimental"});
            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
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
