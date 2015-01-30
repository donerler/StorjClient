using StorjClient.Data;
using System.Threading.Tasks;

namespace StorjClient
{
    public class StorjAccountClient : StorjClientBase
    {
        public StorjAccountClient(string apiUrl)
            : base(apiUrl)
        {

        }

        public async Task<TokenData> NewToken()
        {
            return await PostAsync<TokenData>("accounts/token/new");
        }

        public async Task<PriceData> Prices()
        {
            return await GetAsync<PriceData>("accounts/token/prices");
        }

        public async Task<BalanceData> Balance(string token)
        {
            return await GetAsync<BalanceData>("accounts/token/balance/" + token);
        }

        public async Task<Redeem> Redeem(string promoCode)
        {
            return await GetAsync<Redeem>("accounts/token/redeem/" + promoCode);
        }

        public async Task<DepositWithdraw> Deposit(string token, int bytes, string secret)
        {
            return await PostAsync<DepositWithdraw>("accounts/token/deposit" + token + "?bytes=" + bytes, secret);
        }

        public async Task<DepositWithdraw> Withdraw(string token, int bytes, string secret)
        {
            return await PostAsync<DepositWithdraw>("accounts/token/withdraw" + token + "?bytes=" + bytes, secret);
        }
    }
}
