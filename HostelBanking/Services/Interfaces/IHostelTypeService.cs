﻿using HostelBanking.Entities.DataTransferObjects.HostelType;

namespace HostelBanking.Services.Interfaces
{
    public interface IHostelTypeService
	{
		Task<bool> Create(HostelTypeCreateDto hostelType);
		Task<List<HostelTypeDto>> GetAll();
		Task<bool> Delete(int id);
		Task<HostelTypeDto> GetById(int id);
		Task<bool> Update(HostelTypeUpdateDto archives);
		Task<List<HostelTypeDto>> Search(HostelTypeSearchDto search);
	}
}
