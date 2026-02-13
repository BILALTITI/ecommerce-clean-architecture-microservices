using AutoMapper;
using Basket.API.Controllers;
using Basket.Application.Commands;
using Basket.Application.Queries;
using Basket.Application.Responses;
using Basket.Core.Entities;
using EventBusMessages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Runtime.ExceptionServices;

public class BasketController : BaseApiController
{
    private readonly IMediator _mediator;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IMapper _mapper;

    private readonly ILogger<BasketController> _logger;

    public BasketController(IMediator mediator,IPublishEndpoint publishEndpoint,IMapper mapper,ILogger<BasketController> logger)
    {


        _mediator = mediator;
        _publishEndpoint = publishEndpoint;
        _mapper = mapper;

_logger = logger;       
    }

    [HttpGet("[action]/{userName}")]
    [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetBasketByUserName(string userName)
    {
        _logger.LogInformation("GetBasketByUserName request received. UserName: {UserName}", userName);

        if (string.IsNullOrWhiteSpace(userName))
        {
            _logger.LogWarning("GetBasketByUserName failed - Invalid userName: {UserName}", userName);
            return BadRequest("UserName parameter cannot be null or empty.");
        }

        try
        {
            var query = new GetBasketByUserNameQuery(userName);
            var basket = await _mediator.Send(query);

            if (basket == null)
            {
                _logger.LogWarning("Basket not found for user: {UserName}", userName);
                return NotFound();
            }

            _logger.LogInformation("Basket retrieved successfully. UserName: {UserName}, ItemsCount: {ItemsCount}, TotalPrice: {TotalPrice}", 
                userName, basket.Items?.Count ?? 0, basket.TotalPrice);
            return Ok(basket);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving basket. UserName: {UserName}", userName);
            throw;
        }
    }

    [HttpPost("CreateBasket")]
    [ProducesResponseType(typeof(ShoppingCartResponse), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> CreateBasket(
        [FromBody] CreateShoppingCartCommand shoppingCart)
    {
        _logger.LogInformation("CreateBasket request received. UserName: {UserName}, ItemsCount: {ItemsCount}", 
            shoppingCart?.UserName, shoppingCart?.Items?.Count ?? 0);

        if (shoppingCart == null)
        {
            _logger.LogWarning("CreateBasket failed - Request body is null");
            return BadRequest("Shopping cart data cannot be null.");
        }

        try
        {
            var result = await _mediator.Send(shoppingCart);

            _logger.LogInformation("Basket created successfully. UserName: {UserName}, ItemsCount: {ItemsCount}, TotalPrice: {TotalPrice}", 
                result?.UserName, result?.Items?.Count ?? 0, result?.TotalPrice);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating basket. UserName: {UserName}", shoppingCart?.UserName);
            throw;
        }
    }

    [HttpDelete("[action]/{userName}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    public async Task<IActionResult> DeleteBasketByUserName(string userName)
    {
        _logger.LogInformation("DeleteBasketByUserName request received. UserName: {UserName}", userName);

        if (string.IsNullOrWhiteSpace(userName))
        {
            _logger.LogWarning("DeleteBasketByUserName failed - Invalid userName: {UserName}", userName);
            return BadRequest("UserName parameter cannot be null or empty.");
        }

        try
        {
            var command = new DeleteShoppingCartByUserNameCommand(userName);
            await _mediator.Send(command);

            _logger.LogInformation("Basket deleted successfully. UserName: {UserName}", userName);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting basket. UserName: {UserName}", userName);
            throw;
        }
    }


    [Route("[action]")]
    [HttpPost]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Checkout([FromBody] BasketCheckOut basketCheckOut)
    {
        _logger.LogInformation("Processing checkout for user: {UserName}", basketCheckOut.UserName);

        var query = new GetBasketByUserNameQuery(basketCheckOut.UserName);
        var basket = await _mediator.Send(query);

        if (basket == null)
        {
            _logger.LogWarning("Checkout failed - basket not found for user: {UserName}", basketCheckOut.UserName);
            return BadRequest();
        }

        var eventMessage = _mapper.Map<BasketCheckoutEvent>(basketCheckOut);
        eventMessage.TotalPrice = basket.TotalPrice;

        await _publishEndpoint.Publish(eventMessage);
        _logger.LogInformation("Basket checkout event published for user: {UserName} with total price: {TotalPrice}",
            basketCheckOut.UserName, basket.TotalPrice);

        var command = new DeleteShoppingCartByUserNameCommand(basketCheckOut.UserName);
        await _mediator.Send(command);

        return Accepted();
    }
}
