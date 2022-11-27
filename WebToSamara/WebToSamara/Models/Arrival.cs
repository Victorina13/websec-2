namespace WebToSamara.Models
{
    public class Arrival
    {
        public List<Transport> arrival { get; set; }
        public Arrival(List<Transport> transports)
        {
            arrival = transports;
        }
    }
}
