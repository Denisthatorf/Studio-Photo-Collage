using System.Collections.Generic;
using Windows.UI.Xaml.Media;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Studio_Photo_Collage.Infrastructure.Helpers;
namespace Studio_Photo_Collage.ViewModels.SidePanels
{
    public class FramesPageViewModel : ObservableObject
    {
        public FramesPageViewModel()
        {
            Colors = BrushHelper.GenerateBrushes();
        }

        public List<SolidColorBrush> Colors { get; set; }
    }
}
