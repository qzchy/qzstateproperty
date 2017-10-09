using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QZCHY.Core.Domain.Properties;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;

namespace QZCHY.Services.ExportImport
{
    public class ExportManager : IExportManager
    {

        public ExportManager() {

        }

        public void ExportPropertyToXlsx(Stream stream, IList<Core.Domain.Properties.Property> properties)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("property");

                //标题
                var headers = new[] {
                    "资产名称",
                    "资产地址",
                    "建筑面积",
                    "使用年限"
                };
                for (int i = 0; i < headers.Length; i++)
                {
                    worksheet.Cells[1, i + 1].Value = properties[i];
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                }

                //Excel添加内容
                int row = 2;
                foreach (var property in properties)
                {
                    var col = 1;
                    worksheet.Cells[row, col].Value = property.Name;
                    col++;

                    worksheet.Cells[row, col].Value = property.Address;
                    col++;

                    worksheet.Cells[row, col].Value = property.ConstructArea;
                    col++;

                    worksheet.Cells[row, col].Value = property.LifeTime;
                    col++;

                    row++;
                }

                xlPackage.Save();

            }



        }
    }
}
