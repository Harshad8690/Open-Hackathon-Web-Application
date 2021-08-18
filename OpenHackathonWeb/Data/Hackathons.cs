using System.ComponentModel.DataAnnotations.Schema;

namespace OpenHackathonWeb.Data
{
    public class Hackathons
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public uint Duration { get; set; }

        public ulong PrizeAmount { get; set; }

        [ForeignKey("Users")]
        public int Manager { get; set; }

        public Users Users { get; set; }
    }
}
