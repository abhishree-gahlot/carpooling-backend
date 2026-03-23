namespace CarpoolingSystem.Application.DTOs
{
    public class PaymentConfirmationDto
    {
        public Guid RideRequestId { get; set; }
        public Guid PassengerId { get; set; }
    }
}