using Studio_Photo_Collage.ViewModels;
using System;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Studio_Photo_Collage.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public Frame SidePanelFrame
        {
            get => SidePanel;
        }

        public MainPage()
        {
            this.InitializeComponent();
            /*
            InkDrawingAttributes inkDrawingAttributes = new InkDrawingAttributes();
            inkDrawingAttributes.Color = Windows.UI.Colors.Blue;
            InkCanv.InkPresenter.UpdateDefaultDrawingAttributes(inkDrawingAttributes);
            InkCanv.InkPresenter.InputDeviceTypes = Windows.UI.Core.CoreInputDeviceTypes.Mouse | Windows.UI.Core.CoreInputDeviceTypes.Pen;
            */
        }

        private void Save_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }
    }
}
