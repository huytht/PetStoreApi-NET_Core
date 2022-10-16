namespace PetStoreApi.DTO.ResponseDTO
{
    public class HttpResponseError : HttpResponse
    {
        public string reason { get; set; }
        public string errorMessage { get; set; }
        public HttpResponseError(string reason, string errorMessage) : base(false, System.Net.HttpStatusCode.BadRequest)
        {
            this.reason = reason;
            this.errorMessage = errorMessage;
        }
    }
}
