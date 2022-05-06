using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Studio_Photo_Collage.Models;
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
    public sealed partial class CustomTreeView : UserControl
    {
        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable),
                typeof(CustomTreeView), new PropertyMetadata(DependencyProperty.UnsetValue));

        public ICommand CommandsForBtns
        {
            get { return (ICommand)GetValue(CommandsForBtnsProperty); }
            set { SetValue(CommandsForBtnsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandsForBtns.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandsForBtnsProperty =
            DependencyProperty.Register("CommandsForBtns", typeof(ICommand), typeof(CustomTreeView), new PropertyMetadata(null));
        private DataTemplate nodeItemsDataTemplate;

        public DataTemplate NodeItemsDataTemplate
        {
            get => nodeItemsDataTemplate;
            set 
            {
                nodeItemsDataTemplate = value;
                
            }
        }
        /* public DataTemplate NodeItemsDataTemplate
         {
             get { return (DataTemplate)GetValue(NodeItemsDataTemplateProperty); }
             set { SetValue(NodeItemsDataTemplateProperty, value); }
         }

         // Using a DependencyProperty as the backing store for NodeItemsDataTemplate.  This enables animation, styling, binding, etc...
         public static readonly DependencyProperty NodeItemsDataTemplateProperty =
             DependencyProperty.Register("NodeItemsDataTemplate", typeof(DataTemplate), typeof(CustomTreeView), new PropertyMetadata(null));*/

        public CustomTreeView()
        {
            this.InitializeComponent();
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            CommandsForBtns.Execute((sender as Button).CommandParameter);
        }
    }
}
