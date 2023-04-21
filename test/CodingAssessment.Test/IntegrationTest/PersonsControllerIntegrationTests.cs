using CodingAssessment.Api.DTOs;
using CodingAssessment.Api.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Json;

namespace CodingAssessment.Test.IntegrationTest
{
    public class PersonsControllerIntegrationTests : WebApplicationFactory<Program>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public PersonsControllerIntegrationTests()
        {
            _factory = new WebApplicationFactory<Program>();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => { });
        }

        [Fact]
        public async Task GetById_ReturnsOkResult()
        {
            // Arrange
            var personId = Guid.NewGuid();
            var person = new PersonDto(personId, "John Doe", 38, DateTime.Now.AddYears(-38));
            var mockPersonService = new Mock<IPerson>();
            mockPersonService.Setup(x => x.GetById(personId)).ReturnsAsync(person);
            HttpClient client = CreateClient(mockPersonService);

            // Act
            var response = await client.GetAsync($"/api/codingassesment/v1/persons/{personId}");

            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<PersonDto>(responseContent);

            Assert.NotNull(result);
            Assert.Equal(personId, result.Id);
            Assert.Equal(person.Name, result.Name);
            Assert.Equal(person.Age, result.Age);
            Assert.Equal(person.DateOfBirth, result.DateOfBirth);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var createPersonDto = new Api.DTOs.CreatePersonDto("John Doe", 38, Convert.ToDateTime("1985-09-04"));
            var createPersonResultDto = new CreatePersonResultDto(Guid.NewGuid());
            var mockPersonService = new Mock<IPerson>();
            mockPersonService.Setup(x => x.Insert(createPersonDto)).ReturnsAsync(createPersonResultDto);
            HttpClient client = CreateClient(mockPersonService);

            // Act
            var response = await client.PostAsJsonAsync("/api/codingassesment/v1/persons", createPersonDto);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CreatePersonResultDto>(responseContent);

            Assert.NotNull(result);
            Assert.NotNull(result?.PersonId);
            Assert.Equal(createPersonResultDto.PersonId, result.PersonId);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task GetPersons_ReturnsOkResult()
        {
            //Arrange
            var personsDto = new PersonsDto
            {
                Persons = new List<PersonDto>
                {
                    new PersonDto(Guid.NewGuid(), "John Doe1", 38, DateTime.Now.AddYears(-38)),
                    new PersonDto(Guid.NewGuid(), "John Doe2", 39, DateTime.Now.AddYears(-39)),
                    new PersonDto(Guid.NewGuid(), "John Doe3", 39, DateTime.Now.AddYears(-39)),
                }
            };

            var mockPersonService = new Mock<IPerson>();
            mockPersonService.Setup(x => x.ListPersons()).ReturnsAsync(personsDto);

            HttpClient client = CreateClient(mockPersonService);

            //Act
            var response = await client.GetAsync("/api/codingassesment/v1/persons");

            //Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<PersonsDto>(responseContent);
            Assert.Equal(personsDto.Persons.Count, result.Persons.Count);
        }

        private HttpClient CreateClient(Mock<IPerson> mockPersonService)
        {
            return _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddSingleton(mockPersonService.Object);
                });
            }).CreateClient();
        }
    }
}