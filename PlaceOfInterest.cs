using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace It722_Project
{
    class PlaceOfInterest : IComparable<PlaceOfInterest>
    {
        public int UserID { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string Description { get; set; }
        public int LatitudeY
        {
            get
            {//convert to positive integer number, 
                return (int)((Latitude + 180) *1000);
            }
            set { }
        }
        
        public int LongitudeX
        {
            get
            {//convert to positive integer number, 
                return (int)((Longitude +180)*1000);
            }
            set { }
        }

      

        public int CompareTo(PlaceOfInterest other)
        {
            return UserID - other.UserID;
        }
    }
}
