using System;
using Lumia.Imaging;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Windows.UI.Xaml.Media;

namespace Studio_Photo_Collage.Models
{
    public class ImageSourceAndEffect 
    {
        public ImageSource ImageSource { get; set; }
        public Type EffecteType { get; set; }
        public bool IsActive { get; set; }
        public ImageSourceAndEffect(ImageSource image, Type effecte, bool isActive = false)
        {
            ImageSource = image;
            EffecteType = effecte;
            IsActive = isActive;
        }

    }
}
