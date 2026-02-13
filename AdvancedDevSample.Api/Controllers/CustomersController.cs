using System;
using System.Collections.Generic;
using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Services;
using AdvancedDevSample.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedDevSample.Api.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerService _customerService;

        public CustomersController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET /api/customers
        [HttpGet]
        public ActionResult<IReadOnlyList<CustomerDto>> GetAll()
        {
            var customers = _customerService.GetAll();
            return Ok(customers);
        }

        // GET /api/customers/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<CustomerDto> GetById(Guid id)
        {
            try
            {
                var customer = _customerService.GetById(id);
                return Ok(customer);
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST /api/customers
        [HttpPost]
        public ActionResult<CustomerDto> Create([FromBody] CreateCustomerRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                var created = _customerService.Create(request);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT /api/customers/{id}
        [HttpPut("{id:guid}")]
        public IActionResult Update(Guid id, [FromBody] UpdateCustomerRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                _customerService.Update(id, request);
                return NoContent();
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(ex.Message);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE /api/customers/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _customerService.Delete(id);
                return NoContent();
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST /api/customers/{id}/activate
        [HttpPost("{id:guid}/activate")]
        public IActionResult Activate(Guid id)
        {
            try
            {
                _customerService.Activate(id);
                return NoContent();
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST /api/customers/{id}/deactivate
        [HttpPost("{id:guid}/deactivate")]
        public IActionResult Deactivate(Guid id)
        {
            try
            {
                _customerService.Deactivate(id);
                return NoContent();
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
