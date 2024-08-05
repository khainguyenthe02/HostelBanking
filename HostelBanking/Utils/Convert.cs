using System.Text;

namespace HostelBanking.Utils
{
	public class Convert
	{
		public static string GetMD5Hash(string input)
		{
			try
			{
				System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
				byte[] bs = Encoding.UTF8.GetBytes(input);
				bs = x.ComputeHash(bs);

				var s = new StringBuilder();
				foreach (byte b in bs)
				{
					s.Append(b.ToString("x2"));
				}

				string password = s.ToString();
				return password;
			}
			catch (Exception ex)
			{
				return ex.Message;
			}
		}
		public static string PaymentStatusConvert(int status)
		{
			switch (status)
			{
				case 0:
					return "Chờ thanh toán";
				case 1:
					return "Đã thanh toán";
				default:
					return "";
			}
		}
	}
}
