using BLL_OnlineStore.DTOs.EntitiesDTOs.People_F;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL_OnlineStore.Interfaces.PeopleBusServices
{
    public interface IPersonServices
    {
        Task<List<PersonDTO>?> GetAllPersons();
        Task<PersonDTO?> AddNewPerson(PersonDTO Person);

        // Task<int> CountPersons();
        Task<PersonDTO?> FindPersonByPhonNumber(string PhonNumber);
        Task<PersonDTO?> GetPersonById(int id);

        Task<bool> DeletePersonById(int id);

        Task<bool> UpdatePersonById(PersonDTO Person);
    }
}
