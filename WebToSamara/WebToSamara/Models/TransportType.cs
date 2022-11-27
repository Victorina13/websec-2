namespace WebToSamara.Models
{
    public class TransportType
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public TransportType(int id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}
