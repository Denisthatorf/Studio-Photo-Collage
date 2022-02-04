using GalaSoft.MvvmLight.Command;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Studio_Photo_Collage.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Studio_Photo_Collage.Infrastructure.Converters
{
    public class ProjectToCollage : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value as Project != null)
            {
                var proj = value as Project;
                var arr = proj.PhotoArray;
                var collageGrid = new Grid();
                var mainGrid = FromArrToGridConverter.GetGridWith<Grid>(arr);
                var backgroundGrid = new Grid();

                for (int i = 0; i < mainGrid.Children.Count; i++)
                {
                    var gridInGrid = mainGrid.Children[i] as Grid;
                    gridInGrid.Background = new SolidColorBrush(Colors.Gray);

                    var img = new Image();
                    img.Stretch = Windows.UI.Xaml.Media.Stretch.Fill;
                    gridInGrid.Children.Add(img);
                    gridInGrid.BorderThickness = new Windows.UI.Xaml.Thickness(proj.BorderThickness);

                    SetmageSourceAsync(img, proj, i);
                }

                backgroundGrid.Background = ColorGenerator.GetSolidColorBrush(proj.BorderColor);

                collageGrid.Children.Add(backgroundGrid);
                collageGrid.Children.Add(mainGrid);
                return collageGrid;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }

        private async void SetmageSourceAsync(Image img, Project project, int numberInArr)
        {
            string str = project.ImageArr[numberInArr];
            if (!string.IsNullOrEmpty(str))
            {
                var source = await ImageHelper.FromBase64(str);
            }
        }
    }
}
