using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Net;
using Android.OS;
using Android.Views;

namespace Plugin.XamarinAppRating
{
    /// <summary>
    /// Interface for AppRating
    /// </summary>
    //public static partial class AppRating
    public class AppRatingImplementation : IAppRating
    {
        private static volatile Handler handler;

        /// <summary>
        /// Open specific target rating system
        /// </summary>
        /// <remarks><strong>Required</strong> to alternative open the store if main method fail.</remarks>
        /// <param name="packageName">Package name of your android app.</param>
        public Task PerformPlatformRateAppAsync(string packageName)
        {
            var tcs = new TaskCompletionSource<bool>();

            var activity = Application.Context;
            var url = $"market://details?id={(activity as Context)?.PackageName}";

            try
            {
                activity.PackageManager.GetPackageInfo("com.android.vending", PackageInfoFlags.Activities);
                Intent intent = new Intent(Intent.ActionView, Uri.Parse(url));

                intent.AddFlags(ActivityFlags.NewTask | ActivityFlags.ResetTaskIfNeeded);

                activity.StartActivity(intent);

                tcs.SetResult(true);
            }
            catch (PackageManager.NameNotFoundException)
            {
                // This won't happen. But catching just in case the user has downloaded the app without having Google Play installed.

                if (handler?.Looper != Looper.MainLooper)
                    handler = new Handler(Looper.MainLooper);

                handler.Post(() =>
                {
                    var dialog = new AlertDialog.Builder(activity);
                    dialog.SetTitle("Error");
                    dialog.SetMessage("Cannot open rating because Google Play is not installed.");

                    dialog.SetPositiveButton("OK", (System.EventHandler<DialogClickEventArgs>)null);

                    var alert = dialog.Create();

                    if (Build.VERSION.SdkInt < BuildVersionCodes.M)
                        alert.Window.SetType(WindowManagerTypes.SystemAlert);

                    alert.Show();
                });

                tcs.SetResult(false);
            }
            catch (ActivityNotFoundException)
            {
                // If Google Play fails to load, open the App link on the browser

                var playStoreUrl = $"https://play.google.com/store/apps/details?id={packageName}";

                var browserIntent = new Intent(Intent.ActionView, Uri.Parse(playStoreUrl));
                browserIntent.AddFlags(ActivityFlags.NewTask | ActivityFlags.ResetTaskIfNeeded);

                activity.StartActivity(browserIntent);

                tcs.SetResult(true);
            }

            return tcs.Task;
        }

        public Task PerformPlatformRateAppAsync()
        {
            return Task.FromResult(0);
        }

        public Task PerformPlatformRateAppAsync(string packageName = null, string applicationId = null)
        {
            return Task.FromResult(0);
        }

        public Task PerformPlatformRateAppAsync(string packageName = null, string applicationId = null, string productId = null)
        {
            return Task.FromResult(0);
        }

    }
}