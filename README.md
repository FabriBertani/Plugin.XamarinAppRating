# Plugin.XamarinAppRating
[![NuGet](https://img.shields.io/nuget/v/Plugin.XamarinAppRating.svg?label=NuGet)](https://www.nuget.org/packages/Plugin.XamarinAppRating/)

Plugin.XamarinAppRating gives developers a fast and easy way to ask users to rate the app on the stores.

## Installation
Plugin.XamarinAppRating is available via NuGet. Grab the latest NuGet package and install in your solution:

    Install-Package Plugin.XamarinAppRating

### Platforms supported

|Platform|Version|
|-------------------|:------------------:|
|Xamarin.Android|API 25+|
|Xamarin.iOS|iOS 9.0+|
|UWP|Build 18362+|

## API Usage
Call **CrossAppRating.Current** from any of project to gain access to APIs.

There is only one main method named **PerformPlatformRateAppAsync** but it have different implementations depending on platform.

### Android
```csharp
/// <summary>
/// Open app rating system for Android
/// </summary>
public Task PerformPlatformRateAppAsync(string packageName = null)
```
This method will open _Google Play_ store page of your current application on _Google Play_ app. Otherwise it will try to open the store page on browser.

The **packageName** property it's optional but you should provide it as an alternative to open the store in browser if main method fail.

If both main and alternative methods fail it will display an alert announcing the error.

### iOS
```csharp
/// <summary>
/// Open app rating system for iOS
/// </summary>
public Task PerformPlatformRateAppAsync(string packageName = null, string applicationId = null)
```
The **packageName** property must be null for iOS only.

If device's current OS version is 10.3 or newer, this method would raise in-app review popup of your current application. Otherwise for older iOS versions, this method will open the store page on browser in order to users rate the app.

The **applicationId** is the StoreId of your iOS app. It's optional but you should provide it as an alternative to open the store in browser.

If both main and alternative methods fail it will display an alert announcing the error.

### UWP
```csharp
/// <summary>
/// Open app rating system for UWP
/// </summary>  
public async Task PerformPlatformRateAppAsync(string packageName = null, string applicationId = null, string productId = null)
```
The **packageName** and **applicationId** properties must be null for UWP only.

This method will open _Microsoft Store_ application with the page of your current app.

The **productId** is the ProductId of your UWP app. It's **_Required_**</strong> to open the store page.</param>

If method fail it will display an alert announcing the error.

## Usage
**Warning** - You should be careful about how and when you ask users to rate your app, there may be penalties from stores. As an advice, use a counter on the app start and storage that counter, when the counter reachs certain number it shows a dialog to the users asking them if they want to rate the app first, if they decline restart the counter to ask them later, also leave the option to do it themselves.

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

            // This method use Facebook™ store apps as example.
            await CrossAppRating.Current.PerformPlatformRateAppAsync("com.facebook.katana", "id284882215", "9wzdncrf0083")

            Preferences.Set("application_rated", true);
        }
    }
}
```

## Contributions
Please, feel free to open an [Issue](https://github.com/FabriBertani/Plugin.XamarinAppRating/issues) if you found any bugs or submit a PR.

## License
XamarinAppRating is licensed under [MIT](https://github.com/FabriBertani/Plugin.XamarinAppRating/blob/main/LICENSE).
