namespace PetStoreApi.Data.Entity
{
    public class AppRole
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<AppUserRole> AppUserRoles { get; set; }

        public AppRole()
        {
            AppUserRoles = new HashSet<AppUserRole>();
        }
    }
}
