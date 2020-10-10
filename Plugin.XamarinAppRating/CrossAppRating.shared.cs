using System;

namespace Plugin.XamarinAppRating
{
    /// <summary>
    /// CrossAppRating
    /// </summary>
    public static class CrossAppRating
    {
        static Lazy<IAppRating> implementation = new Lazy<IAppRating>(() => CreateAppRating(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Gets if the plugin is supported on the current platform.
        /// </summary>
        public static bool IsSupported => implementation.Value == null ? false : true;

        /// <summary>
        /// Current plugin implementation to use.
        /// </summary>
        public static IAppRating Current
        {
            get
            {
                IAppRating ret = implementation.Value;

                if (ret == null)
                    throw NotImplementedInReferenceAssembly();

                return ret;
            }
        }

        static IAppRating CreateAppRating()
        {
#if NETSTANDARD1_0 || NETSTANDARD2_0
            return null;
#else
            return new AppRatingImplementation();
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly() =>
            new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
    }
}