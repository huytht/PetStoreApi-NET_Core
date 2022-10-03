namespace PetStoreApi.DTO.MomoDTO
{
    public class MomoRequest
    {
        public string partnerCode { get; set; }
        public string partnerName { get; set; }
        public string storeId { get; set; }
        public string requestType { get; set; }
        public string ipnUrl { get; set; }
        public string redirectUrl { get; set; }
        public string orderId { get; set; }
        public double amount { get; set; }
        public string lang { get; set; }
        public bool autoCapture { get; set; }
        public string orderInfo { get; set; }
        public string requestId { get; set; }
        public string extraData { get; set; }
        public string signature { get; set; }

        public MomoRequest(string partnerCode, string partnerName, string storeId, string requestType, string ipnUrl, string redirectUrl, string orderId, double amount, string lang, bool autoCapture, string orderInfo, string requestId, string extraData, string signature)
        {
            this.partnerCode = partnerCode;
            this.partnerName = partnerName;
            this.storeId = storeId;
            this.requestType = requestType;
            this.ipnUrl = ipnUrl;
            this.redirectUrl = redirectUrl;
            this.orderId = orderId;
            this.amount = amount;
            this.lang = lang;
            this.autoCapture = autoCapture;
            this.orderInfo = orderInfo;
            this.requestId = requestId;
            this.extraData = extraData;
            this.signature = signature;
        }
    }
}
