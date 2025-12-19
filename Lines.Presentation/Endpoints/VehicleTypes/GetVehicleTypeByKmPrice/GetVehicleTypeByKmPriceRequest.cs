namespace Lines.Presentation.Endpoints.VehicleTypes
{

    public class GetVehicleTypeByKmPriceRequest
    {
        public Guid TripRequestId { get; set; }    // 🆕 ID الخاص بالطلب لتحديد المسافة
        public decimal TotalPrice { get; set; }    // 💰 السعر الكلي للرحلة
    }
}
