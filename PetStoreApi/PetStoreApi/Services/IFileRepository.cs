namespace PetStoreApi.Services
{
    public interface IFileRepository
    {
        string Upload(string fileName, IFormFile file);
    }
}
