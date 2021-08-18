using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OpenHackathonWeb.Models
{
    public class HackathonViewModel
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Hackathon Title")]
        public string Title { get; set; }

        [Required]
        [DisplayName("Description")]
        public string Description { get; set; }

        [Required]
        [DisplayName("Duration (in blocks)")]
        public uint Duration { get; set; }

        [Required]
        [DisplayName("Manager WalletAddress")]
        public string ManagerWalletAddress { get; set; }

        [DisplayName("Manager Id")]
        public int ManagerId { get; set; }

        [DisplayName("Manager Name")]
        public string ManagerName { get; set; }

        [Required]
        [DisplayName("Prize Amount (CRS)")]
        public ulong PrizeAmount { get; set; }

        public bool IsUserRegistered { get; set; }
    }
}
