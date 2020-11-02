using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Plugin.XamarinAppRating;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace TestAppNative.Android
{
    [Activity(Label = "TestAppNative", Icon = "@mipmap/ic_launcher", Theme = "@style/AppTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : AppCompatActivity, View.IOnClickListener
    {
        private Button rateAppOnStoreBtn;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Platform.Init(this, savedInstanceState);
            
            SetContentView(Resource.Layout.activity_main);

            rateAppOnStoreBtn = FindViewById<Button>(Resource.Id.rate_on_store_btn);

            rateAppOnStoreBtn.SetOnClickListener(this);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void OnClick(View v)
        {
            if (v.Id == Resource.Id.rate_on_store_btn)
                MainThread.InvokeOnMainThreadAsync(async () => await RateAppOnStore());
        }

        private async Task RateAppOnStore()
        {
            // This method use Facebook™'s store apps as example.
            if (CrossAppRating.IsSupported)
                await CrossAppRating.Current.PerformRatingOnStoreAsync(packageName: "com.facebook.katana");
        }
    }
}