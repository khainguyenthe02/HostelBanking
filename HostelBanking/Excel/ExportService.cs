using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Serilog;

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
                    // Create header row
                    IRow headerRow = excelSheet.CreateRow(0);
                    // Create a bold font and a style for the header row
                    IFont boldFont = templateWorkbook.CreateFont();
                    boldFont.IsBold = true;

                    ICellStyle headerStyle = templateWorkbook.CreateCellStyle();
                    headerStyle.SetFont(boldFont);
                    headerStyle.FillForegroundColor = IndexedColors.Grey25Percent.Index;
                    headerStyle.FillPattern = FillPattern.SolidForeground;

                    // Set header cell values and apply style
                    headerRow.CreateCell(0).SetCellValue("No.");
                    headerRow.GetCell(0).CellStyle = headerStyle;

                    int headerCell = 1;
                    foreach (var property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        var cell = headerRow.CreateCell(headerCell);
                        cell.SetCellValue(property.Name);
                        cell.CellStyle = headerStyle;
                        headerCell++;
                    }
                    // Save the new workbook to the specified output file path
                    IRow row;
                    int countRow = 1; // Bắt đầu từ hàng 1 (hàng 0 là tiêu đề)
                    foreach (T item in obj)
                    {
                        row = excelSheet.CreateRow(countRow);
                        row.CreateCell(0).SetCellValue(countRow); // Thêm cột số thứ tự
                        int cell = 1;
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
                        countRow++;
                    }
                    for (int i = 0; i < headerCell; i++)
                    {
                        excelSheet.AutoSizeColumn(i);
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
