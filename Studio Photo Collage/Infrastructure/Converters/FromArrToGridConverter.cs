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
            if (_photoArray == null)
                return null;

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

            //all column after 
            for (int i = columnPosition + 1; i < countofColumn; i++) 
            {
                if (number == arr[rowPosition, i])
                    columnspan++;
                else
                    break;
            }

            //all row after
            for (int i = rowPosition + 1; i < countofRow; i++)
            {
                if (number == arr[i, columnPosition])
                    rowspan++;
                else
                    break;
            }

            //clean
            for (int i = rowPosition; i < rowPosition + rowspan; i++)
            {
                for (int j = columnPosition; j < columnPosition + columnspan; j++)
                {
                    arr[i,j] = 0;
                }
            }

            arr[rowPosition, columnPosition] = 0;

            element.SetValue(Grid.ColumnProperty, columnPosition);
            element.SetValue(Grid.RowProperty, rowPosition);
            element.SetValue(Grid.RowSpanProperty, rowspan);
            element.SetValue(Grid.ColumnSpanProperty, columnspan);

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
