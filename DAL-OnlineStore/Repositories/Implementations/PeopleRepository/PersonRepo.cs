using DAL_OnlineStore.Context;
using DAL_OnlineStore.Entities.Models.People;
using DAL_OnlineStore.Repositories.Interfaces.PeopleRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Repositories.Implementations.PeopleRepository
{
    public class PersonRepo : IPersonRepo
    {
        private readonly AppDbContext _context;

        public PersonRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Person>?> getAllPersons()
        {
            return await _context.Persons
                                    .AsNoTracking()
                                    .ToListAsync();
        }

        public async Task<Person?> getPersonById(int Id)
        {
            return await _context.Persons.AsNoTracking()
                                            .FirstOrDefaultAsync(a => a.PersonId == Id);
        }
        public async Task<Person?> findPersonByPhonNumber(string phoneNumber)
        {
            return await _context.Persons
                .AsNoTracking()                 
                .FirstOrDefaultAsync(p => p.PhoneNumber == phoneNumber);
        }
        public async Task<Person> addNewPerson(Person Person)
        {
            await _context.Persons.AddAsync(Person);
            await _context.SaveChangesAsync();
            return Person;
        }
        public async Task<bool> deletePersonById(int id)
        {
            var Person = await _context.Persons.FirstOrDefaultAsync(d => d.PersonId == id);
            if (Person == null)
            {
                return false;
            }

            _context.Persons.Remove(Person);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> updatePersonById(Person Person)
        {
            var result = await _context.Persons.FirstOrDefaultAsync(d => d.PersonId == Person.PersonId);
            if (result != null)
            {
                _context.Entry(result).CurrentValues.SetValues(Person);

                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

    }
}
