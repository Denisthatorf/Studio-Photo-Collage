using Microsoft.Toolkit.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Studio_Photo_Collage.Views.PopUps
{
    public sealed partial class ConfirmDialog : ContentDialog
    {

        public ConfirmDialog(string allOrThis)
        {
            this.InitializeComponent();

            var frame = Window.Current.Content as Frame; 
            this.RequestedTheme = frame.ActualTheme;

            AllOrThis.Text = allOrThis;
            if (allOrThis == "this")
            {
                CollageOrcollageS.Text = "collage?";
            }
            else
            {
                CollageOrcollageS.Text = "collages?";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
    }
}
