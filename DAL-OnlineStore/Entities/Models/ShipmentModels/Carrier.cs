using DAL_OnlineStore.Entities.Models.OrderModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Entities.Models.ShipmentModels
{
    public class Carrier
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public IList<Shipment>? Shipments { get; set; } 

    }
}
