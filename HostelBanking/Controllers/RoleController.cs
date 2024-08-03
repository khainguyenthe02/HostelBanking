using HostelBanking.Entities.Const;
using HostelBanking.Entities.DataTransferObjects.HostelType;
using HostelBanking.Entities.DataTransferObjects.Roles;
using HostelBanking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HostelBanking.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class RoleController : ControllerBase
	{
		private readonly IServiceManager _serviceManager;
		public RoleController(IServiceManager serviceManager)
		{
			this._serviceManager = serviceManager;
		}
		[HttpGet("id={id}")]
		public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
		{
			RoleSearchDto search = new();
			search.Id = id;
			var result = await _serviceManager.RoleService.Search(search);
			if (result != null)
			{
				return Ok(result.FirstOrDefault());
			}
			return NoContent();
		}
		[HttpPost("create")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> CreateAsync([FromBody] RoleCreateDto role, CancellationToken cancellationToken)
		{
			var result = await _serviceManager.RoleService.Create( role);
			if (result) return Ok(result);

			return BadRequest(MessageError.ErrorCreate + ":" + result);
		}
		[HttpPost("search")]
		public async Task<IActionResult> SearchDevice([FromBody] RoleSearchDto search, CancellationToken cancellationToken)
		{
			List<RolesDto> result = new();
			result = await _serviceManager.RoleService.Search(search);
			if (result == null) return Ok(new List<RolesDto>());
			return Ok(result);
		}
		[HttpPut("update")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> UpdateAsync([FromBody] RoleUpdateDto role, CancellationToken cancellationToken)
		{
			var result = await _serviceManager.RoleService.Update(role);

			if (result) return Ok(result);

			return BadRequest(MessageError.ErrorUpdate);
		}
		[HttpDelete]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
		{
			await _serviceManager.RoleService.Delete(id);
			return NoContent();
		}
	}
}
