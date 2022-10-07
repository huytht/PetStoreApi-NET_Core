namespace PetStoreApi.Data.Entity
{
    public class AppUserRole
    {
        public Guid UserId { get; set; }
        public int RoleId { get; set; }
        public AppUser AppUser { get; set; }
        public AppRole AppRole { get; set; }

        public AppUserRole()
        {
        }
        public AppUserRole(Guid userId, int roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }
    }
}
