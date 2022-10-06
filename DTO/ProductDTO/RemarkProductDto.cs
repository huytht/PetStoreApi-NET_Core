using PetStoreApi.Data.Entity;

namespace PetStoreApi.DTO.ProductDTO
{
    public class RemarkProductDto
    {
        public Guid ProductId { get; set; }
        public string Remark { get; set; }
        public int Rate { get; set; }

        public bool? Favourite { get; set; }
        public DateTime? Date { get; set; }
        public static RemarkProductDto CreateFromEntity(AppUserProduct src)
        {
            RemarkProductDto dto = new RemarkProductDto();

            dto.ProductId = src.Product.Id;
            dto.Remark = src.Remark;
            dto.Favourite = src.Favourite;
            dto.Date = src.DateModified;

            return dto;
        }
    }
}
