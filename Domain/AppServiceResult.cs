using System.Transactions;

namespace PetStoreApi.Domain
{
    public class AppServiceResult<T> : AppBaseResult where T : class
    {
        public T data { get; set; }
        public AppServiceResult()
        {
        }
        public AppServiceResult(bool success, int errorCode, string message, T _data) : base(success, errorCode, message)
        {
            data = _data;
        }
    }
}
