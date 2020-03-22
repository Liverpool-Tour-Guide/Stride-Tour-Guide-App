using System;
using System.Collections.Generic;
using System.Text;

namespace StrideApp
{
    public class Waypoint
    {   
        public string StorageIndex { get; set; }
        public string LandmarkID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AudioURL { get; set; }
        public bool Visited { get; set; }
        public string MarkColor {
            get
            {
                if (Visited)
                {
                    return "Gray";
                }
                else
                {
                    return "DeepSkyBlue";
                }
            }
        }
        public string TextColor {
            get
            {
                if (Visited)
                {
                    return "Gray";
                }
                else
                {
                    return "Black";
                }
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public void changeVisitStatus()
        {
            Visited = true;
        }
    }
}
