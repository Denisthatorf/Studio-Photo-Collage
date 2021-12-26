using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Studio_Photo_Collage.ViewModels
{
    public class StartPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        public RelayCommand ImageClickCommand { get; private set; }
        public StartPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            ImageClickCommand = new RelayCommand(NavigateCommandAction);
        }

        private void NavigateCommandAction()
        {
            _navigationService.NavigateTo("TemplatesPage");
        }
    }
}
