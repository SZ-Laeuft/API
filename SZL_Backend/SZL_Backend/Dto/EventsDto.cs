namespace SZL_Backend.Dto
{
    public class EventsDto
    {
        public int Eventid { get; set; }

        public string? Name { get; set; }

        public string? Place { get; set; }

        public string? Isactive { get; set; }

        public DateTime? Starttime { get; set; }

        public DateTime? Endtime { get; set; }

        public int? Categoryid { get; set; }
    }
}

namespace SZL_Backend.Dto
{
    public class EventsCreateDto
    {
        public int Eventid { get; set; }

        public string? Name { get; set; }

        public string? Place { get; set; }

        public string? Isactive { get; set; }

        public DateTime? Starttime { get; set; }

        public DateTime? Endtime { get; set; }

        public int? Categoryid { get; set; }
    }
}