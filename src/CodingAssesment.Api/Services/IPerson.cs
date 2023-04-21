using CodingAssessment.Api.DTOs;

namespace CodingAssessment.Api.Services
{
    public interface IPerson
    {
        /// <summary>
        /// Get person by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PersonDto> GetById(Guid id);

        /// <summary>
        /// Insert new person
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        Task<CreatePersonResultDto> Insert(CreatePersonDto person);

        /// <summary>
        /// Get all person
        /// </summary>
        /// <returns></returns>
        Task<PersonsDto> ListPersons();
    }
}
