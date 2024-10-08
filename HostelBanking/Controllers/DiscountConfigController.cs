﻿using HostelBanking.Entities.Const;
using HostelBanking.Entities.Models;
using HostelBanking.Services;
using HostelBanking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetDiscounts()
		{
			var discounts = await _discountService.LoadFromFile();
			return Ok(discounts);
		}
		[HttpPost("create-discount-config")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> AddDiscount([FromBody] Discount discount)
		{
			if (discount == null)
			{
				return BadRequest("Invalid discount data.");
			}
			if (discount.CreatedPrice <= 0 || discount.UpdatedPrice <= 0)
			{
				return BadRequest(MessageError.CreatedOrUpdatedPriceNotEqualsZero);
			}
			if (discount.CountPostToSale <= 0)
			{
				return BadRequest(MessageError.CountPostToSaleNotEqualsZero);
			}
			var result = await _discountService.SaveToFile(discount);
			if(result) return Ok(discount);
			return BadRequest();
		}
	}
}
