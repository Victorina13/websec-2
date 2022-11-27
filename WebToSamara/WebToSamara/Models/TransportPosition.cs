namespace webToSamara.Models
{
    public class TransportPosition
    {
        public List<NextStop> NextStops { get; set; }
        
        public int KR_ID { get; set; }

        public TransportPosition(List<NextStop> NextStops, int KR_ID)
        {
            this.NextStops = NextStops;
            this.KR_ID = KR_ID;
        }
    }
}
