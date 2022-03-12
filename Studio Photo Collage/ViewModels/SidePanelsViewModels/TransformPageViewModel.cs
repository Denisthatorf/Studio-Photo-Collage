using System;
using System.Windows.Input;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using Studio_Photo_Collage.Infrastructure.Messages;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Studio_Photo_Collage.ViewModels.SidePanels
{
    public class TransformPageViewModel : ObservableRecipient
    {
        private ICommand rotateRightCommand;
        private ICommand rotateLeftCommand;
        private ICommand verticalFlipCommand;
        private ICommand horizontalFlipCommand;
        private ICommand zoomInCommand;
        private ICommand zoomOutCommand;

        public ICommand RotateRightCommand => rotateRightCommand
            ?? (rotateRightCommand = new RelayCommand(() =>
            {
                Action<Image> action = (parameter) =>
                {
                    var source = parameter.Source as WriteableBitmap;
                    parameter.Source = source.Rotate(90);
                };

                Messenger.Send(action);
            }));

        public ICommand RotateLeftCommand => rotateLeftCommand
            ?? (rotateLeftCommand = new RelayCommand(() =>
            {
                Action<Image> action = (parameter) =>
                {
                    var source = parameter.Source as WriteableBitmap;
                    parameter.Source = source.Rotate(270);
                };

                Messenger.Send(action);
            }));

        public ICommand HorizontalFlipCommand => horizontalFlipCommand
            ?? (horizontalFlipCommand = new RelayCommand(() =>
            {
                Action<Image> action = (parameter) =>
                {
                    var source = parameter.Source as WriteableBitmap;
                    parameter.Source = source.Flip(WriteableBitmapExtensions.FlipMode.Vertical);
                };
                Messenger.Send(action);
            }));

        public ICommand VerticalFlipCommand => verticalFlipCommand
            ?? (verticalFlipCommand = new RelayCommand(() =>
            {
                Action<Image> action = (parameter) =>
                {
                    var source = parameter.Source as WriteableBitmap;
                    parameter.Source = source.Flip(WriteableBitmapExtensions.FlipMode.Horizontal);
                };
                Messenger.Send(action);
            }));

        public ICommand ZoomInCommand => zoomInCommand
            ?? (zoomInCommand = new RelayCommand(() =>
            {
                Messenger.Send(new ZoomMessage(ZoomType.ZoomIn));
            }));

        public ICommand ZoomOut => zoomOutCommand
            ?? (zoomOutCommand = new RelayCommand(() =>
            {
                Messenger.Send(new ZoomMessage(ZoomType.ZoomOut));
            }));


        public TransformPageViewModel()
        {

        }
    }
}
