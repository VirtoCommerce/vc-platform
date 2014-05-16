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
//using System.Globalization;


namespace VirtoCommerce.ManagementClient.Fulfillment.Report
{
	public class SlipPdfReport
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
				doc.DocumentMetadata(new DocumentMetadata { Author = Agent, Application = "VirtoCommerce", Keywords = "Slip", Subject = "Slip".Localize(), Title = "Slip".Localize() });
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
			 .PagesHeader(header => header.CustomHeader(new GroupingSlipHeaders { PdfRptFont = header.PdfFont }))
			 .MainTableTemplate(template => template.BasicTemplate(BasicTemplate.SilverTemplate))
			 .MainTablePreferences(table =>
			 {
				 table.ColumnsWidthsType(TableColumnWidthType.Relative);
				 table.GroupsPreferences(new GroupsPreferences
				 {
					 GroupType = GroupType.HideGroupingColumns,
					 RepeatHeaderRowPerGroup = true,
					 ShowOneGroupPerPage = true,
					 SpacingBeforeAllGroupsSummary = 15f,
					 NewGroupAvailableSpacingThreshold = 150,
					 SpacingAfterAllGroupsSummary = 15f
				 });
				 table.SpacingAfter(4f);
			 })
			.MainTableDataSource(dataSource => dataSource.StronglyTypedList(items))
			 .MainTableColumns(columns =>
			 {
				 columns.AddColumn(column =>
				 {
					 column.PropertyName<vm.ShipmentItem>(x => x.ShipmentId);
					 column.CellsHorizontalAlignment(HorizontalAlignment.Center);
					 column.Order(0);
					 column.Width(20);
					 column.HeaderCell("ShipmentId");
					 column.Group(
					 (val1, val2) => val1.ToString() == val2.ToString());
				 });

				 columns.AddColumn(column =>
				 {
					 column.PropertyName<vm.ShipmentItem>(x => x.ItemCode);
					 column.CellsHorizontalAlignment(HorizontalAlignment.Left);
					 column.IsVisible(true);
					 column.Order(1);
					 column.Width(20);
				 });

				 columns.AddColumn(column =>
				 {
					 column.PropertyName<vm.ShipmentItem>(x => x.ItemName);
					 column.CellsHorizontalAlignment(HorizontalAlignment.Justified);
					 column.IsVisible(true);
					 column.Order(2);
					 column.Width(40);
				 });

				 columns.AddColumn(column =>
				 {
					 column.PropertyName<vm.ShipmentItem>(x => x.Quantity);
					 column.CellsHorizontalAlignment(HorizontalAlignment.Right);
					 column.IsVisible(true);
					 column.Order(4);
					 column.Width(10);
				 });

				 columns.AddColumn(column =>
				 {
					 column.PropertyName<vm.ShipmentItem>(x => x.UnitPrice);
					 column.CellsHorizontalAlignment(HorizontalAlignment.Right);
					 column.IsVisible(true);
					 column.Order(4);
					 column.Width(20);
					 column.CalculatedField(
						 list =>
						 {
							 if (list == null)
								 return string.Empty;
							 var currency = list.GetSafeStringValueOf<vm.ShipmentItem>(x => x.BillingCurrency);
							 //string curSign;
							 //if (TryGetCurrencySymbol(currency, out curSign))
							 //	currency = curSign;
							 var unitPrice = list.GetSafeStringValueOf<vm.ShipmentItem>(x => x.UnitPrice);
							 return currency + " " + unitPrice;
						 });
				 });
			 })
				//.MainTableEvents(events =>
				//{
				//	events.DataSourceIsEmpty(message: "There is no data available to display.");

			//	events.GroupAdded(args =>
				//	{
				//		var shippingTaxTotal = args.LastOverallAggregateValueOf<vm.ShipmentItem>(s => s.ShippingTaxTotal);

			//		var taxTable = new PdfGrid(args.Table.RelativeWidths)
				//			{
				//				WidthPercentage = args.Table.WidthPercentage,
				//				SpacingBefore = args.Table.SpacingBefore
				//			}; // Create a clone of the MainTable's structure                   

			//		taxTable.AddSimpleRow(
				//			null /* null = empty cell */, null,
				//			(data, cellProperties) =>
				//			{
				//				data.Value = "tax";
				//				cellProperties.PdfFont = args.PdfFont;
				//				cellProperties.HorizontalAlignment = HorizontalAlignment.Right;
				//			},
				//			(data, cellProperties) =>
				//			{
				//				data.Value = string.Format("{0:n0}", shippingTaxTotal);
				//				cellProperties.PdfFont = args.PdfFont;
				//			});

			//		taxTable.AddSimpleRow(
				//			null /* null = empty cell */, null,
				//			(data, cellProperties) =>
				//			{
				//				data.Value = "tax";
				//				cellProperties.PdfFont = args.PdfFont;
				//				cellProperties.HorizontalAlignment = HorizontalAlignment.Right;
				//			},
				//			(data, cellProperties) =>
				//			{
				//				data.Value = string.Format("{0:n0}", shippingTaxTotal);
				//				cellProperties.PdfFont = args.PdfFont;
				//			});

			//		args.PdfDoc.Add(taxTable);
				//	});
				//})
				//.Export(export => export.ToExcel())
			.Generate(data => data.AsPdfFile(documentPath));
		}

		//private bool TryGetCurrencySymbol(string ISOCurrencySymbol, out string symbol)
		//{
		//	symbol = CultureInfo
		//		.GetCultures(CultureTypes.AllCultures)
		//		.Where(c => !c.IsNeutralCulture)
		//		.Select(culture =>
		//		{
		//			try
		//			{
		//				return new RegionInfo(culture.LCID);
		//			}
		//			catch
		//			{
		//				return null;
		//			}
		//		})
		//		.Where(ri => ri != null && ri.ISOCurrencySymbol == ISOCurrencySymbol)
		//		.Select(ri => ri.CurrencySymbol)
		//		.FirstOrDefault();
		//	return symbol != null;
		//}
	}

	public class TotalAggregateFunction : IAggregateFunction
	{
		public void CellAdded(object cellDataValue, bool isNewGroupStarted)
		{
			throw new NotImplementedException();
		}

		public Func<object, string> DisplayFormatFormula
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		public object GroupValue
		{
			get { throw new NotImplementedException(); }
		}

		public object OverallValue
		{
			get { throw new NotImplementedException(); }
		}

		public object ProcessingBoundary(IList<SummaryCellData> columnCellsSummaryData)
		{
			throw new NotImplementedException();
		}
	}

	public class GroupingSlipHeaders : IPageHeader
	{
		public IPdfFont PdfRptFont { set; get; }

		public PdfGrid RenderingGroupHeader(Document pdfDoc, PdfWriter pdfWriter, IList<CellData> newGroupInfo, IList<SummaryCellData> summaryData)
		{
			var shippingAddress = newGroupInfo.GetSafeStringValueOf<vm.ShipmentItem>(x => x.ShippingAddress);
			var billingAddress = newGroupInfo.GetSafeStringValueOf<vm.ShipmentItem>(x => x.BillingAddress);
			var orderDate = newGroupInfo.GetValueOf<vm.ShipmentItem>(x => x.OrderDate);
			var order = newGroupInfo.GetSafeStringValueOf<vm.ShipmentItem>(x => x.Order);
			var customer = newGroupInfo.GetSafeStringValueOf<vm.ShipmentItem>(x => x.Customer);

			var table = new PdfGrid(relativeWidths: new[] { 5f, 5f }) { WidthPercentage = 80 };
			table.AddSimpleRow(
				(cellData, cellProperties) =>
				{
					cellData.Value = "Billing address".Localize();
					cellProperties.PdfFont = PdfRptFont;
					cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
					cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
				},
				(cellData, cellProperties) =>
				{
					cellData.Value = "Shipping address".Localize();
					cellProperties.PdfFont = PdfRptFont;
					cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
					cellProperties.HorizontalAlignment = HorizontalAlignment.Right;
				});

			table.AddSimpleRow(
				(cellData, cellProperties) =>
				{
					cellData.Value = customer;
					cellProperties.PdfFont = PdfRptFont;
					cellProperties.PdfFontStyle = DocumentFontStyle.Normal;
					cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
				},
				(cellData, cellProperties) =>
				{
					cellData.Value = customer;
					cellProperties.PdfFont = PdfRptFont;
					cellProperties.PdfFontStyle = DocumentFontStyle.Normal;
					cellProperties.HorizontalAlignment = HorizontalAlignment.Right;
				});

			table.AddSimpleRow(
				(cellData, cellProperties) =>
				{
					cellData.Value = billingAddress;
					cellProperties.PdfFont = PdfRptFont;
					cellProperties.PdfFontStyle = DocumentFontStyle.Normal;
					cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
				},
				(cellData, cellProperties) =>
				{
					cellData.Value = shippingAddress;
					cellProperties.PdfFont = PdfRptFont;
					cellProperties.PdfFontStyle = DocumentFontStyle.Normal;
					cellProperties.HorizontalAlignment = HorizontalAlignment.Right;
				});

			table.AddSimpleRow(
				(cellData, cellProperties) =>
				{
					cellData.Value = "Invoice and Receipt".Localize();
					cellProperties.PdfFont = PdfRptFont;
					cellProperties.PdfFontStyle = DocumentFontStyle.Bold;
					cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
				},
				(cellData, cellProperties) =>
				{
					cellData.Value = string.Empty;
				});



			table.AddSimpleRow(
				(cellData, cellProperties) =>
				{
					cellData.Value = string.Format("Order date: {0}".Localize(), ((DateTime?)orderDate).Value.ToShortDateString());
					cellProperties.PdfFont = PdfRptFont;
					cellProperties.PdfFontStyle = DocumentFontStyle.Normal;
					cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
				},
				(cellData, cellProperties) =>
				{
					cellData.Value = string.Empty;
				});

			table.AddSimpleRow(
				(cellData, cellProperties) =>
				{
					cellData.Value = string.Format("Order number: {0}".Localize(), order);
					cellProperties.PdfFont = PdfRptFont;
					cellProperties.PdfFontStyle = DocumentFontStyle.Normal;
					cellProperties.HorizontalAlignment = HorizontalAlignment.Left;
				},
				(cellData, cellProperties) =>
				{
					cellData.Value = string.Empty;
				});

			table.HorizontalAlignment = 0;

			return table;
		}

		public PdfGrid RenderingReportHeader(Document pdfDoc, PdfWriter pdfWriter, IList<SummaryCellData> summaryData)
		{

			return null;
		}
	}
}
