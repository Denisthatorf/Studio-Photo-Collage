using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Studio_Photo_Collage.Infrastructure.Helpers;
using System.Collections.Generic;
using Windows.UI.Xaml.Media;

namespace Studio_Photo_Collage.ViewModels.SidePanels
{
    public class BackgroundPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        private RelayCommand _uploadBtnCommand;
        public RelayCommand UploadBtnCommand
        {
            get { return _uploadBtnCommand; }
            set { _uploadBtnCommand = value; }
        }

        public List<SolidColorBrush> Colors { get; set; }

        public BackgroundPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            _uploadBtnCommand = new RelayCommand(() => { });
            Colors = ColorGenerator.GenerateColors();
        }

    }
}

