using Newtonsoft.Json;
using Studio_Photo_Collage.Infrastructure.Helpers;
using Studio_Photo_Collage.ViewModels.SidePanels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Windows.UI.Xaml.Media.Imaging;

namespace Studio_Photo_Collage.Models
{
    [JsonObject(MemberSerialization.Fields)]
    public class Project : INotifyPropertyChanged
    {
        [NonSerialized]
        private static Random rnd = new Random();

        private int Uid;

        private byte[,] _photoArray;
        public byte[,] PhotoArray
        {
            get { return _photoArray; }
            set { _photoArray = value; }
        }

        public DateTime DateOfLastEditing { get; set; }

        public string ProjectName { get; set; }

        public string SaveFormat { get;  set; }

        public int CountOfPhotos
        {
            get
            {
                var list = new List<byte>();
                foreach (var item in _photoArray)
                {
                    if (!list.Contains(item))
                        list.Add(item);
                }
                return list.Count;
            }
        }

        #region UIelement Properties

        private string _colorOfBorders; // background in grid
        public string BorderColor
        {
            get { return _colorOfBorders; }
            set { _colorOfBorders = value; }
        }

        private double _borderThickness;
        public double BorderThickness
        {
            get { return _borderThickness; }
            set { _borderThickness = value;  }
        }

        private double _borderOpacity;
        public double BorderOpacity
        {
            get { return _borderOpacity; }
            set { _borderOpacity = value;  }
        }

        private string[] _arrayOfImages;
        public string[] ImageArr
        {
            get { return _arrayOfImages; }
            set { _arrayOfImages = value; }
        }


        #endregion

        public Project(byte[,] photoArr)
        {
            Uid = rnd.Next();
            SaveFormat = "png";
            _photoArray = photoArr;
            _borderOpacity = 1;
            _colorOfBorders = "#ffff00";
            _arrayOfImages = new string[CountOfPhotos];
        }
        public Project() { }

        public override string ToString()
        {
            return ProjectName;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
           return Equals(obj as Project);
        }

        public bool Equals(Project other)
        {
            if (other == null)
                return false;
            return other.GetHashCode() == this.GetHashCode();
        }

        public override int GetHashCode()
        {
            /* int hashCode = 547882800;

             hashCode = hashCode * -1521134295 + DateOfLastEditing.GetHashCode();
             hashCode = hashCode * -1521134295 + ProjectName.GetDeterministicHashCode();
             hashCode = hashCode * -1521134295 + Uid.GetHashCode();
             foreach (var item in _photoArray)
             {
                 hashCode = hashCode * -1521134295 + item.GetHashCode();
             }
             foreach(var item in _arrayOfImages)
             {
                 if(item != null)
                    hashCode = hashCode * -1521134295 + item.GetDeterministicHashCode();
             }
             return hashCode;*/
            return Uid;
        }
        [field: NonSerializedAttribute()]
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
