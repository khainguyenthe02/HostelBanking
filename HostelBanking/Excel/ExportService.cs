using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Reflection;

namespace HostelBanking.Excel
{
	public class ExportService<T> : IExportService<T>
	{
		public async Task<byte[]> ExportFile(List<T> obj, string fileTemplate)
		{
			var memoryStream = new MemoryStream();
			try
			{
				var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), "ExcelFileTemplate");
				// Tạo thư mục nếu chưa tồn tại
				if (!Directory.Exists(pathToSave))
				{
					Directory.CreateDirectory(pathToSave);
				}
				Log.Information(pathToSave);

				using (var fs = new FileStream(Path.Combine(pathToSave, fileTemplate), FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 4096, useAsync: true))
				{

					// Open the template file as an XSSFWorkbook
					XSSFWorkbook templateWorkbook = new XSSFWorkbook(fs);
					ISheet excelSheet = templateWorkbook.GetSheetAt(0);
					// Save the new workbook to the specified output file path
					IRow row;
					int countRow = 0;// Hàng
					foreach (T item in obj)
					{
						countRow++;
						row = excelSheet.CreateRow(countRow);
						row.CreateCell(0).SetCellValue(countRow); //Thêm cột số thứ tự
						int cell = 1;
						//int cell = 0;
						foreach (var property in item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
						{
							var value = property.GetValue(item);
							if (value != null)
							{
								if (value is DateTime)
								{
									if ((DateTime)value == DateTime.MinValue)
									{
										value = new DateTime(2000, 1, 1);
									}
								}
								if (value is string)
								{
									value = value.ToString().Trim();
								}


							}
							else
							{
								value = "";
							}
							row.CreateCell(cell).SetCellValue(value.ToString());
							cell++;
						}
					}

					templateWorkbook.Write(memoryStream);
					templateWorkbook.Close();
				}
				return memoryStream.ToArray();
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message);
				return memoryStream.ToArray();
			}
		}
	}
}
