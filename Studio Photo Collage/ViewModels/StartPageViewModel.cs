using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Studio_Photo_Collage.ViewModels
{
    public class StartPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        
        public RelayCommand ImageClickCommand { get; private set; }
        public RelayCommand RecentCollCloseCommand { get; private set; }
        public RelayCommand PopUpCloseBtnCommand { get; private set; }


        private bool _isRecentCollagesOpen;
        public bool IsRecentCollagesOpen { get => _isRecentCollagesOpen; set => Set(ref _isRecentCollagesOpen, value); }


        private bool _isConfirmPopUpOpen;
        public bool IsConfirmPopUpOpen { get => _isConfirmPopUpOpen; set => Set(ref _isConfirmPopUpOpen, value); }


        
        private Visibility _isGreetingTextVisible;
        public Visibility IsGreetingTextVisible { get => _isGreetingTextVisible; set => Set(ref _isGreetingTextVisible, value); }


        public StartPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            ImageClickCommand = new RelayCommand(() => _navigationService.NavigateTo("TemplatesPage")) ;
            RecentCollCloseCommand = new RelayCommand(() => IsConfirmPopUpOpen = !IsConfirmPopUpOpen);
            PopUpCloseBtnCommand = new RelayCommand(()=>IsConfirmPopUpOpen = false);

            Windows.UI.Xaml.Window.Current.CoreWindow.KeyDown += (sender, arg) =>
            {
                if (arg.VirtualKey == Windows.System.VirtualKey.Space ||
                  arg.VirtualKey == Windows.System.VirtualKey.Enter)
                {
                    IsRecentCollagesOpen = true;
                    IsGreetingTextVisible = Visibility.Collapsed;
                }
            };

        }
    }
}
