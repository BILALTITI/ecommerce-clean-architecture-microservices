using Asp.Versioning;
using AutoMapper;
using Basket.Application.Commands;
using Basket.Application.Queries;
using Basket.Core.Entities;
using EventBusMessages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers.V2
{


    [ApiVersion("2")]
    [Route("api/v{version:apiversion}/[controller]")]
    [ApiController]
     public class BasketController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;

        private readonly ILogger<BasketController> _logger;

        public BasketController(IMediator mediator, IPublishEndpoint publishEndpoint, IMapper mapper, ILogger<BasketController> logger)
        {


            _mediator = mediator;
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;

            _logger = logger;
        }
        [Route("[action]")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Accepted)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Checkout([FromBody] BasketCheckoutV2 basketCheckOut)
        {
            _logger.LogInformation("Processing checkout for user: {UserName}", basketCheckOut.UserName);

            var query = new GetBasketByUserNameQuery(basketCheckOut.UserName);
            var basket = await _mediator.Send(query);

            if (basket == null)
            {
                _logger.LogWarning("Checkout failed - basket not found for user: {UserName}", basketCheckOut.UserName);
                return BadRequest();
            }

            var eventMessage = _mapper.Map<BasketCheckoutEventV2>(basketCheckOut);
            eventMessage.TotalaPrice = basket.TotalPrice;

            await _publishEndpoint.Publish(eventMessage);
            _logger.LogInformation("Basket checkout event published for user: {UserName} with total price: {TotalPrice} From Version 2",
                basketCheckOut.UserName, basket.TotalPrice);

            var command = new DeleteShoppingCartByUserNameCommand(basketCheckOut.UserName);
            await _mediator.Send(command);

            return Accepted();
        }
    }
}
