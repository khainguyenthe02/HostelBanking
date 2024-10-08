﻿using HostelBanking.Entities;
using HostelBanking.Entities.Const;
using HostelBanking.Entities.DataTransferObjects.HostelType;
using HostelBanking.Entities.DataTransferObjects.PayHistory;
using HostelBanking.Entities.DataTransferObjects.Post;
using HostelBanking.Excel;
using HostelBanking.Excel.DtoExcel;
using HostelBanking.Services;
using HostelBanking.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HostelBanking.Controllers
{
    [Route("api/pay-history")]
    [ApiController]
    public class PayHistoryController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;
        public PayHistoryController(IServiceManager serviceManager)
        {
            this._serviceManager = serviceManager;
        }
        [HttpGet("id={id}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            PayHistorySearchDto search = new();
            search.Id = id;
            var result = await _serviceManager.PayHistoryService.Search(search);
            if (result != null)
            {
                return Ok(result.FirstOrDefault());
            }
            return NoContent();
        }


        [HttpGet("getLastPayOfPost")]
        public async Task<IActionResult> GetlastPayOfPost(int id,CancellationToken cancellationToken)
        {
            List<PayHistoryDto> result = new();
            result = await _serviceManager.PayHistoryService.GetlastPayOfPost(id);
            
            var count = result.Count();
            if (count > 0)
            {
                return Ok(result);
            }
            return Ok(new List<PayHistoryDto>());
        }


        [HttpPost("create")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAsync([FromBody] PayHistoryCreateDto payHistory, CancellationToken cancellationToken)
        {
            var result = await _serviceManager.PayHistoryService.Create(payHistory);
            if (result) return Ok(result);

            return BadRequest(MessageError.ErrorCreate + ":" + result);
        }
        [HttpPost("search")]
        public async Task<IActionResult> SearchDevice([FromBody] PayHistorySearchDto search, CancellationToken cancellationToken)
        {
            List<PayHistoryDto> result = new();
            result = await _serviceManager.PayHistoryService.Search(search);
            if (result == null) return Ok(new List<PayHistoryDto>());
            return Ok(result);
        }
		[HttpPost("search-pay-history-by-post-title")]
		public async Task<IActionResult> SearchPostByTitle([FromBody] PayHistoryFeatPostTitleDto search , CancellationToken cancellationToken)
		{
            List<PayHistoryDto> result = new();
            List<PostDto> resultPost = new();

            if (search != null)
            {
                var post = new PostSearchDto
                {
                    Title = search.PostTitle,
                };
                resultPost = await _serviceManager.PostService.Search(post);
                var payHistory = new PayHistorySearchDto
                {
                    Type = search.Type,
                    AccountId = search.AccountId,
                };
                result = await _serviceManager.PayHistoryService.Search(payHistory);
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
            if (result == null) return Ok(new List<PayHistoryDto>());
            return Ok(result);
        }
		[HttpPut("update")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAsync([FromBody] PayHistoryUpdateDto payHistory, CancellationToken cancellationToken)
        {
            PermissionParam permission = new()
            {
                CancellationToken = cancellationToken

            };
            var result = await _serviceManager.PayHistoryService.Update(payHistory);

            if (result) return Ok(result);

            return BadRequest(MessageError.ErrorUpdate);
        }
        [HttpDelete]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            await _serviceManager.PayHistoryService.Delete(id);
            return Ok("Xóa thành công");
        }

		[HttpPost("ExportListPayHistory")]
		public async Task<IActionResult> ExportListArchives([FromBody] PayHistorySearchDto search, CancellationToken cancellationToken)
		{
			// lấy dữ liệu
			var result = await _serviceManager.PayHistoryService.Search(search);
			if (result == null) return Ok(new List<PayHistoryDto>());

			// format dữ liệu export
			var dataExport = new List<PayHistoryExcelDto>();
			PayHistoryExcelDto exportPayHistory = new();
			foreach (var item in result)
			{
				exportPayHistory = new PayHistoryExcelDto();
                exportPayHistory.PayDate = item.PayDate;
				exportPayHistory.PayCode = item.PayCode;
				exportPayHistory.Type = Utils.Convert.PaymentStatusConvert(item.Type.Value);
				exportPayHistory.Price = item.Price;
                exportPayHistory.PostTitle= item.PostTitle;
                exportPayHistory.AccountName= item.AccountName;
				dataExport.Add(exportPayHistory);
			}

			// kết xuất dữ liệu
			var exportService = new ExportService<PayHistoryExcelDto>();
			return File(await exportService.ExportFile(dataExport, "PayHistoryExport.xlsx"), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "LichSuThanhToan.xlsx");
		}
	}
    
}
