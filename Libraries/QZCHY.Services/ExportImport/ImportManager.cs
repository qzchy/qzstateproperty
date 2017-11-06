using OfficeOpenXml;
using QZCHY.Core;
using QZCHY.Core.Domain.Media;
using QZCHY.Core.Domain.Properties;
using QZCHY.Services.AccountUsers;
using QZCHY.Services.Media;
using QZCHY.Services.Property;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Services.ExportImport
{
    public class ImportManager : IImportManager
    {
        private readonly IPropertyService _propertyService;
        private readonly IGovernmentService _governmentService;
        private readonly IAccountUserService _accountUserService;
        private readonly IPictureService _pictureService;
        private readonly IFileService _fileService;
        private readonly IPropertyNewCreateService _propertyNewCreateService;
        private readonly IWorkContext _workContext;

        public ImportManager(IPropertyService propertyService, IGovernmentService governmentService, IAccountUserService accountUserServic,
            IWorkContext workContext, IPictureService pictureService, IFileService fileService, IPropertyNewCreateService propertyNewCreateService)
        {
            _propertyService = propertyService;
            _governmentService = governmentService;
            _accountUserService = accountUserServic;
            _workContext = workContext;
            _pictureService = pictureService;
            _fileService = fileService;
            _propertyNewCreateService = propertyNewCreateService;
        }


        public string ImportProductsFromXlsx(Stream stream, string path)
        {
            var currentUser = _workContext.CurrentAccountUser;

            IList<Core.Domain.Properties.Property> properties = new List<Core.Domain.Properties.Property>();

            using (var xlPackage = new ExcelPackage(stream))
            {

                var worksheet = xlPackage.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null)
                    throw new Exception("没有找到excel表格");

                #region 导入的资产字段集合
                var excelProperty = new[]
        {
                "资产名称",
                "产权单位",
                "资产类别",
                "地址",
                "建筑面积(平方米)",
                "土地面积(平方米)",
                "房产性质",
                "土地性质",
                "账面价格(万元)",
                "取得时间",
                "使用年限(年)",
                "使用方",
                "自用面积(平方米)",
                "出租面积(平方米)",
                "出借面积(平方米)",
                "闲置面积(平方米)",
                "不动产证",
                "房产证",
                "土地证",
                "空间位置",
                "空间范围",
                 "资产封面",
                "图片附件",
                "其他附件",
                "备注"
                };
                #endregion

                //数据插入
                int row = 2;
                StringBuilder sb = new StringBuilder();
                while (true)
                {
                    #region 循环记录
                    try
                    {
                        bool allColumnsAreEmpty = true;
                        for (var i = 1; i <= excelProperty.Length; i++)
                            if (worksheet.Cells[row, i].Value != null && !String.IsNullOrEmpty(worksheet.Cells[row, i].Value.ToString()))
                            {
                                allColumnsAreEmpty = false;
                                break;
                            }
                        if (allColumnsAreEmpty)
                            break;
                        List<Point> points = new List<Point>();

                        var property = new Core.Domain.Properties.Property() { FromExcelImport = true };

                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "资产名称")].Value != null) property.Name = worksheet.Cells[row, GetColumnIndex(excelProperty, "资产名称")].Value.ToString();
                        else throw new Exception("资产名称不能为空");

                        #region 产权单位
                        if (currentUser.IsParentGovernmentorAuditor())
                        {
                            if (worksheet.Cells[row, GetColumnIndex(excelProperty, "产权单位")].Value != null)
                            {
                                var governmentName = worksheet.Cells[row, GetColumnIndex(excelProperty, "产权单位")].Value.ToString();
                                var government = _governmentService.GetGovernmentUnitByName(governmentName);
                                if (government == null) property.Government = currentUser.Government;
                                else
                                {
                                    if (government.ParentGovernmentId != currentUser.Government.Id) throw new Exception("所填写产权单位并非当前账号直接下级部门");
                                    else property.Government = government;
                                }
                            }
                        }
                        else property.Government = currentUser.Government;
                        #endregion

                        #region 资产类别导入
                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "资产类别")].Value != null)
                        {
                            switch (worksheet.Cells[row, GetColumnIndex(excelProperty, "资产类别")].Value.ToString())
                            {
                                case "房屋":
                                    property.PropertyType = PropertyType.House;
                                    break;
                                case "土地":
                                    property.PropertyType = PropertyType.Land;
                                    break;
                                case "房屋对应土地":
                                    property.PropertyType = PropertyType.LandUnderHouse;
                                    break;
                                case "其他":
                                default:
                                    property.PropertyType = PropertyType.Others;
                                    break;
                            }
                        }
                        else throw new Exception("资产类别不能为空");

                        #endregion

                        #region 基本属性录入
                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "地址")].Value != null) property.Address = worksheet.Cells[row, GetColumnIndex(excelProperty, "地址")].Value.ToString();
                        else throw new Exception("资产地址不能为空");

                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "建筑面积(平方米)")].Value != null)
                        {
                            try
                            {
                                property.ConstructArea = Convert.ToDouble(worksheet.Cells[row, GetColumnIndex(excelProperty, "建筑面积(平方米)")].Value);
                            }
                            catch
                            {
                                throw new Exception("建筑面积必须是一个数字");
                            }
                        }

                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "土地面积(平方米)")].Value != null) property.LandArea = Convert.ToDouble(worksheet.Cells[row, GetColumnIndex(excelProperty, "土地面积(平方米)")].Value);
                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "房产性质")].Value != null) property.PropertyNature = worksheet.Cells[row, GetColumnIndex(excelProperty, "房产性质")].Value.ToString();
                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "土地性质")].Value != null) property.LandNature = worksheet.Cells[row, GetColumnIndex(excelProperty, "土地性质")].Value.ToString();
                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "账面价格(万元)")].Value != null) property.Price = Convert.ToDouble(worksheet.Cells[row, GetColumnIndex(excelProperty, "账面价格(万元)")].Value);
                        else throw new Exception("资产账面价格不能为空");
                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "取得时间")].Value != null)
                        {
                            try
                            {
                                property.GetedDate = Convert.ToDateTime(worksheet.Cells[row, GetColumnIndex(excelProperty, "取得时间")].Value);
                            }
                            catch
                            {
                                throw new Exception("取得时间不是一个标准的日期格式");
                            }
                        }
                        else throw new Exception("资产取得时间不能为空");

                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "使用年限(年)")].Value != null) property.LifeTime = Convert.ToInt32(worksheet.Cells[row, GetColumnIndex(excelProperty, "使用年限(年)")].Value);
                        else throw new Exception("资产地址不能为空");

                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "使用方")].Value != null) property.UsedPeople = worksheet.Cells[row, GetColumnIndex(excelProperty, "使用方")].Value.ToString();
                        else throw new Exception("资产地址不能为空");

                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "自用面积(平方米)")].Value != null) property.CurrentUse_Self = Convert.ToDouble(worksheet.Cells[row, GetColumnIndex(excelProperty, "自用面积(平方米)")].Value);
                        else throw new Exception("资产地址不能为空");
                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "出租面积(平方米)")].Value != null) property.CurrentUse_Rent = Convert.ToDouble(worksheet.Cells[row, GetColumnIndex(excelProperty, "出租面积(平方米)")].Value);
                        else throw new Exception("资产地址不能为空");
                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "出借面积(平方米)")].Value != null) property.CurrentUse_Lend = Convert.ToDouble(worksheet.Cells[row, GetColumnIndex(excelProperty, "出借面积(平方米)")].Value);
                        else throw new Exception("资产地址不能为空");
                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "闲置面积(平方米)")].Value != null) property.CurrentUse_Idle = Convert.ToDouble(worksheet.Cells[row, GetColumnIndex(excelProperty, "闲置面积(平方米)")].Value);
                        else throw new Exception("资产地址不能为空");

                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "不动产证")].Value != null) property.EstateId = worksheet.Cells[row, GetColumnIndex(excelProperty, "不动产证")].Value.ToString();
                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "房产证")].Value != null) property.ConstructId = worksheet.Cells[row, GetColumnIndex(excelProperty, "房产证")].Value.ToString();
                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "土地证")].Value != null) property.LandId = worksheet.Cells[row, GetColumnIndex(excelProperty, "土地证")].Value.ToString();
                        #endregion

                        #region 空间位置录入
                        var location = "";
                        var extent = "";
                        //点
                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "空间位置")].Value != null)
                        {
                            try
                            {
                                location = worksheet.Cells[row, GetColumnIndex(excelProperty, "Location")].Value.ToString();
                                var point = Newtonsoft.Json.JsonConvert.DeserializeObject<Point>(location);
                                var wkt = "POINT(" + point.lng + " " + point.lat + ")";
                                property.Location = DbGeography.FromText(wkt);
                            }
                            catch
                            {
                                throw new Exception("空间位置获取失败");
                            }
                        }
                        else throw new Exception("空间位置不能为空");

                        //面
                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "空间范围")].Value != null)
                        {
                            try
                            {
                                extent = worksheet.Cells[row, GetColumnIndex(excelProperty, "Extent")].Value.ToString();
                                var ewkt = "POLYGON ((";
                                points = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Point>>(extent);
                                foreach (var point in points)
                                {
                                    ewkt += point.lng + " " + point.lat + ",";
                                }
                                ewkt = ewkt.Substring(0, ewkt.Length - 2) + "," + points[0].lng + " " + points[0].lat + "))";
                                property.Extent = DbGeography.FromText(ewkt);
                            }
                            catch
                            {
                                throw new Exception("空间位置获取失败");
                            }
                        }

                        #endregion

                        #region 添加Logo、图片附件、其他附件
                        var logoName = "";
                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "资产封面")].Value != null) logoName = worksheet.Cells[row, GetColumnIndex(excelProperty, "资产封面")].Value.ToString();
                        else throw new Exception("资产封面不能为空");

                        var imgArr = new string[] { };
                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "图片附件")].Value != null) imgArr = worksheet.Cells[row, GetColumnIndex(excelProperty, "图片附件")].Value.ToString().Split(';');
                        var otherFileArr = new string[] { };
                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "其他附件")].Value != null) otherFileArr = worksheet.Cells[row, GetColumnIndex(excelProperty, "其他附件")].Value.ToString().Split(';');

                        var files = System.IO.Directory.GetFiles(path);

                        var pictures = new List<Picture>();
                        var otherFiles = new List<Core.Domain.Media.File>();

                        foreach (var file in files)
                        {
                            if (file.Split('\\')[file.Split('\\').Length - 1] == logoName)
                            {
                                #region 资产封面导入
                                var fileName = System.IO.Path.GetFileName(file);

                                var fileStream = new FileStream(file, FileMode.Open);
                                var fileBinary = new byte[fileStream.Length];
                                fileStream.Read(fileBinary, 0, fileBinary.Length);

                                var picture = _pictureService.InsertPicture(fileBinary, "image/jpeg", "", "", fileName);
                                var url = _pictureService.GetPictureUrl(picture);
                                var propertyPicture = new PropertyPicture
                                {
                                    Picture = picture,
                                    Property = property,
                                    IsLogo = true
                                };
                                property.Pictures.Add(propertyPicture);
                                _propertyService.UpdateProperty(property);
                                fileStream.Flush();
                                fileStream.Close();
                                #endregion
                            }
                            else if (imgArr.Contains(file.Split('\\')[file.Split('\\').Length - 1]))
                            {
                                #region 图片附件
                                var fileName = System.IO.Path.GetFileName(file);

                                var fileStream = new FileStream(file, FileMode.Open);
                                var fileBinary = new byte[fileStream.Length];
                                fileStream.Read(fileBinary, 0, fileBinary.Length);

                                var picture = _pictureService.InsertPicture(fileBinary, "image/jpeg", "", "", fileName);
                                var url = _pictureService.GetPictureUrl(picture);

                                pictures.Add(picture);

                                foreach (var p in pictures)
                                {
                                    var propertyPicture = new PropertyPicture
                                    {
                                        Picture = p,
                                        Property = property,
                                        IsLogo = false
                                    };
                                    property.Pictures.Add(propertyPicture);
                                    _propertyService.UpdateProperty(property);
                                }
                                fileStream.Flush();
                                fileStream.Close();
                                #endregion
                            }
                            else if (otherFileArr.Contains(file.Split('\\')[file.Split('\\').Length - 1]))
                            {
                                #region 其他附件
                                var fileName = System.IO.Path.GetFileName(file);
                                var ext = System.IO.Path.GetExtension(file);
                                var fileStream = new FileStream(file, FileMode.Open);
                                var fileBinary = new byte[fileStream.Length];
                                fileStream.Read(fileBinary, 0, fileBinary.Length);

                                var otherFile = _fileService.InsertFile(fileBinary, fileName, ext, "");
                                var url = _fileService.GetFileUrl(otherFile);
                                otherFiles.Add(otherFile);

                                foreach (var f in otherFiles)
                                {
                                    var propertyFile = new PropertyFile
                                    {
                                        File = f,
                                        Property = property
                                    };
                                    property.Files.Add(propertyFile);
                                    _propertyService.UpdateProperty(property);
                                }
                                fileStream.Flush();
                                fileStream.Close();
                                #endregion
                            }
                        }

                        #endregion

                        property.Region = _propertyService.GetPropertyRegion(property.Location);

                        #region 资产逻辑验证

                        var sum = property.CurrentUse_Idle + property.CurrentUse_Lend + property.CurrentUse_Rent + property.CurrentUse_Self;
                        if (property.PropertyType == PropertyType.House)
                        {
                            if (property.ConstructArea < sum)
                            {
                                throw new Exception("建筑面积应大于自用、出租、出借、闲置面积之和");
                            }
                        }

                        if (property.PropertyType == PropertyType.Land)
                        {
                            if (property.LandArea < sum)
                            {
                                throw new Exception("土地面积应大于自用、出租、出借、闲置面积之和");
                            }
                        }

                        if (!string.IsNullOrEmpty(property.EstateId) &&
                            (!string.IsNullOrEmpty(property.ConstructId) || !string.IsNullOrEmpty(property.LandId)))
                            throw new Exception("不动产证和房产土地证不应同时填写");


                        property.HasConstructID = !string.IsNullOrEmpty(property.EstateId) || !string.IsNullOrEmpty(property.ConstructId);  //是否拥有房产证
                        property.HasLandID = !string.IsNullOrEmpty(property.EstateId) || !string.IsNullOrEmpty(property.LandId);  //是否土地证                     
                        #endregion

                        properties.Add(property);
                        _propertyService.InsertProperty(property);
                    }
                    catch (Exception e)
                    {
                        sb.AppendLine(string.Format("第{0}行数据导入失败，错误原因为：{1}。", row, e.Message));
                    }
                    finally
                    {
                        row++;
                    }
                    #endregion
                }

                foreach (var property in properties)
                {
                    try
                    {
                        //添加一个资产插入申请
                        var propertyNewRecord = new PropertyNewCreate()
                        {
                            Property = property,
                            Title = property.Name,
                            State = PropertyApproveState.Start,
                            ProcessDate = DateTime.Now,
                            SuggestGovernmentId = currentUser.Government.Id
                        };

                        _propertyNewCreateService.InsertPropertyNewCreate(propertyNewRecord);
                    }
                    catch (Exception e)
                    {
                        sb.AppendLine(string.Format("名称为 {0} 的导入失败，错误原因为：{1}。", property.Name, e.Message));
                    }
                    finally
                    {
                        sb.AppendLine("成功导入资产" + properties.Count() + "条。");
                    }
                }

                return sb.ToString();
            }
        }

        protected virtual int GetColumnIndex(string[] properties, string columnName)
        {
            if (properties == null)
                throw new ArgumentNullException("properties");

            if (columnName == null)
                throw new ArgumentNullException("columnName");

            for (int i = 0; i < properties.Length; i++)
                if (properties[i].Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return i + 1; //excel indexes start from 1
            return 0;
        }

        public class Point {
            public double lng { get; set; }
            public double lat { get; set; }
        }


    }
}
