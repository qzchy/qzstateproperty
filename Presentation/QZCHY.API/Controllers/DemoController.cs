using Microsoft.SqlServer.Types;
using QZCHY.Core;
using QZCHY.Core.Domain.AccountUsers;
using QZCHY.Core.Domain.Common;
using QZCHY.Core.Domain.Media;
using QZCHY.Core.Domain.Properties;
using QZCHY.Services.AccountUsers;
using QZCHY.Services.Authentication;
using QZCHY.Services.Common;
using QZCHY.Services.Configuration;
using QZCHY.Services.Messages;
using QZCHY.Services.Property;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Data.OleDb;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace QZCHY.API.Controllers
{
    [RoutePrefix("Demo")]
    public class DemoController : ApiController
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IAccountUserService _accountUserService;
        private readonly IAccountUserRegistrationService _accountUserRegistrationService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly IWorkflowMessageService _workflowMessageService;
        private readonly IGovernmentService _governmentService;
        private readonly IPropertyService _propertyService;
        private readonly IPropertyAllotService _propertyAllotService;
        private readonly IPropertyLendService _propertyLendService;
        private readonly IPropertyNewCreateService _propertyNewCreateService;
        private readonly IPropertyOffService _propertyOffService;
        private readonly IPropertyRentService _propertyRentService;


        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;
        private readonly AccountUserSettings _accountUserSettings;
        private readonly CommonSettings _commonSettings;

        private readonly ISettingService _settingService;

        public DemoController()
        {

        }

        public DemoController(IAuthenticationService authenticationService, IAccountUserService customerService,
            IAccountUserRegistrationService customerRegistrationService,
        IGenericAttributeService genericAttributeService,
       IWorkflowMessageService workflowMessageService, IGovernmentService governmentService, IPropertyService propertyService,
       IPropertyAllotService propertyAllotService, IPropertyLendService propertyLendService, IPropertyNewCreateService propertyNewCreateService,
       IPropertyOffService propertyOffService, IPropertyRentService propertyRentService,
        IWebHelper webHelper,
            IWorkContext workContext,
        AccountUserSettings customerSettings, CommonSettings commonSettings, ISettingService settingService
            )
        {
            _authenticationService = authenticationService;
            _accountUserService = customerService;
            _accountUserRegistrationService = customerRegistrationService;
            _genericAttributeService = genericAttributeService;
            _workflowMessageService = workflowMessageService;
            _governmentService = governmentService;
            _propertyService = propertyService;
            _propertyAllotService = propertyAllotService;
            _propertyLendService = propertyLendService;
            _propertyNewCreateService = propertyNewCreateService;
            _propertyOffService = propertyOffService;
            _propertyRentService = propertyRentService;


            _webHelper = webHelper;
            _workContext = workContext;
            _accountUserSettings = customerSettings;

            _commonSettings = commonSettings;
            _settingService = settingService;
        }

        [Route("settings")]
        public IHttpActionResult GetAll()
        {

            _commonSettings.TelAndMobliePartten = @"^(0[0-9]{2,3}\-)?([2-9][0-9]{6,7})+(\-[0-9]{1,4})?$|(^(13[0-9]|15[0|3|6|7|8|9]|18[0-9])\d{8}$)";
            _commonSettings.Time24Partten = @"^((1|0?)[0-9]|2[0-4]):([0-5][0-9])";           

            _settingService.SaveSetting<CommonSettings>(_commonSettings);

            _settingService.SaveSetting(new MediaSettings
            {
                AvatarPictureSize = 120,
                ProductThumbPictureSize = 415,
                ProductDetailsPictureSize = 550,
                ProductThumbPictureSizeOnProductDetailsPage = 100,
                AssociatedProductPictureSize = 220,
                CategoryThumbPictureSize = 450,
                ManufacturerThumbPictureSize = 420,
                CartThumbPictureSize = 80,
                MiniCartThumbPictureSize = 70,
                AutoCompleteSearchThumbPictureSize = 20,
                MaximumImageSize = 1980,
                DefaultPictureZoomEnabled = false,
                DefaultImageQuality = 80,
                MultipleThumbDirectories = false
            });

            _settingService.SaveSetting(_accountUserSettings);

            return Ok("配置保存成功");
        }

        [HttpGet]
        [Route("SetLocation")]
        public IHttpActionResult Test()
        {
            //var properties = _propertyService.GetAllProperties();
            //foreach (var property in properties)
            //{
            //    if (property.X == 0 || property.Y == 0) continue;
            //    //if (property.Extent != null)
            //    //    property.WKT = property.Extent.ToString();
            //    //else property.WKT = property.Location.ToString();

            //    property.Location = DbGeography.FromText("POINT(" + property.Y + " " + property.X + ")");

            //    _propertyService.UpdateProperty(property);
            //}

           // var governments = _governmentService.GetAllGovernmentUnits();
           //List<string> names = new List<string>() ;
           // foreach (var gov in governments)
           // {
           //     names.Add(gov.Name);
           // }

           // var filePath = @"D:\企业.xls";
           // string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath + ";" + "Extended Properties=Excel 8.0;";
           // System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection(strConn);
           // conn.Open();
           // string strExcel = "";
           // System.Data.OleDb.OleDbDataAdapter myCommand = null;
           // System.Data.DataSet ds = null;
           // strExcel = "select * from [五级$]";
           // myCommand = new System.Data.OleDb.OleDbDataAdapter(strExcel, strConn);
           // ds = new System.Data.DataSet();
           // myCommand.Fill(ds, "table1");

           // var table = ds.Tables[0];

           // for (var i = 0; i < table.Rows.Count; i++) {

           //     var row = table.Rows[i];
           //     if (!names.Contains(row[0].ToString()))
           //     {
           //         GovernmentUnit goverment = new GovernmentUnit();
           //         var g = governments.Where(m => m.Name == row[1].ToString()).FirstOrDefault();
           //         goverment.Name = row[0].ToString();
           //         goverment.ParentGovernmentId = g.Id;

           //         _governmentService.InsertGovernmentUnit(goverment);
           //     }

           // }


            return Ok("finish");
        }

        [HttpGet]
        [Route("SetPropertyConut")]
        public IHttpActionResult SetPropertyConut() {

            var goverments = _governmentService.GetAllGovernmentUnits();

            foreach (var g in goverments) {

                if (g.ParentGovernmentId != 0) {
                    var goverment = _governmentService.GetGovernmentUnitById(g.Id);
                    var parent = _governmentService.GetGovernmentUnitById(g.ParentGovernmentId);

                    goverment.ParentName = parent.Name;
                    _governmentService.UpdateGovernmentUnit(goverment);

                }
               

             
            }


            return Ok("赋值完成");
        }



        [HttpGet]
        [Route("SetRoles")]
        public IHttpActionResult SetRoles()
        {
            #region 用户角色创建

            var roleNames = new List<string> {
                SystemAccountUserRoleNames.Administrators,
                SystemAccountUserRoleNames.DataReviewer,
                SystemAccountUserRoleNames.GovAuditor,
                SystemAccountUserRoleNames.StateOwnerAuditor,
                SystemAccountUserRoleNames.ParentGovernmentorAuditor,
                SystemAccountUserRoleNames.Registered
            };

            foreach (var roleName in roleNames)
            {
                var role = _accountUserService.GetAccountUserRoleBySystemName(roleName);
                if (role == null)
                {
                    role = new AccountUserRole
                    {
                        Name = roleName,
                        Active = true,
                        IsSystemRole = true,
                        SystemName = roleName
                    };

                    _accountUserService.InsertAccountUserRole(role);
                }
            }
            #endregion

            return Ok("角色配置完成");
        }


        [HttpGet]
        [Route("Import")]
        public IHttpActionResult ImportCustomers()
        {
            //return BadRequest("导入关闭");

            var filePath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/import.mdb");

            var result = ReadXlsFile(filePath);

            #region old code
            //#region 用户角色创建
            //var crAdministrators = new AccountUserRole
            //{
            //    Name = SystemAccountUserRoleNames.Administrators,
            //    Active = true,
            //    IsSystemRole = true,
            //    SystemName = "管理员"
            //};

            //var crRegistered = new AccountUserRole
            //{
            //    Name = SystemAccountUserRoleNames.Registered,
            //    Active = true,
            //    IsSystemRole = true,
            //    SystemName = "注册单位",
            //};
            //var crGuests = new AccountUserRole
            //{
            //    Name = SystemAccountUserRoleNames.Guests,
            //    Active = true,
            //    IsSystemRole = true,
            //    SystemName = "访客",
            //};

            //_accountUserService.InsertAccountUserRole(crAdministrators);
            //_accountUserService.InsertAccountUserRole(crRegistered);
            //_accountUserService.InsertAccountUserRole(crGuests);
            //#endregion

            //#region 测试组织机构
            //var csj = _governmentService.GetGovernmentUnitById(37);

            ////var cz = new GovernmentUnit
            ////{
            ////    Name = "市财政局",
            ////    GovernmentType = GovernmentType.Government,
            ////    Person = "联系人",
            ////    Tel="0570-5062456"
            ////};

            ////var ghj = new GovernmentUnit
            ////{
            ////    Name = "市规划局",
            ////    GovernmentType = GovernmentType.Government,
            ////    Person = "规划局",
            ////    Tel="0570-3021456"
            ////};

            ////var kcgh = new GovernmentUnit
            ////{
            ////    Name = "市规划局柯城分局",
            ////    GovernmentType = GovernmentType.Government,
            ////    Person = "市规划局柯城分局",
            ////    Tel = "0570-3021456"
            ////};

            ////var qjfj = new GovernmentUnit
            ////{
            ////    Name = "市规划局衢江分局",
            ////    GovernmentType = GovernmentType.Government,
            ////    Person = "市规划局衢江分局",
            ////    Tel = "0570-3021456"
            ////};

            ////var jjq = new GovernmentUnit
            ////{
            ////    Name = "市规划局绿色产业集聚区分局",
            ////    GovernmentType = GovernmentType.Government,
            ////    Person = "市规划局绿色产业集聚区分局",
            ////    Tel = "0570-3021456"
            ////};

            ////var xq = new GovernmentUnit
            ////{
            ////    Name = "市规划局西区分局",
            ////    GovernmentType = GovernmentType.Government,
            ////    Person = "市规划局西区分局",
            ////    Tel = "0570-3021456"
            ////};

            ////context.Set<GovernmentUnit>().AddOrUpdate(cz, ghj, kcgh, qjfj, jjq, xq);
            ////#endregion

            ////#region 用户创建
            //var user = new AccountUser()
            //{               
            //    UserName = "财政局",
            //    AccountUserGuid = Guid.NewGuid(),
            //    Active = true,
            //    CreatedOn = DateTime.Now,
            //    IsSystemAccount = false,
            //    Password = "123456",
            //    PasswordFormat = PasswordFormat.Clear,
            //    LastActivityDate = DateTime.Now,
            //    Deleted = false,
            //    UpdatedOn = DateTime.Now,
            //    Government=csj
            //};
            //user.AccountUserRoles.Add(crAdministrators);
            //user.AccountUserRoles.Add(crRegistered);
            //_accountUserService.InsertAccountUser(user);

            //#endregion
            #endregion

            return Ok("导入结束\n");
        }

        [HttpGet]
        [Route("Nextstep")]
        public IHttpActionResult Nextstep()
        {
            //return BadRequest("导入关闭");

            var filePath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/0601import.mdb");

            var strConn = "Provider = Microsoft.Jet.OLEDB.4.0;Data Source = " + filePath;
            System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection(strConn);
            conn.Open();
            //string strExcel = "";
            //System.Data.OleDb.OleDbDataAdapter myCommand = null;
            //System.Data.DataSet ds = null;
            //strExcel = "select * from t1";
            //myCommand = new System.Data.OleDb.OleDbDataAdapter(strExcel, strConn);
            //ds = new System.Data.DataSet(); myCommand.Fill(ds, "table1");

            //建立SQL查询   
            OleDbCommand odCommand = conn.CreateCommand();

            //3、输入查询语句 C#操作Access之读取mdb  

            odCommand.CommandText = "select * from sheet2";

            //建立读取   
            OleDbDataReader odrReader = odCommand.ExecuteReader();

            //查询并显示数据   
            int size = odrReader.FieldCount;


            while (odrReader.Read())
            {
                try
                {
                    #region 遍历要素

                    var id = odrReader[1].ToString();
                    int result = 0;

                    if (!int.TryParse(id, out result)) continue;

                    var property = _propertyService.GetAllProperties().Where(p => p.DisplayOrder == result).FirstOrDefault();
                    if (property == null) continue;



                    #region 获取相关参数
                    var name = odrReader[2].ToString();
                    var address = odrReader[3].ToString();
                    bool isDevelopment = odrReader[4].ToString() == "1";
                    bool isStoreup = odrReader[5].ToString() == "1";
                    bool isAuction = odrReader[6].ToString() == "1";
                    bool isAdjust = odrReader[7].ToString() == "1";
                    bool isInjectionCT = odrReader[8].ToString() == "1";
                    bool isInjectionJT = odrReader[9].ToString() == "1";
                    bool isGreenland = odrReader[10].ToString() == "1";
                    bool isSelf = odrReader[11].ToString() == "1";
                    bool isHouse = odrReader[12].ToString() == "1";

                    #endregion

                    //if (property.Name != name || property.Address != address)
                    //    throw new Exception("不匹配");

                    if (isStoreup|| isDevelopment)
                        property.NextStepUsage = NextStepType.Storeup;
                    if (isAdjust)
                        property.NextStepUsage = NextStepType.Adjust;
                    if (isAuction)
                        property.NextStepUsage = NextStepType.Auction;
                    if (isInjectionCT)
                        property.NextStepUsage = NextStepType.InjectionCT;
                    if (isGreenland)
                        property.NextStepUsage = NextStepType.Greenland;
                    if (isInjectionJT)
                        property.NextStepUsage = NextStepType.InjectionJT;
                    if (isHouse)
                        property.NextStepUsage = NextStepType.House;
                    if (isSelf)
                        property.NextStepUsage = NextStepType.Self;

                    #endregion

                    _propertyService.UpdateProperty(property);
                }
                catch (Exception e)
                {

                    //throw new Exception(string.Format("序号为 {0} 的要素异常，错误为：", id, e.Message));
                }


            }

            //关闭连接 C#操作Access之读取mdb  
            odrReader.Close();
            conn.Close();



            return Ok("导入结束\n");
        }

        /// <summary>
        /// 读取xls文件
        /// </summary>
        /// <param name="filePath"></param>
        public string ReadXlsFile(string filePath)
        {
            //var filePath = @"F:\国有资产展示系统\QZCHY.QZStatePropertyManagementSystem\Presentation\QZCHY.API\App_Data\import.xls";

            //string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath + ";" + "Extended Properties=Excel 8.0;";
            var strConn = "Provider = Microsoft.Jet.OLEDB.4.0;Data Source = " + filePath;
            System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection(strConn);
            conn.Open();
            //string strExcel = "";
            //System.Data.OleDb.OleDbDataAdapter myCommand = null;
            //System.Data.DataSet ds = null;
            //strExcel = "select * from t1";
            //myCommand = new System.Data.OleDb.OleDbDataAdapter(strExcel, strConn);
            //ds = new System.Data.DataSet(); myCommand.Fill(ds, "table1");

            //建立SQL查询   
            OleDbCommand odCommand = conn.CreateCommand();

            //3、输入查询语句 C#操作Access之读取mdb  

            odCommand.CommandText = "select * from t0";

            //建立读取   
            OleDbDataReader odrReader = odCommand.ExecuteReader();

            //查询并显示数据   
            int size = odrReader.FieldCount;

            StringBuilder resultSb = new StringBuilder();

            while (odrReader.Read())
            {

                #region 遍历要素
                StringBuilder sb = new StringBuilder();
                GovernmentUnit government = null;
                GovernmentUnit parentGovernment = null;

                var id = odrReader[1].ToString();
                int result = 0;

                if (!int.TryParse(id, out result)) continue;


                #region 获取相关参数
                string field = "";

                string governmentName = odrReader[2].ToString();
                string parentGovernmentName = odrReader[3].ToString();
                bool isXZ = odrReader[4].ToString() == "1";
                bool isSY = odrReader[5].ToString() == "1";
                bool isQY = odrReader[6].ToString() == "1";
                string type = odrReader[7].ToString();
                string name = odrReader[8].ToString();
                string address = odrReader[9].ToString();
                string construstArea = odrReader[10].ToString();
                string landArea = odrReader[11].ToString();
                string constructAndLand = odrReader[12].ToString();
                string constructType = odrReader[13].ToString();
                string landType = odrReader[14].ToString();
                string price = odrReader[15].ToString();
                string getedDate = odrReader[16].ToString();
                string lifeTime = odrReader[17].ToString();
                string usePeople = odrReader[18].ToString();

                string self = odrReader[19].ToString();
                string rent = odrReader[20].ToString();
                string lend = odrReader[21].ToString();
                string idle = odrReader[22].ToString();

                bool isDevelopment = odrReader[23].ToString() == "1";
                bool isStoreup = odrReader[24].ToString() == "1";
                bool isAuction = odrReader[25].ToString() == "1";
                bool isAdjust = odrReader[26].ToString() == "1";
                bool isInjection = odrReader[27].ToString() == "1";
                bool isGreenland = odrReader[28].ToString() == "1";
                bool isSelf = odrReader[29].ToString() == "1";
                string remark = odrReader[30].ToString();
                string geo = odrReader[31].ToString();
                string region = odrReader[32].ToString();
                string hasCID = odrReader[33].ToString();
                string hasLID = odrReader[34].ToString();
                #endregion

                #region 单位更新
                if (parentGovernmentName != governmentName && !string.IsNullOrEmpty(parentGovernmentName))
                {
                    parentGovernment = _governmentService.GetGovernmentUnitByName(parentGovernmentName);
                    if (parentGovernment == null)
                    {
                        parentGovernment = new GovernmentUnit
                        {
                            Name = parentGovernmentName,
                            DisplayOrder = 0
                        };

                        _governmentService.InsertGovernmentUnit(parentGovernment);
                    }
                }

                government = _governmentService.GetGovernmentUnitByName(governmentName);
                if (government == null)
                {
                    government = new GovernmentUnit
                    {
                        Name = governmentName,
                        DisplayOrder = 0
                    };

                    if (parentGovernment != null) government.ParentGovernmentId = parentGovernment.Id;
                    if (isXZ) government.GovernmentType = GovernmentType.Government;
                    if (isSY) government.GovernmentType = GovernmentType.Institution;
                    if (isQY) government.GovernmentType = GovernmentType.Company;

                    _governmentService.InsertGovernmentUnit(government);
                }
                else
                {
                    if (parentGovernment != null) government.ParentGovernmentId = parentGovernment.Id;
                    if (isXZ) government.GovernmentType = GovernmentType.Government;
                    if (isSY) government.GovernmentType = GovernmentType.Institution;
                    if (isQY) government.GovernmentType = GovernmentType.Company;

                    _governmentService.UpdateGovernmentUnit(government);
                }
                #endregion

                try
                {
                    #region 增加资产

                    #region 属性
                    var property = new Property();
                    property.DisplayOrder = int.Parse(id);

                    if (government == null) sb.Append("产权单位为空；");
                    else property.Government = government;

                    switch (type)
                    {
                        case "房屋": property.PropertyType = PropertyType.House; break;
                        case "土地": property.PropertyType = PropertyType.Land; break;
                        case "对应房屋土地": property.PropertyType = PropertyType.LandUnderHouse; break;
                        default: property.PropertyType = PropertyType.Others; break;
                    }

                    if (string.IsNullOrEmpty(name)) { sb.Append("名称为空；"); property.Name = "无名称"; }
                    else property.Name = name;

                    if (string.IsNullOrEmpty(address)) { sb.Append("地址为空；"); }
                    else property.Address = address;

                    if (!string.IsNullOrWhiteSpace(construstArea)) property.ConstructArea = Convert.ToSingle(construstArea);
                    if (!string.IsNullOrWhiteSpace(landArea)) property.LandArea = Convert.ToSingle(landArea);

                    property.PropertyID = constructAndLand;
                    property.HasConstructID = hasCID=="有";
                    property.HasLandID = hasLID== "有";
                    property.PropertyNature = constructType;
                    property.LandNature = landType;

                    if (!string.IsNullOrWhiteSpace(price)) property.Price = Convert.ToSingle(price);

                    if (!string.IsNullOrEmpty(getedDate))
                    {
                        try
                        {
                            property.GetedDate = Convert.ToDateTime(getedDate); ;
                        }
                        catch
                        {
                            sb.Append("取得日期格式不正确；");
                        }

                    }

                    property.LifeTime = string.IsNullOrEmpty(lifeTime) ? 0 : int.Parse(lifeTime);
                    property.UsedPeople = usePeople;

                    if (!string.IsNullOrWhiteSpace(self)) property.CurrentUse_Self = Convert.ToSingle(self);
                    if (!string.IsNullOrWhiteSpace(rent)) property.CurrentUse_Rent = Convert.ToSingle(rent);
                    if (!string.IsNullOrWhiteSpace(lend)) property.CurrentUse_Lend = Convert.ToSingle(lend);
                    if (!string.IsNullOrWhiteSpace(idle)) property.CurrentUse_Idle = Convert.ToSingle(idle);

                    if (Convert.ToInt32(isDevelopment) + Convert.ToInt32(isAuction) + Convert.ToInt32(isAdjust)
                          + Convert.ToInt32(isInjection) + Convert.ToInt32(isGreenland) + Convert.ToInt32(isSelf) > 1)
                    {
                        sb.Append("下一步操作有重复；");
                    }
                    else
                    {
                        //if (isDevelopment) property.NextStepUsage = NextStepType.Development;
                        //if (isStoreup) property.NextStepUsage = NextStepType.Storeup;
                        //if (isAuction) property.NextStepUsage = NextStepType.Auction;
                        //if (isAdjust) property.NextStepUsage = NextStepType.Adjust;
                        //if (isInjection) property.NextStepUsage = NextStepType.InjectionJT;
                        //if (isGreenland) property.NextStepUsage = NextStepType.Greenland;
                        //if (isSelf) property.NextStepUsage = NextStepType.Self;
                    } 
                    #endregion


                    string wkt = "";

                    DbGeography graphy = null;

                    List<Point> points = new List<Point>();

                    try
                    {
                        if (!geo.StartsWith("[")) geo = "[" + geo + "]";

                        points = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Point>>(geo);

                        StringBuilder wktsb = new StringBuilder();

                        //末尾加上起点闭合多边形
                        if (points.Count > 1) points.Add(points[0]);

                        foreach (var point in points)
                        {
                            if(point.lng<point.lat)
                            {
                                var t = point.lng;
                                point.lng = point.lat;
                                point.lat = t;
                            }
                            wktsb.Append(string.Format("{0} {1},", point.lng, point.lat));
                        }

                        if (points.Count == 1 || points.Count == 2)
                        {
                            wkt = string.Format("POINT({0})", wktsb.ToString().TrimEnd(','));
                        }
                        else
                        {
                            wkt = string.Format("POLYGON(({0}))", wktsb.ToString().TrimEnd(','));
                        }
                        property.WKT = wkt;
                        SqlGeography sqlGeography = SqlGeography.Parse(wkt).MakeValid();
                        graphy = DbGeography.FromText(sqlGeography.ToString());
                    }
                    catch (Exception e)
                    {
                        if (points.Count > 0)
                            property.Location = DbGeography.FromText(string.Format("POINT({0} {1})", points[0].lng, points[1].lat));
                    }
                    finally
                    {
                        if (graphy == null)
                        {
                            sb.Append("坐标格式有误，无法生成几何图形；");

                            property.Location = DbGeography.FromText("POINT(118.52 28.88)");
                        }
                        else
                        {
                            try
                            {
                                if (graphy.PointCount > 1)
                                {
                                    if (graphy.PointCount == 2) property.Location = graphy.PointAt(1);
                                    else
                                    {
                                        property.Extent = graphy;

                                        DbGeometry geometry = DbGeometry.FromText(graphy.ToString().Replace("SRID=4326;", ""));
                                        if (geometry.Centroid != null)
                                            property.Location = DbGeography.FromText(string.Format("POINT({0} {1})", geometry.Centroid.XCoordinate, geometry.Centroid.YCoordinate));
                                        else
                                            property.Location = graphy.PointAt(1);
                                    }
                                }
                                else property.Location = graphy;
                            }
                            catch
                            {
                                property.Location = DbGeography.FromText("POINT(118.52 28.88)");
                            }                         
                        }

                        property.X = property.Location.Longitude.Value;
                        property.Y = property.Location.Latitude.Value;
                    }

                    switch (region)
                    {
                        case "老城区": property.Region = Region.OldCity; break;
                        case "西区": property.Region = Region.West; break;
                        case "集聚区": property.Region = Region.Clusters; break;
                        case "柯城区": property.Region = Region.KC; break;
                        case "衢江区": property.Region = Region.QJ; break;
                        default: property.Region = Region.Others; break;
                    }

                    property.Description = remark;
                    property.Error = sb.ToString();

                    _propertyService.InsertProperty(property);

                    #endregion
                }
                catch (Exception e)
                {
                    resultSb.Append(string.Format("序号为 {0} 的要素异常，错误为：{1}\n", id,
                        string.IsNullOrEmpty(e.Message) ? e.InnerException.Message : e.Message));
                    //throw new Exception(string.Format("序号为 {0} 的要素异常，错误为：", id, e.Message));
                }

                #endregion

            }

            //关闭连接 C#操作Access之读取mdb  
            odrReader.Close();
            conn.Close();
         

            return resultSb.ToString();
        }



        [HttpGet]
        [Route("insertgov")]
        public IHttpActionResult insertgov()
        {
            return BadRequest("导入关闭");

            var filePath = System.Web.HttpContext.Current.Server.MapPath("~/App_Data/import.mdb");

            var strConn = "Provider = Microsoft.Jet.OLEDB.4.0;Data Source = " + filePath;
            System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection(strConn);
            conn.Open();
            //string strExcel = "";
            //System.Data.OleDb.OleDbDataAdapter myCommand = null;
            //System.Data.DataSet ds = null;
            //strExcel = "select * from t1";
            //myCommand = new System.Data.OleDb.OleDbDataAdapter(strExcel, strConn);
            //ds = new System.Data.DataSet(); myCommand.Fill(ds, "table1");

            //建立SQL查询   
            OleDbCommand odCommand = conn.CreateCommand();

            //3、输入查询语句 C#操作Access之读取mdb  

            odCommand.CommandText = "select * from sheet1";

            //建立读取   
            OleDbDataReader odrReader = odCommand.ExecuteReader();

            //查询并显示数据   
            int size = odrReader.FieldCount;


            while (odrReader.Read())
            {
                #region 获取相关参数
                var displayOrder = odrReader[1].ToString();
                var parent = odrReader[2].ToString();
                var name = odrReader[3].ToString();
                var address = odrReader[4].ToString();
                bool isGov = odrReader[5].ToString() == "1";
                bool isIns = odrReader[6].ToString() == "1";
                bool isGroup = odrReader[7].ToString() == "1";
                bool ishb = odrReader[8].ToString() == "1";
                bool iszr = odrReader[9].ToString() == "1";
                bool iszy = odrReader[10].ToString() == "1";
                var landArea = odrReader[11].ToString();
                var oArea = odrReader[12].ToString();
                var cArea = odrReader[13].ToString();
                var floor = odrReader[14].ToString();
                bool ccard = odrReader[15].ToString() == "1";
                bool lcard = odrReader[16].ToString() == "1";
                bool rentin = odrReader[17].ToString() == "1";
                bool rent = odrReader[18].ToString() == "1";
                bool lend = odrReader[19].ToString() == "1";
                var remark = odrReader[20].ToString();
                var x = odrReader[21].ToString();
                var y = odrReader[22].ToString();
                #endregion

                try
                {
                    #region 遍历要素

                    var id = odrReader[0].ToString();
                    int result = 0;

                    if (!int.TryParse(id, out result)) continue;

                    var government = _governmentService.GetGovernmentUnitByName(name);

                    if (government == null)
                        government = new GovernmentUnit();

                    if (!string.IsNullOrEmpty(displayOrder))
                    {
                        government.DisplayOrder = Convert.ToInt32(displayOrder);
                    }
                    else
                    {
                        var orders = parent.Split('.');
                        //获取最后一个作为显示次序
                        government.DisplayOrder = Convert.ToInt32(orders[orders.Length - 1]);

                        if (government.ParentGovernmentId == 0)
                        {
                            //寻找父节点
                            var all = _governmentService.GetAllGovernmentUnits();

                            var f = all.Where(g => g.DisplayOrder.ToString() == orders[0] && g.ParentGovernmentId == 0).FirstOrDefault();

                            if (f == null)
                                throw new Exception(string.Format("名称为{0}的找不到父节点", name));

                            if (orders.Length == 2)
                                government.ParentGovernmentId = f.Id;
                            else if (orders.Length == 3)
                            {
                                var s = all.Where(g => g.DisplayOrder.ToString() == orders[1] && g.ParentGovernmentId == f.Id).FirstOrDefault();
                                if (s == null) throw new Exception(string.Format("名称为{0}的找不到父节点", name));
                                government.ParentGovernmentId = s.Id;
                            }
                        }
                    }
                    government.Name = name;
                    government.Address = address;
                    if (isGov) government.GovernmentType = GovernmentType.Government;
                    if (isIns) government.GovernmentType = GovernmentType.Institution;
                    if (isGroup) government.GovernmentType = GovernmentType.Group;
                    if (ishb) government.LandOrigin = "划拨";
                    if (iszr) government.LandOrigin = "转让";
                    if (iszy) government.LandOrigin = "租用";

                    government.LandArea = string.IsNullOrEmpty(landArea) ? 0 : Convert.ToDouble(landArea);
                    government.ConstructArea = string.IsNullOrEmpty(cArea) ? 0 : Convert.ToDouble(cArea);
                    government.Floor = string.IsNullOrEmpty(floor) ? 0 : Convert.ToInt32(floor);

                    government.HasConstructCard = ccard;
                    government.HasLandCard = lcard;
                    government.HasLendInCard = rentin;
                    government.HasRentCard = rent;
                    government.HasLendInCard = lend;

                    government.Remark = remark;

                    if (!string.IsNullOrEmpty(x) && !string.IsNullOrEmpty(y))
                    {
                        government.X = Convert.ToDouble(x);
                        government.Y = Convert.ToDouble(y);
                        government.Location = DbGeography.FromText("POINT(" + government.X + " " + government.Y + ")");
                    }
                    else
                        DbGeography.FromText("POINT(0 0)");


                        #endregion

                        if (government.Id == 0) _governmentService.InsertGovernmentUnit(government);
                    else _governmentService.UpdateGovernmentUnit(government);


                }
                catch (Exception e)
                {

                    throw new Exception(string.Format("序号为 {0} 的要素异常，错误为：{1}", name, e.Message));
                }


            }

            //关闭连接 C#操作Access之读取mdb  
            odrReader.Close();
            conn.Close();



            return Ok("导入结束\n");
        }


        public class Point
        {
            public double lng { get; set; }

            public double lat { get; set; }
        }
    }
}
