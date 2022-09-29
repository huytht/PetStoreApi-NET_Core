namespace PetStoreApi.DTO.PurchaseDTO
{
    public class PurchaseResponse
    {
        public Guid OrderTrackingNumber { get; set; }
        public DateTime DateCreated { get; set; }

        public PurchaseResponse(Guid orderTrackingNumber, DateTime dateCreated)
        {
            OrderTrackingNumber = orderTrackingNumber;
            DateCreated = dateCreated;
        }
    }
}
