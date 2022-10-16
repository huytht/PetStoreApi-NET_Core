using System.ComponentModel.DataAnnotations;
using System.Net;

namespace PetStoreApi.DTO.ResponseDTO
{
    public class HttpResponse
    {
        public bool success { get; set; }
        public string httpStatus { get; set; }
        public HttpStatusCode httpStatusCode { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM-dd-yyyy HH:mm:ss}")]
        public DateTime timestamp { get; set; } = DateTime.UtcNow;
        public HttpResponse(bool success, HttpStatusCode httpStatusCode)
        {
            this.success = success;
            this.httpStatus = httpStatusCode.ToString();
            this.httpStatusCode = httpStatusCode;
        }
    }
}
