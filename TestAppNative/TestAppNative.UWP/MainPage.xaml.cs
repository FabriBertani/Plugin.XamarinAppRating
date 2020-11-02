using Plugin.XamarinAppRating;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Xamarin.Essentials;

namespace TestAppNative.UWP
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void RateAppOnStore_Click(object sender, RoutedEventArgs e)
        {
            if (CrossAppRating.IsSupported)
                Task.Run(() => RateAppOnStore());
        }

        private async Task RateAppOnStore()
        {
            // This method use Facebook™'s store apps as example.
            await CrossAppRating.Current.PerformRatingOnStoreAsync(productId: "9wzdncrf0083");
        }
    }
}