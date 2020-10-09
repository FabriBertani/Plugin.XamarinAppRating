using System;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml.Controls;

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
        /// <param name="packageName">Leave it null for UWP only.</param>
        /// <param name="applicationId">Leave it null for UWP only.</param>
        /// <param name="productId">Product id of your UWP app. <strong>Required</strong> to alternative open the store if main method fail.</param>
        public async Task PerformPlatformRateAppAsync(string packageName = null, string applicationId = null, string productId = null)
        {
            try
            {
                await Launcher.LaunchUriAsync(new Uri($"ms-windows-store://review/?ProductId={productId}"));
            }
            catch (Exception)
            {
                ContentDialog alert = new ContentDialog
                {
                    Title = "ERROR",
                    Content = "Cannot open rating because Microsoft Store was unable to launch.",
                    CloseButtonText = "OK"
                };

                await alert.ShowAsync();
            }
        }

        public Task PerformPlatformRateAppAsync()
        {
            return Task.FromResult(0);
        }

        public Task PerformPlatformRateAppAsync(string packageName = null)
        {
            return Task.FromResult(0);
        }

        public Task PerformPlatformRateAppAsync(string packageName = null, string applicationId = null)
        {
            return Task.FromResult(0);
        }
    }
}