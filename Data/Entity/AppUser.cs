namespace PetStoreApi.Data.Entity
{
    public class AppUser
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool Enabled { get; set; }
        public bool AccountNonLocked { get; set; }
        public Guid UserInfoId { get; set; }
        public UserInfo UserInfo { get; set; }
        public ICollection<AppUserRole> AppUserRoles { get; set; }
        public ICollection<VerificationToken> VerificationTokens { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; }
        public ICollection<AppUserProduct> AppUserProducts { get; set; }
        public AppUser()
        {
            AppUserRoles = new HashSet<AppUserRole>();
            VerificationTokens = new HashSet<VerificationToken>();
            Orders = new HashSet<Order>();
            RefreshTokens = new HashSet<RefreshToken>();
            AppUserProducts = new HashSet<AppUserProduct>();
        }
    }
}
