using CodingAssessment.Api.DTOs;
using CodingAssessment.Api.Entities;
using CodingAssessment.Api.Helpers;

namespace CodingAssessment.Api.Services
{
    public class Person : IPerson
    {
        private readonly List<PersonEntity> _data;

        public Person()
        {
            _data = new List<PersonEntity>();
        }

        /// <inheritdoc/>
        public async Task<PersonDto> GetById(Guid personId)
        {
            var person = await Task.FromResult(_data.FirstOrDefault(p => p.Id == personId));

            if (person is null) throw new NotFoundException($"Person id {personId} not found.");

            return new PersonDto(person.Id, person.Name, person.Age, person.DateOfBirth);
        }

        /// <inheritdoc/>
        public async Task<CreatePersonResultDto> Insert(CreatePersonDto createPersonDto)
        {
            if (await Task.FromResult(_data.Any(p => p.Name == createPersonDto.Name)))
                throw new ConflictException($"Person name {createPersonDto.Name} already exist");

            if (createPersonDto.DateOfBirth.Year > DateTime.Now.AddYears(1).Year)
                throw new BadRequestException("Invaid date of birth");

            if (GetAge(createPersonDto.DateOfBirth) != createPersonDto.Age)
                throw new BadRequestException("Invaid age");

            var person = new PersonEntity
            {
                Age = createPersonDto.Age,
                Name = createPersonDto.Name,
                DateOfBirth = createPersonDto.DateOfBirth
            };

            _data.Add(person);

            return new CreatePersonResultDto(person.Id);
        }

        /// <inheritdoc/>
        public async Task<PersonsDto> ListPersons()
        {
            var listPersons = await Task.FromResult(_data.Select(person =>
            new PersonDto(person.Id,
                          person.Name,
                          person.Age,
                          person.DateOfBirth)));

            return new PersonsDto
            {
                Persons = listPersons.ToList()
            };
        }


        private int GetAge(DateTime bornDate)
        {
            DateTime today = DateTime.Today;
            int age = today.Year - bornDate.Year;
            if (bornDate > today.AddYears(-age))
                age--;

            return age;
        }
    }
}
