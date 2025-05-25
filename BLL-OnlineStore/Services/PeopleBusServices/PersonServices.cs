using AutoMapper;
using BLL_OnlineStore.DTOs.EntitiesDTOs.People_F;
using DAL_OnlineStore.Entities.Models.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_OnlineStore.Repositories.Interfaces.PeopleRepository;
using BLL_OnlineStore.Interfaces.PeopleBusServices;

namespace BLL_OnlineStore.Services.PeopleBusServices
{
    public class personServices : IPersonServices
    {
        private readonly IPersonRepo _repo;
        private readonly IMapper _mapper;

        public personServices(IPersonRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;

        }

        public async Task<List<PersonDTO>?> GetAllPersons()
        {
            var Persons = await _repo.getAllPersons();
            if (Persons == null)
                return null;

            return _mapper.Map<List<PersonDTO>>(Persons);

        }

        public async Task<PersonDTO?> GetPersonById(int ID)
        {
            var Person = await _repo.getPersonById(ID);
            if (Person == null) return null;

            return _mapper.Map<PersonDTO>(Person);
        }


       public async Task<PersonDTO?> FindPersonByPhonNumber(string PhonNumber)
        {
            var person = await _repo.findPersonByPhonNumber(PhonNumber);
            if (person == null) return null;
            return _mapper?.Map<PersonDTO>(person);
        }


        public async Task<PersonDTO?> AddNewPerson(PersonDTO DTO)
        {
            var Person = _mapper.Map<Person>(DTO);

            var NewPerson = await _repo.addNewPerson(Person);
            if (NewPerson != null)
            {
                return _mapper.Map<PersonDTO?>(NewPerson);
            }
            return null;
        }
        public async Task<bool> UpdatePersonById(PersonDTO PersonDTO)
        {
            if (PersonDTO == null)
                return false;

            var Person = _mapper.Map<Person>(PersonDTO);
            return await _repo.updatePersonById(Person);
        }
        public async Task<bool> DeletePersonById(int id)
        {
            return await _repo.deletePersonById(id);
        }

    }
}
