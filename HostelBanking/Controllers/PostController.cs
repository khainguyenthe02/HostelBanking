using HostelBanking.Entities.Const;
using HostelBanking.Entities.DataTransferObjects.Account;
using HostelBanking.Entities.DataTransferObjects.HostelType;
using HostelBanking.Entities.DataTransferObjects.Post;
using HostelBanking.Entities.Models.Post;
using HostelBanking.Services.Interfaces;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace HostelBanking.Controllers
{
    [Route("api/post")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        private readonly IDiscountService _discountService;
        public PostController(IServiceManager serviceManager, IDiscountService discountService)
        {
            this._serviceManager = serviceManager;
            this._discountService = discountService;
        }
        [HttpPost("create")]
        //[Authorize]
        public async Task<IActionResult> CreateAsync([FromBody] PostCreateDto postDto, CancellationToken cancellationToken)
        {
            postDto.ModifiedDate = DateTime.Now;
			var created = await _serviceManager.PostService.Create(postDto);
			if (created!= null)
			{

                List<PostDto> result = new();
                result = await _serviceManager.PostService.GetAll();
                var count = result.Count();
                if (count > 0)
                {
                    created.Id=(int)result.Last().Id;
                    return Ok(created);
                }

               	
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
        [HttpGet("get-price-after-discount")]
        public async Task<IActionResult> GetPrice([FromBody] int userId)
        {
            var discount = await _discountService.LoadFromFile();
            var user = await _serviceManager.UserService.GetById(userId);
            if(user == null)
            {
                return BadRequest(MessageError.UserOrPostNotExist);
            }
            var postSearch = new PostSearchDto
            {
                AccountId = userId
            };
            var listPost = await _serviceManager.PostService.Search(postSearch);
            if (listPost == null || listPost.Count == 0)
            {
                return Ok(discount.CreatedPrice);
			}
            else
            {
                int multiple = listPost.Count / discount.CountPostToSale;
                if (multiple > 1)
                {
                    multiple = multiple * (discount.PercentSale/100);
                    if(multiple > 50)
                    {
                        multiple = 50;
                    }
                }
                discount.CreatedPrice = discount.CreatedPrice - discount.CreatedPrice;

			}

        }
        [HttpPost("search")]
        public async Task<IActionResult> SearchDevice([FromBody] PostSearchDto search, CancellationToken cancellationToken)
        {
            List<PostDto> result = new();
            result = await _serviceManager.PostService.Search(search);
            result= result.OrderByDescending(p=>p.ModifiedDate).ToList();
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




        [HttpPost("searchManager")]
        public async Task<IActionResult> SearchManager([FromBody] PostSearchDto search, CancellationToken cancellationToken)
        {
            List<PostDto> result = new();
            result = await _serviceManager.PostService.SearchManager(search);
            result = result.OrderByDescending(p => p.ModifiedDate).ToList();
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
            return Ok("Xóa thành công");
        }


        [HttpPut("update-post")]
        public async Task<IActionResult> UpdatePost([FromBody] PostUpdateDto postUpdateDto, CancellationToken cancellationToken)
        {
            
            var post = await _serviceManager.PostService.GetByIdUpdate((int)postUpdateDto.Id);
            if (post == null) return StatusCode((int)HttpStatusCode.BadRequest, "Bài viết không tồn tại");
            
            if (await _serviceManager.PostService.Update(postUpdateDto))
            {
                return Ok();
            }
            return BadRequest(MessageError.ErrorUpdate);
        }

        [HttpGet("newest")]
        public async Task<IActionResult> Newest( CancellationToken cancellationToken)
        {
            List<PostDto> result = new();
            result = await _serviceManager.PostService.GetNewest();
            var count = result.Count();
            if (count > 0)
            {
                
                return Ok(result);
            }
            return Ok(new List<PostDto>());
        }

        [HttpGet("mostView")]
        public async Task<IActionResult> MostView(CancellationToken cancellationToken)
        {
            List<PostDto> result = new();
            result = await _serviceManager.PostService.GetMostView();
            var count = result.Count();
            if (count > 0)
            {

                return Ok(result);
            }
            return Ok(new List<PostDto>());
        }
    }
}
