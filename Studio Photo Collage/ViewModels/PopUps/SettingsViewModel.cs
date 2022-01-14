using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Toolkit.Uwp;
using Studio_Photo_Collage.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Resources.Core;
using Windows.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Studio_Photo_Collage.ViewModels.PopUps
{
    public class SettingsPageViewModel : ViewModelBase
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
        public async Task ChangeTheme(ElementTheme newtheme)
        {
            await ThemeSelectorService.SetThemeAsync(newtheme);
        }

        private string _versionDescription;

        public string VersionDescription
        {
            get { return _versionDescription; }

            set { Set(ref _versionDescription, value); }
        }

        public SettingsPageViewModel()
        {
            LanguageComBox_SelectedItm = CultureInfo.CurrentCulture;
            ThemeComBox_SelectedItem = ElementTheme.Default;
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
