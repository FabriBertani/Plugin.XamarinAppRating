using System.Threading.Tasks;

namespace Plugin.XamarinAppRating
{
    /// <summary>
    /// Interface for AppRating
    /// </summary>
    public class AppRatingImplementation : IAppRating
    {
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

        public Task PerformPlatformRateAppAsync(string packageName = null, string applicationId = null, string productId = null)
        {
            return Task.FromResult(0);
        }
    }
}