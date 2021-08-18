using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OpenHackathonWeb.Models
{
    public class DepositFundViewModel
    {
        [DisplayName("Amount (CRS)")]
        [Required]
        public ulong Amount { get; set; }
    }
}
