using System;
using System.Collections.Generic;
using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedDevSample.Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductsController(ProductService productService)
        {
            _productService = productService;
        }

        // GET /api/products
        [HttpGet]
        public ActionResult<IReadOnlyList<ProductDto>> GetAll()
        {
            var products = _productService.GetAll();
            return Ok(products);
        }

        // GET /api/products/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<ProductDto> GetById(Guid id)
        {
            try
            {
                var product = _productService.GetById(id);
                return Ok(product);
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST /api/products
        [HttpPost]
        public ActionResult<ProductDto> Create([FromBody] CreateProductRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var created = _productService.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT /api/products/{id}/price
        [HttpPut("{id:guid}/price")]
        public IActionResult ChangePrice(Guid id, [FromBody] ChangePriceRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            try
            {
                _productService.ChangePrice(id, request);
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

        // POST /api/products/{id}/activate
        [HttpPost("{id:guid}/activate")]
        public IActionResult Activate(Guid id)
        {
            try
            {
                _productService.Activate(id);
                return NoContent();
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST /api/products/{id}/deactivate
        [HttpPost("{id:guid}/deactivate")]
        public IActionResult Deactivate(Guid id)
        {
            try
            {
                _productService.Deactivate(id);
                return NoContent();
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}