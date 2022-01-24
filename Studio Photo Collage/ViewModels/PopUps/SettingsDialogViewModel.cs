using GalaSoft.MvvmLight;
using Microsoft.Toolkit.Uwp;
using Studio_Photo_Collage.Infrastructure;
using Studio_Photo_Collage.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Resources.Core;
using Windows.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Studio_Photo_Collage.ViewModels.PopUps
{
    public class SettingsDialogViewModel : ViewModelBase
    {
        private CultureInfo _languageComBox_SelectedItm;
        public CultureInfo LanguageComBox_SelectedItm
        {
            get { return _languageComBox_SelectedItm; }
            set
            {
                Set(ref _languageComBox_SelectedItm, value);
                Thread.CurrentThread.CurrentCulture = value;
                ApplicationLanguages.PrimaryLanguageOverride = value.ToString();
                ResourceContext.GetForCurrentView().Reset();
                ResourceContext.GetForViewIndependentUse().Reset();
                _ = ViewModelLocator.ReloadCurrentPage();
            }
        }

        private ElementTheme _themeComBox_SelectedItem;
        public ElementTheme ThemeComBox_SelectedItem
        {
            get { return _themeComBox_SelectedItem; }
            set
            {
                Set(ref _themeComBox_SelectedItem, value);
                _ = ChangeTheme(_themeComBox_SelectedItem);
            }
        }

        private string _versionDescription;
        public string VersionDescription
        {
            get { return _versionDescription; }

            set { Set(ref _versionDescription, value); }
        }

        public List<SolidColorBrush> Colors {get; }

        private ElementTheme _themeOfSettings;
        public ElementTheme ThemeOfSettings { get => _themeOfSettings; set => Set(ref _themeOfSettings, value); }



        public SettingsDialogViewModel()
        {
            LanguageComBox_SelectedItm = CultureInfo.CurrentCulture;
            ThemeComBox_SelectedItem = ElementTheme.Default;

            Colors = new List<SolidColorBrush>()
            {
            ColorGenerator.GetSolidColorBrush("FFBA00"),
            ColorGenerator.GetSolidColorBrush("F76304"),
            ColorGenerator.GetSolidColorBrush("DB3800"),
            ColorGenerator.GetSolidColorBrush("D23135"),
            ColorGenerator.GetSolidColorBrush("E9091E"),
            ColorGenerator.GetSolidColorBrush("C40051"),
            ColorGenerator.GetSolidColorBrush("E4008D"),
            ColorGenerator.GetSolidColorBrush("C336B5"),
            ColorGenerator.GetSolidColorBrush("891099"),
            ColorGenerator.GetSolidColorBrush("754CAB"),
            ColorGenerator.GetSolidColorBrush("8F8DD9"),
            ColorGenerator.GetSolidColorBrush("6B69D7"),
            ColorGenerator.GetSolidColorBrush("0063B3"),
            ColorGenerator.GetSolidColorBrush("0079D8"),
            ColorGenerator.GetSolidColorBrush("009ABD"),
            ColorGenerator.GetSolidColorBrush("00B8C4"),
            ColorGenerator.GetSolidColorBrush("00B395"),
            ColorGenerator.GetSolidColorBrush("008675"),
            ColorGenerator.GetSolidColorBrush("078A3C"),
            ColorGenerator.GetSolidColorBrush("505C6B"),
            ColorGenerator.GetSolidColorBrush("7F745F")
            };
        }

        public async Task ChangeTheme(ElementTheme newtheme)
        {
            await ThemeSelectorService.SetThemeAsync(newtheme);
            ThemeOfSettings = newtheme;
        }


        /*      public async Task InitializeAsync()
              {
                  VersionDescription = GetVersionDescription();
                  await Task.CompletedTask;
              }*/

        private string GetVersionDescription()
        {
            var appName = "AppDisplayName".GetLocalized();
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
    }
}
