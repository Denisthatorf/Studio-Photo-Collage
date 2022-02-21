using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Studio_Photo_Collage.Infrastructure.Converters;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Studio_Photo_Collage.ViewModels;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Resources.Core;
using Windows.Globalization;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Studio_Photo_Collage.Infrastructure.Services
{
    public class SettingServise
    {
        private const string ThemeSettingsKey = "AppBackgroundRequestedTheme";
        private const string CustomColorSettingsKey = "CustomColor";

        public ElementTheme Theme { get; set; }
        public Color CustomBrush { get; set; }
        public CultureInfo Language { get; set; }

        public SettingServise() { }
        public async void LoadStartSetting()
        {
            await SetApplicationThemeFromSettings();
            await SetCustomColorFromSettings();
            SetLanguageFromSettings();
        }
        public void SetLanguage(CultureInfo cultureInfo)
        {
            Language = cultureInfo;
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            ApplicationLanguages.PrimaryLanguageOverride = cultureInfo.ToString();
            ResourceContext.GetForCurrentView().Reset();
            ResourceContext.GetForViewIndependentUse().Reset();

            ViewModelLocator.ReloadCurrentPage();
        }
        public async Task SetRequestedThemeAsync(ElementTheme theme)
        {
            Theme = theme;
            await ApplicationData.Current.LocalSettings.SaveAsync<ElementTheme>(ThemeSettingsKey, theme);
            await SetApplicationRequestedThemeAsync();
        }
        public async Task SetCutomColorAsync(Color color)
        {
            CustomBrush = color;
            SetApplicationCustomColor();
            await ApplicationData.Current.LocalSettings.SaveAsync<Color>(CustomColorSettingsKey, color);
        }

        private void SetApplicationCustomColor()
        {
            var brush = (SolidColorBrush)App.Current.Resources["CustomBrush"];
            brush.Color = CustomBrush;

            ViewModelLocator.ReloadCurrentPage();
        }
        private async Task SetApplicationRequestedThemeAsync()
        {
            foreach (var view in CoreApplication.Views)
            {
                await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    if (Window.Current.Content is FrameworkElement frameworkElement)
                    {
                        frameworkElement.RequestedTheme = Theme;
                    }
                });
            }
        }

        private async Task SetCustomColorFromSettings()
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            var color = await localSettings.ReadAsync<Color>(CustomColorSettingsKey);

            var userColor = (Color)Application.Current.Resources["SystemAccentColor"];

            CustomBrush = color == default ? userColor : color;
            await SetCutomColorAsync(CustomBrush);
        }
        private async Task SetApplicationThemeFromSettings()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            var theme = await localSettings.ReadAsync<ElementTheme>(ThemeSettingsKey);

            Theme = theme;
            await SetRequestedThemeAsync(Theme);
        }
        private void SetLanguageFromSettings()
        {
            var selectedLanguage = ApplicationLanguages.PrimaryLanguageOverride;
            var conv = new CultureInfoToFullStringNameConverter();
            var culture = (CultureInfo)conv.ConvertBack(selectedLanguage, null, null, null);

            Language = culture;
            SetLanguage(Language);
        }
    }
}
