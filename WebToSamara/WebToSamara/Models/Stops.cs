using System.Xml;

namespace WebToSamara.Models
{
    public class Stops
    {
        public List<Stop> StopsList{ get; set; }
        public Stops()
        {
            StopsList = new List<Stop>();
            this.LoadFromXml();
        }

        public Stops(List<Stop> stopsList)
        {
            StopsList = stopsList;
        }
        private void LoadFromXml()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(@"C:\Users\iru\source\repos\websec-2\classifiers\stops.xml");
            XmlElement? xRoot = xDoc.DocumentElement;
            if (xRoot != null)
            {
                foreach (XmlElement xnode in xRoot)
                {
                    List<XmlNode> nodes = new List<XmlNode>();
                    foreach (var node in xnode.ChildNodes)
                    {
                        nodes.Add((XmlNode)node);
                    }
                    Stop stop = new Stop();
                    stop.KS_ID = int.Parse(nodes.First(x => x.Name == "KS_ID").InnerText);
                    stop.Title = nodes.First(x => x.Name == "title").InnerText;
                    stop.AdjacentStreet = nodes.First(x => x.Name == "adjacentStreet").InnerText;
                    stop.Direction = nodes.First(x => x.Name == "direction").InnerText;
                    stop.TitleEn = nodes.First(x => x.Name == "titleEn").InnerText;
                    stop.AdjacentStreetEn = nodes.First(x => x.Name == "adjacentStreetEn").InnerText;
                    stop.DirectionEn = nodes.First(x => x.Name == "directionEn").InnerText;
                    stop.TitleEs = nodes.First(x => x.Name == "titleEs").InnerText;
                    stop.AdjacentStreetEs = nodes.First(x => x.Name == "adjacentStreetEs").InnerText;
                    stop.DirectionEs = nodes.First(x => x.Name == "directionEs").InnerText;
                    StopsList.Add(stop);

                }
            }
        }
    }
}
