using PayPal.Api;

namespace PetStoreApi.Services
{
    public interface IPaypalRepository
    {
        Payment CreatePayment(double amount, string returnUrl, string cancelUrl, string intent);
        Payment ExecutePayment(string paymentId, string payerId);
    }
}
