using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Views;
using Studio_Photo_Collage.Infrastructure.Helpers;
using System.Collections.Generic;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Studio_Photo_Collage.ViewModels.SidePanels
{
    public class BackgroundPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        private ICommand _uploadBtnCommand;
        public ICommand UploadBtnCommand
        {
            get { return _uploadBtnCommand; }
            set { _uploadBtnCommand = value; }
        }

        private ICommand _colorBtnCommand;
        public ICommand ColorBtnCommand
        {
            get
            {
                if (_colorBtnCommand == null)
                    _colorBtnCommand = new RelayCommand<SolidColorBrush>((parametr) => { Messenger.Default.Send(parametr); });
                return _colorBtnCommand;
            }
        }

        private int _bordersThickness = 0;
        public int BordersThickness 
        { 
            get => _bordersThickness; 
            set { 
                Set(ref _bordersThickness, value);
                Messenger.Default.Send(new Thickness(value)); }
        }

        private int _borderOpacity = 1;
        public int BorderOpacity { get => _borderOpacity;
            set 
            {
                Set(ref _borderOpacity, value);
                Messenger.Default.Send(value / 100.0);
            }
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

