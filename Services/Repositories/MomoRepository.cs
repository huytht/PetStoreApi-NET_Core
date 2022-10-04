using Microsoft.Extensions.Options;
using Org.BouncyCastle.Asn1.Crmf;
using Org.BouncyCastle.Utilities.Encoders;
using PetStoreApi.Domain;
using PetStoreApi.DTO.MomoDTO;
using System.Security.Cryptography;
using System;
using Org.BouncyCastle.Crypto.Tls;
using java.nio.charset;
using javax.crypto.spec;
using javax.crypto;
using System.Text;
using Newtonsoft.Json;
using static com.sun.tools.@internal.xjc.reader.xmlschema.bindinfo.BIConversion;
using sun.misc;

namespace PetStoreApi.Services.Repositories
{
    public class MomoRepository : IMomoRepository
    {
        private IHttpClientFactory _factory;
        private readonly MomoAuthOption _options;
        private readonly ILogger<MomoRepository> _logger;

        public MomoRepository(IHttpClientFactory factory, IOptions<MomoAuthOption> options, ILogger<MomoRepository> logger)
        {
            _factory = factory;
            _options = options.Value;
            _logger = logger;
        }

        public async Task<AppServiceResult<object>> CreatePaymentMomo(long amount, string notifyUrl, string returnUrl)
        {
            try
            {
                HttpClient client = _factory.CreateClient();
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                long date = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                string requestId = date + "id";
                string orderId = date + ":012345678";
                string requestType = "captureWallet";
                string orderInfo = "Thanh toán qua ví MoMo";
                string extraData = "ew0KImVtYWlsIjogImh1b25neGRAZ21haWwuY29tIg0KfQ==";
                string signature = "accessKey=" + _options.DevAccessKey + "&amount=" + amount + "&extraData=" + extraData + "&ipnUrl=" + notifyUrl + "&orderId=" + orderId + "&orderInfo=" + orderInfo + "&partnerCode=" + _options.DevPartnerCode + "&redirectUrl=" + returnUrl + "&requestId=" + requestId + "&requestType=" + requestType;

                byte[] hmacKeyBytes = Encoding.UTF8.GetBytes(_options.DevSecretKey);
                var sha256Hash = new HMACSHA256(hmacKeyBytes);
                byte[] dataBytes = Encoding.UTF8.GetBytes(signature);
                byte[] res = sha256Hash.ComputeHash(dataBytes);

                signature = Hex.ToHexString(res);

                var myRequest = new MomoRequest
                (
                    _options.DevPartnerCode,
                    "Test",
                    _options.DevPartnerCode,
                    requestType,
                    notifyUrl,
                    returnUrl,
                    orderId,
                    amount,
                    "vi",
                    true,
                    orderInfo,
                    requestId,
                    extraData,
                    signature
                );
                //var stringContent = new StringContent(JsonConvert.SerializeObject(myRequest), Encoding.UTF8, "application/json");
                var jsonRequest = JsonConvert.SerializeObject(myRequest);
                byte[] requestBytes = Encoding.UTF8.GetBytes(jsonRequest);
                var content = new ByteArrayContent(requestBytes);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                var response = client.PostAsync(_options.DevMomoEndpoint + "/create", content).Result;
                var responseContent = response.Content.ReadAsStringAsync().Result;

                return new AppServiceResult<object>(true, 0, "Succeed!", JsonConvert.DeserializeObject(responseContent));

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new AppServiceResult<object>(false, 99, "Unknown", null);
            }
        }
    }
}
