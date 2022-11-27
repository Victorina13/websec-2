namespace WebToSamara.Models
{
    public class Stop
    {
        public int KS_ID { get; set; }
        public string? Title { get; set; } = null;
        public string? AdjacentStreet { get; set; } = null;
        public string? Direction { get; set; } = null;
        public string? TitleEn { get; set; } = null;
        public string? AdjacentStreetEn { get; set; } = null;
        public string? DirectionEn { get; set; } = null;
        public string? TitleEs { get; set; } = null;
        public string? AdjacentStreetEs { get; set; } = null;
        public string? DirectionEs { get; set; } = null;
        public int? Cluster { get; set; } = null;
        public List<string>? BusesMunicipal { get; set; } = null;
        public List<string>? BusesCommercial { get; set; } = null;
        public List<string>? BusesPrigorod { get; set; } = null;
        public List<string>? BusesSeason { get; set; } = null;
        public List<string>? BusesSpecial { get; set; } = null;
        public List<string>? BusesIntercity { get; set; } = null;
        public List<string>? Trams { get; set; } = null;
        public List<string>? Trolleybuses { get; set; } = null;
        public List<string>? Metros { get; set; } = null;
        public List<string>? ElectricTrains { get; set; } = null;
        public List<string>? RiverTransports { get; set; } = null;
        public string? InfotabloExists { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }
        public int? Angle { get; set; }
        public int? ScheduleTime { get; set; }
        public Stop() { }

        public Stop(
            int KS_ID,
            string Title,
            string AdjacentStreet,
            string Direction,
            int ScheduleTime
            )
        {
            this.KS_ID = KS_ID;
            this.Title = Title;
            this.AdjacentStreet = AdjacentStreet;
            this.Direction = Direction;
            this.ScheduleTime = ScheduleTime;
        }
        public Stop( int ks_ID, string? title, string? adjacentStreet, string? direction,
                      string? titleEn, string? adjacentStreetEn, string? directionEn,
                      string? titleEs, string? adjacentStreetEs, string? directionEs, int scheduleTime,int cluster,
                      List<string> busesMunicipal, List<string> busesCommercial, List<string> busesPrigorod,
                      List<string> busesSeason, List<string> busesSpecial, List<string> busesIntercity,
                      List<string> trams, List<string> trolleybuses, List<string> metros, List<string> electricTrains,
                      List<string> riverTransports, string? infotabloExists, string? latitude, string? longitude, int? angle)
        {
            KS_ID = ks_ID;
            Title = title;
            AdjacentStreet = adjacentStreet;
            Direction = direction;
            TitleEn = titleEn;
            AdjacentStreetEn = adjacentStreetEn;
            DirectionEn = directionEn;
            TitleEs = titleEs;
            AdjacentStreetEs = adjacentStreetEs;
            DirectionEs = directionEs;
            ScheduleTime = scheduleTime;
            Cluster = cluster;
            BusesMunicipal = busesMunicipal;
            BusesCommercial = busesCommercial;
            BusesPrigorod = busesPrigorod;
            BusesSeason = busesSeason;
            BusesSpecial = busesSpecial;
            BusesIntercity = busesIntercity;
            Trams = trams;
            Metros = metros;
            ElectricTrains = electricTrains;
            Trolleybuses = trolleybuses;
            RiverTransports = riverTransports;
            InfotabloExists = infotabloExists;
            Latitude = latitude;
            Longitude = longitude;
            Angle = angle;

        }

    }
}
