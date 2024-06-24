namespace HostelBanking.Entities.Models
{
	public class Base
	{
		public int Id { set; get; } // Id
		public bool? DeleteFlag { set; get; }

		public Base()
		{
			DeleteFlag = false;
		}
	}
}
