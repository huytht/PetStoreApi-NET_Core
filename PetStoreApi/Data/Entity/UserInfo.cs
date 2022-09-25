namespace PetStoreApi.Data.Entity
{
    public class UserInfo
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Phone { get; set; }
        public string AvatarImg { get; set; }
        public AppUser AppUser { get; set; }
    }
}
