using Newtonsoft.Json;
using System.Collections.Generic;

namespace OpenHackathonWeb.API
{
    public class ContractCallRequest
    {
        public ContractCallRequest()
        {
            Parameters = new List<string>();
        }

        [JsonProperty("amount")]
        public ulong Amount { get; set; }

        [JsonProperty("contractAddress")]
        public string ContractAddress { get; set; }

        [JsonProperty("methodName")]
        public string MethodName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("sender")]
        public string Sender { get; set; }

        [JsonProperty("walletName")]
        public string WalletName { get; set; }

        [JsonProperty("accountName")]
        public string AccountName { get; set; }

        [JsonProperty("outpoints")]
        public object Outpoints { get; set; }

        [JsonProperty("feeAmount")]
        public string FeeAmount { get; set; }

        [JsonProperty("gasPrice")]
        public int GasPrice { get; set; }

        [JsonProperty("gasLimit")]
        public int GasLimit { get; set; }

        [JsonProperty("parameters")]
        public List<string> Parameters { get; set; }
    }
}
