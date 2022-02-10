using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Studio_Photo_Collage.Infrastructure;
using Studio_Photo_Collage.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel;
using Windows.Globalization;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Studio_Photo_Collage.ViewModels.PopUps
{
    public class SettingsDialogViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        private CultureInfo _languageComBox_SelectedItm;
        public CultureInfo LanguageComBox_SelectedItm
        {
            get { return _languageComBox_SelectedItm; }
            set
            {
                Set(ref _languageComBox_SelectedItm, value);
                Thread.CurrentThread.CurrentCulture = value;
                ApplicationLanguages.PrimaryLanguageOverride = value?.ToString();
                //ResourceContext.GetForCurrentView().Reset();
                //ResourceContext.GetForViewIndependentUse().Reset();
                ViewModelLocator.ReloadCurrentPage();
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

        private ICommand _changeMainColorCommand;
        public ICommand ChangeMainColorCommand
        {
            get
            {
                if (_changeMainColorCommand == null)
                    _changeMainColorCommand = new RelayCommand<Color>((parametr) =>
                    {
                        (App.Current.Resources["MainBorderBrush"] as SolidColorBrush).Color = parametr;
                        _navigationService.NavigateTo(_navigationService.CurrentPageKey.ToString(), DateTime.Now.Ticks);
                    });
                return _changeMainColorCommand;
            }
        }

        private string _versionDescription;
        public string VersionDescription
        {
            get { return _versionDescription; }

            set { Set(ref _versionDescription, value); }
        }

        public List<Brush> Colors { get; }

        private ElementTheme _themeOfSettings;
        public ElementTheme ThemeOfSettings
        {
            get => _themeOfSettings;
            set => Set(ref _themeOfSettings, value);
        }



        public SettingsDialogViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            var frame = Window.Current.Content as Frame;

            _languageComBox_SelectedItm = GetStartLanguage();
            _themeComBox_SelectedItem = frame.RequestedTheme;
            VersionDescription = GetVersionDescription();

            Colors = new List<Brush>()
            {
            BrushGenerator.GetBrushFromHexOrStrImgBase64("FFBA00"),
            BrushGenerator.GetBrushFromHexOrStrImgBase64("F76304"),
            BrushGenerator.GetBrushFromHexOrStrImgBase64("DB3800"),
            BrushGenerator.GetBrushFromHexOrStrImgBase64("D23135"),
            BrushGenerator.GetBrushFromHexOrStrImgBase64("E9091E"),
            BrushGenerator.GetBrushFromHexOrStrImgBase64("C40051"),
            BrushGenerator.GetBrushFromHexOrStrImgBase64("E4008D"),
            BrushGenerator.GetBrushFromHexOrStrImgBase64("C336B5"),
            BrushGenerator.GetBrushFromHexOrStrImgBase64("891099"),
            BrushGenerator.GetBrushFromHexOrStrImgBase64("754CAB"),
            BrushGenerator.GetBrushFromHexOrStrImgBase64("8F8DD9"),
            BrushGenerator.GetBrushFromHexOrStrImgBase64("6B69D7"),
            BrushGenerator.GetBrushFromHexOrStrImgBase64("0063B3"),
            BrushGenerator.GetBrushFromHexOrStrImgBase64("0079D8"),
            BrushGenerator.GetBrushFromHexOrStrImgBase64("009ABD"),
            BrushGenerator.GetBrushFromHexOrStrImgBase64("00B8C4"),
            BrushGenerator.GetBrushFromHexOrStrImgBase64("00B395"),
            BrushGenerator.GetBrushFromHexOrStrImgBase64("008675"),
            BrushGenerator.GetBrushFromHexOrStrImgBase64("078A3C"),
            BrushGenerator.GetBrushFromHexOrStrImgBase64("505C6B"),
            BrushGenerator.GetBrushFromHexOrStrImgBase64("7F745F")
            };
        }

        public async Task ChangeTheme(ElementTheme newtheme)
        {
            await ThemeSelectorService.SetThemeAsync(newtheme);
            ThemeOfSettings = newtheme;
        }

        private CultureInfo GetStartLanguage()
        {
            var selectedLanguage = ApplicationLanguages.PrimaryLanguageOverride;
            var conv = new CultureInfoToFullStringNameConverter();
            var culture = (CultureInfo)conv.ConvertBack(selectedLanguage, null, null, null);
            return culture;
        }
        /*  public async Task InitializeAsync()
          {
                   VersionDescription = GetVersionDescription();
                   await Task.CompletedTask;
          }*/

        private string GetVersionDescription()
        {
            //var appName = "AppDisplayName".GetLocalized();
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"Version {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
    }
}
