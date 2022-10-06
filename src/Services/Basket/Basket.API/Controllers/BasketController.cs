using Basket.API.Entities;
using Basket.API.GrpcServices;
using Basket.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Basket.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BasketController : ControllerBase
{
    private readonly IBasketRepository _repository;
    private readonly DiscountGrpcService _discountGrpcService;

    public BasketController(IBasketRepository repository,
        DiscountGrpcService discountGrpcService)
    {
        _repository = repository;
        _discountGrpcService = discountGrpcService;
    }

    [HttpGet("{userName}", Name = "GetBasket")]
    [ProducesResponseType(typeof(ShoppingCart), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetBasket(string userName)
        => Ok((await _repository.GetBasket(userName)) ?? new ShoppingCart(userName));

    [HttpPost]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateBasket([FromBody] ShoppingCart basket)
    {
        basket.Items.ToList().ForEach(async i =>
        {
            var coupoun = await _discountGrpcService
                .GetDiscount(i.ProductName);

            i.Price -= coupoun.Amount;
        });

        return Ok(await _repository.UpdateBasket(basket));
    }

    [HttpDelete("{userName}", Name = "DeleteBasket")]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteBasket(string userName)
    {
        await _repository.DeleteBasket(userName);
        return Ok();
    }
}

