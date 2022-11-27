namespace webToSamara.Models
{
    public class NextStop
    {
        public int KS_ID { get; set; }
        public double Time { get; set; }

        public NextStop(int KS_ID, double Time)
        {
            this.KS_ID = KS_ID;
            this.Time = Time;
        }
    }
}
