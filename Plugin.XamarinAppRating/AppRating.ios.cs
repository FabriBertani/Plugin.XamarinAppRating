using System;
using System.Threading.Tasks;
using Foundation;
using StoreKit;
using UIKit;

namespace Plugin.XamarinAppRating
{
    /// <summary>
    /// Interface for AppRating
    /// </summary>
    public class AppRatingImplementation : IAppRating
    {
        /// <summary>
        /// Open specific target rating system
        /// </summary>
        /// <param name="packageName">Leave it null for iOS only.</param>
        /// <param name="applicationId">Store id of your iOS app. <strong>Required</strong> to alternative open the store if main method fail.</param>
        public Task PerformPlatformRateAppAsync(string packageName = null, string applicationId = null)
        {
            var tcs = new TaskCompletionSource<bool>();

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 3))
            {
                SKStoreReviewController.RequestReview();

                tcs.SetResult(true);
            }
            else
            {
                var storeUrl = $"itms-apps://itunes.apple.com/app/{applicationId}";
                var url = storeUrl + "?action=write-review";

                try
                {
                    UIApplication.SharedApplication.OpenUrl(new NSUrl(url));

                    tcs.SetResult(true);
                }
                catch (Exception)
                {
                    UIApplication.SharedApplication.InvokeOnMainThread(() =>
                    {
                        var alert = UIAlertController.Create("ERROR",
                                                             "Cannot open rating because App Store was unable to launch.",
                                                             UIAlertControllerStyle.Alert);

                        var positiveAction = UIAlertAction.Create("OK",
                                                                  UIAlertActionStyle.Default,
                                                                  actionPositive =>
                                                                  {
                                                                      tcs.SetResult(false);

                                                                      alert.DismissViewController(true, null);
                                                                  });

                        alert.AddAction(positiveAction);

                        UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
                    });
                }
            }

            return tcs.Task;
        }

        public Task PerformPlatformRateAppAsync()
        {
            return Task.FromResult(0);
        }

        public Task PerformPlatformRateAppAsync(string packageName = null)
        {
            return Task.FromResult(0);
        }

        public Task PerformPlatformRateAppAsync(string packageName = null, string applicationId = null, string productId = null)
        {
            return Task.FromResult(0);
        }
    }
}