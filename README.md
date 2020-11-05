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
|UWP|Build 17763+|

## Version 1.1.0
> :warning: `PerformPlatformRateAppAsync` method **was marked obsolte** on `v1.1.0` and it will be removed on further versions, please consider use new methods `PerformInAppRateAsync` or `PerformRatingOnStoreAsync`.

### New Features
- Added support to perform rating in-app by using new method `PerformInAppRateAsync()` (available on both native projects and Xamarin.Forms).
- Added new method to perform rating opening the platform store app or store page on platform browser, by using new method `PerformRatingOnStoreAsync()` with individual named arguments for each platform (available on both native projects and Xamarin.Forms).

## API Usage
Call `CrossAppRating.Current` from any project to gain access to APIs.

There are two main methods, `PerformInAppRateAsync` and `PerformRatingOnStoreAsync`.

### Android
```csharp
/// <summary>
/// Perform rating without leaving the app.
/// </summary>
public Task PerformInAppRateAsync();
```
This method will open in-app review dialog, using the `packageName` declared on the `AndroidManifest` file.

> :warning: This method it's only available for API 28 and above, if your target version it's lower it will display an alert announcing that it's not supported.

If your Target Android version is 27 or below please consider use the next method.

```csharp
/// <summary>
/// Perform rating on the current OS store app or open the store page on browser.
/// </summary>
public Task PerformRatingOnStoreAsync()
```
This method will open **_Google Play app_** on the store page of your current application. Otherwise it will try to open the store page on browser.

If neither store page nor browser store page works, it will display an alert announcing the error.

`packageName` **must** be provided as named argument to open the store page on store app or browser.

#### Example
```csharp
if (CrossAppRating.IsSupported)
    await CrossAppRating.Current.PerformRatingOnStoreAsync(packageName: "com.facebook.katana");
```

### iOS
```csharp
/// <summary>
/// Perform rating without leaving the app.
/// </summary>
public Task PerformInAppRateAsync();
```
If the device current OS version is 10.3 or newer, this method will raise an in-app review popup of your current application, otherwise it will display an alert announcing that it's not supported.

```csharp
/// <summary>
/// Perform rating on the current OS store app or open the store page on browser.
/// </summary>
public Task PerformRatingOnStoreAsync()
```
This method will open **App Store app** on the store page of your current application. Otherwise it will try to open the store page on browser.

If the method fails, it will display an alert announcing the error.

`applicationId` property is the **_StoreId_** of your iOS app and it **must** be provided as named argument to open the store page on store app or browser.

#### Example
```csharp
if (CrossAppRating.IsSupported)
    await CrossAppRating.Current.PerformRatingOnStoreAsync(applicationId: "id284882215");
```

### UWP
```csharp
/// <summary>
/// Perform rating without leaving the app.
/// </summary>
public Task PerformInAppRateAsync();
```
If the target version build is 17763 or above , this method will raise an in-app review dialog of your current application, otherwise it will display an alert announcing that it's not supported.

```csharp
/// <summary>
/// Perform rating on the current OS store app or open the store page on browser.
/// </summary>
public Task PerformRatingOnStoreAsync()
```
This method will open **_Microsoft Store application_** with the page of your current app.

If method fail it will display an alert announcing the error.

`productId` is the **_ProductId_** of your UWP app and it **must** be provided as named argument to open the store page app.

### Example
```csharp
if (CrossAppRating.IsSupported)
    await CrossAppRating.Current.PerformRatingOnStoreAsync(productId: "9wzdncrf0083");
```

## Usage
> :warning: **Warning** - You should be careful about **how and when** you ask users to rate your app, there may be penalties from stores. As advice I recommend to use a counter on the app start and storage that count, then when the counter reachs certain number, display a dialog asking to the users if they want to rate the app, if they decline the offer, reset the counter to ask them later, also leave the option to do it themselves.

```csharp
public partial class MainPage : ContentPage
{
    private const string androidPackageName = "com.facebook.katana";
    private const string iOSApplicationId = "id284882215";
    private const string uwpProductId = "9wzdncrf0083";

    public MainPage()
    {
        InitializeComponent();

        if (!Preferences.Get("application_rated", false))
            Task.Run(() => CheckAppCountAndRate());
    }

    private async Task CheckAppCountAndRate()
    {
        if (Preferences.Get("application_counter", 0) >= 5)
        {
            if (!await DisplayAlert("Rate this App!", "Are you enjoying the app so far? Would you like to leave a review in the store?", "Yes", "No"))
            {
                Preferences.Set("application_counter", 0);

                return;
            }

            await RateApplicationInApp();
        }
    }

    private Task RateApplicationInApp()
    {
        if (CrossAppRating.IsSupported)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                // This method will simulate Facebook™ app to in-app rating as example.
                await CrossAppRating.Current.PerformInAppRateAsync();
            });

            Preferences.Set("application_rated", true);
        }

        return Task.CompletedTask;
    }

    private Task RateApplicationOnStore()
    {
        if (CrossAppRating.IsSupported)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                // This method use Facebook™'s store apps as example.
                await CrossAppRating.Current.PerformRatingOnStoreAsync(packageName: androidPackageName, applicationId: iOSApplicationId, productId: uwpProductId);
            });

            Preferences.Set("application_rated", true);
        }

        return Task.CompletedTask;
    }

    private void InAppRating_Clicked(object sender, EventArgs e)
    {
        if (!Preferences.Get("application_rated", false))
            Task.Run(() => RateApplicationInApp());
    }

    private void AppRateOnStore_Clicked(object sender, EventArgs e)
    {
        if (!Preferences.Get("application_rated", false))
            Task.Run(() => RateApplicationOnStore());
    }
}
```

## Contributions
Please, feel free to open an [Issue](https://github.com/FabriBertani/Plugin.XamarinAppRating/issues) if you found any bugs or submit a PR.

## License
XamarinAppRating is licensed under [MIT](https://github.com/FabriBertani/Plugin.XamarinAppRating/blob/main/LICENSE).
