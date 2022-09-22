namespace PetStoreApi.Domain
{
    public class FilterParam
    {
        public int BreedId { get; set; }
        public int CategoryId { get; set; }

        public FilterParam()
        {
        }
        public FilterParam(int breedId, int categoryId)
        {
            BreedId = breedId;
            CategoryId = categoryId;
        }
    }
}
