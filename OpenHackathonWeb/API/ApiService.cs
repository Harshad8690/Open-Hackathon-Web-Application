using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OpenHackathonWeb.Helpers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OpenHackathonWeb.API
{
    public class ApiService : IApiService
    {
        private readonly IOptions<AppSettings> _appSettings;

        public ApiService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
        }

        private static StringContent CreateJsonContent(object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return content;
        }

        private ContractCallRequest PrepareContractCallRequest()
        {
            return new ContractCallRequest()
            {
                Amount = Convert.ToUInt64(_appSettings.Value.Amount),
                ContractAddress = _appSettings.Value.ContractAddress,
                WalletName = _appSettings.Value.WalletName,
                Password = _appSettings.Value.WalletPassword,
                AccountName = _appSettings.Value.AccountName,
                Outpoints = null,
                FeeAmount = _appSettings.Value.FeeAmount,
                GasPrice = Convert.ToInt32(_appSettings.Value.GasPrice),
                GasLimit = Convert.ToInt32(_appSettings.Value.GasLimit)
            };
        }

        public async Task<TransactionResponse> CreateHackathon(string title, uint duration, string managerWalletAddress, string senderWalletAddress, ulong prizeAmount)
        {
            try
            {
                var callRequest = PrepareContractCallRequest();
                callRequest.MethodName = "CreateHackathon";
                callRequest.Sender = senderWalletAddress;
                callRequest.Amount = prizeAmount;

                var parameters = new List<string>
                {
                    $"{(int) SerializedType.String}#{title}",
                    $"{(int) SerializedType.UInt32}#{duration}",
                    $"{(int) SerializedType.Address}#{managerWalletAddress}"                    
                };

                callRequest.Parameters = parameters;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{_appSettings.Value.BaseUrl}");

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.PostAsync("SmartContractWallet/call", CreateJsonContent(callRequest));
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<TransactionResponse>(content);
                    }

                    return new TransactionResponse { Success = false, Message = response.RequestMessage.ToString() };
                }
            }
            catch (Exception ex)
            {
                return new TransactionResponse { Success = false, Message = ex.Message };
            }
        }

        public async Task<TransactionReceipt> GetReceipt(string txHash)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{_appSettings.Value.BaseUrl}");

                    var response = await client.GetAsync($"SmartContracts/receipt?txHash={txHash}");

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var result = JsonConvert.DeserializeObject<TransactionReceipt>(content);
                        return result;
                    }
                    return new TransactionReceipt { Success = false, Error = response.RequestMessage.ToString() };
                }
            }
            catch (Exception ex)
            {
                return new TransactionReceipt { Success = false, Error = ex.Message };
            }
        }

        public async Task<TransactionResponse> DepositFund(string senderWalletAddress, ulong amount)
        {
            try
            {
                var requestBody = PrepareContractCallRequest();
                requestBody.Amount = amount;
                requestBody.MethodName = "Receive";
                requestBody.Sender = senderWalletAddress;
                requestBody.Parameters = null;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{_appSettings.Value.BaseUrl}");

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.PostAsync("SmartContractWallet/call", CreateJsonContent(requestBody));
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<TransactionResponse>(content);
                    }

                    return new TransactionResponse { Success = false, Message = response.RequestMessage.ToString() };
                }
            }
            catch (Exception ex)
            {
                return new TransactionResponse { Success = false, Message = ex.Message };
            }
        }

        public async Task<TransactionResponse> SetPrize(int hackathonId, int rank, ulong prizeAmount, string senderWalletAddress)
        {
            try
            {
                var requestBody = PrepareContractCallRequest();
                requestBody.MethodName = "SetWinnerPrize";
                requestBody.Sender = senderWalletAddress;

                var parameters = new List<string>();
                parameters.Add($"{(int)SerializedType.UInt32}#{hackathonId}");
                parameters.Add($"{(int)SerializedType.UInt32}#{rank}");
                parameters.Add($"{(int)SerializedType.UInt64}#{prizeAmount}");

                requestBody.Parameters = parameters;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{_appSettings.Value.BaseUrl}");

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.PostAsync("SmartContractWallet/call", CreateJsonContent(requestBody));
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<TransactionResponse>(content);
                    }

                    return new TransactionResponse { Success = false, Message = response.RequestMessage.ToString() };
                }
            }
            catch (Exception ex)
            {
                return new TransactionResponse { Success = false, Message = ex.Message };
            }
        }

        public async Task<TransactionResponse> AnnounceWinner(int hackathonId, int rank, string winnerWalletAddress, string senderWalletAddress)
        {
            try
            {
                var requestBody = PrepareContractCallRequest();
                requestBody.MethodName = "AnnounceWinner";
                requestBody.Sender = senderWalletAddress;

                var parameters = new List<string>
                {
                    $"{(int) SerializedType.UInt32}#{hackathonId}",
                    $"{(int) SerializedType.UInt32}#{rank}",
                    $"{(int) SerializedType.Address}#{winnerWalletAddress}"
                };

                requestBody.Parameters = parameters;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{_appSettings.Value.BaseUrl}");

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.PostAsync("SmartContractWallet/call", CreateJsonContent(requestBody));
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<TransactionResponse>(content);
                    }

                    return new TransactionResponse { Success = false, Message = response.RequestMessage.ToString() };
                }
            }
            catch (Exception ex)
            {
                return new TransactionResponse { Success = false, Message = ex.Message };
            }
        }

        public async Task<TransactionResponse> Register(int hackathonId, string senderWalletAddress)
        {
            try
            {
                var requestBody = PrepareContractCallRequest();
                requestBody.MethodName = "Register";
                requestBody.Sender = senderWalletAddress;

                var parameters = new List<string> { $"{(int)SerializedType.UInt32}#{hackathonId}" };

                requestBody.Parameters = parameters;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri($"{_appSettings.Value.BaseUrl}");

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = await client.PostAsync("SmartContractWallet/call", CreateJsonContent(requestBody));
                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<TransactionResponse>(content);
                    }

                    return new TransactionResponse { Success = false, Message = response.RequestMessage.ToString() };
                }
            }
            catch (Exception ex)
            {
                return new TransactionResponse { Success = false, Message = ex.Message };
            }
        }

        public async Task<ulong> AddressBalance(string walletAddress)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri($"{_appSettings.Value.BaseUrl}");

                var response = await client.GetAsync($"SmartContractWallet/address-balance?address={walletAddress}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var responseData = JsonConvert.DeserializeObject<ulong>(content);

                    return responseData;
                }

                return 0;
            }
        }
    }
}
