# Changelog
## 1.1.0 (11/04/2020)
**Breaking changes:**
- `PerformPlatformRateAppAsync()` method was marked as obsolete and it will be removed on further versions.
- Removed `netstandard` implementation.

**Implemented enhancements:**
- Added in-app rating for UWP [\#1](https://github.com/FabriBertani/Plugin.XamarinAppRating/issues/1) ([dpaulino](https://github.com/dpaulino))
- Added `PerformInAppRateAsync()` method to request rating within the application.
- Added `PerformRatingOnStoreAsync()` method to request rating opening the store app or store page on platform browser.

**Fixed bugs:**
- Fixed various issues and code smells.