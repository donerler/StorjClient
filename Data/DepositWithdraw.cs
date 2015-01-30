using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace StorjClient.Data
{
    public class DepositWithdraw
    {
        DepositWithdrawStatus Status { get; set; }
    }

    public enum DepositWithdrawStatus
    {
        [EnumMember(Value = "invalid-authentication")]
        InvalidAuthentication,
        [EnumMember(Value = "bad-request")]
        BadRequest,
        [EnumMember(Value = "balance-insufficient")]
        BalanceInsufficient,
        Error
    }
}
