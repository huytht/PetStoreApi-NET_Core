namespace PetStoreApi.Data.Entity
{
    public class VerificationToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public bool IsSend { get; set; }
        public bool IsVerify { get; set; }
        public DateTime? VerifyDate { get; set; }
        public Guid UserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
