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

        public async Task<AppServiceResult<MomoResponse>> CreatePaymentMomo(double amount, string notifyUrl, string returnUrl)
        {
            try
            {
                HttpClient client = _factory.CreateClient();

                long date = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                string requestId = date + "id";
                string orderId = date + ":012345678";
                bool autoCapture = true;
                string requestType = "captureWallet";
                string orderInfo = "Thanh toán qua ví MoMo";
                string extraData = "ew0KImVtYWlsIjogImh1b25neGRAZ21haWwuY29tIg0KfQ==";
                string signature = "accessKey=" + _options.DevAccessKey + "&amount=" + amount + "&extraData=" + extraData + "&ipnUrl=" + notifyUrl + "&orderId=" + orderId + "&orderInfo=" + orderInfo + "&partnerCode=" + _options.DevPartnerCode + "&redirectUrl=" + returnUrl + "&requestId=" + requestId + "&requestType=" + requestType;
                Mac hMacSHA256 = Mac.getInstance("HmacSHA256");
                byte[] hmacKeyBytes = Encoding.UTF8.GetBytes(_options.DevPartnerCode);
                SecretKeySpec secretKeySpec = new SecretKeySpec(hmacKeyBytes, "HmacSHA256");
                hMacSHA256.init(secretKeySpec);
                byte[] dataBytes = Encoding.UTF8.GetBytes(signature);
                byte[] res = hMacSHA256.doFinal(dataBytes);

                var hexString = BitConverter.ToString(res);
                signature = hexString.Replace("-", "");
                var stringContent = new StringContent(JsonConvert.SerializeObject(new MomoRequest
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
                    autoCapture,
                    orderInfo,
                    requestId,
                    extraData,
                    signature
                )), Encoding.UTF8, "application/json");
                
                var response = await client.PostAsync(_options.DevMomoEndpoint + "/create", stringContent);
                var responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("============> " + responseContent);
                var result = JsonConvert.DeserializeObject<MomoResponse>(responseContent);

                return new AppServiceResult<MomoResponse>(true, 0, "Succeed!", result);

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return new AppServiceResult<MomoResponse>(false, 99, "Unknown", null);
            }
        }
    }
}
