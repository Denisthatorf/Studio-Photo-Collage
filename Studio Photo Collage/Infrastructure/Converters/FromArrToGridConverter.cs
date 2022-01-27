using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Studio_Photo_Collage.Infrastructure.Converters
{
    public class FromArrToGridConverter : IValueConverter
    {
        public static Grid GetGridWith<T>(byte[,] _photoArray) where T: UIElement, new()
        {
            int row = _photoArray.GetLength(0);
            int column = _photoArray.GetLength(1);
            var grid = CreateGrid(row, column);

            byte[,] arr = new byte[row, column];
            Array.Copy(_photoArray, arr, arr.Length);

            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    var rect = UIelementsFromData<T>(ref arr, i, j);
                    if (rect != null)
                        grid.Children.Add(rect);
                }
            }
            return grid;
        }

        public static Grid CreateGrid(int row, int column)
        {
            var grid = new Grid();
            for (int i = 0; i < row; i++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
            }
            for (int i = 0; i < column; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
            }


            // grid.RenderTransformOrigin = new Windows.Foundation.Point(0.5, 0.5);
            //  grid.RenderTransform = new Windows.UI.Xaml.Media.RotateTransform() { Angle = 90 * j };

            return grid;
        }

        public static UIElement UIelementsFromData<T>(ref byte[,] arr, int rowPosition, int columnPosition) where T : UIElement, new()
        {
            byte number = arr[rowPosition, columnPosition];
            if (number == 0)
                return null;

            int rowspan = 1;
            int columnspan = 1;
            int countofRow = arr.GetLength(0);
            int countofColumn = arr.GetLength(1);

            T element = new T();

            for (int i = columnPosition + 1; i < countofColumn; i++) //all column after
            {
                if (number == arr[rowPosition, i])
                {
                    arr[rowPosition, i] = 0;
                    columnspan++;
                }
                else
                    break;
            }

            for (int i = rowPosition + 1; i < countofRow; i++) //all row aftr
            {
                if (number == arr[i, columnPosition])
                {
                    arr[i, columnPosition] = 0;
                    rowspan++;
                }
                else
                    break;
            }

            // if (rowspan > 1 && columnspan > 1)
            //throw new Exception();

            arr[rowPosition, columnPosition] = 0;

            element.SetValue(Grid.ColumnProperty, columnPosition);
            element.SetValue(Grid.RowProperty, rowPosition);
            element.SetValue(Grid.RowSpanProperty, rowspan);
            element.SetValue(Grid.ColumnSpanProperty, columnspan);

           // rect.Fill = new SolidColorBrush(Colors.Aqua);
            return element;
        }

        public object ConvertFromProjectTo<T>(object value) where T: UIElement, new()
        {
            var grid = GetGridWith<T>(value as byte[,]);
            return grid;
        }
        /////

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var grid = GetGridWith<Rectangle>(value as byte[,]);
            return grid;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
