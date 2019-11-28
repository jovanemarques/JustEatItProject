using System.ComponentModel.DataAnnotations.Schema;

namespace JustEatIt.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        [ForeignKey("DishAvail")]
        public int DishAvailabilityId { get; set; }  

        public virtual DishAvailability DishAvail { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }

        public virtual Order Order { get; set; }

        public int Quantity { get; set; }
    }
}