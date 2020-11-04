using Plugin.XamarinAppRating;
using System;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Essentials;

namespace TestAppNative.iOS
{
    public partial class HomeViewController : UIViewController
    {
        public HomeViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            rateAppOnStoreButton.TouchUpInside += RateAppOnStoreButton_TouchUpInside;
        }

        private void RateAppOnStoreButton_TouchUpInside(object sender, EventArgs args)
        {
            MainThread.InvokeOnMainThreadAsync(async () => await RateAppOnStore());
        }

        private async Task RateAppOnStore()
        {
            // This method use Facebook™'s store apps as example.
            if (CrossAppRating.IsSupported)
                await CrossAppRating.Current.PerformRatingOnStoreAsync(applicationId: "id284882215");
        }
    }
}