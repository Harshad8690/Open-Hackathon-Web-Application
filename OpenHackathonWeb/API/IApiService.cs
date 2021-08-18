using System.Threading.Tasks;

namespace OpenHackathonWeb.API
{
    public interface IApiService
    {
        Task<TransactionResponse> CreateHackathon(string title, uint duration, string managerWalletAddress, string senderWalletAddress, ulong prizeAmount);

        Task<TransactionReceipt> GetReceipt(string txHash);

        Task<TransactionResponse> DepositFund(string senderWalletAddress, ulong amount);

        Task<TransactionResponse> SetPrize(int hackathonId, int rank, ulong prizeAmount, string senderWalletAddress);

        Task<TransactionResponse> AnnounceWinner(int hackathonId, int rank, string winnerWalletAddress, string senderWalletAddress);

        Task<TransactionResponse> Register(int hackathonId, string senderWalletAddress);

        Task<ulong> AddressBalance(string walletAddress);
    }
}
