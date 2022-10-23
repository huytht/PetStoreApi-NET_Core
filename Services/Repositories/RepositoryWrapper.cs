using PetStoreApi.Helpers;

namespace PetStoreApi.Services.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private DataContext _context;

        private IBreedRepository _breedRepository;
        private ICategoryRepository _categoryRepository;
        private IOriginRepository _originRepository;
        public IBreedRepository breedRepository
        {
            get
            {
                if (_breedRepository == null)
                {
                    _breedRepository = new BreedRepository(_context);
                }
                return _breedRepository;
            }
        }
        public ICategoryRepository categoryRepository
        {
            get
            {
                if (_categoryRepository == null)
                {
                    _categoryRepository = new CategoryRepository(_context);
                }
                return _categoryRepository;
            }
        }
        public IOriginRepository originRepository
        {
            get
            {
                if (_originRepository == null)
                {
                    _originRepository = new OriginRepository(_context);
                }
                return _originRepository;
            }
        }
        public RepositoryWrapper(DataContext context)
        {
            _context = context;
        }
        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();  
        }
    }
}
