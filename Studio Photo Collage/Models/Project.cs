using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Studio_Photo_Collage.Models
{
    [JsonObject(MemberSerialization.Fields)]
    public class Project
    {
        [NonSerialized]
        private static readonly Random rnd = new Random();
        private readonly int uid;

        public int CountOfPhotos
        {
            get
            {
                var list = new List<byte>();
                foreach (var item in PhotoArray)
                {
                    if (!list.Contains(item))
                    {
                        list.Add(item);
                    }
                }

                return list.Count;
            }
        }
        public byte[,] PhotoArray { get; set; }
        public DateTime DateOfLastEditing { get; set; }
        public string ProjectName { get; set; }

        #region UIelement prop
        public string BackgroundColor { get; set; }
        public double BorderThickness { get; set; }
        public double BorderOpacity { get; set; }
        public string[] ImageArr { get; set; }
        public float ZoomFactor { get; set; }
        #endregion

        public Project(byte[,] photoArr)
        {
            uid = rnd.Next();
            PhotoArray = photoArr;
            BorderOpacity = 1;
            BackgroundColor = "#ffff00";
            ImageArr = new string[CountOfPhotos];
        }
        public Project() { }

        public override string ToString()
        {
            return ProjectName;
        }

        #region Equels overriding
        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return Equals(obj as Project);
        }
        public bool Equals(Project other)
        {
            return !(other is null) && other.GetHashCode() == this.GetHashCode();
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
            return uid;
        }
        public static bool operator ==(Project first, Project second)
        {
            if (!(first is null || second is null))
            {
                return first.Equals(second);
            }

            return false;
        }
        public static bool operator !=(Project first, Project second) => !(first == second);
        #endregion

        [OnSerializing]
        private void OnDeserializingMethod(StreamingContext context)
        {
            DateOfLastEditing = DateTime.Now;
        }
    }
}
