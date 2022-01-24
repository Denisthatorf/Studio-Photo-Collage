using Microsoft.Toolkit.Uwp;
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
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Studio_Photo_Collage.Views.PopUps
{
    public sealed partial class ConfirmDialog : ContentDialog
    {
        private string _allOrThis;
        public string AllOrThis
        {
            get { return _allOrThis; }
            set 
            { 
                _allOrThis = value.GetLocalized();
            }
        }

        public ConfirmDialog()
        {
            this.InitializeComponent();

            var solcolor = (SolidColorBrush)Application.Current.Resources["PopUpsBackground"];
            this.Background = new SolidColorBrush(solcolor.Color);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
