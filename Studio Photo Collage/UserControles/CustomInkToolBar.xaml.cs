using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Studio_Photo_Collage.UserControles
{
    public sealed partial class CustomInkToolBar : UserControl
    {


        public InkCanvas InkCanv
        {
            get { return (InkCanvas)GetValue(InkCanvProperty); }
            set { SetValue(InkCanvProperty, value); }
        }

        public static readonly DependencyProperty InkCanvProperty =
            DependencyProperty.Register("InkCanv", typeof(InkCanvas), typeof(CustomInkToolBar), new PropertyMetadata(null, OnCanvasChanged));

        private static void OnCanvasChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var inkDrawingAttributes = new InkDrawingAttributes();
            inkDrawingAttributes.Color = ColorHelper.FromHsl(0, 1, 0.5);
            inkDrawingAttributes.Size = new Windows.Foundation.Size(2, 2);
            (e.NewValue as InkCanvas).InkPresenter.UpdateDefaultDrawingAttributes(inkDrawingAttributes);

            (e.NewValue as InkCanvas).InkPresenter.InputDeviceTypes
               = Windows.UI.Core.CoreInputDeviceTypes.Mouse | Windows.UI.Core.CoreInputDeviceTypes.Pen;
        }

        public CustomInkToolBar()
        {
            this.InitializeComponent();
        }
        private void ColorSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            var inkDrawingAttributes = new InkDrawingAttributes();
            inkDrawingAttributes.Color = ColorHelper.FromHsl(e.NewValue * 3.59, 1, 0.5);
            inkDrawingAttributes.Size = new Windows.Foundation.Size(StrokeSizeSlider.Value, StrokeSizeSlider.Value);
            InkCanv.InkPresenter.UpdateDefaultDrawingAttributes(inkDrawingAttributes);
        }

        private void StrokeSizeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            var inkDrawingAttributes = new InkDrawingAttributes();
            inkDrawingAttributes.Color = ColorHelper.FromHsl(ColorSlider.Value * 3.59, 1, 0.5);
            inkDrawingAttributes.Size = new Windows.Foundation.Size(e.NewValue, e.NewValue);
            InkCanv.InkPresenter.UpdateDefaultDrawingAttributes(inkDrawingAttributes);
        }

        private void RubberAppBarToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            Pen.IsChecked = false;
        }

        private void PenAppBarToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            Rubber.IsChecked = false;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ColorSlider.ValueChanged += ColorSlider_ValueChanged;
            StrokeSizeSlider.ValueChanged += StrokeSizeSlider_ValueChanged;
        }
    }
}
