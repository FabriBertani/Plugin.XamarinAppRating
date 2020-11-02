using System;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Views;
#if !MONODROID_80 || !MONODROID_81
using Com.Google.Android.Play.Core.Review;
using Com.Google.Android.Play.Core.Tasks;
#endif
using Xamarin.Essentials;
using Task = System.Threading.Tasks.Task;

namespace Plugin.XamarinAppRating
{
    /// <summary>
    /// Implementation for AppRating
    /// </summary>
//public static partial class AppRating
#if !MONODROID_80 || !MONODROID_81
    public class AppRatingImplementation : Java.Lang.Object, IAppRating, IOnCompleteListener
#else
    public class AppRatingImplementation : Java.Lang.Object, IAppRating
#endif
    {
        private static volatile Handler handler;

        private TaskCompletionSource<bool> inAppRateTCS;

#if !MONODROID_80 || !MONODROID_81
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
#else
        /// <summary>
        /// Open Android in-app review popup of your current application.
        /// </summary>        
        /// <remarks>This method is <b>not</b> supported on Android Oreo or below.</remarks>
        public Task PerformInAppRateAsync()
        {
            var tcs = new TaskCompletionSource<bool>();

            ShowAlertMessage("NOT SUPPORTED", "Your current Android version doesn't support in-app rating.");

            tcs.SetResult(false);

            return tcs.Task;
        }
#endif

        /// <summary>
        /// Perform rating on the current OS store app or open the store page on browser.
        /// </summary>
        /// <param name="packageName">Use this for Android.</param>
        /// <param name="applicationId">Use this for iOS.</param>
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
                    Intent intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(url));

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

#if !MONODROID_80 || !MONODROID_81
        public void OnComplete(Com.Google.Android.Play.Core.Tasks.Task task)
        {
            if (!task.IsSuccessful)
            {
                inAppRateTCS.TrySetResult(false);

                launchTask?.Dispose();

                return;
            }

            try
            {
                var reviewInfo = (ReviewInfo)task.GetResult(Java.Lang.Class.FromType(typeof(ReviewInfo)));

                launchTask = reviewManager.LaunchReviewFlow(Platform.CurrentActivity, reviewInfo);

                launchTask.AddOnCompleteListener(this);
            }
            catch (System.Exception ex)
            {
                ShowAlertMessage("ERROR", "There was an error launching in-app review. Please try again.");

                inAppRateTCS.TrySetResult(false);

                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }
#endif

        private void ShowAlertMessage(string title, string message)
        {
            if (handler?.Looper != Looper.MainLooper)
                handler = new Handler(Looper.MainLooper);

            handler.Post(() =>
            {
                var dialog = new AlertDialog.Builder(Application.Context);
                dialog.SetTitle(title);
                dialog.SetMessage(message);

                dialog.SetPositiveButton("OK", (System.EventHandler<DialogClickEventArgs>)null);

                var alert = dialog.Create();

                if (Build.VERSION.SdkInt < BuildVersionCodes.M)
                    alert.Window.SetType(WindowManagerTypes.SystemAlert);

                alert.Show();
            });

            handler.Dispose();
        }
    }
}