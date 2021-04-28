using System;
using System.Threading.Tasks;
using AppKit;
using Foundation;
using Plugin.XamarinAppRating;
using Xamarin.Essentials;

namespace TestAppNative.MacOS
{
    public partial class ViewController : NSViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.Layer.BackgroundColor = NSColor.White.CGColor;
        }

        public override NSObject RepresentedObject
        {
            get => base.RepresentedObject;
            set
            {
                base.RepresentedObject = value;
            }
        }

        partial void rateAppOnStoreButton(NSObject sender)
        {
            MainThread.InvokeOnMainThreadAsync(async () => await RateAppOnStore());
        }

        private async Task RateAppOnStore()
        {
            // This method use Facebook?'s store apps as example.
            if (CrossAppRating.IsSupported)
                await CrossAppRating.Current.PerformRatingOnStoreAsync(applicationId: "id284882215");
        }
    }
}
