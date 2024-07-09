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
        public async Task<IActionResult> CreateAsync([FromBody] PostCreateDto post, CancellationToken cancellationToken)
        {
			var created = await _serviceManager.PostService.Create(postDto);
			if (created!= null)
			{
				var latestPost = await _serviceManager.PostService.GetLatestPost();
				if (latestPost != null)
				{
					var imageList = postDto.ImageList.Select(image =>
					{
						image.PostId = latestPost.Id;
						return image;
					}).ToList();

					var createImageTasks = imageList.Select(image => _serviceManager.PostImageService.Create(image)).ToList();
					var imageResults = await Task.WhenAll(createImageTasks);

					if (imageResults.All(result => result))
					{
						return Ok();
					}
				}
			}
			return BadRequest(MessageError.ErrorCreate);
		}
    }
}
