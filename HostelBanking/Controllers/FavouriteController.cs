using HostelBanking.Entities.Const;
using HostelBanking.Entities.DataTransferObjects.Comment;
using HostelBanking.Entities.DataTransferObjects.Favorite;
using HostelBanking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HostelBanking.Controllers
{
    [Route("api/favorite")]
    [ApiController]
    public class FavouriteController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public FavouriteController(IServiceManager serviceManager)
        {
            this._serviceManager = serviceManager;
        }
        [HttpGet("id={id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            FavoriteSearchDto search = new();
            search.Id = id;
            var result = await _serviceManager.FavoriteService.Search(search);
            if (result != null)
            {
                return Ok(result.FirstOrDefault());
            }
            return NoContent();
        }
        [HttpPost("create")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAsync([FromBody] FavoriteCreateDto favorite, CancellationToken cancellationToken)
        {
            FavoriteSearchDto search = new()
            {
                AccountId = favorite.AccountId,
                PostId = favorite.PostId,
            };
            var favoriteExist = await _serviceManager.FavoriteService.Search(search);
            if (favoriteExist != null)
            {
                return BadRequest(MessageError.PostIsFavorited);
            }
            var result = await _serviceManager.FavoriteService.Create(favorite);
            if (result) return Ok(result);

            return BadRequest(MessageError.ErrorCreate + ":" + result);
        }
        [HttpPost("search")]
        public async Task<IActionResult> SearchDevice([FromBody] FavoriteSearchDto search, CancellationToken cancellationToken)
        {
            List<FavoriteDto> result = new();
            result = await _serviceManager.FavoriteService.Search(search);
            var count = result.Count();
            if (count > 0)
            {
                var pageIndex = search.PageNumber;
                int pageSize = (int)search.PageSize;
                var numberPage = Math.Ceiling((float)(count / pageSize));
                int start = (int)(pageIndex - 1) * pageSize;
                var favorites = result.Skip(start).Take(pageSize);
                return Ok(new
                {
                    data = favorites,
                    totalItem = result.Count,
                    numberPage,
                    search.PageNumber,
                    search.PageSize
                });
            }
            return Ok(new List<FavoriteDto>());
        }
        [HttpPut("update")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAsync([FromBody] FavoriteUpdateDto favorite, CancellationToken cancellationToken)
        {
            var result = await _serviceManager.FavoriteService.Update(favorite);

            if (result) return Ok(result);

            return BadRequest(MessageError.ErrorUpdate);
        }
    }
}
