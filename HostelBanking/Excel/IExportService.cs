using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
namespace HostelBanking.Excel
{
	public interface IExportService<T>
	{
		Task<byte[]> ExportFile(List<T> obj, string fileTemplate);
	}
}
