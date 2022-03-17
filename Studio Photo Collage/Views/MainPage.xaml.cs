using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.Graphics.Canvas;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Uwp.Helpers;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Studio_Photo_Collage.ViewModels;
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
    public sealed partial class MainPage : Page
    {
        MainPageViewModel VM => Ioc.Default.GetService<MainPageViewModel>();

        public Frame RootFrame => SidePanel;
        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = VM;

            Window.Current.SetTitleBar(MainTitleBar);

            InkToolbar.InkDrawningAttributesChangedEvent += (o, e) => VM.ChangeInkCanvasAttributes(e.Attributes);
            InkToolbar.ModeChangedEvent += (o, e) => VM.ChangeInkCanvasMode(e.Mode);
            Paint.Checked += (o, e) => VM.ActivateInkCanvas();
            Paint.Unchecked += (o, e) => VM.DeactivateInkCanvas();
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
