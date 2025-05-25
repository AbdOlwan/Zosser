using DAL_OnlineStore.Entities.Models.OrderModels;
using DAL_OnlineStore.Entities.Models.ReviewModels;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Entities.Models.People
{
    public class Customer 
    {
        public int CustomerId { get; set; }
        public int PersonId { get; set; }
        // ربط مع AspNetUsers
        public IList<Order>? orders { get; set; }
        public IList<CustomerAddress>? addresses { get; set; } 
        public IList<Review>? reviews { get; set; }
        public Person Person { get; set; } = null!;

    }
}
