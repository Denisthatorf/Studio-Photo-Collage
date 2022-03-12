using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Studio_Photo_Collage.Infrastructure.Converters;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Studio_Photo_Collage.ViewModels;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.Resources.Core;
using Windows.Foundation;
using Windows.Globalization;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Studio_Photo_Collage.Infrastructure.Services
{
    public class SettingServise
    {
        private const string ThemeSettingsKey = "AppBackgroundRequestedTheme";
        private const string CustomColorSettingsKey = "CustomColor";
        private const string LocalFolderKey = "LocalFolder";

        private string localFolder;
        private Color customBrush;
        private ElementTheme theme;
        private CultureInfo language;

        public ElementTheme Theme { get => theme; }
        public Color CustomBrush { get => customBrush; }
        public CultureInfo Language { get => language; }
        public string LocalFolder { get => localFolder; }

        public SettingServise() { }
        public async void LoadStartSetting()
        {
            await SetApplicationThemeFromSettings();
            await SetCustomColorFromSettings();
            await SetLocalFolderFromSettings();
            SetLanguageFromSettings();
            SetAnotherSettings();
            SetTitleBarTheme();
        }
        public void SetLanguage(CultureInfo cultureInfo)
        {
            language = cultureInfo;
            Thread.CurrentThread.CurrentCulture = cultureInfo;
            ApplicationLanguages.PrimaryLanguageOverride = cultureInfo.ToString();
            ResourceContext.GetForCurrentView().Reset();
            ResourceContext.GetForViewIndependentUse().Reset();

            ViewModelLocator.ReloadCurrentPage();
        }
        public void SetLocalFolder()
        {
            _ = ApplicationData.Current.LocalSettings.SaveAsync<string>(LocalFolder, LocalFolder);
        }
        public async Task SetRequestedThemeAsync(ElementTheme theme)
        {
            this.theme = theme;
            await ApplicationData.Current.LocalSettings.SaveAsync<ElementTheme>(ThemeSettingsKey, theme);
            await SetApplicationRequestedThemeAsync();
            SetTitleBarTheme();
        }
        public async Task SetCutomColorAsync(Color color)
        {
            customBrush = color;
            SetApplicationCustomColor();
            await ApplicationData.Current.LocalSettings.SaveAsync<Color>(CustomColorSettingsKey, color);
        }

        private void SetTitleBarTheme()
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.ButtonBackgroundColor = Colors.Transparent;

            var theme = App.Current.RequestedTheme;
            switch (Theme)
            {
                case ElementTheme.Default:
                    titleBar.ButtonForegroundColor = theme == ApplicationTheme.Light ? Colors.Black : Colors.White;
                    break;
                case ElementTheme.Light:
                    titleBar.ButtonForegroundColor = Colors.Black;
                    break;
                case ElementTheme.Dark:
                    titleBar.ButtonForegroundColor = Colors.White;
                    titleBar.ForegroundColor = Colors.White;
                    break;
            }
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

            customBrush = color == default ? userColor : color;
            await SetCutomColorAsync(CustomBrush);
        }
        private async Task SetApplicationThemeFromSettings()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            var theme = await localSettings.ReadAsync<ElementTheme>(ThemeSettingsKey);

            this.theme = theme;
            await SetRequestedThemeAsync(Theme);
        }
        private async Task SetLocalFolderFromSettings()
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localFolder = await localSettings.ReadAsync<string>(LocalFolderKey);
        }
        private void SetLanguageFromSettings()
        {
            var selectedLanguage = ApplicationLanguages.PrimaryLanguageOverride;
            var conv = new CultureInfoToFullStringNameConverter();
            var culture = (CultureInfo)conv.ConvertBack(selectedLanguage, null, null, null);

            language = culture;
            SetLanguage(Language);
        }
        private void SetAnotherSettings()
        {
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;

            var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
            coreTitleBar.ExtendViewIntoTitleBar = true;

            Application.Current.Resources["ContentDialogBorderWidth"] = new Thickness(2);
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(500, 500));
        }
    }
}
