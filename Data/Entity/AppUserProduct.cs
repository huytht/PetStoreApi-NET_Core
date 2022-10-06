namespace PetStoreApi.Data.Entity
{
    public class AppUserProduct
    {
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public AppUser AppUser { get; set; }
        public int? Rate { get; set; }
        public string? Remark { get; set; }
        public bool? Favourite { get; set; }
        public DateTime DateModified { get; private set; }
    }
}
