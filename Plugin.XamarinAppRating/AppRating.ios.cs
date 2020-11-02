using System;
using System.Threading.Tasks;
using Foundation;
using StoreKit;
using UIKit;

namespace Plugin.XamarinAppRating
{
    /// <summary>
    /// Implementation for AppRating
    /// </summary>
    public class AppRatingImplementation : IAppRating
    {
        /// <summary>
        /// Open iOS in-app review popup of your current application.
        /// </summary>
        public Task PerformInAppRateAsync()
        {
            var tcs = new TaskCompletionSource<bool>();

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 3))
            {
                SKStoreReviewController.RequestReview();

                tcs.SetResult(true);
            }

            return tcs.Task;
        }

        /// <summary>
        /// Perform rating on the current OS store app or open the store page on browser.
        /// </summary>
        /// <param name="packageName">Use this for Android.</param>
        /// <param name="applicationId">Use this for iOS.</param>
        /// <param name="productId">Use this for UWP.</param>
        public Task PerformRatingOnStoreAsync(string packageName = "", string applicationId = "", string productId = "")
        {
            var tcs = new TaskCompletionSource<bool>();

            if (!string.IsNullOrEmpty(applicationId))
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
                    DisplayErrorAlert("Cannot open rating because App Store was unable to launch.");

                    tcs.SetResult(false);
                }
            }
            else
            {
                DisplayErrorAlert("Please, provide the ApplicationId for Apple App Store.");

                tcs.SetResult(false);
            }

            return tcs.Task;
        }

        [Obsolete("Please use PerformInAppRateAsync or PerformRatingOnStoreAsync instead.")]
        public Task PerformPlatformRateAppAsync()
        {
            return Task.CompletedTask;
        }

        [Obsolete("Please use PerformInAppRateAsync or PerformRatingOnStoreAsync instead.")]
        public Task PerformPlatformRateAppAsync(string packageName = null)
        {
            return Task.CompletedTask;
        }

        [Obsolete("Please use PerformInAppRateAsync or PerformRatingOnStoreAsync instead.")]
        public async Task PerformPlatformRateAppAsync(string packageName = null, string applicationId = null)
        {
            // This implementation will be left until next version
            // in which it will be finally removed

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 3))
                await PerformInAppRateAsync();
            else
                await PerformRatingOnStoreAsync(applicationId: applicationId);
        }

        [Obsolete("Please use PerformInAppRateAsync or PerformRatingOnStoreAsync instead.")]
        public Task PerformPlatformRateAppAsync(string packageName = null, string applicationId = null, string productId = null)
        {
            return Task.CompletedTask;
        }

        private void DisplayErrorAlert(string message)
        {
            NSRunLoop.Main.InvokeOnMainThread(() =>
            {
                var alert = UIAlertController.Create("ERROR",
                                                     message,
                                                     UIAlertControllerStyle.Alert);

                var positiveAction = UIAlertAction.Create("OK",
                                                          UIAlertActionStyle.Default,
                                                          actionPositive => alert.DismissViewController(true, null));

                alert.AddAction(positiveAction);

                UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
            });
        }
    }
}