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
            //初始化===================
            //初始化一个Google 地图  
            gMapControl.MapProvider = GoogleMapProvider.Instance;
            GMaps.Instance.Mode = AccessMode.ServerOnly;
            //设定显示中心点
            gMapControl.Position = new PointLatLng(-46.403, 168.375);
            //设定是否显示中心十字
            gMapControl.ShowCenter = false;
            //初始化===================




           

            var points = GetLocationData();
            points.Sort();
            //foreach (var item in points)
            //{
            //    notes.AppendText(item.UserID.ToString() + "\n");
            //}

            gMapControl.Overlays.Add(PlaceMarkers(points));



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
                        Latitude = double.Parse(point["latitude"]),
                        Longitude = double.Parse(point["longitude"]),
                        Description = point["description"]
                    });
                }
            }
            return list;
        }


        
        private GMapOverlay PlaceMarkers(List<PlaceOfInterest> list) {
            GMapOverlay finishedOverlay = new GMapOverlay("markers");

            Random r = new Random();
            int FormerPointUserId = -1;
            Color FormaerColor=Color.White;
            foreach (var point in list)
            {
                GMapMarker marker = new GMarkerGoogle(new PointLatLng(point.Latitude, point.Longitude), GMarkerGoogleType.pink_pushpin);
                marker.ToolTipMode = MarkerTooltipMode.Always;
                marker.ToolTipText = string.Format("UserId:{0}\nDesc:{1}", point.UserID, point.Description);
          
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
