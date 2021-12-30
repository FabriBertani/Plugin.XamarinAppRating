# Changelog
## 1.2.1 (12/30/2021)
[Full Changelog](https://github.com/FabriBertani/Plugin.XamarinAppRating/compare/v1.2.0...v1.2.1)

**Implemented enhancements:**
- Updated libraries.

**Fixed bugs:**
- Fixed crash on Android when trying to open an error popup.

## 1.2.0 (04/28/2021)
[Full Changelog](https://github.com/FabriBertani/Plugin.XamarinAppRating/compare/v1.1.0...v1.2.0)

**Breaking changes:**
- `PerformPlatformRateAppAsync()` was fully removed.

**Implemented enhancements:**
- Added support for macOS and tvOS.
- Merged Android methods for different OS versions into a single method.
- Updated TestApp sample to Xamarin.Forms 5.0.0.2012
- Added macOS to Xamarin.Forms sample project.
- Added macOS to Xamarin native sample project.

## 1.1.0 (04/11/2020)

[Full Changelog](https://github.com/FabriBertani/Plugin.XamarinAppRating/compare/v1.0.1...v1.1.0)

**Breaking changes:**
- `PerformPlatformRateAppAsync()` method was marked as obsolete and it will be removed on further versions.
- Removed `netstandard` implementation.

**Implemented enhancements:**
- Added in-app rating for UWP [\#1](https://github.com/FabriBertani/Plugin.XamarinAppRating/issues/1) ([dpaulino](https://github.com/dpaulino))
- Added `PerformInAppRateAsync()` method to request rating within the application.
- Added `PerformRatingOnStoreAsync()` method to request rating opening the store app or store page on platform browser.

**Fixed bugs:**
- Fixed various issues and code smells.
