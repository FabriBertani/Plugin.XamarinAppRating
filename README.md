# Plugin.XamarinAppRating
[![NuGet](https://img.shields.io/nuget/v/Plugin.XamarinAppRating.svg?label=NuGet)](https://www.nuget.org/packages/Plugin.XamarinAppRating/)

Plugin.XamarinAppRating gives developers a fast and easy way to ask users to rate the app on the stores.

## Installation
Plugin.XamarinAppRating is available via NuGet, grab the latest package and install in your solution:

    Install-Package Plugin.XamarinAppRating

### Platforms supported

|Platform|Version|
|-------------------|:------------------:|
|Xamarin.Android|API 25+|
|Xamarin.iOS|iOS 9.0+|
|UWP|Build 18362+|

## API Usage
Call `CrossAppRating.Current` from any project to gain access to APIs.

There is only one main method named `PerformPlatformRateAppAsync` but it has different implementations depending on platform.

### Android
```csharp
/// <summary>
/// Open app rating system for Android
/// </summary>
public Task PerformPlatformRateAppAsync(string packageName = null)
```
This method will open **_Google Play app_** on the store page of your current application. Otherwise it will try to open the store page on browser.

If neither main nor alternative methods work, it will display an alert announcing the error.

`packageName` property it's optional but you should provide it as an alternative to open the store in browser if main method fail.

### iOS
```csharp
/// <summary>
/// Open app rating system for iOS
/// </summary>
public Task PerformPlatformRateAppAsync(string packageName = null, string applicationId = null)
```
If the device current OS version is 10.3 or newer, this method will raise an in-app review popup of your current application. Otherwise for older iOS versions, it will open the store page on browser.

If neither main nor alternative methods work, it will display an alert announcing the error.

`packageName` property must be null for iOS only.

`applicationId` property is the **_StoreId_** of your iOS app, it's optional but you should provide it as an alternative to open the store in browser.

### UWP
```csharp
/// <summary>
/// Open app rating system for UWP
/// </summary>  
public async Task PerformPlatformRateAppAsync(string packageName = null, string applicationId = null, string productId = null)
```
`packageName` and `applicationId` properties must be null for UWP only.

This method will open **_Microsoft Store application_** with the page of your current app.

If method fail it will display an alert announcing the error.

`productId` is the **_ProductId_** of your UWP app. It's **_Required_**</strong> to open the store page.</param>


## Usage
**Warning** - You should be careful about **how and when** you ask users to rate your app, there may be penalties from stores. As advice I recommend to use a counter on the app start and storage that counter, then when the counter reachs certain number, display a dialog asking to the users if they want to rate the app, if they decline the offer, reset the counter to ask them later, also leave the option to do it themselves.

```csharp
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        var appAlreadyRated = Preferences.Get("application_rated");

        if (!appAlreadyRated)
          Task.Run(() => CheckAppCountAndRate());
    }

    private async Task CheckAppCountAndRate()
    {
        var applicationCount = Preferences.Get("application_counter");

        if (applicationCount >= 5)
        {
            if (!await DisplayAlert ("Rate this App!", "Are you enjoying the app so far? Would you like to leave a review in the store?", "Yes", "No");)
            {
                Preferences.Set("application_counter", 0);

                return;
            }

            // For UWP this must be invoked on main thread in order to open the Windows Store app.
            Device.BeginInvokeOnMainThread(async () =>
            {
                // This method use Facebook™ store apps as example.
                await CrossAppRating.Current.PerformPlatformRateAppAsync("com.facebook.katana", "id284882215", "9wzdncrf0083")
            });

            Preferences.Set("application_rated", true);
        }
    }
}
```

## Contributions
Please, feel free to open an [Issue](https://github.com/FabriBertani/Plugin.XamarinAppRating/issues) if you found any bugs or submit a PR.

## License
XamarinAppRating is licensed under [MIT](https://github.com/FabriBertani/Plugin.XamarinAppRating/blob/main/LICENSE).
