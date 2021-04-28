using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Com.Google.Android.Play.Core.Review;
using Com.Google.Android.Play.Core.Tasks;
using Xamarin.Essentials;
using Task = System.Threading.Tasks.Task;

namespace Plugin.XamarinAppRating
{
    /// <summary>
    /// Implementation for AppRating
    /// </summary>
    public class AppRatingImplementation : Java.Lang.Object, IAppRating, IOnCompleteListener
    {
        private static volatile Handler handler;

        private TaskCompletionSource<bool> inAppRateTCS;

        private IReviewManager reviewManager;

        private Com.Google.Android.Play.Core.Tasks.Task launchTask;

        /// <summary>
        /// Open Android in-app review popup of your current application.
        /// </summary>
        public async Task PerformInAppRateAsync()
        {
            inAppRateTCS?.TrySetCanceled();

            inAppRateTCS = new TaskCompletionSource<bool>();

            reviewManager = ReviewManagerFactory.Create(Application.Context);

            var request = reviewManager.RequestReviewFlow();

            request.AddOnCompleteListener(this);

            await inAppRateTCS.Task;

            reviewManager.Dispose();

            request.Dispose();
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

            if (!string.IsNullOrEmpty(packageName))
            {
                var context = Application.Context;
                var url = $"market://details?id={(context as Context)?.PackageName}";

                try
                {
                    context.PackageManager.GetPackageInfo("com.android.vending", PackageInfoFlags.Activities);
                    Intent intent = new(Intent.ActionView, Android.Net.Uri.Parse(url));

                    intent.AddFlags(ActivityFlags.ClearTop);

                    intent.AddFlags(ActivityFlags.NewTask | ActivityFlags.ResetTaskIfNeeded);

                    context.StartActivity(intent);

                    tcs.SetResult(true);
                }
                catch (PackageManager.NameNotFoundException)
                {
                    // This won't happen. But catching just in case the user has downloaded the app without having Google Play installed.

                    ShowAlertMessage("Error", "Cannot open rating because Google Play is not installed.");

                    tcs.SetResult(false);
                }
                catch (ActivityNotFoundException)
                {
                    // If Google Play fails to load, open the App link on the browser

                    var playStoreUrl = $"https://play.google.com/store/apps/details?id={packageName}";

                    var browserIntent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(playStoreUrl));
                    browserIntent.AddFlags(ActivityFlags.NewTask | ActivityFlags.ResetTaskIfNeeded);

                    context.StartActivity(browserIntent);

                    tcs.SetResult(true);
                }
            }
            else
            {
                ShowAlertMessage("Error", "Please, provide the application PackageName for Google Play Store.");

                tcs.SetResult(false);
            }

            return tcs.Task;
        }

        [Obsolete("Please use PerformInAppRateAsync or PerformRatingOnStoreAsync instead.")]
        public async Task PerformPlatformRateAppAsync(string packageName)
        {
            // This implementation will be left until next version
            // in which it will be finally removed

            await PerformRatingOnStoreAsync(packageName: packageName);
        }

        [Obsolete("Please use PerformInAppRateAsync or PerformRatingOnStoreAsync instead.")]
        public Task PerformPlatformRateAppAsync()
        {
            return Task.CompletedTask;
        }

        [Obsolete("Please use PerformInAppRateAsync or PerformRatingOnStoreAsync instead.")]
        public Task PerformPlatformRateAppAsync(string packageName = null, string applicationId = null)
        {
            return Task.CompletedTask;
        }

        [Obsolete("Please use PerformInAppRateAsync or PerformRatingOnStoreAsync instead.")]
        public Task PerformPlatformRateAppAsync(string packageName = null, string applicationId = null, string productId = null)
        {
            return Task.CompletedTask;
        }
        public void OnComplete(Com.Google.Android.Play.Core.Tasks.Task task)
        {
            if (!task.IsSuccessful)
            {
                inAppRateTCS.TrySetResult(false);

                launchTask?.Dispose();

                return;
            }

            if (Platform.CurrentActivity == null)
                throw new NullReferenceException("Please ensure that you have installed Xamarin.Essential package and that it's initialized on your MainActivity.cs");

            try
            {
                ReviewInfo reviewInfo = (ReviewInfo)task.GetResult(Java.Lang.Class.FromType(typeof(ReviewInfo)));

                launchTask = reviewManager.LaunchReviewFlow(Platform.CurrentActivity, reviewInfo);

                launchTask.AddOnCompleteListener(this);
            }
            catch (Exception ex)
            {
                ShowAlertMessage("ERROR", "There was an error launching in-app review. Please try again.");

                inAppRateTCS.TrySetResult(false);

                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private void ShowAlertMessage(string title, string message)
        {
            if (handler?.Looper != Looper.MainLooper)
                handler = new Handler(Looper.MainLooper);

            handler.Post(() =>
            {
                var dialog = new AlertDialog.Builder(Application.Context);
                dialog.SetTitle(title);
                dialog.SetMessage(message);

                dialog.SetPositiveButton("OK", (EventHandler<DialogClickEventArgs>)null);

                var alert = dialog.Create();

                if (Build.VERSION.SdkInt < BuildVersionCodes.M)
                    alert.Window.SetType(WindowManagerTypes.SystemAlert);

                alert.Show();
            });

            handler.Dispose();
        }
    }
}