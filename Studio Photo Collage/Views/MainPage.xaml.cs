using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.Graphics.Canvas;
using Microsoft.Toolkit.Uwp.Helpers;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Studio_Photo_Collage.Views.PopUps;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using Windows.Graphics.Printing;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Printing;
using ColorHelper = Microsoft.Toolkit.Uwp.Helpers.ColorHelper;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Studio_Photo_Collage.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public Frame RootFrame => SidePanel;
        public MainPage()
        {
            this.InitializeComponent();
            Window.Current.SetTitleBar(MainTitleBar);
            // PaintFrame.Navigate(typeof(PaintPopUpPage)); 
        }

        private void Save_PointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SidePanel.Height = e.NewSize.Height;
        }
    }
}
