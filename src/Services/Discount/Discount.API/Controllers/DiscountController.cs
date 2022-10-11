using Discount.API.Entities;
using Discount.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Discount.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DiscountController : ControllerBase
{
    private readonly IDiscountRepository _repository;

    public DiscountController(IDiscountRepository repository)
        => _repository = repository;

    [HttpGet("{productName}", Name = "GetDiscount")]
    [Authorize]
    [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetDiscount(string productName)
        => Ok(await _repository.GetDiscount(productName));

    [HttpPost]
    [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
    [Authorize]
    public async Task<IActionResult> CreateDiscount([FromBody] Coupon coupon)
    {
        await _repository.CreateDiscount(coupon);
        return CreatedAtRoute("GetDiscount", new { productName = coupon.ProductName }, coupon);
    }

    [HttpPut]
    [Authorize]
    [ProducesResponseType(typeof(Coupon), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> UpdateDiscount([FromBody] Coupon coupon)
        => Ok(await _repository.UpdateDiscount(coupon));

    [HttpDelete("{productName}", Name = "DeleteDiscount")]
    [Authorize]
    [ProducesResponseType(typeof(void), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteDiscount(string productName)
        => Ok(await _repository.DeleteDiscount(productName));
}

