using FlashOffer.API.Application.Resources;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.Application.Common.Interfaces;

public interface IExcelService
{
	byte[] ExportToExcel<T>(
		IEnumerable<T> data,
		Dictionary<string, Func<T, object>> columnConfig,
		string sheetName,
		string titleKey,
		IStringLocalizer<SharedResource> localizer);
}