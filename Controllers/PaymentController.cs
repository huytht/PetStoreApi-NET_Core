using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PayPal;
using PayPal.Api;
using PetStoreApi.Domain;
using PetStoreApi.DTO.MomoDTO;
using PetStoreApi.Services;
using System.Collections.Specialized;
using System.Text;

namespace PetStoreApi.Controllers
{
    [ApiController]
    [Route("api/pay")]
    public class PaymentController : ControllerBase
    {
        public const string URL_PAYPAL_SUCCESS = "api/pay/success";
        public const string URL_PAYPAL_CANCEL = "api/pay/cancel";
        private readonly Variable _options;
        private readonly IPaypalRepository _paypalRepository;
        private readonly IMomoRepository _momoRepository;
        private readonly ILogger<PaymentController> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IOrderRepository _orderRepository;

        public PaymentController(IPaypalRepository paypalRepository, ILogger<PaymentController> logger, IWebHostEnvironment hostingEnvironment, IOrderRepository orderRepository, IOptions<Variable> options, IMomoRepository momoRepository)
        {
            _paypalRepository = paypalRepository;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _orderRepository = orderRepository;
            _options = options.Value;
            _momoRepository = momoRepository;
        }

        [HttpPost("paypal")]
        public IActionResult CreatePaymentPaypal(double amount, Guid orderTrackingNumber)
        {
            try
            {
                string cancelUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/{URL_PAYPAL_CANCEL}";
                string successUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/{URL_PAYPAL_SUCCESS}";
                var payment = _paypalRepository.CreatePayment(amount, successUrl + "?orderTrackingNumber=" + orderTrackingNumber, cancelUrl, "sale");
                foreach (Links links in payment.links)
                {
                    if (links.rel.Equals("approval_url"))
                    {
                        return Ok(links.href);
                    }
                }
            }
            catch (PayPalException e)
            {
                _logger.LogError(e.Message);
                return BadRequest(e.Message);
            }
            return BadRequest();
        }
        [HttpPost("momo")]
        public async Task<IActionResult> CreatePaymentMomo(long amount, Guid orderTrackingNumber)
        {
            string cancelUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/{URL_PAYPAL_CANCEL}";
            string successUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/{URL_PAYPAL_SUCCESS}";

            AppServiceResult<object> result = await _momoRepository.CreatePaymentMomo(amount, cancelUrl, successUrl + "?orderTrackingNumber=" + orderTrackingNumber);

            return result.success ? Ok(result) : BadRequest(result);
        }
        [HttpGet("cancel")]
        public IActionResult CancelPay()
        {
            string tempateFilePath = _hostingEnvironment.ContentRootPath + "/Templates/cancel.html";

            return Content(System.IO.File.ReadAllText(tempateFilePath).Replace("HOME_PAGE_CLIENT_URL", _options.HomePageClient), "text/html", Encoding.UTF8);
        }

        [HttpGet("success")]
        public IActionResult SuccessPay(Guid orderTrackingNumber, string? paymentId = "", string? token = "", string? PayerID = "")
        {
            Payment payment = _paypalRepository.ExecutePayment(paymentId, PayerID);

            _orderRepository.UpdateOrderStatus(orderTrackingNumber, 2);

            string tempateFilePath = _hostingEnvironment.ContentRootPath + "/Templates/success.html";

            return Content(System.IO.File.ReadAllText(tempateFilePath).Replace("HOME_PAGE_CLIENT_URL", _options.HomePageClient), "text/html", Encoding.UTF8);
        }
    }
}
