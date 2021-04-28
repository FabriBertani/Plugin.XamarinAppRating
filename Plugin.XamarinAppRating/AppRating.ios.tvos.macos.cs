using System;
using System.Threading.Tasks;
using Foundation;
using StoreKit;
#if !__MACOS__
using UIKit;
#else
using AppKit;
#endif

namespace Plugin.XamarinAppRating
{
    /// <summary>
    /// Implementation for AppRating
    /// </summary>
    public class AppRatingImplementation : IAppRating
    {
        /// <summary>
        /// Open in-app review popup of your current application.
        /// </summary>
        public Task PerformInAppRateAsync()
        {
            var tcs = new TaskCompletionSource<bool>();

#if __TVOS__
            throw new NotSupportedException("tvOS not support in app rating.");

#elif __IOS__
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 3))
            {
                SKStoreReviewController.RequestReview();

                tcs.SetResult(true);
            }
            else
            {
                DisplayErrorAlert("Your current iOS version doesn't support in-app rating.");

                tcs.SetResult(false);
            }
#elif __MACOS__
            using var info = new NSProcessInfo();

            Version version = new(0, 0);

            if (Version.TryParse(info.OperatingSystem.ToString(), out var number))
                version = number;

            if (version >= new Version(10, 14))
            {
                SKStoreReviewController.RequestReview();

                tcs.SetResult(true);
            }
            else
            {
                DisplayErrorAlert("Your current iOS version doesn't support in-app rating.");

                tcs.SetResult(false);
            }
#endif

            return tcs.Task;
        }

        /// <summary>
        /// Perform rating on the current OS store app or open the store page on browser.
        /// </summary>
        /// <param name="packageName">Use this for Android.</param>
        /// <param name="applicationId">Use this for iOS/macOS/tvOS.</param>
        /// <param name="productId">Use this for UWP.</param>
        public Task PerformRatingOnStoreAsync(string packageName = "", string applicationId = "", string productId = "")
        {
            var tcs = new TaskCompletionSource<bool>();

            if (!string.IsNullOrEmpty(applicationId))
            {
                var url = string.Empty;
#if __IOS__
                url = $"itms-apps://itunes.apple.com/app/{applicationId}?action=write-review";
#elif __TVOS__
                url = $"com.apple.TVAppStore://itunes.apple.com/app/{applicationId}?action=write-review";
#elif __MACOS__
                url = $"macappstore://itunes.apple.com/app/{applicationId}?action=write-review";
#endif

                try
                {
#if __IOS__
                    UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
#elif __MACOS__
                    AppKit.NSWorkspace.SharedWorkspace.OpenUrl(new NSUrl(url));
#endif

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

        private void DisplayErrorAlert(string message)
        {
            NSRunLoop.Main.InvokeOnMainThread(() =>
            {
#if __IOS__ || __TVOS__
                var alert = UIAlertController.Create("ERROR",
                                                     message,
                                                     UIAlertControllerStyle.Alert);

                var positiveAction = UIAlertAction.Create("OK",
                                                          UIAlertActionStyle.Default,
                                                          actionPositive => alert.DismissViewController(true, null));

                alert.AddAction(positiveAction);

                UIApplication.SharedApplication.KeyWindow.RootViewController.PresentViewController(alert, true, null);
# elif __MACOS__
                var alert = new NSAlert()
                {
                    AlertStyle = NSAlertStyle.Informational,
                    InformativeText = message,
                    MessageText = "ERROR"
                };

                alert.AddButton("OK");

                alert.RunModal();
# endif
            });
        }
    }
}