using CommonServiceLocator;
using Studio_Photo_Collage.ViewModels.SidePanels;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Studio_Photo_Collage.Views.SidePanels
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TransformPage : Page
    {
        private TransformPageViewModel ViewModel
        {
            get => ServiceLocator.Current.GetInstance<TransformPageViewModel>();
        }

        public TransformPage()
        {
            this.InitializeComponent();
        }
    }
}
