using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Services.Store;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace Plugin.XamarinAppRating
{
    /// <summary>
    /// Implementation for AppRating
    /// </summary>
    public class AppRatingImplementation : IAppRating
    {
        /// <summary>
        /// Open UWP in-app review popup of your current application.
        /// </summary>
        /// <remarks>to use this method your UWP <b>Target Version</b> must be 10.0.17763 or above.</remarks>
        public async Task PerformInAppRateAsync()
        {
            var dispatcher = CoreApplication.MainView?.CoreWindow?.Dispatcher;

            await dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                StoreContext storeContext = StoreContext.GetDefault();
                
                var result = await storeContext.RequestRateAndReviewAppAsync();

                switch(result.Status)
                {
                    case StoreRateAndReviewStatus.Error:
                        await ShowErrorMessage("ERROR", "There was en error trying to opening in-app rating.");

                        break;
                    case StoreRateAndReviewStatus.CanceledByUser:
                        await ShowErrorMessage("ERROR", "In-app rating action canceled by user.");

                        break;
                    case StoreRateAndReviewStatus.NetworkError:
                        await ShowErrorMessage("ERROR", "Please check your internet connection first.");

                        break;
                }                
            });
        }

        /// <summary>
        /// Perform rating on the current OS store app or open the store page on browser.
        /// </summary>
        /// <param name="packageName">Use this for Android.</param>
        /// <param name="applicationId">Use this for iOS.</param>
        /// <param name="productId">Use this for UWP.</param>
        public async Task PerformRatingOnStoreAsync(string packageName = "", string applicationId = "", string productId = "")
        {
            if (!string.IsNullOrEmpty(productId))
            {
                try
                {
                    await Launcher.LaunchUriAsync(new Uri($"ms-windows-store://review/?ProductId={productId}"));
                }
                catch (Exception)
                {
                    await ShowErrorMessage("ERROR", "Cannot open rating because Microsoft Store was unable to launch.");
                }
            }
            else
            {
                await ShowErrorMessage("ERROR", "Please, provide the application ProductId for Microsoft Store.");
            }
        }

        [Obsolete("Please use PerformInAppRateAsync or PerformRatingOnStoreAsync instead.")]
        public async Task PerformPlatformRateAppAsync(string packageName = null, string applicationId = null, string productId = null)
        {
            // This implementation will be left until next version
            // in which it will be finally removed

            await this.PerformRatingOnStoreAsync(productId: productId);
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
        public Task PerformPlatformRateAppAsync(string packageName = null, string applicationId = null)
        {
            return Task.CompletedTask;
        }

        private async Task ShowErrorMessage(string title, string message)
        {
            ContentDialog alert = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "OK"
            };

            await alert.ShowAsync();
        }
    }
}