    namespace SZL_Backend.DTO;

    public class EventsDTO
    {
        public int Eventid { get; set; }

        public string? Name { get; set; }

        public string? Place { get; set; }

        public string? Isactive { get; set; }

        public DateTime? Starttime { get; set; }

        public DateTime? Endtime { get; set; }

        public int? Categoryid { get; set; }
    }