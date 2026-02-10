using System;
using AdvancedDevSample.Application.DTOs;
using AdvancedDevSample.Domain.Exceptions;
using AdvancedDevSample.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedDevSample.Api.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        // POST /api/orders
        [HttpPost]
        public ActionResult<OrderDto> Create([FromBody] CreateOrderRequest? request)
        {
            var created = _orderService.Create(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // GET /api/orders/{id}
        [HttpGet("{id:guid}")]
        public ActionResult<OrderDto> GetById(Guid id)
        {
            try
            {
                var order = _orderService.GetById(id);
                return Ok(order);
            }
            catch (ApplicationServiceException ex)
            {
                return NotFound(ex.Message);
            }
        }

        // POST /api/orders/{id}/lines
        [HttpPost("{id:guid}/lines")]
        public IActionResult AddLine(Guid id, [FromBody] AddOrderLineRequest request)
        {
            try
            {
                _orderService.AddLine(id, request);
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

        // PUT /api/orders/{id}/lines
        [HttpPut("{id:guid}/lines")]
        public IActionResult ChangeLineQuantity(Guid id, [FromBody] ChangeOrderLineQuantityRequest request)
        {
            try
            {
                _orderService.ChangeLineQuantity(id, request);
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

        // DELETE /api/orders/{id}/lines/{productId}
        [HttpDelete("{id:guid}/lines/{productId:guid}")]
        public IActionResult RemoveLine(Guid id, Guid productId)
        {
            try
            {
                _orderService.RemoveLine(id, productId);
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

        // POST /api/orders/{id}/confirm
        [HttpPost("{id:guid}/confirm")]
        public IActionResult Confirm(Guid id)
        {
            try
            {
                _orderService.Confirm(id);
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

        // POST /api/orders/{id}/cancel
        [HttpPost("{id:guid}/cancel")]
        public IActionResult Cancel(Guid id)
        {
            try
            {
                _orderService.Cancel(id);
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
    }
}

