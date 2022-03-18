using System;
using Studio_Photo_Collage.Models;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Studio_Photo_Collage.Infrastructure.Events
{
    public class SelectedImageChangedEventArg : EventArgs
    {
        public ImageSource ImageSource { get; set; }
        public ImageInfo ImageInfo { get; set; }

        public SelectedImageChangedEventArg(ImageSource imageSource, ImageInfo imageInfo)
        {
            ImageSource = imageSource;
            ImageInfo = imageInfo;
        }
    }
}
