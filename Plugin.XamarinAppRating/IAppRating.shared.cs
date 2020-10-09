using System.Threading.Tasks;

namespace Plugin.XamarinAppRating
{
    /// <summary>
    /// IAppRating interface
    /// </summary>
    public interface IAppRating
    {
        /// <summary>
        /// Open rating system on your selected devices.
        /// </summary>
        /// <remarks>This method will only work on <b>Android</b> and <b>iOS</b> using deafult data from the app. For UWP see inner method implementation.</remarks>
        Task PerformPlatformRateAppAsync();

        /// <summary>
        /// Open rating system on your selected devices.
        /// </summary>
        /// <remarks>Required to alternative open the store if main method fail.</remarks>
        /// <param name="packageName">Package name of your android app.</param>
        Task PerformPlatformRateAppAsync(string packageName = null);

        /// <summary>
        /// Open rating system on your selected devices.
        /// </summary>
        /// <param name="packageName">Leave it null for iOS only.</param>
        /// <param name="applicationId">Store id of your iOS app. <strong>Required</strong> to alternative open the store if main method fail.</param>
        Task PerformPlatformRateAppAsync(string packageName = null, string applicationId = null);

        /// <summary>
        /// Open rating system on your selected devices.
        /// </summary>  
        /// <param name="packageName">Leave it null for UWP only.</param>
        /// <param name="applicationId">Leave it null for UWP only.</param>
        /// <param name="productId">Product id of your UWP app. <strong>Required</strong> to alternative open the store if main method fail.</param>
        Task PerformPlatformRateAppAsync(string packageName = null, string applicationId = null, string productId = null);
    }
}