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
            var createPost = await _serviceManager.PostService.Create(post);
            if (createPost)
            {
                var postInfo = post.Adapt<Post>();
                var postdto = await _serviceManager.PostService.GetById(postInfo.Id);
                var postImageLst = post.ImageList;
                foreach (var image in postImageLst)
                {
                    image.PostId = postInfo.Id;
                    var result = await _serviceManager.PostImageService.Create(image);
                    if (!result)
                    {
                        return BadRequest(MessageError.PostImageError + image.ImageUrl);
                    }
                }
                return Ok();
            }
            return BadRequest(MessageError.ErrorCreate);
        }
    }
}
