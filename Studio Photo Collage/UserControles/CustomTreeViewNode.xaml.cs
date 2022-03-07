using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class CustomTreeViewNode : UserControl
    {
        public UIElement NodeItem
        {
            get { return (UIElement)GetValue(NodeItemProperty); }
            set { SetValue(NodeItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NodeItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NodeItemProperty =
            DependencyProperty.Register("NodeItem", typeof(UIElement), typeof(CustomTreeViewNode), new PropertyMetadata(null));



        public UIElement NodeContent
        {
            get { return (UIElement)GetValue(NodeContentProperty); }
            set { SetValue(NodeContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NodeContent.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NodeContentProperty =
            DependencyProperty.Register("NodeContent", typeof(UIElement), typeof(CustomTreeViewNode), new PropertyMetadata(null));


        public CustomTreeViewNode()
        {
            this.InitializeComponent();
        }

        private void Node_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (NodeItem.Visibility == Visibility.Collapsed)
            {
                NodeItem.Visibility = Visibility.Visible;
                TreeViewSign.Visibility = Visibility.Collapsed;
            }
            else if (NodeItem.Visibility == Visibility.Visible)
            {
                NodeItem.Visibility = Visibility.Collapsed;
                TreeViewSign.Visibility = Visibility.Visible;
            }
        }
    }
}
