using HostelBanking.Entities.Const;
using HostelBanking.Entities.DataTransferObjects.Account;
using HostelBanking.Entities.DataTransferObjects.HostelType;
using HostelBanking.Services;
using HostelBanking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HostelBanking.Controllers
{
    [ApiController]
	[Route("api/[controller]")]
	public class HostelTypeController : ControllerBase
	{
		private readonly IServiceManager _serviceManager;
		public HostelTypeController(IServiceManager serviceManager)
		{
			this._serviceManager = serviceManager;
		}
		[HttpGet("id={id}")]
		public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
		{
			HostelTypeSearchDto search = new();
			search.Id = id;
			var result = await _serviceManager.HostelTypeService.Search(search);
			if (result != null)
			{
				return Ok(result.FirstOrDefault());
			}
			return NoContent();
		}
		[HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAsync([FromBody] HostelTypeCreateDto hostelType, CancellationToken cancellationToken)
		{
			var result = await _serviceManager.HostelTypeService.Create(hostelType);
			if (result) return Ok(result);

			return BadRequest(MessageError.ErrorCreate + ":" + result);
		}
		[HttpPost("search")]
        public async Task<IActionResult> SearchDevice([FromBody] HostelTypeSearchDto search, CancellationToken cancellationToken)
		{
			List<HostelTypeDto> result = new();
			result = await _serviceManager.HostelTypeService.Search(search);
			var count = result.Count();
			if (count > 0)
			{
				var pageIndex = search.PageNumber;
				int pageSize = (int)search.PageSize;
				var numberPage = Math.Ceiling((float)(count / pageSize));
				int start = (pageIndex - 1) * pageSize;
				var post = result.Skip(start).Take(pageSize);
				return Ok(new
				{
					data = post,
					totalItem = result.Count,
					numberPage,
					search.PageNumber,
					search.PageSize
				});
			}
			return Ok(new List<HostelTypeDto>());
		}
		[HttpPut("update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAsync([FromBody] HostelTypeUpdateDto hostelType, CancellationToken cancellationToken)
		{
			var result = await _serviceManager.HostelTypeService.Update(hostelType);

			if (result) return Ok(result);

			return BadRequest(MessageError.ErrorUpdate);
		}
		[HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
		{
			await _serviceManager.HostelTypeService.Delete(id);
			return NoContent();
		}
	}
}
