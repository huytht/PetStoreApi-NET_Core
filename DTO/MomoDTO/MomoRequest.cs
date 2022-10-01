namespace PetStoreApi.DTO.MomoDTO
{
    public class MomoRequest
    {
        public string PartnerCode { get; set; }
        public string PartnerName { get; set; }
        public string StoreId { get; set; }
        public string RequestType { get; set; }
        public string IpnUrl { get; set; }
        public string RedirectUrl { get; set; }
        public string OrderId { get; set; }
        public double Amount { get; set; }
        public string Lang { get; set; }
        public bool AutoCapture { get; set; }
        public string OrderInfo { get; set; }
        public string RequestId { get; set; }
        public string ExtraData { get; set; }
        public string Signature { get; set; }

        public MomoRequest(string partnerCode, string partnerName, string storeId, string requestType, string ipnUrl, string redirectUrl, string orderId, double amount, string lang, bool autoCapture, string orderInfo, string requestId, string extraData, string signature)
        {
            PartnerCode = partnerCode;
            PartnerName = partnerName;
            StoreId = storeId;
            RequestType = requestType;
            IpnUrl = ipnUrl;
            RedirectUrl = redirectUrl;
            OrderId = orderId;
            Amount = amount;
            Lang = lang;
            AutoCapture = autoCapture;
            OrderInfo = orderInfo;
            RequestId = requestId;
            ExtraData = extraData;
            Signature = signature;
        }
    }
}
