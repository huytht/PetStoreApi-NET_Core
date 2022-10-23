namespace PetStoreApi.Services
{
    public interface IRepositoryWrapper
    {
        IBreedRepository breedRepository { get; }
        ICategoryRepository categoryRepository { get; }
        IOriginRepository originRepository { get; }
        Task SaveAsync();
    }
}
