namespace PetStoreApi.Data.Entity
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public AppUser AppUser { get; set; }
        public string Token { get; set; }
        public string JwtId { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime IssuedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}
