using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class BtnWithBluring : UserControl
    {


        public Grid ContentOfBtn
        {
            get {
                return (Grid)GetValue(ContentOfBtnProperty); 
            }
            set 
            { 
                SetValue(ContentOfBtnProperty, value); 
                ContentGrid.Children.Add(ContentOfBtn as Windows.UI.Xaml.UIElement); 
            }
        }

        // Using a DependencyProperty as the backing store for ContentOfBtn.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentOfBtnProperty =
            DependencyProperty.Register("ContentOfBtn", typeof(Grid), typeof(BtnWithBluring), new PropertyMetadata(0));



        public ICommand MyCommand
        {
            get { return (ICommand)GetValue(MyCommandProperty); }
            set { SetValue(MyCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyCommandProperty =
            DependencyProperty.Register("MyCommand", typeof(ICommand), typeof(BtnWithBluring), new PropertyMetadata(0));



        public string ProjectName
        {
            get { return (string)GetValue(ProjectNameProperty); }
            set { SetValue(ProjectNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProjectName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProjectNameProperty =
            DependencyProperty.Register("ProjectName", typeof(string), typeof(BtnWithBluring), new PropertyMetadata(0));



        public string Date
        {
            get { return (string)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Date.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DateProperty =
            DependencyProperty.Register("Date", typeof(string), typeof(BtnWithBluring), new PropertyMetadata(0));




        public BtnWithBluring()
        {
            this.InitializeComponent();
        }
    }
}
