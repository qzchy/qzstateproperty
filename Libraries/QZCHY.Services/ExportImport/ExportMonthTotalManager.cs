using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QZCHY.Core.Domain.Properties;
using QZCHY.Services.Property;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using QZCHY.Core;

namespace QZCHY.Services.ExportImport
{

    public class ExportMonthTotalManager : IExportMonthTotalManager
    {
        private readonly IPropertyService _propertyService;
        private readonly IMonthTotalService _monthTotalService;
        public ExportMonthTotalManager(IPropertyService propertyService,IMonthTotalService monthTotalService)
        {
            _propertyService = propertyService;
            _monthTotalService = monthTotalService;
        }
        public void ExportMonthTotal(Stream stream, List<Core.Domain.Properties.Property> properties)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            using (var xlPackage = new ExcelPackage(stream)) {
                var worksheet = xlPackage.Workbook.Worksheets.Add("统计表");

                var headers = new string[] {"资产名称","资产类别","产权单位","资产所在地","建筑面积","对应土地面积","账面价值","土地面积","账面价值","自用","出租","出借","闲置","本月租金收益" };
                for (int j = 0; j < headers.Count(); j++)
                {
                    worksheet.Cells[1, j + 1].Value = headers[j];
                    worksheet.Cells[1, j + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, j + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    worksheet.Cells[1, j + 1].Style.Font.Bold = true;
                }

                int row = 2;

                foreach (var property in properties) {
                    var monthTotal = _monthTotalService.GetPropertyMonthTotal(property.Id, DateTime.Now.Month).Last();
                    worksheet.Cells[row, 1].Value = property.Name;
                    worksheet.Cells[row, 2].Value = property.PropertyType.ToDescription();
                    worksheet.Cells[row, 3].Value = property.Government.Name;
                    worksheet.Cells[row, 4].Value = property.Address;
                    worksheet.Cells[row, 5].Value = property.ConstructArea;
                    worksheet.Cells[row, 6].Value = property.ConstructArea;
                    worksheet.Cells[row, 7].Value = property.Price;
                    worksheet.Cells[row, 8].Value = property.LandArea;
                    worksheet.Cells[row, 9].Value = property.Price;

                    var monthtotals = _monthTotalService.GetPropertyMonthTotal(property.Id, DateTime.Now.Month).ToList();
                    var monthtotal = monthtotals[monthtotals.Count()-1];
                    if (monthtotal != null)
                    {
                        worksheet.Cells[row, 10].Value = monthtotal.CurrentUse_Self;
                        worksheet.Cells[row, 11].Value = monthtotal.CurrentUse_Rent;
                        worksheet.Cells[row, 12].Value = monthtotal.CurrentUse_Lend;
                        worksheet.Cells[row, 13].Value = monthtotal.CurrentUse_Idle;
                        worksheet.Cells[row, 14].Value = monthtotal.Price;
                    }
                    else {
                        worksheet.Cells[row, 10].Value = property.CurrentUse_Self;
                        worksheet.Cells[row, 11].Value = property.CurrentUse_Rent;
                        worksheet.Cells[row, 12].Value = property.CurrentUse_Lend;
                        worksheet.Cells[row, 13].Value = property.CurrentUse_Idle;
                        worksheet.Cells[row, 14].Value = 0;
                    }

                    row++;            
                }

                xlPackage.Save();

            }

        }
    }
}
