using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Studio_Photo_Collage.ViewModels.SidePanels;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Studio_Photo_Collage.Views.SidePanelsViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TransformPage : Page
    {
        private TransformPageViewModel VM => Ioc.Default.GetRequiredService<TransformPageViewModel>();
        public TransformPage()
        {
            this.InitializeComponent();
        }
    }
}
