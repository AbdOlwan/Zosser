using DAL_OnlineStore.Entities.Models.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL_OnlineStore.Repositories.Interfaces.PeopleRepository
{
    public interface IPersonRepo
    {
        Task<List<Person>?> getAllPersons();
        Task<Person> addNewPerson(Person Person);

        // Task<int> countPersons();

        Task<Person?> findPersonByPhonNumber(string phoneNumber);

        Task<Person?> getPersonById(int id);

        Task<bool> deletePersonById(int id);

        Task<bool> updatePersonById(Person Person);
    }
}
