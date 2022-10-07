using AutoMapper;
using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _repository;
    private readonly IMapper _mapping;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly DiscountGrpcService _discountGrpcService;

    public BasketController(IBasketRepository repository,
        DiscountGrpcService discountGrpcService,
        IMapper mapping,
        IPublishEndpoint publishEndpoint)
    {
        _repository = repository;
        _discountGrpcService = discountGrpcService;
        _mapping = mapping;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet("{userName}", Name = "GetBasket")]
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetBasket(string userName)
        => Ok((await _repository.GetBasket(userName)) ?? new ShoppingCart(userName));

    [HttpPost]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateBasket([FromBody] ShoppingCart basket)
    {

        foreach (var item in basket.Items)
        {
            var coupoun = await _discountGrpcService
                .GetDiscount(item.ProductName);

            item.Price -= coupoun.Amount;
        };

        return Ok(await _repository.UpdateBasket(basket));
    }

    [HttpDelete("{userName}", Name = "DeleteBasket")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteBasket(string userName)
    {
        await _repository.DeleteBasket(userName);
        return Ok();
    }

    [HttpPost]
    [Route("[action]")]
    [ProducesResponseType((int)HttpStatusCode.Accepted)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
    {
        var basket = await _repository.GetBasket(basketCheckout.UserName);
        if (basket == null)
            return BadRequest();

        var eventMessage = _mapping.Map<BasketCheckoutEvent>(basketCheckout);
        eventMessage.TotalPrice = basket.TotalPrice;

        await _publishEndpoint.Publish(eventMessage);

        await _repository.DeleteBasket(basket.UserName);

        return Accepted();
    }
}

