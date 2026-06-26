using OfficeOpenXml;
using OfficeOpenXml.Style;
using FlashOffer.API.Application.Common.Interfaces;
using FlashOffer.API.Application.Resources;
using Microsoft.Extensions.Localization;

namespace FlashOffer.API.Infrastructure.Services;

public class ExcelService : IExcelService
{
	public byte[] ExportToExcel<T>(
		IEnumerable<T> data,
		Dictionary<string, Func<T, object>> columnConfig,
		string sheetName,
		string titleKey,
		IStringLocalizer<SharedResource> localizer)
	{
		using var package = new ExcelPackage();
		var worksheet = package.Workbook.Worksheets.Add(sheetName);

		// Title
		var currentRow = 1;
		if (!string.IsNullOrEmpty(titleKey))
		{
			var title = localizer[titleKey];
			worksheet.Cells[currentRow, 1, currentRow, columnConfig.Count]
				.Merge = true;
			worksheet.Cells[currentRow, 1].Value = title;
			worksheet.Cells[currentRow, 1].Style.Font.Size = 16;
			worksheet.Cells[currentRow, 1].Style.Font.Bold = true;
			worksheet.Cells[currentRow, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; // Căn giữa
			currentRow += 2;
		}

		// Header
		var colIndex = 1;
		foreach (var header in columnConfig.Keys)
		{
			worksheet.Cells[currentRow, colIndex].Value = localizer[header]; // Dịch header
			worksheet.Cells[currentRow, colIndex].Style.Font.Bold = true;
			worksheet.Cells[currentRow, colIndex].Style.Fill.PatternType = ExcelFillStyle.Solid;
			worksheet.Cells[currentRow, colIndex].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
			worksheet.Cells[currentRow, colIndex].Style.Border.BorderAround(ExcelBorderStyle.Thin);
			worksheet.Cells[currentRow, colIndex].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; // Căn trái header
			colIndex++;
		}

		// Data
		var rowIndex = currentRow + 1;
		foreach (var item in data)
		{
			colIndex = 1;
			foreach (var config in columnConfig.Values)
			{
				var value = config(item);
				var cell = worksheet.Cells[rowIndex, colIndex];

				if (value is DateTime dateTime)
				{
					cell.Value = dateTime;
					cell.Style.Numberformat.Format = "yyyy-MM-dd HH:mm:ss";
				}
				else if (value is decimal or double or float)
				{
					cell.Value = value;
					cell.Style.Numberformat.Format = "#,##0";
				}
				else
				{
					cell.Value = value;
				}
				cell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
				cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; // Căn trái data
				colIndex++;
			}
			rowIndex++;
		}

		// AutoFit + format
		worksheet.Cells[currentRow, 1, rowIndex - 1, columnConfig.Count]
			.AutoFitColumns();
		worksheet.Cells[currentRow, 1, rowIndex - 1, columnConfig.Count]
			.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

		// Footer
		var footerRow = rowIndex + 1;
		worksheet.Cells[footerRow, 1].Value = $"Exported: {DateTime.Now:yyyy-MM-dd HH:mm:ss}";
		worksheet.Cells[footerRow, 1].Style.Font.Size = 9;
		worksheet.Cells[footerRow, 1].Style.Font.Italic = true;
		worksheet.Cells[footerRow, 1].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

		return package.GetAsByteArray();
	}
}