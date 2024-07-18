namespace HostelBanking.Entities.Enum
{
	public enum AccountStatus
	{
		INACTIVE,
		ACTIVE,
		BLOCK
	}
	public enum PaymentStatus
	{
		PENDING,
		PAID
	}
	public enum PriceRange
	{
		ALL,
		BELOW_ONE,
		ONE_TO_TWO,
		TWO_TO_FOUR,
		FOUR_TO_SIX,
		SIX_TO_EIGHT,
		EIGHT_TO_TEN,
		ABOVE_TEN,
		AGREEMENT
    }
	public enum AcreageRange
	{
		ALL,
		BELOW_TWENTY,
		TWENTY_TO_FORTY,
		FORTY_TO_SIXTY,
		SIXTY_TO_EIGHTY,
		EIGHTY_TO_HUNDRED,
		ABOVE_HUNDRED
	}
	public enum ReportStatus
	{
		PENDING,
		ACCEPTED,
		REJECTED
	}
	public enum PayHistoryStatus
	{
		CREATE,
		UPDATE
	}
}
