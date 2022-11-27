using System.Linq;
using System.Xml;
using toSamara.Model;
using Route = toSamara.Model.Route;

namespace webToSamara.Models
{
    public class Routes
    {
        public List<Route> RoutesList { get; set; }

        public Routes()
        {
            RoutesList = new List<Route>();
            LoadFromXml();
        }

        private void LoadFromXml()
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(@"C:\Users\iru\source\repos\websec-2\classifiers\routesAndStopsCorrespondence.xml");
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
                    var stopNodes = nodes.Where(x => x.Name == "stop");
                    var transportNode = nodes.First(x => x.Name == "transportType");
                    var route = new Route(
                        KR_ID: int.Parse(nodes.First(x => x.Name == "KR_ID").InnerText),
                        Number: nodes.First(x => x.Name == "number").InnerText,
                        Direction: nodes.First(x => x.Name == "direction").InnerText,
                        RealtimeForecast: nodes.First(x => x.Name == "realtimeForecast").InnerText == "1" ? true : false,
                        TransportTypeObj: new TransportType(
                                                    id: int.Parse(transportNode.ChildNodes?[0]?.InnerText!),
                                                    title: transportNode.ChildNodes?[1]?.InnerText!),
                        Performing: nodes.First(x => x.Name == "performing").InnerText == "1" ? true : false,
                        Stops: stopNodes.Select(x => new Stop(
                                KS_ID: int.Parse(x.ChildNodes?[0]?.InnerText!),
                                Title: x.ChildNodes?[1]?.InnerText!,
                                AdjacentStreet: x.ChildNodes?[2]?.InnerText!,
                                Direction: x.ChildNodes?[3]?.InnerText!,
                                ScheduleTime: int.Parse(x.ChildNodes?[4]?.InnerText!)
                                )
                            ).ToList()
                        );
                    RoutesList.Add(route);
                }
            }
        }
    }
}
