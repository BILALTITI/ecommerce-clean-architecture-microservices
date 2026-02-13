using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Commands;
using Ordering.Application.Queries;
using Ordering.Application.Responses;
using System.Net;

namespace Ordering.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : BaseApiController
    {
        private readonly ILogger<OrderController> _logger;

        private readonly IMediator _mediator;

        public OrderController(ILogger<OrderController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }
        [HttpGet("{userName}", Name = "GetOrderByUserName")]
        [ProducesResponseType(typeof(IEnumerable<OrderResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOrderByUserName(string userName)
        {
            _logger.LogInformation("GetOrderByUserName request received. UserName: {UserName}", userName);

            if (string.IsNullOrWhiteSpace(userName))
            {
                _logger.LogWarning("GetOrderByUserName failed - Invalid userName: {UserName}", userName);
                return BadRequest("UserName parameter cannot be null or empty.");
            }

            try
            {
                var query = new GetOrderListQueries(userName);
                var orderList = await _mediator.Send(query);

                if (orderList == null || !orderList.Any())
                {
                    _logger.LogWarning("No orders found for user: {UserName}", userName);
                    return NotFound();
                }

                _logger.LogInformation("Orders retrieved successfully. UserName: {UserName}, OrdersCount: {OrdersCount}", 
                    userName, orderList.Count());
                return Ok(orderList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving orders. UserName: {UserName}", userName);
                throw;
            }
        }

        [HttpPost(Name = "CheckoutOrder")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<int>> CheckoutOrder([FromBody] CheckoutOrderCommand command)
        {
            _logger.LogInformation("CheckoutOrder request received. UserName: {UserName}, TotalPrice: {TotalPrice}", 
                command?.UserName, command?.TotalPrice);

            if (command == null)
            {
                _logger.LogWarning("CheckoutOrder failed - Request body is null");
                return BadRequest("Order data cannot be null.");
            }

            try
            {
                var result = await _mediator.Send(command);

                _logger.LogInformation("Order checkout completed successfully. OrderId: {OrderId}, UserName: {UserName}, TotalPrice: {TotalPrice}", 
                    result, command.UserName, command.TotalPrice);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during order checkout. UserName: {UserName}", command?.UserName);
                throw;
            }
        }


        [HttpPut(Name = "UpdateOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult<int>> UpdateOrder([FromBody] UpdateOrderCommand command)
        {
            _logger.LogInformation("UpdateOrder request received. OrderId: {OrderId}, UserName: {UserName}", 
                command?.Id, command?.UserName);

            if (command == null)
            {
                _logger.LogWarning("UpdateOrder failed - Request body is null");
                return BadRequest("Order data cannot be null.");
            }

            try
            {
                var result = await _mediator.Send(command);

                _logger.LogInformation("Order updated successfully. OrderId: {OrderId}", command.Id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order. OrderId: {OrderId}", command?.Id);
                throw;
            }
        }


        [HttpDelete(Name = "DeleteOrder")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<ActionResult> DeleteOrder([FromBody] DeleteOrderCommand command)
        {
            _logger.LogInformation("DeleteOrder request received. OrderId: {OrderId}", command?.Id);

            if (command == null)
            {
                _logger.LogWarning("DeleteOrder failed - Request body is null");
                return BadRequest("Order data cannot be null.");
            }

            try
            {
                var result = await _mediator.Send(command);

                _logger.LogInformation("Order deleted successfully. OrderId: {OrderId}", command.Id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order. OrderId: {OrderId}", command?.Id);
                throw;
            }
        }
    }
}
