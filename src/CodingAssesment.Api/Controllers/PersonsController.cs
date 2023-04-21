﻿using CodingAssessment.Api.DTOs;
using CodingAssessment.Api.Helpers;
using CodingAssessment.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodingAssessment.Api.Controllers
{
    [Route("api/codingassesment/v1/persons")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IPerson _person;

        public PersonsController(IPerson person)
        {
            _person = person;
        }

        /// <summary>
        /// Get person by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{personId}")]
        [ProducesResponseType(typeof(PersonDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetById(Guid personId)
        {
            return Ok(await _person.GetById(personId));
        }

        /// <summary>
        /// Create new person
        /// </summary>
        /// <param name="createPersonDto"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(PersonDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Create([FromBody] CreatePersonDto createPersonDto)
        {
            var person = await _person.Insert(createPersonDto);
            return CreatedAtAction(nameof(GetById), new { personId = person.Id }, new { personId = person.Id });
        }

        /// <summary>
        /// Get persons list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(PersonsDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetPersons()
        {
            var persons = await _person.ListPersons();
            return Ok(persons);
        }

    }
}
