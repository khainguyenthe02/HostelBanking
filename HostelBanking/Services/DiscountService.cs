	using HostelBanking.Entities.Models;
	using HostelBanking.Services.Interfaces;
	using System.Text.Json;

	namespace HostelBanking.Services
	{
		public class DiscountService : IDiscountService
		{
			private readonly string _filePath;
			public DiscountService()
			{

				_filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "discount.json");
			}
			public async Task<Discount> LoadFromFile()
			{
				string path = Directory.GetCurrentDirectory();
				if (!File.Exists(_filePath))
					{
						return new Discount();
					}
				var json = await File.ReadAllTextAsync(_filePath);
				var discounts = JsonSerializer.Deserialize<Discount>(json);

				return discounts ?? new Discount();
			}

			public async Task<bool> SaveToFile(Discount discount)
			{
			try
			{
				var json = JsonSerializer.Serialize(discount, new JsonSerializerOptions
				{
					WriteIndented = true
				});
				await File.WriteAllTextAsync(_filePath, json);

				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}
	}
}
