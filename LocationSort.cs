using System.Collections.Generic;

namespace It722_Project
{
    class LocationSort : IComparer<PlaceOfInterest>
      //class RadioSort : IComparer<PlaceOfInterest>
    {
       
        public int Compare(PlaceOfInterest a, PlaceOfInterest b)
        {
            //if (a.LongitudeX !=b.LongitudeX)
            //{
            //    return a.LongitudeX - b.LongitudeX;
            //}
            //else if (a.LatitudeY !=b.LatitudeY)
            //{
            //    return a.LatitudeY - b.LatitudeY;
            //}
         
            return 0;
        }
        //public int Compare(PlaceOfInterest pt1, PlaceOfInterest pt2)
        //{
        //    return (int)(SignedArea(_pivot, pt1, pt2) * 10000);
        //}
    }

}
