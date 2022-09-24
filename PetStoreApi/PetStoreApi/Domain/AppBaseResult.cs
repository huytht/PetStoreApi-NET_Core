namespace PetStoreApi.Domain
{
    public class AppBaseResult
    {
        public bool success { get; set; }
        public int errorCode { get; set; }
        public string message { get; set; }

        public AppBaseResult()
        {
        }
        public AppBaseResult(bool _success, int _errorCode, string _message)
        {
            success = _success;
            errorCode = _errorCode;
            message = _message;
        }
        public static AppBaseResult GenarateIsSucceed()
        {
            return new AppBaseResult(true, 0, "Succeed!");
        }
        public static AppBaseResult GenarateIsFailed(int errorCode, String message)
        {
            return new AppBaseResult(false, errorCode, message);
        }
    }
}
