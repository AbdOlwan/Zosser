using DAL_OnlineStore.Context;
using DAL_OnlineStore.Entities.Models.ProductModels;
using DAL_OnlineStore.Entities.Models.ShipmentModels;
using DAL_OnlineStore.Repositories.Interfaces.ShipmentRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Repositories.Implementations.ShipmentRepository
{
    public class ShipmentRepo : IShipmentRepo
    {
        private readonly AppDbContext _context;

        public ShipmentRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Shipment>> GetAllShipmentsAsync()
        {
            return await _context.Shipments
                .Include(s => s.Order)
                .Include(s => s.Carrier)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Shipment?> GetShipmentByIdAsync(int id)
        {
            return await _context.Shipments
                .Include(s => s.Order)
                .Include(s => s.Carrier)
                .FirstOrDefaultAsync(s => s.ShipmentID == id);
        }

        public async Task<Shipment> AddShipmentAsync(Shipment shipment)
        {
            await _context.Shipments.AddAsync(shipment);
            await _context.SaveChangesAsync();
            return shipment;
        }

        public async Task UpdateShipmentAsync(Shipment shipment)
        {
            _context.Shipments.Update(shipment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteShipmentAsync(int id)
        {
            var shipment = await _context.Shipments.FindAsync(id);
            if (shipment != null)
            {
                _context.Shipments.Remove(shipment);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ShipmentExistsAsync(int id)
        {
            return await _context.Shipments.AnyAsync(s => s.ShipmentID == id);
        }
    }

}
