using Plugin.XamarinAppRating;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TestApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            if (!Preferences.Get("application_rated", false))
                Task.Run(() => CheckAppCountAndRate());
        }

        private async Task CheckAppCountAndRate()
        {
            if (Preferences.Get("application_counter", 0) >= 5)
            {
                if (!await DisplayAlert("Rate this App!", "Are you enjoying the app so far? Would you like to leave a review in the store?", "Yes", "No"))
                {
                    Preferences.Set("application_counter", 0);

                    return;
                }

                await RateApplication();
            }
        }

        private Task RateApplication()
        {
            if (CrossAppRating.IsSupported)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    // This method use Facebook™'s store apps as example.
                    await CrossAppRating.Current.PerformPlatformRateAppAsync("com.facebook.katana", "id284882215", "9wzdncrf0083");
                });

                Preferences.Set("application_rated", true);
            }

            return Task.FromResult(0);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (!Preferences.Get("application_rated", false))
                Task.Run(() => RateApplication());
        }
    }
}