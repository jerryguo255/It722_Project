using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace It722_Project
{
    class RadioSort : IComparer<PlaceOfInterest>
        {
            private PlaceOfInterest _pivot;


            public RadioSort(PlaceOfInterest pivot)
            {
                _pivot = pivot;
            }

            public int Compare(PlaceOfInterest pt1, PlaceOfInterest pt2)
            {

            return SignedArea(_pivot, pt1, pt2);
            //  long ss = SignedArea(_pivot, pt1, pt2);
            //if (ss > 0)
            //{
            //    return 1;
            //}
            //else if (ss==0){
            //    return 0;
            //}
            //return -1;
            }

            public int SignedArea(PlaceOfInterest a, PlaceOfInterest b, PlaceOfInterest c)
            {
            //abba bccb caac
            return (int)(a.LongitudeX * b.LatitudeY - b.LongitudeX * a.LatitudeY +
                     b.LongitudeX * c.LatitudeY - c.LongitudeX * b.LatitudeY +
                     c.LongitudeX * a.LatitudeY - a.LongitudeX * c.LatitudeY);

            //return (a.Longitude * b.Latitude - b.Longitude * a.Latitude +
            //            b.Longitude * c.Latitude - c.Longitude * b.Latitude +
            //            c.Longitude * a.Latitude - a.Longitude * c.Latitude);
            // a.X * b.Y - b.X * a.Y + b.X * c.Y - c.X * b.Y + c.X * a.Y - a.X * c.Y;
        }
        }

}
