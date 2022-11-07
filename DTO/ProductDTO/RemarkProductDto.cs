using PetStoreApi.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace PetStoreApi.DTO.ProductDTO
{
    public class RemarkProductDto
    {
        public Guid ProductId { get; set; }
        public string Remark { get; set; }
        public int? Rate { get; set; }
        public string? AvatarImg { get; set; }
        public bool? Favourite { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM-dd-yyyy HH:mm:ss}")]
        public DateTime Date { get; set; }
        public string? Username { get; set; }
        public static RemarkProductDto CreateFromEntity(AppUserProduct src)
        {
            RemarkProductDto dto = new RemarkProductDto();

            dto.ProductId = src.Product.Id;
            dto.Rate = src.Rate;
            dto.Remark = src.Remark;
            dto.Favourite = src.Favourite;
            dto.Date = src.DateModified;
            dto.Username = src.AppUser.Username;
            dto.AvatarImg = src.AppUser.UserInfo.AvatarImg;

            return dto;
        }
    }
}
