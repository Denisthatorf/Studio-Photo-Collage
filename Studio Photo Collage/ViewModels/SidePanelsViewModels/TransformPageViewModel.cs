using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Messaging;

namespace Studio_Photo_Collage.ViewModels.SidePanels
{
    public class TransformPageViewModel : ObservableRecipient
    {
        public TransformPageViewModel()
        {

        }
        public void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var panel = e.ClickedItem as StackPanel;
            var str = panel.Name;
            Action<Image> action = (parameter) => { };

            switch (str)
            {
                case "RotateRight":
                    action = (parameter) =>
                    {
                        var source = parameter.Source as WriteableBitmap;
                        parameter.Source = source.Rotate(90);
                    };
                    break;
                case "RotateLeft":
                    action = (parameter) =>
                    {
                        var source = parameter.Source as WriteableBitmap;
                        parameter.Source = source.Rotate(270);
                    };
                    break;
                case "VerticalFlip":
                    action = (parameter) =>
                    {
                        var source = parameter.Source as WriteableBitmap;
                        parameter.Source = source.Flip(WriteableBitmapExtensions.FlipMode.Horizontal); 
                    };
                    break;
                case "HorizontalFlip":
                    action = (parameter) =>
                    {
                        var source = parameter.Source as WriteableBitmap;
                        parameter.Source = source.Flip(WriteableBitmapExtensions.FlipMode.Vertical); 
                    };
                    break;
                case "ZoomIn":
                    throw new NotImplementedException();
                case "ZoomOut":
                    throw new NotImplementedException();
            }

            Messenger.Send<Action<Image>>
                (new Action<Image>(action));
        }
    }
}
