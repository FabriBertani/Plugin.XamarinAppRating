using Plugin.XamarinAppRating;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TestApp
{
    public partial class MainPage : ContentPage
    {
        private const string androidPackageName = "com.facebook.katana";
        private const string iOSApplicationId = "id284882215";
        private const string uwpProductId = "9wzdncrf0083";

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

                await RateApplicationInApp();
            }
        }

        private Task RateApplicationInApp()
        {
            if (CrossAppRating.IsSupported)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    // This method will simulate Facebook™ app to in-app rating as example.
                    await CrossAppRating.Current.PerformInAppRateAsync();
                });

                Preferences.Set("application_rated", true);
            }

            return Task.CompletedTask;
        }

        private Task RateApplicationOnStore()
        {
            if (CrossAppRating.IsSupported)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    // This method use Facebook™'s store apps as example.
                    await CrossAppRating.Current.PerformRatingOnStoreAsync(packageName: androidPackageName, applicationId: iOSApplicationId, productId: uwpProductId);
                });

                Preferences.Set("application_rated", true);
            }

            return Task.CompletedTask;
        }

        private void InAppRating_Clicked(object sender, EventArgs e)
        {
            if (!Preferences.Get("application_rated", false))
                Task.Run(() => RateApplicationInApp());
        }

        private void AppRateOnStore_Clicked(object sender, EventArgs e)
        {
            if (!Preferences.Get("application_rated", false))
                Task.Run(() => RateApplicationOnStore());
        }
    }
}