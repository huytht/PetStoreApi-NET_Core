using System.Net;

namespace PetStoreApi.DTO.ResponseDTO
{
    public class HttpResponse
    {
        public bool success { get; set; }
        public HttpStatusCode httpStatus { get; set; }

        public HttpResponse(bool success, HttpStatusCode httpStatus)
        {
            this.success = success;
            this.httpStatus = httpStatus;
        }
    }
}
