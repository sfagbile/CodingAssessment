using System.ComponentModel.DataAnnotations;

namespace CodingAssessment.Api.DTOs;
public record CreatePersonDto([Required] string Name, [Required] int Age, [Required] DateTime DateOfBirth);

