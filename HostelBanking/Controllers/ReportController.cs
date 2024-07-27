using HostelBanking.Entities.Const;
using HostelBanking.Entities.DataTransferObjects.Comment;
using HostelBanking.Entities.DataTransferObjects.Favorite;
using HostelBanking.Entities.DataTransferObjects.PayHistory;
using HostelBanking.Entities.DataTransferObjects.Post;
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
			var reportSearch = new ReportSearchDto
			{
				PostId = postId,
				AccountId = report.AccountId
			};
			var reportLast = (await _serviceManager.ReportService.Search(reportSearch)).LastOrDefault();
			if(reportLast != null)
			{
				var now = DateTime.Now;
				if(now < reportLast.CreateDate.AddHours(24))
				{
					return BadRequest(MessageError.UserHasReportedYet + reportLast.CreateDate+ ". " + MessageError.ReportLessThan24h);
				}
			}
			var result = await _serviceManager.ReportService.Create(report);
			if (result) return Ok(result);
			return BadRequest(MessageError.ErrorCreate);
		}
		[HttpPost("search-report-by-post-title")]
		public async Task<IActionResult> SearchPostByTitle([FromBody] ReportFeatPostTitleDto search, CancellationToken cancellationToken)
		{
			List<ReportDto> result = new();
			List<PostDto> resultPost = new();

			if (search != null)
			{
				var post = new PostSearchDto
				{
					Title = search.PostTitle,
				};
				resultPost = await _serviceManager.PostService.Search(post);
				var report = new ReportSearchDto
				{
					ReportStatus = search.ReportStatus,
					AccountId = search.AccountId,
				};
				result = await _serviceManager.ReportService.Search(report);
			}
			result = result.Where(ph => resultPost.Any(rp => rp.Id == ph.PostId)).ToList();
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
			if (result == null) return Ok(new List<ReportDto>());
			return Ok(result);
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
		//[Authorize(Roles = "Admin")]
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
