﻿using HostelBanking.Entities.DataTransferObjects.Account;
using HostelBanking.Entities.DataTransferObjects.Comment;
using HostelBanking.Entities.DataTransferObjects.Post;
using HostelBanking.Entities.DataTransferObjects.Report;
using HostelBanking.Entities.Enum;
using HostelBanking.Entities.Models.Comment;
using HostelBanking.Entities.Models.Report;
using HostelBanking.Repositories.Interfaces;
using HostelBanking.Services.Interfaces;
using Mapster;

namespace HostelBanking.Services
{
	public class ReportService : IReportService
	{
		private readonly IRepositoryManager _repositoryManager;
		public ReportService(IRepositoryManager repositoryManager)
		{
			this._repositoryManager = repositoryManager;
		}
        public async Task<bool> Create(ReportCreateDto report)
        {
            var hostelTypeInfo = report.Adapt<Report>();
            hostelTypeInfo.CreateDate = DateTime.Now;
            hostelTypeInfo.ReportStatus = (int) ReportStatus.PENDING;
            var result = await _repositoryManager.ReportRepository.Create(hostelTypeInfo);
            return result;
        }

        public async Task<bool> Delete(int id)
        {
            ReportSearchDto search = new()
            {
                Id = id,
            };
            var hostelTypeInfo = await _repositoryManager.ReportRepository.GetById((int)id);
            if (hostelTypeInfo != null)
            {
                var hostelTypeUpdate = new Report();
                hostelTypeUpdate.Id = id;
                hostelTypeUpdate.DeleteFlag = true;
                hostelTypeUpdate.CreateDate = hostelTypeInfo.CreateDate;
                var result = await _repositoryManager.ReportRepository.Update(hostelTypeUpdate);
                return true;
            }
            return false;
        }

        public async Task<List<ReportDto>> GetAll()
        {
            var result = await _repositoryManager.ReportRepository.GetAll();
            var resultDto = result.Adapt<List<ReportDto>>();
            return await FilterData(resultDto);
        }

        public async Task<ReportDto> GetById(int id)
        {
            var result = await _repositoryManager.ReportRepository.GetById(id);
            var resultDto = result.Adapt<ReportDto>();
            return (await FilterData(new() { resultDto })).FirstOrDefault();
        }

        public async Task<List<ReportDto>> Search(ReportSearchDto search)
        {
            var result = await _repositoryManager.ReportRepository.Search(search);
            var resultDto = result.Adapt<List<ReportDto>>();
            return await FilterData(resultDto);
        }

        public async Task<bool> Update(ReportUpdateDto report)
        {
            var reportModel = await _repositoryManager.ReportRepository.GetById((int)report.Id);
            if(reportModel != null)
            {
				var hostelTypeInfo = report.Adapt<Report>();
                hostelTypeInfo.CreateDate = reportModel.CreateDate;
				var result = await _repositoryManager.ReportRepository.Update(hostelTypeInfo);
                if (result)
                {
                    var reportSearch = new ReportSearchDto
                    {
                        PostId = reportModel.PostId,
                        ReportStatus = (int)ReportStatus.ACCEPTED
                    };
                    var reportAcceptedList = await _repositoryManager.ReportRepository.Search(reportSearch);
                    if(reportAcceptedList.Count > 10)
                    {
                        var deletePost = await _repositoryManager.PostRepository.Delete(hostelTypeInfo.PostId);
                        if (deletePost)
                        {
                            var userReports = await _repositoryManager.ReportRepository.Search(new ReportSearchDto
                            {
                                AccountId = reportModel.AccountId,
                                ReportStatus = (int)ReportStatus.ACCEPTED
                            });
                            var distinctPostIds = userReports.Select(r => r.PostId).Distinct().ToList();
                            if (distinctPostIds.Count > 10)
                            {
                                var deleteUser = await _repositoryManager.UserRepository.Delete(reportModel.AccountId);
                                return deleteUser;
                            }
                        }
                    }
                    return true;
                }
			}
            return false;
        }
        public async Task<List<ReportDto>> FilterData(List<ReportDto> lst)
        {
            if (lst?.Count > 0)
            {
                var userIdLst = lst.Where(x => x.AccountId.HasValue).Select(x => x.AccountId.GetValueOrDefault()).ToList();
                if (userIdLst.Count > 0)
                {
                    var searchUser = new UserSearchDto()
                    {
                        IdLst = userIdLst
                    };
                    var users = (await _repositoryManager.UserRepository.Search(searchUser))?.ToDictionary(x => x.Id, x => x.FullName);
                    if (users?.Count > 0)
                    {
                        foreach (var item in lst)
                        {
                            if (item.AccountId.HasValue && users.ContainsKey(item.AccountId.Value))
                            {
                                item.AccountName = users[item.AccountId.Value];
                            }
                        }
                    }
                }
                var postIdLst = lst.Where(x => x.PostId.HasValue).Select(x => x.PostId.GetValueOrDefault()).ToList();
                if (postIdLst.Count > 0)
                {
                    var searchPost = new PostSearchDto()
                    {
                        IdLst = postIdLst
                    };
                    var posts = (await _repositoryManager.PostRepository.Search(searchPost))?.ToDictionary(x => x.Id, x => x.Title);
                    if (posts?.Count > 0)
                    {
                        foreach (var item in lst)
                        {
                            if (item.PostId.HasValue && posts.ContainsKey(item.PostId.Value))
                            {
                                item.PostTitle = posts[item.PostId.Value];
                            }
                        }
                    }
                }
            }
            return lst;
        }
    }
}
