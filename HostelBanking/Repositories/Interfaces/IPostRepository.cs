﻿using HostelBanking.Entities.DataTransferObjects.HostelType;
using HostelBanking.Entities.DataTransferObjects.Post;
using HostelBanking.Entities.Models.HostelType;
using HostelBanking.Entities.Models.Post;

namespace HostelBanking.Repositories.Interfaces
{
	public interface IPostRepository
	{
		Task<int> Create(Post post);
		Task<List<Post>> GetAll();
		Task<List<Post>> GetNewest();
        Task<List<Post>> GetMostView();
        Task<Post> GetLatestPost();
		Task<bool> Update(Post post);
		Task<bool> Delete(int id);
		Task<Post> GetById(int id);
		Task<List<Post>> Search(PostSearchDto search);
		Task<List<Post>> SearchManager(PostSearchDto search);
    }
}
