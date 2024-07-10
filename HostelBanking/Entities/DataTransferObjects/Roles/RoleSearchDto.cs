namespace HostelBanking.Entities.DataTransferObjects.Roles
{
	public class RoleSearchDto
	{
		public int? Id { get; set; }
		public string? RoleName { get; set; }
		public string? Information { get; set; }
		public List<int> IdLst { get; set; }
		public int PageNumber { get; set; } = 1;
		public int PageSize { get; set; } = 10;
	}
}
