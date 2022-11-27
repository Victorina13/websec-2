namespace toSamara.Model
{
    public class ScheduleDepartureTime
    {
        public int Number { get; set; }
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
        public ScheduleDepartureTime(int number, int requestedStopId, string modelTitle, int hullNo, double delayTime,
            int nextStopId, int time, double timeInSeconds, string stateNumber, string type, double spanLength, double remainingLength)
        {
            Number = number;
            RequestedStopId = requestedStopId;
            ModelTitle = modelTitle;
            HullNo = hullNo;
            DelayTime = delayTime;
            NextStopId = nextStopId;
            Time = time;
            TimeInSeconds = timeInSeconds;
            StateNumber = stateNumber;
            Type = type;
            SpanLength = spanLength;
            RemainingLength = remainingLength;
        }
    }
}
