using System.Xml;

namespace WebToSamara.Models
{
    public class Route
    {
        public int KR_ID { get; set; }
        public string Number { get; set; }
        public string Direction { get; set; }
        public bool RealtimeForecast { get; set; }
        public TransportType TransportTypeObj { get; set; }   
        public bool Performing { get; set; }
        public List<Stop> Stops { get; set; }

        public Route(int KR_ID,
                    string Number,
                    string Direction,
                    bool RealtimeForecast,
                    TransportType TransportTypeObj,
                    bool Performing,
                    List<Stop> Stops)
        {
            this.KR_ID = KR_ID;
            this.Number = Number;
            this.Direction = Direction;
            this.RealtimeForecast = RealtimeForecast;
            this.TransportTypeObj = TransportTypeObj;
            this.Performing = Performing;
            this.Stops = Stops;
        }
    }
}
