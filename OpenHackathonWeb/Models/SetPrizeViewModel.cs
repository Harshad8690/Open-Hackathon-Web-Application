namespace OpenHackathonWeb.Models
{
    public class SetPrizeViewModel
    {
        public int HackathonId { get; set; }

        public int Rank { get; set; }

        public ulong Amount { get; set; }

        public ulong SatoshiAmount { get; set; }
    }
}
