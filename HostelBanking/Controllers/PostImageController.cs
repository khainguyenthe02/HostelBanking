using HostelBanking.Entities;
using HostelBanking.Entities.Const;
using HostelBanking.Entities.DataTransferObjects.HostelType;
using HostelBanking.Entities.DataTransferObjects.PostImage;
using HostelBanking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HostelBanking.Controllers
{
    [Route("api/post-image")]
    [ApiController]
    public class PostImageController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public PostImageController(IServiceManager serviceManager)
        {
            this._serviceManager = serviceManager;
        }
        [HttpGet("id={id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            PostImageSearchDto search = new();
            search.Id = id;
            var result = await _serviceManager.PostImageService.Search(search);
            if (result != null)
            {
                return Ok(result.FirstOrDefault());
            }
            return NoContent();
        }
        [HttpPost("create")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAsync([FromBody] PostImageCreateDto postImage, CancellationToken cancellationToken)
        {
            var result = await _serviceManager.PostImageService.Create(postImage);
            if (result) return Ok(result);

            return BadRequest(MessageError.ErrorCreate + ":" + result);
        }
        [HttpPost("search")]
        public async Task<IActionResult> SearchDevice([FromBody] PostImageSearchDto search, CancellationToken cancellationToken)
        {
            List<PostImageDto> result = new();
            result = await _serviceManager.PostImageService.Search(search);
            if (result == null) return Ok(new List<PostImageDto>());
            return Ok(result);
        }
        [HttpPut("update")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAsync([FromBody] PostImageUpdateDto postImage, CancellationToken cancellationToken)
        {
			PermissionParam permission = new()
			{
				CancellationToken = cancellationToken

			};
			var result = await _serviceManager.PostImageService.Update(postImage);

            if (result) return Ok(result);

            return BadRequest(MessageError.ErrorUpdate);
        }
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            await _serviceManager.PostImageService.Delete(id);
            return NoContent();
        }
    }
}
