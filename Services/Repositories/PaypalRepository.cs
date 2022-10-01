using com.sun.org.glassfish.gmbal;
using java.util;
using Microsoft.Extensions.Options;
using org.omg.PortableInterceptor;
using PayPal;
using PayPal.Api;
using PetStoreApi.Domain;

namespace PetStoreApi.Services.Repositories
{
    public class PaypalRepository : IPaypalRepository
    {
        private readonly PayPalAuthOption _options;
        private APIContext _apiContext;
        private readonly ILogger<PaypalRepository> _logger;

        public PaypalRepository(IOptions<PayPalAuthOption> options, APIContext apiContext, ILogger<PaypalRepository> logger)
        {
            _options = options.Value;
            _apiContext = apiContext;
            _logger = logger;
        }

        public Payment CreatePayment(double total, string returnUrl, string cancelUrl, string intent)
        {
            Dictionary<string, string> sdkConfig = new Dictionary<string, string>();
            sdkConfig.Add("mode", "sandbox");
            _apiContext = new APIContext(new OAuthTokenCredential(_options.PayPalClientId, _options.PayPalClientSecret, sdkConfig).GetAccessToken());

            Amount amount = new Amount();
            amount.currency = "USD";
            amount.total = total.ToString("0.00");
            Transaction transaction = new Transaction();
            transaction.description = "Checkout order";
            transaction.amount = amount;
            List<Transaction> transactions = new List<Transaction>();
            transactions.Add(transaction);
            Payer payer = new Payer();
            payer.payment_method = "paypal";
            Payment payment = new Payment();
            payment.intent = intent;
            payment.payer = payer;
            payment.transactions = transactions;
            RedirectUrls redirectUrls = new RedirectUrls();
            redirectUrls.cancel_url = cancelUrl;
            redirectUrls.return_url = returnUrl;
            payment.redirect_urls = redirectUrls;
            _apiContext.MaskRequestId = true;
            return payment.Create(_apiContext);
        }

        public Payment ExecutePayment(string paymentId, string payerId)
        {
            Payment payment = new Payment();
            if (paymentId.Equals("") == false && payerId.Equals("") == false)
            {
                try
                {
                    Dictionary<string, string> sdkConfig = new Dictionary<string, string>();
                    sdkConfig.Add("mode", "sandbox");
                    _apiContext = new APIContext(new OAuthTokenCredential(_options.PayPalClientId, _options.PayPalClientSecret, sdkConfig).GetAccessToken());
                    payment.id = paymentId;
                    PaymentExecution paymentExecute = new PaymentExecution();
                    paymentExecute.payer_id = payerId;
                    return payment.Execute(_apiContext, paymentExecute);
                }
                catch (PayPalException ex)
                {
                    _logger.LogError(ex.Message);
                }
                
            }
            return payment;
        }
    }
}
