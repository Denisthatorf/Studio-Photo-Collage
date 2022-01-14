using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Studio_Photo_Collage.Views;
using Studio_Photo_Collage.Views.SidePanels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Studio_Photo_Collage.Views.PopUps;
using Studio_Photo_Collage.Views.PopUps.Settings;
using Windows.UI.Xaml.Controls;
using NavigationViewItem = Microsoft.UI.Xaml.Controls.NavigationViewItem;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml;
using System.Globalization;
using Windows.ApplicationModel.Core;
using GalaSoft.MvvmLight.Messaging;
using Windows.UI.Core;
using System.Windows.Input;
using Studio_Photo_Collage.Infrastructure.Helpers;

namespace Studio_Photo_Collage.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly INavigationService NavigationService;

        private ICommand _backBtnCommand;
        public ICommand BackBtncommand
        {
            get
            {
                if (_backBtnCommand == null)
                    _backBtnCommand = new RelayCommand(() => { NavigationService.NavigateTo("TemplatesPage"); });
                return _backBtnCommand;
            }
        }

        private BtnNameEnum _checkBoxesEnum;
        public BtnNameEnum CheckBoxesEnum
        {
            get { 
                return _checkBoxesEnum; }
            set { 
                if (_checkBoxesEnum != value) 
                Set(ref _checkBoxesEnum, value);

                var type = StringToTypeOfPageHelper.Convert(_checkBoxesEnum.ToString());
                if (type != null)
                {

                    SideFrame.Navigate(type);
                    RaisePropertyChanged("SideFrame");
                }
                else
                    SideFrame = new Frame();
            }
        }



        public Frame SettingsFrame { get; }
        public Frame PaintFrame { get; }

        private Frame _sidePanel;
        public Frame SideFrame { get => _sidePanel;
            set { 
                    Set(ref _sidePanel, value); 
            } }

        

        public MainPageViewModel(INavigationService _navigationService) 
        {
            SettingsFrame = new Frame();
            SettingsFrame.Navigate(typeof(SettingsPage));

            PaintFrame = new Frame();
            PaintFrame.Navigate(typeof(PaintPopUpPage));

            SideFrame = new Frame();

            NavigationService = _navigationService;
        }
    }
}
