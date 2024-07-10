using HostelBanking.Entities.Const;
using HostelBanking.Entities.DataTransferObjects.Account;
using HostelBanking.Entities.DataTransferObjects.HostelType;
using HostelBanking.Entities.DataTransferObjects.Post;
using HostelBanking.Entities.DataTransferObjects.PostImage;
using HostelBanking.Entities.Models.Post;
using HostelBanking.Services.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HostelBanking.Controllers
{
    [Route("api/post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public PostController(IServiceManager serviceManager)
        {
            this._serviceManager = serviceManager;
        }
        [HttpPost("create")]
        //[Authorize]
        public async Task<IActionResult> CreateAsync([FromBody] PostCreateDto postDto, CancellationToken cancellationToken)
        {
            postDto.CreateDate = DateTime.Now.Date;
            postDto.ModifiedDate = DateTime.Now.Date;
			var created = await _serviceManager.PostService.Create(postDto);
			if (created!= null)
			{
			return Ok(created);	
			}
			return BadRequest(MessageError.ErrorCreate);
		}
        [HttpGet("id={id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var result = await _serviceManager.PostService.GetById(id);
            if (result != null)
            {
                return Ok(result);
            }
            return NoContent();
        }
        [HttpGet("get-all-posts")]
        public async Task<IActionResult> GetPosts()
        {
            List<PostDto> postDto;
            postDto = await _serviceManager.PostService.GetAll();
            if (postDto == null) postDto = new();
            return Ok(postDto);
        }
        [HttpPost("search")]
        public async Task<IActionResult> SearchDevice([FromBody] PostSearchDto search, CancellationToken cancellationToken)
        {
            List<PostDto> result = new();
            result = await _serviceManager.PostService.Search(search);
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
			 return Ok(new List<PostDto>());
		}
		[HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            await _serviceManager.PostService.Delete(id);
            return Ok();
        }
    }
}
