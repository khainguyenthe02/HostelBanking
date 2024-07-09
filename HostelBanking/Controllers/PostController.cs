using HostelBanking.Entities.Const;
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
        [Authorize]
        public async Task<IActionResult> CreateAsync([FromBody] PostCreateDto postDto, CancellationToken cancellationToken)
        {
			var created = await _serviceManager.PostService.Create(postDto);
			if (created!= null)
			{
			return Ok();	
			}
			return BadRequest(MessageError.ErrorCreate);
		}
    }
}
