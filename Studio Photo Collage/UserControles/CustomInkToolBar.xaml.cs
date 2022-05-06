using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.Toolkit.Uwp.Helpers;
using Studio_Photo_Collage.Infrastructure.Events;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ColorHelper = Microsoft.Toolkit.Uwp.Helpers.ColorHelper;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Studio_Photo_Collage.UserControles
{
    public sealed partial class CustomInkToolBar : UserControl
    {
        public event EventHandler<InkDrawningAttributesChangeEventArgs> InkDrawningAttributesChangedEvent;
        public event EventHandler<InkInputProcessingModeChangedEventArgs> ModeChangedEvent;

        public CustomInkToolBar()
        {
            this.InitializeComponent();
            Pen.IsChecked = true;
        }
        private void ColorSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            var inkDrawingAttributes = new InkDrawingAttributes();

            if (e.NewValue == 0)
            {
                inkDrawingAttributes.Color = Colors.Black;
            }
            else if (e.NewValue == 100)
            {
                inkDrawingAttributes.Color = Colors.White;
            }
            else
            {
                inkDrawingAttributes.Color = ColorHelper.FromHsl((100 - e.NewValue) * 3.59, 1, 0.5);
            }

            inkDrawingAttributes.Size = new Windows.Foundation.Size(StrokeSizeSlider.Value, StrokeSizeSlider.Value);
            InkDrawningAttributesChangedEvent?.Invoke(this, new InkDrawningAttributesChangeEventArgs(inkDrawingAttributes));
        }

        private void StrokeSizeSlider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            var inkDrawingAttributes = new InkDrawingAttributes();
            inkDrawingAttributes.Color = ColorHelper.FromHsl(ColorSlider.Value * 3.59, 1, 0.5);
            inkDrawingAttributes.Size = new Windows.Foundation.Size(e.NewValue, e.NewValue);
            InkDrawningAttributesChangedEvent?.Invoke(this, new InkDrawningAttributesChangeEventArgs(inkDrawingAttributes));
        }

        private void RubberAppBarToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            Pen.IsChecked = false;
            ModeChangedEvent?.Invoke(this, new InkInputProcessingModeChangedEventArgs(InkInputProcessingMode.Erasing));
        }

        private void PenAppBarToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            Rubber.IsChecked = false;
            ModeChangedEvent?.Invoke(this, new InkInputProcessingModeChangedEventArgs(InkInputProcessingMode.Inking));
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            ColorSlider.ValueChanged += ColorSlider_ValueChanged;
            StrokeSizeSlider.ValueChanged += StrokeSizeSlider_ValueChanged;
        }
    }
}
