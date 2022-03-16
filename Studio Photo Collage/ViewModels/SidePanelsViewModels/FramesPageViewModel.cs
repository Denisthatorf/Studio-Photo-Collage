using System.Collections.Generic;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Windows.UI.Xaml.Media;

namespace Studio_Photo_Collage.ViewModels.SidePanelsViewModels
{
    public class FramesPageViewModel : ObservableObject
    {
        public FramesPageViewModel()
        {
            Colors = ColorGenerator.GenerateBrushes();
        }

        public List<SolidColorBrush> Colors { get; }
    }
}
