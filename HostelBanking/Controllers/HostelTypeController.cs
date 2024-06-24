using HostelBanking.Entities.DataTransferObjects;
using HostelBanking.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HostelBanking.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class HostelTypeController : ControllerBase
	{
		private readonly IServiceManager serviceManager;
		public HostelTypeController(IServiceManager serviceManager)
		{
			this.serviceManager = serviceManager;
		}
		[HttpGet("id={id}")]
		public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
		{
			HostelTypeSearchDto search = new();
			search.Id = id;
			var result = await serviceManager.HostelTypeService.Search(search);
			if (result != null)
			{
				return Ok(result.FirstOrDefault());
			}
			return NoContent();
		}
	}
}
