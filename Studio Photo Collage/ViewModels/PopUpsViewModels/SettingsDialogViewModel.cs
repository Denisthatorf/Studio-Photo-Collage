using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Studio_Photo_Collage.Infrastructure.Converters;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Windows.ApplicationModel;
using Windows.Globalization;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Studio_Photo_Collage.ViewModels.PopUpsViewModels
{
    public class SettingsDialogViewModel : ObservableObject
    {
        private CultureInfo _languageComBox_SelectedItm;
        private ElementTheme _themeComBox_SelectedItem;
        private ElementTheme _themeOfSettings;
        private ICommand _changeMainColorCommand;
        private string _versionDescription;

        public CultureInfo LanguageComBox_SelectedItm
        {
            get { return _languageComBox_SelectedItm; }
            set
            {
                SetProperty(ref _languageComBox_SelectedItm, value);
                Thread.CurrentThread.CurrentCulture = value;
                ApplicationLanguages.PrimaryLanguageOverride = value?.ToString();
                //ResourceContext.GetForCurrentView().Reset();
                //ResourceContext.GetForViewIndependentUse().Reset();
                //ViewModelLocator.ReloadCurrentPage();
            }
        }
        public ElementTheme ThemeComBox_SelectedItem
        {
            get { return _themeComBox_SelectedItem; }
            set
            {
                SetProperty(ref _themeComBox_SelectedItem, value);
                _ = ChangeTheme(_themeComBox_SelectedItem);
            }
        }

        
        public ICommand ChangeMainColorCommand
        {
            get
            {
                if (_changeMainColorCommand == null)
                {
                    _changeMainColorCommand = new RelayCommand<Color>((parametr) =>
                    {
                        (App.Current.Resources["MainBorderBrush"] as SolidColorBrush).Color = parametr;
                        //_navigationService.NavigateTo(_navigationService.CurrentPageKey.ToString(), DateTime.Now.Ticks);
                    });
                }

                return _changeMainColorCommand;
            }
        }

     
        public string VersionDescription { get; set; }

        public List<Brush> Colors { get; }

        public ElementTheme ThemeOfSettings
        {
            get => _themeOfSettings;
            set => SetProperty(ref _themeOfSettings, value);
        }

        public SettingsDialogViewModel()
        {
            var frame = Window.Current.Content as Frame;
            Colors = new List<Brush>();
            FillByBrush();
            _languageComBox_SelectedItm = GetStartLanguage();
            _themeComBox_SelectedItem = frame.RequestedTheme;
            VersionDescription = GetVersionDescription();

        }

        public async Task ChangeTheme(ElementTheme newtheme)
        {
            await ThemeSelectorService.SetThemeAsync(newtheme);
            ThemeOfSettings = newtheme;
        }

        private static string GetVersionDescription()
        {
            //var appName = "AppDisplayName".GetLocalized();
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"Version {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
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

        private void FillByBrush()
        {
            Colors.Add(BrushHelper.GetBrushFromHexOrStrImgBase64("FFBA00"));
            Colors.Add(BrushHelper.GetBrushFromHexOrStrImgBase64("F76304"));
            Colors.Add(BrushHelper.GetBrushFromHexOrStrImgBase64("DB3800"));
            Colors.Add(BrushHelper.GetBrushFromHexOrStrImgBase64("D23135"));
            Colors.Add(BrushHelper.GetBrushFromHexOrStrImgBase64("E9091E"));
            Colors.Add(BrushHelper.GetBrushFromHexOrStrImgBase64("C40051"));
            Colors.Add(BrushHelper.GetBrushFromHexOrStrImgBase64("E4008D"));
            Colors.Add(BrushHelper.GetBrushFromHexOrStrImgBase64("C336B5"));
            Colors.Add(BrushHelper.GetBrushFromHexOrStrImgBase64("891099"));
            Colors.Add(BrushHelper.GetBrushFromHexOrStrImgBase64("754CAB"));
            Colors.Add(BrushHelper.GetBrushFromHexOrStrImgBase64("8F8DD9"));
            Colors.Add(BrushHelper.GetBrushFromHexOrStrImgBase64("6B69D7"));
            Colors.Add(BrushHelper.GetBrushFromHexOrStrImgBase64("0063B3"));
            Colors.Add(BrushHelper.GetBrushFromHexOrStrImgBase64("0079D8"));
            Colors.Add(BrushHelper.GetBrushFromHexOrStrImgBase64("009ABD"));
            Colors.Add(BrushHelper.GetBrushFromHexOrStrImgBase64("00B8C4"));
            Colors.Add(BrushHelper.GetBrushFromHexOrStrImgBase64("00B395"));
            Colors.Add(BrushHelper.GetBrushFromHexOrStrImgBase64("008675"));
            Colors.Add(BrushHelper.GetBrushFromHexOrStrImgBase64("078A3C"));
            Colors.Add(BrushHelper.GetBrushFromHexOrStrImgBase64("505C6B"));
            Colors.Add(BrushHelper.GetBrushFromHexOrStrImgBase64("7F745F"));

        }
    }
}
