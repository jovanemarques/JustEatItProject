namespace JustEatIt.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public DishAvailability DishAvail { get; set; }

        public int Quantity { get; set; }
    }
}