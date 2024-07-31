using HostelBanking.Entities.Models;
using HostelBanking.Services;
using HostelBanking.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HostelBanking.Controllers
{
	[Route("api/discount-config")]
	[ApiController]
	public class DiscountConfigController : ControllerBase
	{
		private readonly IDiscountService _discountService;

		public DiscountConfigController(IDiscountService discountService)
		{
			_discountService = discountService;
		}
		[HttpGet("get-all")]
		public async Task<IActionResult> GetDiscounts()
		{
			var discounts = await _discountService.LoadFromFile();
			return Ok(discounts);
		}
		[HttpPost("create-discount-config")]
		public async Task<IActionResult> AddDiscount([FromBody] Discount discount)
		{
			if (discount == null)
			{
				return BadRequest("Invalid discount data.");
			}
			var result = await _discountService.SaveToFile(discount);
			if(result) return Ok(discount);
			return BadRequest();
		}
	}
}
