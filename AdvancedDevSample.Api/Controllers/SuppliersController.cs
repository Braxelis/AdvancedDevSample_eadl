using System;
using System.Collections.Generic;
using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Application.Services;
using AdvancedDevSample.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedDevSample.Api.Controllers
{
    [ApiController]
    [Route("api/suppliers")]
    public class SuppliersController : ControllerBase
    {
        private readonly SupplierService _supplierService;

        public SuppliersController(SupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        // GET /api/suppliers
        [HttpGet]
        public ActionResult<IReadOnlyList<SupplierDto>> GetAll()
        {
            var suppliers = _supplierService.GetAll();
            return Ok(suppliers);
        }

        // GET /api/suppliers/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<SupplierDto> GetById(Guid id)
        {
            try
            {
                var supplier = _supplierService.GetById(id);
                return Ok(supplier);
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST /api/suppliers
        [HttpPost]
        public ActionResult<SupplierDto> Create([FromBody] CreateSupplierRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                var created = _supplierService.Create(request);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (DomainException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT /api/suppliers/{id}
        [HttpPut("{id:guid}")]
        public IActionResult Update(Guid id, [FromBody] UpdateSupplierRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                _supplierService.Update(id, request);
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

        // DELETE /api/suppliers/{id}
        [HttpDelete("{id:guid}")]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _supplierService.Delete(id);
                return NoContent();
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST /api/suppliers/{id}/activate
        [HttpPost("{id:guid}/activate")]
        public IActionResult Activate(Guid id)
        {
            try
            {
                _supplierService.Activate(id);
                return NoContent();
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST /api/suppliers/{id}/deactivate
        [HttpPost("{id:guid}/deactivate")]
        public IActionResult Deactivate(Guid id)
        {
            try
            {
                _supplierService.Deactivate(id);
                return NoContent();
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
