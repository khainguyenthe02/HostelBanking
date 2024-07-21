using HostelBanking.Entities.Const;
using HostelBanking.Entities.DataTransferObjects.Comment;
using HostelBanking.Entities.DataTransferObjects.Favorite;
using HostelBanking.Entities.DataTransferObjects.Report;
using HostelBanking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HostelBanking.Controllers
{
	[Route("api/report")]
	[ApiController]
	public class ReportController : ControllerBase
	{
		private readonly IServiceManager _serviceManager;
		public ReportController(IServiceManager serviceManager)
		{
			this._serviceManager = serviceManager;
		}
		[HttpGet("get-by-id={id}")]
		public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
		{
			ReportSearchDto search = new();
			search.Id = id;
			var result = await _serviceManager.ReportService.Search(search);
			if (result != null)
			{
				return Ok(result.FirstOrDefault());
			}
			return NoContent();
		}
		[HttpPost("create")]
		//[Authorize(Roles = "Admin")]
		public async Task<IActionResult> CreateAsync([FromBody] ReportCreateDto report, CancellationToken cancellationToken)
		{
			var postId = report.PostId;
			var post = await _serviceManager.PostService.GetById(postId);
			if (post == null)
			{
				return BadRequest(MessageError.UserOrPostNotExist);
			}
			var result = await _serviceManager.ReportService.Create(report);
			if (result) return Ok(result);

			return BadRequest(MessageError.ErrorCreate + ":" + result);
		}
		[HttpPost("search")]
		public async Task<IActionResult> SearchDevice([FromBody] ReportSearchDto search, CancellationToken cancellationToken)
		{
			List<ReportDto> result = new();
			result = await _serviceManager.ReportService.Search(search);
			var count = result.Count();
			if (count > 0)
			{
				var pageIndex = search.PageNumber;
				int pageSize = (int)search.PageSize;
				var numberPage = Math.Ceiling((float)(count / pageSize));
				int start = (pageIndex - 1) * pageSize;
				var comments = result.Skip(start).Take(pageSize);
				return Ok(new
				{
					data = comments,
					totalItem = result.Count,
					numberPage,
					search.PageNumber,
					search.PageSize
				});
			}
			return Ok(new List<ReportDto>());
		}
		[HttpPut("update")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> UpdateAsync([FromBody] ReportUpdateDto report, CancellationToken cancellationToken)
		{
			var result = await _serviceManager.ReportService.Update(report);

			if (result) return Ok(result);

			return BadRequest(MessageError.ErrorUpdate);
		}
		[HttpDelete]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
		{
			await _serviceManager.ReportService.Delete(id);
			return NoContent();
		}
	}
}
