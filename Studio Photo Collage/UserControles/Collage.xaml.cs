using Studio_Photo_Collage.Infrastructure.Converters;
using Studio_Photo_Collage.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Studio_Photo_Collage.UserControles
{
    public sealed partial class Collage : UserControl
    {


        public Project Project
        {
            get { return (Project)GetValue(ProjectProperty); }
            set { SetValue(ProjectProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Project.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProjectProperty =
            DependencyProperty.Register("Project", typeof(Project), typeof(Collage), new PropertyMetadata(null, Fill));


         private static void Fill(DependencyObject d, DependencyPropertyChangedEventArgs e)
         {
         }

        public Collage()
        {
            this.InitializeComponent();
        }

        private void Fill()
        {
            var converter = new FromArrToGridConverter();
            var grid = (UIElement)converter.ConvertFromProjectTo<Image>(new byte[,] { { 3 }, { 4 } });
            (grid as Grid).Background = new SolidColorBrush(Colors.Green);
            BitmapImage One = new BitmapImage(new Uri("ms-appx:///Assets/StartPageCentral.jpg"));
            ((grid as Grid).Children[0] as Image).Source = One;
            ((grid as Grid).Children[1] as Image).Source = One;
            Content = grid;
        }
    }
}
