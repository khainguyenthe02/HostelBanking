namespace HostelBanking.Entities.DataTransferObjects.HostelType
{
    public class HostelTypeSearchDto
    {
        public int? Id { get; set; }
        public string? HostelTypeName { get; set; }
		public string? Information { get; set; }
        public List<int>? IdLst { get; set; }
		public int PageNumber { get; set; } = 1;
		public int PageSize { get; set; } = 10;
	}
}

