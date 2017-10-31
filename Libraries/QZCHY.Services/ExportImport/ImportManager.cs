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
        private readonly IPropertyNewCreateService _propertyNewCreateService;
        private readonly IWorkContext _workContext;

        public ImportManager(IPropertyService propertyService, IGovernmentService governmentService, IAccountUserService accountUserServic, IWorkContext workContext, IPictureService pictureService, IPropertyNewCreateService propertyNewCreateService)
        {
            _propertyService = propertyService;
            _governmentService = governmentService;
            _accountUserService = accountUserServic;
            _workContext = workContext;
            _pictureService = pictureService;
            _propertyNewCreateService = propertyNewCreateService;
        }


        public string ImportProductsFromXlsx(Stream stream,string path)
        {
            IList<Core.Domain.Properties.Property> properties = new List<Core.Domain.Properties.Property>();

            using (var xlPackage = new ExcelPackage(stream)) {

                var worksheet = xlPackage.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null)
                    throw new Exception("没有找到excel表格");

                var excelProperty = new[]
                {
                "资产名称",
                "产权单位",
                "资产类别",
                "所处区域",
                "地址",
                "建筑面积(平方米)",
                "土地面积(平方米)",
                "产权证号",
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
                "Location",
                "Extent",
                 "Logo",
                "图片附件"
                };        

                //数据插入
                int row = 2;
                StringBuilder sb = new StringBuilder();
                while (true) {

                    bool allColumnsAreEmpty = true;
                    for (var i = 1; i <= excelProperty.Length; i++)
                        if (worksheet.Cells[row, i].Value != null && !String.IsNullOrEmpty(worksheet.Cells[row, i].Value.ToString()))
                        {
                            allColumnsAreEmpty = false;
                            break;
                        }
                    if (allColumnsAreEmpty)
                        break;
              
                        var property = new Core.Domain.Properties.Property();
                        
                        List<Point> points = new List<Point>();
                        if(worksheet.Cells[row, GetColumnIndex(excelProperty, "资产名称")].Value !=null)  property.Name = worksheet.Cells[row, GetColumnIndex(excelProperty, "资产名称")].Value.ToString();
                        property.Government = _workContext.CurrentAccountUser.Government;

                        #region 资产类别导入
                    if (worksheet.Cells[row, GetColumnIndex(excelProperty, "资产类别")].Value != null) {
                        switch (worksheet.Cells[row, GetColumnIndex(excelProperty, "资产类别")].Value.ToString()) {
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
                                property.PropertyType = PropertyType.Others;
                                break;

                        }
                    }
                    #endregion

                        #region 所处区域
                    if (worksheet.Cells[row, GetColumnIndex(excelProperty, "所处区域")].Value != null) {
                        switch (worksheet.Cells[row, GetColumnIndex(excelProperty, "所处区域")].Value.ToString()) {
                            case "集聚区":
                                property.Region = Region.Clusters;
                                break;
                            case "柯城区":
                                property.Region = Region.KC;
                                break;
                            case "老城区":
                                property.Region = Region.OldCity;
                                break;
                            case "其他":
                                property.Region = Region.Others;
                                break;
                            case "衢江区":
                                property.Region = Region.QJ;
                                break;
                            case "西区":
                                property.Region = Region.West;
                                break;
                        }
                    }
                    #endregion

                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "地址")].Value != null) property.Address = worksheet.Cells[row, GetColumnIndex(excelProperty, "地址")].Value.ToString();
                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "建筑面积(平方米)")].Value != null) property.ConstructArea =Convert.ToDouble( worksheet.Cells[row, GetColumnIndex(excelProperty, "建筑面积(平方米)")].Value);
                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "土地面积(平方米)")].Value != null) property.LandArea =Convert.ToDouble( worksheet.Cells[row, GetColumnIndex(excelProperty, "土地面积(平方米)")].Value);
                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "产权证号")].Value != null) property.PropertyID = worksheet.Cells[row, GetColumnIndex(excelProperty, "产权证号")].Value.ToString();
                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "房产性质")].Value != null) property.PropertyNature = worksheet.Cells[row, GetColumnIndex(excelProperty, "房产性质")].Value.ToString();
                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "土地性质")].Value != null) property.LandNature = worksheet.Cells[row, GetColumnIndex(excelProperty, "土地性质")].Value.ToString();
                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "账面价格(万元)")].Value != null) property.Price =Convert.ToDouble( worksheet.Cells[row, GetColumnIndex(excelProperty, "账面价格(万元)")].Value);
                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "取得时间")].Value != null) property.GetedDate = Convert.ToDateTime( worksheet.Cells[row, GetColumnIndex(excelProperty, "取得时间")].Value);

                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "使用年限(年)")].Value != null) property.LifeTime =Convert.ToInt32( worksheet.Cells[row, GetColumnIndex(excelProperty, "使用年限(年)")].Value);
                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "使用方")].Value != null) property.UsedPeople = worksheet.Cells[row, GetColumnIndex(excelProperty, "使用方")].Value.ToString();
                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "自用面积(平方米)")].Value != null) property.CurrentUse_Self =Convert.ToDouble( worksheet.Cells[row, GetColumnIndex(excelProperty, "自用面积(平方米)")].Value);

                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "出租面积(平方米)")].Value != null) property.CurrentUse_Rent =Convert.ToDouble( worksheet.Cells[row, GetColumnIndex(excelProperty, "出租面积(平方米)")].Value);
                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "出借面积(平方米)")].Value != null) property.CurrentUse_Lend =Convert.ToDouble( worksheet.Cells[row, GetColumnIndex(excelProperty, "出借面积(平方米)")].Value);
                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "闲置面积(平方米)")].Value != null) property.CurrentUse_Idle =Convert.ToDouble( worksheet.Cells[row, GetColumnIndex(excelProperty, "闲置面积(平方米)")].Value);
                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "不动产证")].Value != null) property.EstateId = worksheet.Cells[row, GetColumnIndex(excelProperty, "不动产证")].Value.ToString();
                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "房产证")].Value != null) property.ConstructId = worksheet.Cells[row, GetColumnIndex(excelProperty, "房产证")].Value.ToString();
                        if (worksheet.Cells[row, GetColumnIndex(excelProperty, "土地证")].Value != null) property.LandId = worksheet.Cells[row, GetColumnIndex(excelProperty, "土地证")].Value.ToString();

                        if (property.ConstructId != null) property.HasConstructID = true;
                        else { property.HasConstructID = false; }

                         if (property.LandId != null) property.HasLandID = true;
                         else { property.HasLandID = false; }

                         #region 坐标转换
                    var location = "";
                    var extent = "";
                    //点
                    if (worksheet.Cells[row, GetColumnIndex(excelProperty, "Location")].Value != null)
                    {
                        location = worksheet.Cells[row, GetColumnIndex(excelProperty, "Location")].Value.ToString();                   
                        var point = Newtonsoft.Json.JsonConvert.DeserializeObject<Point>(location);
                        var  wkt = "POINT(" + point.lng + " " + point.lat + ")";
                         property.Location = DbGeography.FromText(wkt);
                    }
                    //面
                    if (worksheet.Cells[row, GetColumnIndex(excelProperty, "Extent")].Value != null)
                    {
                        extent = worksheet.Cells[row, GetColumnIndex(excelProperty, "Extent")].Value.ToString();                              
                        var ewkt = "POLYGON ((";
                        points = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Point>>(extent);
                        foreach (var point in points) {
                            ewkt += point.lng + " " + point.lat + ",";
                        }
                        ewkt = ewkt.Substring(0, ewkt.Length - 2) +","+points[0].lng+" "+points[0].lat+ "))";
                        property.Extent = DbGeography.FromText(ewkt);
                    }

                    #endregion

                         #region 添加Logo图片
                    var logoName = "";
                    if(worksheet.Cells[row, GetColumnIndex(excelProperty, "Logo")].Value!=null) logoName=worksheet.Cells[row, GetColumnIndex(excelProperty, "Logo")].Value.ToString();
                    var imgArr = new string[] { };
                    if(worksheet.Cells[row, GetColumnIndex(excelProperty, "图片附件")].Value!=null) imgArr=worksheet.Cells[row, GetColumnIndex(excelProperty, "图片附件")].Value.ToString().Split(';');
                
                        var files = System.IO.Directory.GetFiles(path);
                        var pictures = new List<Picture>();
                    foreach (var file in files) {
                        if (file.Split('\\')[file.Split('\\').Length - 1] == logoName)
                        {
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
                        }
                        else if (imgArr.Contains(file.Split('\\')[file.Split('\\').Length - 1]))
                        {

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
                        }
                    }

                    #endregion

                    if (property.Name == null || property.Address == null || worksheet.Cells[row, GetColumnIndex(excelProperty, "资产类别")].Value == null || property.UsedPeople == null || location == "" || logoName == "" )
                    {
                        sb.AppendLine("第" + row + "行数据有问题，请检查必填项是否有空，或者建筑面积和应该大于四项使用面积之和。");
                    }
                    else {
                        properties.Add(property);
                        _propertyService.InsertProperty(property);
                    }                  
                        row++;                    
                }
                sb.AppendLine("成功导入资产"+properties.Count()+"条。");
                foreach (var property in properties)
                {
                    //添加一个资产插入申请
                    var propertyNewRecord = new PropertyNewCreate()
                    {
                        Property = property,
                        Title = property.Name,
                        State = PropertyApproveState.Start,
                        ProcessDate = DateTime.Now,
                        SuggestGovernmentId = _workContext.CurrentAccountUser.Government.Id
                    };

                    _propertyNewCreateService.InsertPropertyNewCreate(propertyNewRecord);
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
