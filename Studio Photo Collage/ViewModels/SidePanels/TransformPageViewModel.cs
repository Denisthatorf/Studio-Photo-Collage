using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Studio_Photo_Collage.ViewModels.SidePanels
{
    public class TransformPageViewModel : ViewModelBase
    {
        public TransformPageViewModel()
        {

        }
        public void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var panel = e.ClickedItem as StackPanel;
            var str = panel.Name;
            Action<Image> action = (parameter)=> { };

            switch (str)
            {
                case "RotateRight":
                    action = (parameter) => {
                        var source = parameter.Source as WriteableBitmap;
                        parameter.Source = source.Rotate(90);
                    };
                    break;
                case "RotateLeft":
                    action = (parameter) => {
                        var source = parameter.Source as WriteableBitmap;
                        parameter.Source = source.Rotate(270);
                    };
                    break;
                case "VerticalFlip":
                    action = (parameter) => {
                        var source = parameter.Source as WriteableBitmap;
                        parameter.Source = source.Flip(WriteableBitmapExtensions.FlipMode.Horizontal); // it is correct
                    };
                    break;
                case "HorizontalFlip":
                    action = (parameter) => {
                        var source = parameter.Source as WriteableBitmap;
                        parameter.Source = source.Flip(WriteableBitmapExtensions.FlipMode.Vertical); // It is correct
                    };
                    break;
                case "ZoomIn":
                    throw new NotImplementedException();
                    break;
                case "ZoomOut":
                    throw new NotImplementedException();
                    break;
            }

            MessengerInstance.Send<NotificationMessageAction<Image>>
                     (new NotificationMessageAction<Image>("str", action));
        }
    }
}
