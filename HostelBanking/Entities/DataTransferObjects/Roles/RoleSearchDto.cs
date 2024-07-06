namespace HostelBanking.Entities.DataTransferObjects.Roles
{
	public class RoleSearchDto
	{
		public int? Id { get; set; }
		public string? RoleName { get; set; }
		public string? Information { get; set; }
		public List<int> IdLst { get; set; }
	}
}
