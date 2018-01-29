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
using QZCHY.Services.Property;
using System.Reflection;
using QZCHY.Core;

namespace QZCHY.Services.ExportImport
{
    public class ExportManager : IExportManager
    {
        private readonly IPropertyService _propertyService;
        public ExportManager(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        public void ExportPropertyToXlsx(Stream stream, IList<Core.Domain.Properties.Property> properties, IList<string> headers)
        {
            if (stream == null) throw new ArgumentNullException("stream");

            using (var xlPackage = new ExcelPackage(stream))
            {
                var worksheet = xlPackage.Workbook.Worksheets.Add("资产基本表");
                                
                for (int i = 0; i < headers.Count(); i++)
                {
                    switch (headers[i])
                    {
                        case "Name":
                            worksheet.Cells[1, i + 1].Value = "资产名称";
                            break;
                        case "Government":
                            worksheet.Cells[1, i + 1].Value = "产权单位";
                            break;
                        case "PGovernment":
                            worksheet.Cells[1, i + 1].Value = "上级单位";
                            break;
                        case "GovernmentType":
                            worksheet.Cells[1, i + 1].Value = "单位性质";
                            break;
                        case "PropertyType":
                            worksheet.Cells[1, i + 1].Value = "资产类别";
                            break;
                        case "Region":
                            worksheet.Cells[1, i + 1].Value = "所处区域";
                            break;
                        case "Address":
                            worksheet.Cells[1, i + 1].Value = "地址";
                            break;
                        case "ConstructArea":
                            worksheet.Cells[1, i + 1].Value = "建筑面积(平方米)";
                            break;
                        case "LandArea":
                            worksheet.Cells[1, i + 1].Value = "土地面积(平方米)";
                            break;
                        case "PropertyID":
                            worksheet.Cells[1, i + 1].Value = "产权证号";
                            break;
                        case "PropertyNature":
                            worksheet.Cells[1, i + 1].Value = "房产性质";
                            break;
                        case "LandNature":
                            worksheet.Cells[1, i + 1].Value = "土地性质";
                            break;
                        case "Price":
                            worksheet.Cells[1, i + 1].Value = "账面价格(万元)";
                            break;
                        case "GetedDate":
                            worksheet.Cells[1, i + 1].Value = "取得时间";
                            break;
                        case "LifeTime":
                            worksheet.Cells[1, i + 1].Value = "使用年限(年)";
                            break;
                        case "UsedPeople":
                            worksheet.Cells[1, i + 1].Value = "使用方";
                            break;
                        case "CurrentUse_Self":
                            worksheet.Cells[1, i + 1].Value = "自用面积(平方米)";
                            break;
                        case "CurrentUse_Rent":
                            worksheet.Cells[1, i + 1].Value = "出租面积(平方米)";
                            break;
                        case "CurrentUse_Lend":
                            worksheet.Cells[1, i + 1].Value = "出借面积(平方米)";
                            break;
                        case "CurrentUse_Idle":
                            worksheet.Cells[1, i + 1].Value = "闲置面积(平方米)";
                            break;
                        case "NextStepUsage":
                            worksheet.Cells[1, i + 1].Value = "下步使用";
                            break;
                        case "EstateId":
                            worksheet.Cells[1, i + 1].Value = "不动产证";
                            break;
                        case "ConstructId":
                            worksheet.Cells[1, i + 1].Value = "房产证";
                            break;
                        case "LandId":
                            worksheet.Cells[1, i + 1].Value = "土地证";
                            break;
                        case "HasConstructID":
                            worksheet.Cells[1, i + 1].Value = "有无房产证";
                            break;
                        case "HasLandID":
                            worksheet.Cells[1, i + 1].Value = "有无土地证";
                            break;


                    }
                    worksheet.Cells[1, i + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[1, i + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    worksheet.Cells[1, i + 1].Style.Font.Bold = true;
                }

                var worksheet1 = xlPackage.Workbook.Worksheets.Add("出租表");
                var headers1 = new string[] {"资产名称", "出租方", "出租日期", "归还日期", "出租面积(平方米)", "出租总金额(元)","备注" };
                for (int j = 0; j < headers1.Count(); j++)
                {
                    worksheet1.Cells[1, j + 1].Value = headers1[j];
                    worksheet1.Cells[1, j + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet1.Cells[1, j + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    worksheet1.Cells[1, j + 1].Style.Font.Bold = true;
                }


                var worksheet2 = xlPackage.Workbook.Worksheets.Add("出借表");
                var headers2 = new string[] { "资产名称","出借方", "出借日期", "出借面积(平方米)","备注" };
                for (int k = 0; k < headers2.Count(); k++)
                {
                    worksheet2.Cells[1, k + 1].Value = headers2[k];
                    worksheet2.Cells[1, k + 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet2.Cells[1, k + 1].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                    worksheet2.Cells[1, k + 1].Style.Font.Bold = true;
                }          
                
                //Excel添加内容
                int row = 2;
                int row1 = 2;
                int row2 = 2;
                foreach (var property in properties)
                {
                    Type t = property.GetType();
                    PropertyInfo[] PropertyList = t.GetProperties();

                    #region 资产出租表
                    if (!headers.Contains("Rent"))
                    {
                        if (xlPackage.Workbook.Worksheets.Contains(worksheet1)) xlPackage.Workbook.Worksheets.Delete(2);
                    }
                    else
                    {
                        foreach (var rent in property.Rents)
                        {
                            int col1 = 1;
                            worksheet1.Cells[row1, col1].Value = rent.Title;
                            col1++;

                            worksheet1.Cells[row1, col1].Value = rent.Name;
                            col1++;

                            worksheet1.Cells[row1, col1].Value = rent.RentTime.ToString("yyyy-MM-dd");
                            col1++;

                            worksheet1.Cells[row1, col1].Value = rent.BackTime.ToString("yyyy-MM-dd");
                            col1++;

                            worksheet1.Cells[row1, col1].Value = rent.RentArea;
                            col1++;

                            worksheet1.Cells[row1, col1].Value = rent.PriceString;
                            col1++;

                            worksheet1.Cells[row1, col1].Value = rent.Remark;
                            col1++;

                            row1++;
                        }
                        //  headers.Remove("Rent");
                    }
                    #endregion

                    #region 资产出借表
                    if (!headers.Contains("Lend"))
                    {
                        if (!headers.Contains("Rent"))
                        {
                            if (xlPackage.Workbook.Worksheets.Contains(worksheet2)) xlPackage.Workbook.Worksheets.Delete(2);
                        }
                        else {
                            if (xlPackage.Workbook.Worksheets.Contains(worksheet2)) xlPackage.Workbook.Worksheets.Delete(3);
                        }
                    }
                    else 
                    {

                        foreach (var lend in property.Lends)
                        {
                            int col2 = 1;
                            worksheet2.Cells[row2, col2].Value = lend.Title;
                            col2++;

                            worksheet2.Cells[row2, col2].Value = lend.Name;
                            col2++;

                            worksheet2.Cells[row2, col2].Value = lend.LendTime;
                            col2++;

                            worksheet2.Cells[row2, col2].Value = lend.LendArea;
                            col2++;

                            worksheet2.Cells[row2, col2].Value = lend.Remark;
                            col2++;

                            row2++;
                        }

                    }
                    #endregion

                   
              
                    for (int col=1;col <= headers.Count(); col++)
                        {
                        foreach (var p in PropertyList) {
                            if (p.Name == headers[col - 1])
                            {
                                if (p.Name == "Government")
                                {
                                    var name = property.Government.Name;
                                    var type = property.Government.GovernmentType.ToDescription();
                                    worksheet.Cells[row, col].Value = name;
                                    worksheet.Cells[row, 5].Value = type ;
                                }
                                else if (p.Name == "PGovernment") worksheet.Cells[row, col].Value = property.Government.ParentName;            
                                else if (p.Name == "Region") worksheet.Cells[row, col].Value = property.Region.ToDescription();
                                else if (p.Name == "PropertyType") worksheet.Cells[row, col].Value = property.PropertyType.ToDescription();
                                else if (p.Name == "NextStepUsage") worksheet.Cells[row, col].Value = property.NextStepUsage.ToDescription();
                                else if (p.Name == "GetedDate") worksheet.Cells[row, col].Value = (property.GetedDate == null ? "" : Convert.ToDateTime(property.GetedDate).ToString("yyyy-MM-dd"));
                                else if (p.Name == "HasConstructID") worksheet.Cells[row, col].Value = (property.HasConstructID == true ? "是" : "否");
                                else if (p.Name == "HasLandID") worksheet.Cells[row, col].Value = (property.HasLandID == true ? "是" : "否");
                                else worksheet.Cells[row, col].Value = p.GetValue(property);

                                }
                              
                            }
                        }
                    
                                    
                    
                 
                    row++;
                }
                if (headers.Contains("Rent") && headers.Contains("Lend")) {
                    worksheet.DeleteColumn(headers.Count());
                    worksheet.DeleteColumn(headers.Count()-1);
                } 
                if(headers.Contains("Rent")&& !headers.Contains("Lend")) worksheet.DeleteColumn(headers.Count());
                if (!headers.Contains("Rent") && headers.Contains("Lend")) worksheet.DeleteColumn(headers.Count());

                xlPackage.Save();

            }



        }
    }
}
