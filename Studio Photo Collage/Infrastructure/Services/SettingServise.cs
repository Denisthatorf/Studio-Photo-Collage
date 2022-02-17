using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Studio_Photo_Collage.Infrastructure.Converters;
using Studio_Photo_Collage.Infrastructure.Helpers;
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
        private INavigationService navigationService;
        private ElementTheme theme;
        private SolidColorBrush customBrush;
        private CultureInfo language;

        public ElementTheme Theme
        {
            get => theme;
            set
            {
                this.theme = value;
                SetRequestedThemeAsync();
                LoadToLocalSettings(nameof(Theme), value.ToString());
            }
        }
        public SolidColorBrush CustomBrush
        {
            get => customBrush;
            set
            {
                customBrush = value;
                SetApplicationCustomColor();
                LoadToLocalSettings(nameof(CustomBrush), CustomBrush.Color.ToString());
            }
        }
        public CultureInfo Language
        {
            get => language;
            set
            {
                language = value;
                SetApplicationLanguage();
            }
        }

        private static ElementTheme ApplicationThemeFromSettings
        {
            get
            {
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                localSettings.Values.TryGetValue("Theme", out object themeStr);

                var converter = new ThemeToStringConverter();
                var userTheme = Application.Current.RequestedTheme;
                var theme = converter.ConvertBack(themeStr, null, null, null);

                return (ElementTheme)(theme ?? userTheme);
            }
        }
        private static SolidColorBrush CustomBrushFromSettings
        {
            get
            {
                var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                localSettings.Values.TryGetValue("CustomBrush", out object brushStr);

                var userColor = (Color)Application.Current.Resources["SystemAccentColor"];
                var userBrush = new SolidColorBrush(userColor);

                var brush = BrushGenerator.GetBrushFromHexOrStrImgBase64((string)brushStr);
                return (SolidColorBrush)(brush ?? userBrush);
            }
        }
        private static CultureInfo LanguageFromSettings
        {
            get
            {
                var selectedLanguage = ApplicationLanguages.PrimaryLanguageOverride;
                var conv = new CultureInfoToFullStringNameConverter();
                var culture = (CultureInfo)conv.ConvertBack(selectedLanguage, null, null, null);
                return culture;
            }
        }

        public SettingServise(INavigationService navigationService)
        {
            this.navigationService = navigationService;
        }
        public void LoadStartSetting()
        {
            Theme = ApplicationThemeFromSettings;
            CustomBrush = CustomBrushFromSettings;
            Language = LanguageFromSettings;
        }

        private static void LoadToLocalSettings(string key, string data)
        {
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values.TryGetValue(key, out object brush);

            if (brush == null)
            {
                localSettings.Values.Add(key, data);
            }
            else
            {
                localSettings.Values[key] = data;
            }
        }

        private void SetApplicationLanguage()
        {
            Thread.CurrentThread.CurrentCulture = language;
            ApplicationLanguages.PrimaryLanguageOverride = language.ToString();
            ResourceContext.GetForCurrentView().Reset();
            ResourceContext.GetForViewIndependentUse().Reset();
            navigationService.Navigate(navigationService.CurrentPageType, "reload");
        }
        private void SetApplicationCustomColor()
        {
            var brush = (SolidColorBrush)App.Current.Resources["CustomBrush"];
            brush.Color = customBrush.Color;

            navigationService.Navigate(navigationService.CurrentPageType, DateTime.Now.Ticks);
        }
        private async void SetRequestedThemeAsync()
        {
            foreach (var view in CoreApplication.Views)
            {
                await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    if (Window.Current.Content is FrameworkElement frameworkElement)
                    {
                        frameworkElement.RequestedTheme = theme;
                    }
                });
            }
        }
    }
}
