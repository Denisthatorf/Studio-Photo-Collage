using GalaSoft.MvvmLight;
using Studio_Photo_Collage.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace Studio_Photo_Collage.ViewModels.SidePanels
{
    public class FramesPageViewModel : ViewModelBase
    {
        public FramesPageViewModel()
        {
            Colors = BrushGenerator.GenerateBrushes();
        }

        public List<SolidColorBrush> Colors { get; set; }
    }
}
