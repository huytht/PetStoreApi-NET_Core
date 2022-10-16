namespace PetStoreApi.DTO.ResponseDTO
{
    public class HttpResponseSuccess<T> : HttpResponse where T : class 
    {
        public T data { get; set; }
        public HttpResponseSuccess(T data) : base(true, System.Net.HttpStatusCode.OK)
        {
            this.data = data;
        }
    }
}
