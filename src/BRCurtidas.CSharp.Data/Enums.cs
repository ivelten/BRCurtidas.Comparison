namespace BRCurtidas.CSharp.Data
{
    public enum PaymentMethod
    {
        PayPal = 1,
        PagSeguro = 2,
        WireTransfer = 3
    }

    public enum OrderStatus
    {
        Created = 1,
        WaitingPayment = 2,
        Paid = 3,
        Cancelled = 4,
        Delivered = 5
    }

    public enum SocialNetworkType
    {
        Facebook = 1,
        Instagram = 2,
        YouTube = 3
    }
}