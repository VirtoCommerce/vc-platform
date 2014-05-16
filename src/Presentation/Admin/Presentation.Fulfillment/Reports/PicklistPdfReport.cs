using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfRpt.Core.Contracts;
using PdfRpt.Core.Helper;
using PdfRpt.FluentInterface;
using VirtoCommerce.Foundation.Orders.Model;
using vm = VirtoCommerce.ManagementClient.Fulfillment.Model;


namespace VirtoCommerce.ManagementClient.Fulfillment.Report
{
	public class PicklistPdfReport
	{
		public IPdfReportData CreatePdfReport(ObservableCollection<Shipment> source, string Agent)
		{
			var items = new List<vm.ShipmentItem>();
			source.SelectMany(x => x.ShipmentItems).ToList().ForEach(item => items.Add(new vm.ShipmentItem(item)));

			var tempPath = Path.GetTempPath();
			var documentPath = string.Format("{0}{1}{2}.pdf", tempPath, tempPath.EndsWith(Path.DirectorySeparatorChar.ToString()) ? string.Empty : Path.DirectorySeparatorChar.ToString(), Guid.NewGuid().ToString("N"));

			return new PdfReport().DocumentPreferences(doc =>
			{
				doc.RunDirection(PdfRunDirection.LeftToRight);
				doc.Orientation(PageOrientation.Portrait);
				doc.PageSize(PdfPageSize.A4);
				doc.DocumentMetadata(new DocumentMetadata { Author = Agent, Application = "VirtoCommerce", Keywords = "Picklist", Subject = "Picklist".Localize(), Title = "Picklist".Localize() });
				doc.Compression(new CompressionSettings
				{
					EnableCompression = true,
					EnableFullCompression = true
				});
				// add print command dialog on open to the pdf
				doc.PrintingPreferences(new PrintingPreferences
				{
					ShowPrintDialogAutomatically = true
				});
			})
			 .DefaultFonts(fonts =>
			 {
				 fonts.Path(Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "fonts\\arial.ttf"),
							Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), "fonts\\verdana.ttf"));
				 fonts.Size(11);
				 fonts.Color(System.Drawing.Color.Black);
			 })
			 .PagesFooter(footer => footer.DefaultFooter(DateTime.Now.ToString("MM/dd/yyyy")))
			 .PagesHeader(header => header.CustomHeader(new GroupingHeaders { PdfRptFont = header.PdfFont }))
			 .MainTableTemplate(template => template.BasicTemplate(BasicTemplate.SilverTemplate))
			 .MainTablePreferences(table =>
			 {
				 table.ColumnsWidthsType(TableColumnWidthType.Relative);
				 table.GroupsPreferences(new GroupsPreferences
				 {
					 GroupType = GroupType.HideGroupingColumns,
					 RepeatHeaderRowPerGroup = true,
					 ShowOneGroupPerPage = false,
					 SpacingBeforeAllGroupsSummary = 5f,
					 NewGroupAvailableSpacingThreshold = 150,
					 SpacingAfterAllGroupsSummary = 10f
				 });
				 table.SpacingAfter(4f);
			 })
			.MainTableDataSource(dataSource => dataSource.StronglyTypedList(items))
			 .MainTableColumns(columns =>
			 {
				 columns.AddColumn(column =>
				 {
					 column.PropertyName("rowNo");
					 column.IsRowNumber(true);
					 column.CellsHorizontalAlignment(HorizontalAlignment.Center);
					 column.IsVisible(true);
					 column.Order(0);
					 column.Width(5);
					 column.HeaderCell("#");
				 });

				 columns.AddColumn(column =>
				 {
					 column.PropertyName<vm.ShipmentItem>(x => x.ShipmentId);
					 column.CellsHorizontalAlignment(HorizontalAlignment.Center);
					 column.Order(1);
					 column.Width(20);
					 column.HeaderCell("ShipmentId");
					 column.Group(
					 (val1, val2) => val1.ToString() == val2.ToString());
				 });

				 columns.AddColumn(column =>
				 {
					 column.PropertyName<vm.ShipmentItem>(x => x.ItemName);
					 column.CellsHorizontalAlignment(HorizontalAlignment.Justified);
					 column.IsVisible(true);
					 column.Order(2);
					 column.Width(50);
				 });

				 columns.AddColumn(column =>
				 {
					 column.PropertyName<vm.ShipmentItem>(x => x.ItemCode);
					 column.CellsHorizontalAlignment(HorizontalAlignment.Right);
					 column.IsVisible(true);
					 column.Order(3);
					 column.Width(15);
				 });

				 columns.AddColumn(column =>
				 {
					 column.PropertyName<vm.ShipmentItem>(x => x.Quantity);
					 column.CellsHorizontalAlignment(HorizontalAlignment.Center);
					 column.IsVisible(true);
					 column.Order(4);
					 column.Width(10);
				 });

			 })
			.MainTableEvents(events => events.DataSourceIsEmpty(message: "There is no data available to display.".Localize()))
				//.Export(export => export.ToExcel())
			.Generate(data => data.AsPdfFile(documentPath));
		}
	}

	public class GroupingHeaders : IPageHeader
	{
		public IPdfFont PdfRptFont { set; get; }

		public PdfGrid RenderingGroupHeader(Document pdfDoc, PdfWriter pdfWriter, IList<CellData> newGroupInfo, IList<SummaryCellData> summaryData)
		{
			var groupName = newGroupInfo.GetSafeStringValueOf<vm.ShipmentItem>(x => x.ShipmentId);

			var table = new PdfGrid(relativeWidths: new[] { 1f, 5f }) { WidthPercentage = 100 };
			table.AddSimpleRow(
				(cellData, cellProperties) =>
				{
					cellData.Value = "Shipment:".Localize();
					cellProperties.PdfFont = PdfRptFont;
					cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
					cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
				},
				(cellData, cellProperties) =>
				{
					cellData.Value = groupName;
					cellProperties.PdfFont = PdfRptFont;
					cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
				});

			return table.AddBorderToTable(borderColor: BaseColor.LIGHT_GRAY, spacingBefore: 10f);
		}

		public PdfGrid RenderingReportHeader(Document pdfDoc, PdfWriter pdfWriter, IList<SummaryCellData> summaryData)
		{
			var table = new PdfGrid(numColumns: 1) { WidthPercentage = 100 };

			table.AddSimpleRow(
			   (cellData, cellProperties) =>
			   {
				   cellData.Value = "Picklist".Localize();
				   cellProperties.PdfFont = PdfRptFont;
				   cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
				   cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
			   });
			return table.AddBorderToTable();
		}
	}
}
