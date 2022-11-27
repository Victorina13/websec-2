namespace toSamara.Model
{
    public class Transport
    {
        public int KR_ID { get; set; }
        public string NextStopName { get; set; }
        public string Quality { get; set; }
        public bool ForInvalid { get; set; }
        public double ScheduleTimeTo { get; set; }
        public ScheduleDepartureTime ScheduleDepartureTimeObj { get; set; }
        public string Number { get; set; }
        public int RequestedStopId { get; set; }
        public string ModelTitle { get; set; }
        public int HullNo { get; set; }
        public double DelayTime { get; set; }
        public int NextStopId { get; set; }
        public int Time { get; set; }
        public double TimeInSeconds { get; set; }
        public string StateNumber { get; set; }
        public string Type { get; set; }
        public double SpanLength { get; set; }
        public double RemainingLength { get; set; }
    }
}
