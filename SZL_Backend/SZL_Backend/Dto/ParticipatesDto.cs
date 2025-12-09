namespace SZL_Backend.Dto
{
    public class ParticipatesDto
    {
        public int Participateid { get; set; }

        public int? Teamid { get; set; }

        public int? Tagid { get; set; }

        public int? Runnerid { get; set; }

        public int? Eventid { get; set; }
    }
}

namespace SZL_Backend.Dto
{
    public class ParticipatesCreateDto
    {
        public int? Teamid { get; set; }

        public int? Tagid { get; set; }

        public int? Runnerid { get; set; }

        public int? Eventid { get; set; }
    }
}