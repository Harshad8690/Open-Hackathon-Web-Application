using System.ComponentModel.DataAnnotations.Schema;

namespace OpenHackathonWeb.Data
{
    public class HackathonRegistrations
    {
        public int Id { get; set; }

        [ForeignKey("Hackathons")]
        public int HackathonId { get; set; }

        [ForeignKey("Users")]
        public int UserId { get; set; }
    }
}
