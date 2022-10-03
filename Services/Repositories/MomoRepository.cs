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
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                //long date = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                //string requestId = DateTimeOffset.Now.ToUnixTimeMilliseconds() + "id";
                //string orderId = DateTimeOffset.Now.ToUnixTimeMilliseconds() + ":012345678";
                //bool autoCapture = true;
                //string requestType = "captureWallet";
                string orderInfo = "Thanh toán qua ví MoMo";
                //string extraData = "ew0KImVtYWlsIjogImh1b25neGRAZ21haWwuY29tIg0KfQ==";
                string signature = "accessKey=" + _options.DevAccessKey + "&amount=" + amount + "&extraData=ew0KImVtYWlsIjogImh1b25neGRAZ21haWwuY29tIg0KfQ==&ipnUrl=" + notifyUrl + "&orderId=" + DateTimeOffset.Now.ToUnixTimeMilliseconds() + ":012345678" + "&orderInfo=" + orderInfo + "&partnerCode=" + _options.DevPartnerCode + "&redirectUrl=" + returnUrl + "&requestId=" + DateTimeOffset.Now.ToUnixTimeMilliseconds() + "id" + "&requestType=captureWallet";
                Mac hMacSHA256 = Mac.getInstance("HmacSHA256");
                byte[] hmacKeyBytes = Encoding.UTF8.GetBytes(_options.DevPartnerCode);
                SecretKeySpec secretKeySpec = new SecretKeySpec(hmacKeyBytes, "HmacSHA256");
                hMacSHA256.init(secretKeySpec);
                byte[] dataBytes = Encoding.UTF8.GetBytes(signature);
                byte[] res = hMacSHA256.doFinal(dataBytes);

                var hexString = BitConverter.ToString(res);
                signature = hexString.Replace("-", "");
                Console.WriteLine("=====================> " + signature);
                var myRequest = new MomoRequest
                (
                    _options.DevPartnerCode,
                    "Test",
                    _options.DevPartnerCode,
                    "captureWallet",
                    notifyUrl,
                    returnUrl,
                    DateTimeOffset.Now.ToUnixTimeMilliseconds() + ":012345678",
                    amount,
                    "vi",
                    true,
                    "Thanh toán qua ví MoMo",
                    DateTimeOffset.Now.ToUnixTimeMilliseconds() + "id",
                    "ew0KImVtYWlsIjogImh1b25neGRAZ21haWwuY29tIg0KfQ==",
                    signature
                );
                //var stringContent = new StringContent(JsonConvert.SerializeObject(myRequest), Encoding.UTF8, "application/json");
                var jsonRequest = JsonConvert.SerializeObject(myRequest);
                byte[] requestBytes = Encoding.UTF8.GetBytes(jsonRequest);
                var content = new ByteArrayContent(requestBytes);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

                var response = client.PostAsync(_options.DevMomoEndpoint + "/create", content).Result;
                var responseContent = response.Content.ReadAsStringAsync().Result;
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
