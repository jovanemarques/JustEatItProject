using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JustEatIt.Models.ViewModels
{
    public class PartnerDashboardViewModel
    {
        public DateTime Date { get; set; }

        public int DishesMenu { get; set; }

        public int PendingOrders { get; set; }

        public int OrdersFinished { get; set; }

        public int DishesAvailable { get; set; }

        public int DishesSold { get; set; }

        public IEnumerable<JustEatIt.Models.Order> OpenedOrders { get; set; }
    }
}
