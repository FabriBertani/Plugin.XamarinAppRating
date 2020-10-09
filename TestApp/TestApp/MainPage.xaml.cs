using Plugin.XamarinAppRating;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TestApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            // This method use Facebook™'s store apps as example.
            Device.BeginInvokeOnMainThread(async () => await CrossAppRating.Current.PerformPlatformRateAppAsync("com.facebook.katana", "id284882215", "9wzdncrf0083"));
        }
    }
}