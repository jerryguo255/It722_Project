using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.MapProviders;
using System.Net;
using System.Web.Script.Serialization;


namespace It722_Project
{
    public partial class GMapForm : Form
    {
        public GMapForm()
        {
            InitializeComponent();
        }

        private void gMapControl_Load(object sender, EventArgs e)
        {
            
            //initialize a Google map instance --- 
            gMapControl.MapProvider = GoogleMapProvider.Instance;
            GMaps.Instance.Mode = AccessMode.ServerOnly;
            //set the 
            gMapControl.Position = new PointLatLng(-46.403, 168.375);
            //set if showing the center cross
            gMapControl.ShowCenter = false;
            //initialize a Google map instance --- 






            var points = GetLocationData();
            //p.Longitude <170&& p.Longitude > 160  ]]  p.UserID == 2016000506
           var 特定用户 = points.ToList();
            //特定用户.Sort();
            //points.Sort(new RadioSort(FindPivot(points)));
         // 特定用户.Sort(new LocationSort());
      //      var DeDuplicatedPoints =  DeDuplicate(特定用户);
            var outputPoints = 特定用户.ToList();
            //foreach (var item in points)
            //{
            //    notes.AppendText(item.LongitudeX.ToString() + ":" + item.LatitudeY.ToString() + "\n");
            //}
             var pivot = FindPivot(特定用户);
             var sss=   ConvexHull(pivot, 特定用户);


            //for (int i = 0; i < outputPoints.Count; i++)
            //{
            //    notes.AppendText(i+":"+ outputPoints[i].Longitude.ToString() + "\n");
            //}

            //add markers overlay
            gMapControl.Overlays.Add(PlaceMarkers(outputPoints));


            //add route overlay
            gMapControl.Overlays.Add(GenerateRoute(sss));



        }


        private GMapOverlay GenerateRoute(List<PlaceOfInterest> points) {
            List<PointLatLng> pointsss = new List<PointLatLng>();
            foreach (var point in points)
            {
                pointsss.Add(new PointLatLng(point.Latitude, point.Longitude));
            }
            //connecting as a circle
             pointsss.Add(new PointLatLng(points[0].Latitude, points[0].Longitude));
            GMapOverlay routes = new GMapOverlay("routes");
            GMapRoute route = new GMapRoute(pointsss, "A Route");
            route.Stroke = new Pen(Color.Red, 2);
            routes.Routes.Add(route);

            return routes;
        }


        private List<PlaceOfInterest> DeDuplicate(List<PlaceOfInterest> points) {
            //sort by position before!!

            var point = points[0];

            for (int i = 1; i < points.Count; i++)
            {
                if (points[i].LatitudeY == point.LatitudeY&& points[i].LongitudeX == point.LongitudeX)
                {
                    points.RemoveAt(i);
                }
                else
                {
                    point = points[i];
                }
            }

            return points;
        }


       private PlaceOfInterest FindPivot(List<PlaceOfInterest> points)
        {
            PlaceOfInterest lowestLeftPoint = points[0];
            for (int i = 1; i < points.Count; i++)
            {
                if (points[i].LongitudeX == lowestLeftPoint.LongitudeX)
                {
                    if (points[i].LatitudeY < lowestLeftPoint.LatitudeY)
                    {
                        lowestLeftPoint = points[i];
                    }

                }
                else if (points[i].LongitudeX < lowestLeftPoint.LongitudeX)
                {
                    lowestLeftPoint = points[i];
                }
            }

            //return _points.First(c => c.Y == _points.Max(x => x.Y));

            return lowestLeftPoint;
        }


        private List<PlaceOfInterest> GetLocationData()
        {
            string url = @"http://developer.kensnz.com/getlocdata";
            var list = new List<PlaceOfInterest>();
            using (WebClient client = new WebClient())
            {
                var json = client.DownloadString(url);
                JavaScriptSerializer ser = new JavaScriptSerializer();
                Dictionary<string, string>[] JSONArray = ser.Deserialize<Dictionary<string, string>[]>(json);
                // notes.AppendText(json + "\n\n");
                foreach (Dictionary<string, string> point in JSONArray)
                {
                    list.Add(new PlaceOfInterest
                    {
                        UserID = int.Parse(point["userid"]),
                        Latitude = Math.Round(double.Parse(point["latitude"]),8),
                        Longitude = Math.Round(double.Parse(point["longitude"]),8),
                        Description = point["description"]
                    });
                }
            }
            return list;
        }

        private List<PlaceOfInterest> ConvexHull(PlaceOfInterest pivot, List<PlaceOfInterest> points)
        {
            List<PlaceOfInterest> Hull = new List<PlaceOfInterest>();

            points.Remove(pivot);
            var s = new RadioSort(pivot);
            points.Sort(s);
            Hull.Add(pivot);  // first point
            Hull.Add(points[0]); // second point
            points.RemoveAt(0);
            Hull.Add(points[0]);// third point
          points.RemoveAt(0);
            while (points.Count != 0)
            {
                long value = s.SignedArea(Hull[Hull.Count - 2], Hull[Hull.Count - 1], points[0]);
                if (value < 0)
                {
                    Hull.Add(points[0]);
                    points.RemoveAt(0);
                }
                else if (value == 0)
                {
                    Hull.RemoveAt(Hull.Count - 1);
                    Hull.Add(points[0]);
                    points.RemoveAt(0);
                }
                else
                {
                    Hull.RemoveAt(Hull.Count - 1);
                    //Hull.Add(points[0]);
                    // points.RemoveAt(0);
                }
            }
            return Hull;
        }

        private GMapOverlay PlaceMarkers(List<PlaceOfInterest> list) {
            GMapOverlay finishedOverlay = new GMapOverlay("markers");

            Random r = new Random();
            int FormerPointUserId = -1;
            Color FormaerColor=Color.White;
            foreach (var point in list)
            {
                GMapMarker marker = new GMarkerGoogle(new PointLatLng(point.Latitude, point.Longitude), GMarkerGoogleType.pink_pushpin);
                marker.ToolTipMode = MarkerTooltipMode.OnMouseOver;
                marker.ToolTipText = string.Format("UserId:{0}\nDesc:{1}\n{2}:{3}", point.UserID, point.Description,point.Latitude.ToString(),point.Longitude.ToString());
          
                marker.ToolTip.Foreground = Brushes.Black;
                //border
                marker.ToolTip.Stroke = Pens.Red;
                marker.ToolTip.TextPadding = new Size(10, 10);
                if (point.UserID != FormerPointUserId)
                {
                    var color = Color.FromArgb(r.Next(256), r.Next(256), r.Next(256));
                    marker.ToolTip.Fill = new SolidBrush(color);

                    FormaerColor = color;
                    FormerPointUserId = point.UserID;
                }
                else
                {
                    marker.ToolTip.Fill = new SolidBrush(FormaerColor);
                }
               

                finishedOverlay.Markers.Add(marker);
            }




            return finishedOverlay;
        }
    }
}
