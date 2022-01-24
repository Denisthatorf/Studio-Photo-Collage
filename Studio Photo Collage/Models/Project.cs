using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace Studio_Photo_Collage.Models
{
    public class Project
    {
        private byte[,] _photoArray;
        public byte[,] PhotoArray
        {
            get { return _photoArray; }
            set {_photoArray = value; }
        }

        public int CountOfPhoto { get; }

        public DateTime DateOfLastEditing { get; set; }

        public string ProjectName { get; set; }

        public Project(byte[,] _photoArr)
        {
            _photoArray = _photoArr;
        }

        public Project()
        {
        }

        public Project GetRotatedProject(int count)
        {
            var width = _photoArray.GetUpperBound(0) + 1;
            var height = _photoArray.GetUpperBound(1) + 1;
            byte[,] arr = new byte[width,height];

            Array.Copy(_photoArray, arr, _photoArray.Length);
            for (int i = 0; i < count; i++)
            {
                arr = RotateRight(arr);
            }
            return new Project(arr);
        }

        public byte[,] RotateRight(byte[,] arr)
        {
            int width;
            int height;
            byte[,] dst;

            width = arr.GetUpperBound(0) + 1;
            height = arr.GetUpperBound(1) + 1;

            var src = new byte[width, height];
            Array.Copy(arr, src, src.Length);

            dst = new byte[height, width];

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    int newRow;
                    int newCol;

                    newRow = col;
                    newCol = height - (row + 1);

                    dst[newCol, newRow] = src[col, row];
                }
            }

            return dst;
        }
    }
}
