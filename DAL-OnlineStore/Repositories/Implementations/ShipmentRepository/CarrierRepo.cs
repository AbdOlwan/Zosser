using DAL_OnlineStore.Context;
using DAL_OnlineStore.Entities.Models.ProductModels;
using DAL_OnlineStore.Entities.Models.ShipmentModels;
using DAL_OnlineStore.Repositories.Interfaces.ShipmentRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Repositories.Implementations.ShipmentRepository
{
    public class CarrierRepo : ICarrierRepo
    {
        private readonly AppDbContext _context;

        public CarrierRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Carrier>?> getAllCarriers()
        {
            return await _context.Carriers
                                    .AsNoTracking()
                                    .ToListAsync();
        }

        public async Task<Carrier?> getCarrierById(int CarrierId)
        {
            return await _context.Carriers.AsNoTracking()
                                            .FirstOrDefaultAsync(a => a.Id == CarrierId);
        }

        public async Task<Carrier> addNewCarrier(Carrier carrier)
        {
            await _context.Carriers.AddAsync(carrier);
            await _context.SaveChangesAsync();
            return carrier;
        }
        public async Task<bool> deleteCarrierById(int id)
        {
            var Carrier = await _context.Carriers.FirstOrDefaultAsync(d => d.Id == id);
            if (Carrier == null)
            {
                return false;
            }

            _context.Carriers.Remove(Carrier);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> updateCarrierById(Carrier carrier)
        {
            var result = await _context.Carriers.FirstOrDefaultAsync(d => d.Id == carrier.Id);
            if (result != null)
            {
                result.Id = carrier.Id;
                _context.Entry(result).CurrentValues.SetValues(carrier);

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }


        //public async Task<int> countCarriers()
        //{
        //    throw new NotImplementedException();
        //}

        //public async Task<List<Carrier>?> getDoctorAppointments(int doctorId)
        //{
        //    return await _context.Appointments.Where(a => a.DoctorID == doctorId)
        //                .AsNoTracking()
        //                .ToListAsync();
        //}

        //public async Task<List<Appointment>?> getPatientAppointments(int patientId)
        //{
        //    return await _context.Appointments.Where(a => a.PatientID == patientId)
        //                                             .AsNoTracking()
        //                                             .ToListAsync();
        //}

    }
}
