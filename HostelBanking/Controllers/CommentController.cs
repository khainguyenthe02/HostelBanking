using HostelBanking.Entities.Const;
using HostelBanking.Entities.DataTransferObjects.Comment;
using HostelBanking.Entities.DataTransferObjects.HostelType;
using HostelBanking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HostelBanking.Controllers
{
    [Route("api/comment")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public CommentController(IServiceManager serviceManager)
        {
            this._serviceManager = serviceManager;
        }
        [HttpGet("id={id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            CommentSearchDto search = new();
            search.Id = id;
            var result = await _serviceManager.CommentService.Search(search);
            if (result != null)
            {
                return Ok(result.FirstOrDefault());
            }
            return NoContent();
        }
        [HttpPost("create")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAsync([FromBody] CommentCreateDto comment, CancellationToken cancellationToken)
        {
            var result = await _serviceManager.CommentService.Create(comment);
            if (result) return Ok(result);

            return BadRequest(MessageError.ErrorCreate + ":" + result);
        }
        [HttpPost("search")]
        public async Task<IActionResult> SearchDevice([FromBody] CommentSearchDto search, CancellationToken cancellationToken)
        {
            List<CommentDto> result = new();
            result = await _serviceManager.CommentService.Search(search);
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
            return Ok(new List<CommentDto>());
        }
        [HttpPut("update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAsync([FromBody] CommentUpdateDto comment, CancellationToken cancellationToken)
        {
            var result = await _serviceManager.CommentService.Update(comment);

            if (result) return Ok(result);

            return BadRequest(MessageError.ErrorUpdate);
        }
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            await _serviceManager.CommentService.Delete(id);
            return NoContent();
        }
    }
}
