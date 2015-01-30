
namespace StorjClient.Data
{
    public class Redeem
    {
        RedeemStatus Status { get; set; }
    }

    public enum RedeemStatus
    {
        Ok,
        Error
    }
}
