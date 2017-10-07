using Microsoft.SqlServer.Types;
using QZCHY.Core;
using QZCHY.Core.Domain.AccountUsers;
using QZCHY.Core.Domain.Common;
using QZCHY.Core.Domain.Media;
using QZCHY.Core.Domain.Properties;
using QZCHY.Core.Domain.Security;
using QZCHY.Services.AccountUsers;
using QZCHY.Services.Authentication;
using QZCHY.Services.Common;
using QZCHY.Services.Configuration;
using QZCHY.Services.Media;
using QZCHY.Services.Messages;
using QZCHY.Services.Property;
using QZCHY.Services.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Data.OleDb;
using System.IO;
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
        private readonly IPropertyEditService _propertyEditService;
        private readonly IPropertyOffService _propertyOffService;
        private readonly IPropertyRentService _propertyRentService;
        private readonly ICopyPropertyService _copyPropertyService;
        private readonly IEncryptionService _encryptionService;
        private readonly IPictureService _pictureService;

        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;
        private readonly AccountUserSettings _accountUserSettings;
        private readonly CommonSettings _commonSettings;
        private readonly SecuritySettings _securitySettings;
        private readonly ISettingService _settingService;

        public DemoController()
        {

        }

        public DemoController(IAuthenticationService authenticationService, IAccountUserService customerService,
            IAccountUserRegistrationService customerRegistrationService,
        IGenericAttributeService genericAttributeService,
       IWorkflowMessageService workflowMessageService, IGovernmentService governmentService, IPropertyService propertyService,
       IPropertyAllotService propertyAllotService, IPropertyLendService propertyLendService, IPropertyNewCreateService propertyNewCreateService,
       IPropertyOffService propertyOffService, IPropertyRentService propertyRentService, IEncryptionService encryptionService, IPictureService pictureService,
        IPropertyEditService propertyEditService, ICopyPropertyService copyPropertyService,
        IWebHelper webHelper,
            IWorkContext workContext,
        AccountUserSettings customerSettings, CommonSettings commonSettings, SecuritySettings securitySettings, ISettingService settingService
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
            _propertyEditService = propertyEditService;
            _propertyOffService = propertyOffService;
            _propertyRentService = propertyRentService;
            _copyPropertyService = copyPropertyService;
            _encryptionService = encryptionService;
            _pictureService = pictureService;

            _webHelper = webHelper;
            _workContext = workContext;
            _accountUserSettings = customerSettings;

            _commonSettings = commonSettings;
            _securitySettings = securitySettings;
            _settingService = settingService;
        }

        [HttpGet]
        [Route("settings")]
        public IHttpActionResult GetAll()
        {
            _accountUserSettings.DefaultPasswordFormat = PasswordFormat.Encrypted;

            _settingService.SaveSetting<AccountUserSettings>(_accountUserSettings);

            _securitySettings.EncryptionKey = "qzczjwithqzghchy";
            _settingService.SaveSetting(_securitySettings);

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
        [Route("resetpwd")]
        public IHttpActionResult ResetPWD()
        {
            var users = _accountUserService.GetAllAccountUsers();
            foreach (var user in users)
            {
                user.PasswordFormat = PasswordFormat.Encrypted;
                user.Password = _encryptionService.EncryptText(user.Password);

                _accountUserService.UpdateAccountUser(user);
            }

            return Ok();
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
        public IHttpActionResult SetPropertyConut()
        {

            var goverments = _governmentService.GetAllGovernmentUnits();
            var role = _accountUserService.GetAccountUserRoleBySystemName(SystemAccountUserRoleNames.ParentGovernmentorAuditor);
            foreach (var g in goverments)
            {
                g.PropertyConut = g.Properties.Count;
                if (g.ParentGovernmentId != 0)
                {
                    var parent = _governmentService.GetGovernmentUnitById(g.ParentGovernmentId);

                    g.ParentName = parent.Name;



                }

                _governmentService.UpdateGovernmentUnit(g);

                var users = g.Users;
                foreach (var user in users)
                {
                    if (g.ParentGovernmentId == 0)
                    {
                        if (user.AccountUserRoles.Where(ur => ur.Name == SystemAccountUserRoleNames.ParentGovernmentorAuditor).Count() == 0)
                        {

                            user.AccountUserRoles.Add(role);
                            _accountUserService.UpdateAccountUser(user);
                        }
                    }
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
            #region 用户角色创建
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
            #endregion

            #region 测试组织机构
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
            #endregion

            #region 用户创建
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

            #endregion
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

                    if (isStoreup || isDevelopment)
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
                    property.HasConstructID = hasCID == "有";
                    property.HasLandID = hasLID == "有";
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
                            if (point.lng < point.lat)
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
        [Route("InsertHSGS")]
        public IHttpActionResult InsertHSGS()
        {
            #region MyRegion
            var properties = new List<Property>() {
new Property {PropertyType= PropertyType.House , Name="维拉小镇维拉小镇1幢1单元101室",Address="维拉小镇维拉小镇1幢1单元101室",ConstructArea=70.73,LandArea=15.33,EstateId="衢市不动产权0020344号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/21"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇维拉小镇1幢1单元102室",Address="维拉小镇维拉小镇1幢1单元102室",ConstructArea=54.15,LandArea=11.74,EstateId="衢市不动产权0019622号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/18"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇维拉小镇1幢2单元101室",Address="维拉小镇维拉小镇1幢2单元101室",ConstructArea=54.15,LandArea=11.74,EstateId="衢市不动产权0019542号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/18"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇维拉小镇1幢2单元102室",Address="维拉小镇维拉小镇1幢2单元102室",ConstructArea=69.96,LandArea=15.17,EstateId="衢市不动产权0020806号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇维拉小镇1幢3单元101室",Address="维拉小镇维拉小镇1幢3单元101室",ConstructArea=69.96,LandArea=15.17,EstateId="衢市不动产权0019938号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/19"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇维拉小镇1幢3单元102室",Address="维拉小镇维拉小镇1幢3单元102室",ConstructArea=54.15,LandArea=11.74,EstateId="衢市不动产权0020686号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇维拉小镇1幢4单元101室",Address="维拉小镇维拉小镇1幢4单元101室",ConstructArea=54.15,LandArea=11.74,EstateId="衢市不动产权0020692号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇维拉小镇1幢4单元102室",Address="维拉小镇维拉小镇1幢4单元102室",ConstructArea=70.73,LandArea=15.33,EstateId="衢市不动产权0020811号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇维拉小镇1幢1单元201室",Address="维拉小镇维拉小镇1幢1单元201室",ConstructArea=70.73,LandArea=15.33,EstateId="衢市不动产权0020688号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇维拉小镇1幢1单元202室",Address="维拉小镇维拉小镇1幢1单元202室",ConstructArea=54.15,LandArea=11.74,EstateId="衢市不动产权0019618号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/18"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇维拉小镇1幢2单元201室",Address="维拉小镇维拉小镇1幢2单元201室",ConstructArea=54.15,LandArea=11.74,EstateId="衢市不动产权0020940号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇维拉小镇1幢2单元202室",Address="维拉小镇维拉小镇1幢2单元202室",ConstructArea=69.96,LandArea=15.17,EstateId="衢市不动产权0020677号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇维拉小镇1幢3单元201室",Address="维拉小镇维拉小镇1幢3单元201室",ConstructArea=69.96,LandArea=15.17,EstateId="衢市不动产权0019829号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/19"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇维拉小镇1幢3单元202室",Address="维拉小镇维拉小镇1幢3单元202室",ConstructArea=54.15,LandArea=11.74,EstateId="衢市不动产权0019736号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/19"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇维拉小镇1幢4单元201室",Address="维拉小镇维拉小镇1幢4单元201室",ConstructArea=54.15,LandArea=11.74,EstateId="衢市不动产权0020718号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇维拉小镇1幢4单元202室",Address="维拉小镇维拉小镇1幢4单元202室",ConstructArea=70.73,LandArea=15.33,EstateId="衢市不动产权0020625号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇维拉小镇1幢1单元301室",Address="维拉小镇维拉小镇1幢1单元301室",ConstructArea=70.73,LandArea=15.33,EstateId="衢市不动产权0020696号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇维拉小镇1幢1单元302室",Address="维拉小镇维拉小镇1幢1单元302室",ConstructArea=54.15,LandArea=11.74,EstateId="衢市不动产权0019666号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/18"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇维拉小镇1幢2单元301室",Address="维拉小镇维拉小镇1幢2单元301室",ConstructArea=54.15,LandArea=11.74,EstateId="衢市不动产权0020594号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇维拉小镇1幢2单元302室",Address="维拉小镇维拉小镇1幢2单元302室",ConstructArea=69.96,LandArea=15.17,EstateId="衢市不动产权0020673号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇维拉小镇1幢3单元301室",Address="维拉小镇维拉小镇1幢3单元301室",ConstructArea=69.96,LandArea=15.17,EstateId="衢市不动产权0019784号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/19"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇维拉小镇1幢3单元302室",Address="维拉小镇维拉小镇1幢3单元302室",ConstructArea=54.15,LandArea=11.74,EstateId="衢市不动产权0019676号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/18"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇维拉小镇1幢4单元301室",Address="维拉小镇维拉小镇1幢4单元301室",ConstructArea=54.15,LandArea=11.74,EstateId="衢市不动产权0020721号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇维拉小镇1幢4单元302室",Address="维拉小镇维拉小镇1幢4单元302室",ConstructArea=70.73,LandArea=15.33,EstateId="衢市不动产权0020681号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇维拉小镇1幢1单元401室",Address="维拉小镇维拉小镇1幢1单元401室",ConstructArea=70.73,LandArea=15.33,EstateId="衢市不动产权0019572号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/18"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇维拉小镇1幢1单元402室",Address="维拉小镇维拉小镇1幢1单元402室",ConstructArea=54.15,LandArea=11.74,EstateId="衢市不动产权0019667号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/18"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇维拉小镇1幢2单元401室",Address="维拉小镇维拉小镇1幢2单元401室",ConstructArea=54.15,LandArea=11.74,EstateId="衢市不动产权0020612号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇维拉小镇1幢2单元402室",Address="维拉小镇维拉小镇1幢2单元402室",ConstructArea=69.96,LandArea=15.17,EstateId="衢市不动产权0020693号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇1幢3单元401室",Address="维拉小镇1幢3单元401室",ConstructArea=69.96,LandArea=15.17,EstateId="衢市不动产权0019747号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/19"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇1幢3单元402室",Address="维拉小镇1幢3单元402室",ConstructArea=54.15,LandArea=11.74,EstateId="衢市不动产权0020331号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/21"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇1幢4单元401室",Address="维拉小镇1幢4单元401室",ConstructArea=54.15,LandArea=11.74,EstateId="衢市不动产权0020808号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇1幢4单元402室",Address="维拉小镇1幢4单元402室",ConstructArea=70.73,LandArea=15.33,EstateId="衢市不动产权0020603号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇1幢1单元501室",Address="维拉小镇1幢1单元501室",ConstructArea=70.73,LandArea=15.33,EstateId="衢市不动产权0019559号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/18"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇1幢1单元502室",Address="维拉小镇1幢1单元502室",ConstructArea=54.15,LandArea=11.74,EstateId="衢市不动产权0019557号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/18"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇1幢2单元501室",Address="维拉小镇1幢2单元501室",ConstructArea=54.15,LandArea=11.74,EstateId="衢市不动产权0020938号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇1幢2单元502室",Address="维拉小镇1幢2单元502室",ConstructArea=69.96,LandArea=15.17,EstateId="衢市不动产权0020690号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇1幢3单元501室",Address="维拉小镇1幢3单元501室",ConstructArea=69.96,LandArea=15.17,EstateId="衢市不动产权0019763号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/19"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇1幢3单元502室",Address="维拉小镇1幢3单元502室",ConstructArea=54.15,LandArea=11.74,EstateId="衢市不动产权0019928号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/19"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇1幢4单元501室",Address="维拉小镇1幢4单元501室",ConstructArea=54.15,LandArea=11.74,EstateId="衢市不动产权0020810号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇1幢4单元502室",Address="维拉小镇1幢4单元502室",ConstructArea=70.73,LandArea=15.33,EstateId="衢市不动产权0020602号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢1单元101室",Address="维拉小镇2幢1单元101室",ConstructArea=70.73,LandArea=15.33,EstateId="衢市不动产权0017616号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/10"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢1单元102室",Address="维拉小镇2幢1单元102室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0018312号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/12"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢2单元101室",Address="维拉小镇2幢2单元101室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0017691号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/18"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢2单元102室",Address="维拉小镇2幢2单元102室",ConstructArea=69.96,LandArea=15.16,EstateId="衢市不动产权0017672号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/10"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢3单元101室",Address="维拉小镇2幢3单元101室",ConstructArea=69.96,LandArea=15.16,EstateId="衢市不动产权0018251号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/11"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢3单元102室",Address="维拉小镇2幢3单元102室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0017665号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/10"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢4单元101室",Address="维拉小镇2幢4单元101室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0018306号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/12"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢4单元102室",Address="维拉小镇2幢4单元102室",ConstructArea=70.73,LandArea=15.33,EstateId="衢市不动产权0018179号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/11"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢1单元201室",Address="维拉小镇2幢1单元201室",ConstructArea=70.73,LandArea=15.33,EstateId="衢市不动产权0018174号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/11"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢1单元202室",Address="维拉小镇2幢1单元202室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0018311号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/12"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢2单元201室",Address="维拉小镇2幢2单元201室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0017694号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/10"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢2单元202室",Address="维拉小镇2幢2单元202室",ConstructArea=69.96,LandArea=15.16,EstateId="衢市不动产权0018314号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/12"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢3单元201室",Address="维拉小镇2幢3单元201室",ConstructArea=69.96,LandArea=15.16,EstateId="衢市不动产权0018268号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/11"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢3单元202室",Address="维拉小镇2幢3单元202室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0018271号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/11"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢4单元201室",Address="维拉小镇2幢4单元201室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0018223号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/11"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢4单元202室",Address="维拉小镇2幢4单元202室",ConstructArea=70.73,LandArea=15.33,EstateId="衢市不动产权0018196号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/11"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢1单元301室",Address="维拉小镇2幢1单元301室",ConstructArea=70.73,LandArea=15.33,EstateId="衢市不动产权0017618号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/10"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢1单元302室",Address="维拉小镇2幢1单元302室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0017626号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/10"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢2单元301室",Address="维拉小镇2幢2单元301室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0017714号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/10"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢2单元302室",Address="维拉小镇2幢2单元302室",ConstructArea=69.96,LandArea=15.17,EstateId="衢市不动产权0017675号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/10"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢3单元301室",Address="维拉小镇2幢3单元301室",ConstructArea=69.96,LandArea=15.16,EstateId="衢市不动产权0017701号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/10"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢3单元302室",Address="维拉小镇2幢3单元302室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0018272号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/11"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢4单元301室",Address="维拉小镇2幢4单元301室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0018308号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/12"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢4单元302室",Address="维拉小镇2幢4单元302室",ConstructArea=70.73,LandArea=15.33,EstateId="衢市不动产权0018253号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/11"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢1单元401室",Address="维拉小镇2幢1单元401室",ConstructArea=70.73,LandArea=15.33,EstateId="衢市不动产权0017619号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/10"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢1单元402室",Address="维拉小镇2幢1单元402室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0017630号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/10"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢2单元401室",Address="维拉小镇2幢2单元401室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0018172号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/11"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢2单元402室",Address="维拉小镇2幢2单元402室",ConstructArea=69.96,LandArea=15.16,EstateId="衢市不动产权0017669号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/10"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢3单元401室",Address="维拉小镇2幢3单元401室",ConstructArea=69.96,LandArea=15.16,EstateId="衢市不动产权0018269号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/11"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢3单元402室",Address="维拉小镇2幢3单元402室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0018316号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/12"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢4单元401室",Address="维拉小镇2幢4单元401室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0018307号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/12"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢4单元402室",Address="维拉小镇2幢4单元402室",ConstructArea=70.73,LandArea=15.33,EstateId="衢市不动产权0018303号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/12"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢1单元501室",Address="维拉小镇2幢1单元501室",ConstructArea=70.73,LandArea=15.33,EstateId="衢市不动产权0017624号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/10"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢1单元502室",Address="维拉小镇2幢1单元502室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0017697号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/10"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢2单元501室",Address="维拉小镇2幢2单元501室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0018309号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/12"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢2单元502室",Address="维拉小镇2幢2单元502室",ConstructArea=69.96,LandArea=15.16,EstateId="衢市不动产权0017678号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/10"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢3单元501室",Address="维拉小镇2幢3单元501室",ConstructArea=69.96,LandArea=15.16,EstateId="衢市不动产权0017703号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/10"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢3单元502室",Address="维拉小镇2幢3单元502室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0018273号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/11"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢4单元501室",Address="维拉小镇2幢4单元501室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0018246号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/11"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇2幢4单元502室",Address="维拉小镇2幢4单元502室",ConstructArea=70.73,LandArea=15.33,EstateId="衢市不动产权0018305号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/12"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢1单元101室",Address="维拉小镇3幢1单元101室",ConstructArea=70.73,LandArea=15.32,EstateId="衢市不动产权0018810号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/13"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢1单元102室",Address="维拉小镇3幢1单元102室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0018822号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/13"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢2单元101室",Address="维拉小镇3幢2单元101室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0018758号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/13"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢2单元102室",Address="维拉小镇3幢2单元102室",ConstructArea=69.96,LandArea=15.15,EstateId="衢市不动产权0018649号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/13"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢3单元101室",Address="维拉小镇3幢3单元101室",ConstructArea=69.96,LandArea=15.15,EstateId="衢市不动产权0020626号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢3单元102室",Address="维拉小镇3幢3单元102室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0019769号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/19"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢4单元101室",Address="维拉小镇3幢4单元101室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0020609号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢4单元102室",Address="维拉小镇3幢4单元102室",ConstructArea=70.73,LandArea=15.32,EstateId="衢市不动产权0021008号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/25"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢1单元201室",Address="维拉小镇3幢1单元201室",ConstructArea=70.73,LandArea=15.32,EstateId="衢市不动产权0018816号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/13"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢1单元202室",Address="维拉小镇3幢1单元202室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0018831号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/13"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢2单元201室",Address="维拉小镇3幢2单元201室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0018760号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/13"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢2单元202室",Address="维拉小镇3幢2单元202室",ConstructArea=69.96,LandArea=15.15,EstateId="衢市不动产权0018650号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/13"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢3单元201室",Address="维拉小镇3幢3单元201室",ConstructArea=69.96,LandArea=15.15,EstateId="衢市不动产权0020674号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢3单元202室",Address="维拉小镇3幢3单元202室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0020621号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢4单元201室",Address="维拉小镇3幢4单元201室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0020604号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢4单元202室",Address="维拉小镇3幢4单元202室",ConstructArea=70.73,LandArea=15.32,EstateId="衢市不动产权0021003号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/25"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢1单元301室",Address="维拉小镇3幢1单元301室",ConstructArea=70.73,LandArea=15.32,EstateId="衢市不动产权0018808号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/13"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢1单元302室",Address="维拉小镇3幢1单元302室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0018820号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/13"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢2单元301室",Address="维拉小镇3幢2单元301室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0018778号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/13"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢2单元302室",Address="维拉小镇3幢2单元302室",ConstructArea=69.96,LandArea=15.15,EstateId="衢市不动产权0018672号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/13"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢3单元301室",Address="维拉小镇3幢3单元301室",ConstructArea=69.96,LandArea=15.15,EstateId="衢市不动产权0020671号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢3单元302室",Address="维拉小镇3幢3单元302室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0019122号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/17"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢4单元301室",Address="维拉小镇3幢4单元301室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0020165号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/20"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢4单元302室",Address="维拉小镇3幢4单元302室",ConstructArea=70.73,LandArea=15.32,EstateId="衢市不动产权0020597号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢1单元401室",Address="维拉小镇3幢1单元401室",ConstructArea=70.73,LandArea=15.32,EstateId="衢市不动产权0018755号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/13"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢1单元402室",Address="维拉小镇3幢1单元402室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0018830号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/13"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢2单元401室",Address="维拉小镇3幢2单元401室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0018763号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/13"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢2单元402室",Address="维拉小镇3幢2单元402室",ConstructArea=69.96,LandArea=15.15,EstateId="衢市不动产权0020815号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢3单元401室",Address="维拉小镇3幢3单元401室",ConstructArea=69.96,LandArea=15.15,EstateId="衢市不动产权0020670号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢3单元402室",Address="维拉小镇3幢3单元402室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0021023号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/25"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢4单元401室",Address="维拉小镇3幢4单元401室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0021014号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/25"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢4单元402室",Address="维拉小镇3幢4单元402室",ConstructArea=70.73,LandArea=15.32,EstateId="衢市不动产权0020598号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢1单元501室",Address="维拉小镇3幢1单元501室",ConstructArea=70.73,LandArea=15.32,EstateId="衢市不动产权0018757号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/13"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢1单元502室",Address="维拉小镇3幢1单元502室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0018818号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/13"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢2单元501室",Address="维拉小镇3幢2单元501室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0018777号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/13"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢2单元502室",Address="维拉小镇3幢2单元502室",ConstructArea=69.96,LandArea=15.15,EstateId="衢市不动产权0021038号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/25"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢3单元501室",Address="维拉小镇3幢3单元501室",ConstructArea=69.96,LandArea=15.15,EstateId="衢市不动产权0021033号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/25"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢3单元502室",Address="维拉小镇3幢3单元502室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0021031号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/25"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢4单元501室",Address="维拉小镇3幢4单元501室",ConstructArea=54.15,LandArea=11.73,EstateId="衢市不动产权0021016号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/25"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇3幢4单元502室",Address="维拉小镇3幢4单元502室",ConstructArea=70.73,LandArea=15.32,EstateId="衢市不动产权0020668号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢1单元101室",Address="维拉小镇4幢1单元101室",ConstructArea=70.73,LandArea=15.36,EstateId="衢市不动产权0018905号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/14"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢1单元102室",Address="维拉小镇4幢1单元102室",ConstructArea=54.15,LandArea=11.76,EstateId="衢市不动产权0020611号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢2单元101室",Address="维拉小镇4幢2单元101室",ConstructArea=54.15,LandArea=11.76,EstateId="衢市不动产权0020702号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢2单元102室",Address="维拉小镇4幢2单元102室",ConstructArea=69.96,LandArea=15.19,EstateId="衢市不动产权0020942号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢3单元101室",Address="维拉小镇4幢3单元101室",ConstructArea=69.96,LandArea=15.19,EstateId="衢市不动产权002029号 ",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/19"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢3单元102室",Address="维拉小镇4幢3单元102室",ConstructArea=54.15,LandArea=11.76,EstateId="衢市不动产权0019715号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/18"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢4单元101室",Address="维拉小镇4幢4单元101室",ConstructArea=54.15,LandArea=11.76,EstateId="衢市不动产权0020680号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢4单元102室",Address="维拉小镇4幢4单元102室",ConstructArea=70.73,LandArea=15.36,EstateId="衢市不动产权0018919号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/14"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢1单元201室",Address="维拉小镇4幢1单元201室",ConstructArea=70.73,LandArea=15.36,EstateId="衢市不动产权0019121号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/17"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢1单元202室",Address="维拉小镇4幢1单元202室",ConstructArea=54.15,LandArea=11.76,EstateId="衢市不动产权0020610号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢2单元201室",Address="维拉小镇4幢2单元201室",ConstructArea=54.15,LandArea=11.76,EstateId="衢市不动产权0020698号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢2单元202室",Address="维拉小镇4幢2单元202室",ConstructArea=69.96,LandArea=15.19,EstateId="衢市不动产权0020346号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/21"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢3单元201室",Address="维拉小镇4幢3单元201室",ConstructArea=69.96,LandArea=15.19,EstateId="衢市不动产权0019949号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/19"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢3单元202室",Address="维拉小镇4幢3单元202室",ConstructArea=54.15,LandArea=11.76,EstateId="衢市不动产权0019809号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/19"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢4单元201室",Address="维拉小镇4幢4单元201室",ConstructArea=54.15,LandArea=11.76,EstateId="衢市不动产权0020675号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢4单元202室",Address="维拉小镇4幢4单元202室",ConstructArea=70.73,LandArea=15.36,EstateId="衢市不动产权0018920号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/14"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢1单元301室",Address="维拉小镇4幢1单元301室",ConstructArea=70.73,LandArea=15.36,EstateId="衢市不动产权0018971号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/17"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢1单元302室",Address="维拉小镇4幢1单元302室",ConstructArea=54.15,LandArea=11.76,EstateId="衢市不动产权0020608号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢2单元301室",Address="维拉小镇4幢2单元301室",ConstructArea=54.15,LandArea=11.76,EstateId="衢市不动产权0019673号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/18"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢2单元302室",Address="维拉小镇4幢2单元302室",ConstructArea=69.96,LandArea=15.19,EstateId="衢市不动产权0020347号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/21"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢3单元301室",Address="维拉小镇4幢3单元301室",ConstructArea=69.96,LandArea=15.19,EstateId="衢市不动产权0020340号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/21"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢3单元302室",Address="维拉小镇4幢3单元302室",ConstructArea=54.15,LandArea=11.76,EstateId="衢市不动产权0019961号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/19"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢4单元301室",Address="维拉小镇4幢4单元301室",ConstructArea=54.15,LandArea=11.76,EstateId="衢市不动产权0020672号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢4单元302室",Address="维拉小镇4幢4单元302室",ConstructArea=70.73,LandArea=15.36,EstateId="衢市不动产权0018915号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/14"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢1单元401室",Address="维拉小镇4幢1单元401室",ConstructArea=70.73,LandArea=15.36,EstateId="衢市不动产权0020717号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢1单元402室",Address="维拉小镇4幢1单元402室",ConstructArea=54.15,LandArea=11.76,EstateId="衢市不动产权0020683号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢2单元401室",Address="维拉小镇4幢2单元401室",ConstructArea=54.15,LandArea=11.76,EstateId="衢市不动产权0019722号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/18"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢2单元402室",Address="维拉小镇4幢2单元402室",ConstructArea=69.96,LandArea=15.19,EstateId="衢市不动产权0020694号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢3单元401室",Address="维拉小镇4幢3单元401室",ConstructArea=69.96,LandArea=15.19,EstateId="衢市不动产权0020606号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢3单元402室",Address="维拉小镇4幢3单元402室",ConstructArea=54.15,LandArea=11.76,EstateId="衢市不动产权0019959号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/19"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢4单元401室",Address="维拉小镇4幢4单元401室",ConstructArea=54.15,LandArea=11.76,EstateId="衢市不动产权0020338号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/21"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢4单元402室",Address="维拉小镇4幢4单元402室",ConstructArea=70.73,LandArea=15.36,EstateId="衢市不动产权0018916号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/14"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢1单元501室",Address="维拉小镇4幢1单元501室",ConstructArea=70.73,LandArea=15.36,EstateId="衢市不动产权0020716号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢1单元502室",Address="维拉小镇4幢1单元502室",ConstructArea=54.15,LandArea=11.76,EstateId="衢市不动产权0020685号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢2单元501室",Address="维拉小镇4幢2单元501室",ConstructArea=54.15,LandArea=11.76,EstateId="衢市不动产权0019702号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/18"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢2单元502室",Address="维拉小镇4幢2单元502室",ConstructArea=69.96,LandArea=15.19,EstateId="衢市不动产权0020713号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢3单元501室",Address="维拉小镇4幢3单元501室",ConstructArea=69.96,LandArea=15.19,EstateId="衢市不动产权0018928号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/14"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢3单元502室",Address="维拉小镇4幢3单元502室",ConstructArea=54.15,LandArea=11.76,EstateId="衢市不动产权0019950号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/19"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢4单元501室",Address="维拉小镇4幢4单元501室",ConstructArea=54.15,LandArea=11.76,EstateId="衢市不动产权0020820号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇4幢4单元502室",Address="维拉小镇4幢4单元502室",ConstructArea=70.73,LandArea=15.36,EstateId="衢市不动产权0018917号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/14"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢1单元101室",Address="维拉小镇5幢1单元101室",ConstructArea=70.73,LandArea=15.35,EstateId="衢市不动产权0018302号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/12"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢1单元102室",Address="维拉小镇5幢1单元102室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0018425号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/12"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢2单元101室",Address="维拉小镇5幢2单元101室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0018414号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/12"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢2单元102室",Address="维拉小镇5幢2单元102室",ConstructArea=69.96,LandArea=15.18,EstateId="衢市不动产权0018287号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/11"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢3单元101室",Address="维拉小镇5幢3单元101室",ConstructArea=69.96,LandArea=15.18,EstateId="衢市不动产权0018376号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/12"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢3单元102室",Address="维拉小镇5幢3单元102室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0018795号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/13"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢4单元101室",Address="维拉小镇5幢4单元101室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0018698号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/13"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢4单元102室",Address="维拉小镇5幢4单元102室",ConstructArea=70.73,LandArea=15.35,EstateId="衢市不动产权0018662号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/13"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢1单元201室",Address="维拉小镇5幢1单元201室",ConstructArea=70.73,LandArea=15.35,EstateId="衢市不动产权0018300号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/12"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢1单元202室",Address="维拉小镇5幢1单元202室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0018494号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/12"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢2单元201室",Address="维拉小镇5幢2单元201室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0018398号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/12"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢2单元202室",Address="维拉小镇5幢2单元202室",ConstructArea=69.96,LandArea=15.18,EstateId="衢市不动产权0018449号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/12"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢3单元201室",Address="维拉小镇5幢3单元201室",ConstructArea=69.96,LandArea=15.18,EstateId="衢市不动产权0018381号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/12"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢3单元202室",Address="维拉小镇5幢3单元202室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0018386号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/12"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢4单元201室",Address="维拉小镇5幢4单元201室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0018799号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/13"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢4单元202室",Address="维拉小镇5幢4单元202室",ConstructArea=70.73,LandArea=15.35,EstateId="衢市不动产权0018652号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/13"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢1单元301室",Address="维拉小镇5幢1单元301室",ConstructArea=70.73,LandArea=15.35,EstateId="衢市不动产权0016970号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/10"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢1单元302室",Address="维拉小镇5幢1单元302室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0018508号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/12"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢2单元301室",Address="维拉小镇5幢2单元301室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0018384号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/12"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢2单元302室",Address="维拉小镇5幢2单元302室",ConstructArea=69.96,LandArea=15.18,EstateId="衢市不动产权0018461号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/12"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢3单元301室",Address="维拉小镇5幢3单元301室",ConstructArea=69.96,LandArea=15.18,EstateId="衢市不动产权0018382号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/12"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢3单元302室",Address="维拉小镇5幢3单元302室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0018656号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/13"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢4单元301室",Address="维拉小镇5幢4单元301室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0018684号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/13"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢4单元302室",Address="维拉小镇5幢4单元302室",ConstructArea=70.73,LandArea=15.35,EstateId="衢市不动产权0018651号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/13"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢1单元401室",Address="维拉小镇5幢1单元401室",ConstructArea=70.73,LandArea=15.35,EstateId="衢市不动产权0018798号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/13"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢1单元402室",Address="维拉小镇5幢1单元402室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0018493号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/12"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢2单元401室",Address="维拉小镇5幢2单元401室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0018399号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/12"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢2单元402室",Address="维拉小镇5幢2单元402室",ConstructArea=69.96,LandArea=15.18,EstateId="衢市不动产权0018283号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/11"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢3单元401室",Address="维拉小镇5幢3单元401室",ConstructArea=69.96,LandArea=15.18,EstateId="衢市不动产权0018388号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/12"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢3单元402室",Address="维拉小镇5幢3单元402室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0018697号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/13"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢4单元401室",Address="维拉小镇5幢4单元401室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0018696号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/13"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢4单元402室",Address="维拉小镇5幢4单元402室",ConstructArea=70.73,LandArea=15.35,EstateId="衢市不动产权0018797号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/13"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢1单元501室",Address="维拉小镇5幢1单元501室",ConstructArea=70.73,LandArea=15.35,EstateId="衢市不动产权0018301号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/12"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢1单元502室",Address="维拉小镇5幢1单元502室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0018492号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/12"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢2单元501室",Address="维拉小镇5幢2单元501室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0018413号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/12"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢2单元502室",Address="维拉小镇5幢2单元502室",ConstructArea=69.96,LandArea=15.18,EstateId="衢市不动产权0018429号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/12"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢3单元501室",Address="维拉小镇5幢3单元501室",ConstructArea=69.96,LandArea=15.18,EstateId="衢市不动产权0018495号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/12"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢3单元502室",Address="维拉小镇5幢3单元502室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0018655号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/13"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢4单元501室",Address="维拉小镇5幢4单元501室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0018686号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/13"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇5幢4单元502室",Address="维拉小镇5幢4单元502室",ConstructArea=70.73,LandArea=15.35,EstateId="衢市不动产权0018692号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/13"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢1单元101室",Address="维拉小镇6幢1单元101室",ConstructArea=70.73,LandArea=15.35,EstateId="衢市不动产权0019194号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/17"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢1单元102室",Address="维拉小镇6幢1单元102室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0019124号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/17"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢2单元101室",Address="维拉小镇6幢2单元101室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0019261号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/17"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢2单元102室",Address="维拉小镇6幢2单元102室",ConstructArea=69.96,LandArea=15.18,EstateId="衢市不动产权0020617号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢3单元101室",Address="维拉小镇6幢3单元101室",ConstructArea=69.96,LandArea=15.18,EstateId="衢市不动产权0019151号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/17"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢3单元102室",Address="维拉小镇6幢3单元102室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0019178号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/17"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢4单元101室",Address="维拉小镇6幢4单元101室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0019188号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/17"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢4单元102室",Address="维拉小镇6幢4单元102室",ConstructArea=70.73,LandArea=15.35,EstateId="衢市不动产权0020333号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/21"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢1单元201室",Address="维拉小镇6幢1单元201室",ConstructArea=70.73,LandArea=15.35,EstateId="衢市不动产权0019190号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/17"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢1单元202室",Address="维拉小镇6幢1单元202室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0019126号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/17"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢2单元201室",Address="维拉小镇6幢2单元201室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0018317号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/12"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢2单元202室",Address="维拉小镇6幢2单元202室",ConstructArea=69.96,LandArea=15.18,EstateId="衢市不动产权0021046号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/25"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢3单元201室",Address="维拉小镇6幢3单元201室",ConstructArea=69.96,LandArea=15.18,EstateId="衢市不动产权0019154号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/17"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢3单元202室",Address="维拉小镇6幢3单元202室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0019180号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/17"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢4单元201室",Address="维拉小镇6幢4单元201室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0019128号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/17"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢4单元202室",Address="维拉小镇6幢4单元202室",ConstructArea=70.73,LandArea=15.35,EstateId="衢市不动产权0020620号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢1单元301室",Address="维拉小镇6幢1单元301室",ConstructArea=70.73,LandArea=15.35,EstateId="衢市不动产权0019192号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/17"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢1单元302室",Address="维拉小镇6幢1单元302室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0020607号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢2单元301室",Address="维拉小镇6幢2单元301室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0020351号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/21"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢2单元302室",Address="维拉小镇6幢2单元302室",ConstructArea=69.96,LandArea=15.18,EstateId="衢市不动产权0021045号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/25"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢3单元301室",Address="维拉小镇6幢3单元301室",ConstructArea=69.96,LandArea=15.18,EstateId="衢市不动产权0019155号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/17"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢3单元302室",Address="维拉小镇6幢3单元302室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0019182号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/17"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢4单元301室",Address="维拉小镇6幢4单元301室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0019132号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/17"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢4单元302室",Address="维拉小镇6幢4单元302室",ConstructArea=70.73,LandArea=15.35,EstateId="衢市不动产权0020618号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢1单元401室",Address="维拉小镇6幢1单元401室",ConstructArea=70.73,LandArea=15.35,EstateId="衢市不动产权0020352号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/21"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢1单元402室",Address="维拉小镇6幢1单元402室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0020615号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢2单元401室",Address="维拉小镇6幢2单元401室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0021155号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/25"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢2单元402室",Address="维拉小镇6幢2单元402室",ConstructArea=69.96,LandArea=15.18,EstateId="衢市不动产权0020936号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢3单元401室",Address="维拉小镇6幢3单元401室",ConstructArea=69.96,LandArea=15.18,EstateId="衢市不动产权0020708号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢3单元402室",Address="维拉小镇6幢3单元402室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0020337号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/21"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢4单元401室",Address="维拉小镇6幢4单元401室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0019125号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/17"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢4单元402室",Address="维拉小镇6幢4单元402室",ConstructArea=70.73,LandArea=15.35,EstateId="衢市不动产权0019161号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/17"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢1单元501室",Address="维拉小镇6幢1单元501室",ConstructArea=70.73,LandArea=15.35,EstateId="衢市不动产权0019133号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/17"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢1单元502室",Address="维拉小镇6幢1单元502室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0020613号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/24"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢2单元501室",Address="维拉小镇6幢2单元501室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0019185号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/17"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢2单元502室",Address="维拉小镇6幢2单元502室",ConstructArea=69.96,LandArea=15.18,EstateId="衢市不动产权0021043号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/25"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢3单元501室",Address="维拉小镇6幢3单元501室",ConstructArea=69.96,LandArea=15.18,EstateId="衢市不动产权0019198号",PropertyNature="公共租赁",LandNature="出让",Price=12.59,GetedDate=Convert.ToDateTime("2017/4/17"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    69.96   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢3单元502室",Address="维拉小镇6幢3单元502室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0020349号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/21"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢4单元501室",Address="维拉小镇6幢4单元501室",ConstructArea=54.15,LandArea=11.75,EstateId="衢市不动产权0019130号",PropertyNature="公共租赁",LandNature="出让",Price=9.75,GetedDate=Convert.ToDateTime("2017/4/17"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self= 54.15   },
new Property {PropertyType= PropertyType.House , Name="维拉小镇6幢4单元502室",Address="维拉小镇6幢4单元502室",ConstructArea=70.73,LandArea=15.35,EstateId="衢市不动产权0019162号",PropertyNature="公共租赁",LandNature="出让",Price=12.73,GetedDate=Convert.ToDateTime("2017/4/17"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=    70.73   },
new Property {PropertyType= PropertyType.House , Name="衢州市华枫路16号",Address="衢州市华枫路16号",ConstructArea=0.00,LandArea=87725.99,EstateId="浙（2017）衢州市不动产权第0037861号 ",PropertyNature="国有",LandNature="国有",GetedDate=Convert.ToDateTime("2017/8/10"),LifeTime=70, UsedPeople="汇盛",CurrentUse_Self=  87725.99    }
            };
            #endregion

            var g = _governmentService.GetGovernmentUnitByName("汇盛公司");

            var geo1="[{ \"lat\":28.940045200288296,\"lng\":118.96144151687622},{ \"lat\":28.94018467515707,\"lng\":118.96187603473663},{ \"lat\":28.940077386796474,\"lng\":118.96191895008087},{ \"lat\":28.93993254750967,\"lng\":118.96150052547455}]";
            var geo2 = "[{ \"lat\":28.939831210300326,\"lng\":118.96154344081879},{ \"lat\":28.93994922749698,\"lng\":118.96196186542511},{ \"lat\":28.939836574718356,\"lng\":118.96201550960541},{ \"lat\":28.93969709984958,\"lng\":118.96159172058105}]";
            var geo3= "[{\"lat\":28.939562989398837,\"lng\":118.9616346359253},{\"lat\":28.93969709984958,\"lng\":118.96206378936768},{\"lat\":28.939595175907016,\"lng\":118.96211206912994},{\"lat\":28.93946106545627,\"lng\":118.9616721868515}]";
            var geo4 = "[{\"lat\":28.94019599072635,\"lng\":118.96196186542511},{\"lat\":28.940324736759067,\"lng\":118.96239638328552},{\"lat\":28.94022817723453,\"lng\":118.96244466304779},{\"lat\":28.940094066783786,\"lng\":118.96201014518738}]";
            var geo5= "[{\"lat\":28.939943863078952,\"lng\":118.96204769611359},{\"lat\":28.940094066783786,\"lng\":118.96249830722809},{\"lat\":28.93998141400516,\"lng\":118.96254122257233},{\"lat\":28.939852667972445,\"lng\":118.96209597587585}]";
            var geo6 = "[{\"lat\":28.9397185575217,\"lng\":118.96214425563812},{\"lat\":28.939852667972445,\"lng\":118.96258413791656},{\"lat\":28.93975074402988,\"lng\":118.9626270532608},{\"lat\":28.939627362415195,\"lng\":118.96219789981842}]";
            var geo7 = "[{\"lat\":28.908155243843794,\"lng\":118.84716063737869},{\"lat\":28.905809484422207,\"lng\":118.85152995586395},{\"lat\":28.904404006898403,\"lng\":118.85053217411041},{\"lat\":28.906796537339687,\"lng\":118.84623527526855}]";

            try
            {
                foreach (var property in properties)
                {
                    property.Region = Region.Clusters;
                    property.Government = g;
                    property.HasConstructID = true;
                    property.HasLandID = true;

                    string geo = geo7;
                    if (property.Name.Contains("维拉小镇1幢")) geo = geo1;
                    else if (property.Name.Contains("维拉小镇2幢")) geo = geo2;
                    else if (property.Name.Contains("维拉小镇3幢")) geo = geo3;
                    else if (property.Name.Contains("维拉小镇4幢")) geo = geo4;
                    else if (property.Name.Contains("维拉小镇5幢")) geo = geo5;
                    else if (property.Name.Contains("维拉小镇6幢")) geo = geo6;

                    var points = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Point>>(geo);
                    var wkt = "";

                    StringBuilder wktsb = new StringBuilder();

                    //末尾加上起点闭合多边形
                    if (points.Count > 1) points.Add(points[0]);

                    foreach (var point in points)
                    {
                        if (point.lng < point.lat)
                        {
                            var t = point.lng;
                            point.lng = point.lat;
                            point.lat = t;
                        }
                        wktsb.Append(string.Format("{0} {1},", point.lng, point.lat));
                    }

                    wkt = string.Format("POLYGON(({0}))", wktsb.ToString().TrimEnd(','));
                    property.WKT = wkt;
                    SqlGeography sqlGeography = SqlGeography.Parse(wkt).MakeValid();
                    var graphy = DbGeography.FromText(sqlGeography.ToString());
                    property.Extent = graphy;

                    DbGeometry geometry = DbGeometry.FromText(graphy.ToString().Replace("SRID=4326;", ""));
                    if (geometry.Centroid != null)
                        property.Location = DbGeography.FromText(string.Format("POINT({0} {1})", geometry.Centroid.XCoordinate, geometry.Centroid.YCoordinate));
                    else
                        property.Location = graphy.PointAt(1);
                     
                    _propertyService.InsertProperty(property);

                    var propertyNewCreate = new PropertyNewCreate
                    {
                        Property_Id = property.Id,
                        State = PropertyApproveState.Start,
                        ProcessDate = DateTime.Now,
                        SuggestGovernmentId = g.Id,
                        Title = property.Name,
                    };

                    _propertyNewCreateService.InsertPropertyNewCreate(propertyNewCreate);
                }
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }


            return Ok("success");
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

        [HttpGet]
        [Route("Temp3")]
        public IHttpActionResult InsertLendAndRent()
        {
            //拷贝图片
            var targetPath = System.Web.HttpContext.Current.Server.MapPath("~/Content/images/");
            var directionName = @"C:\房产证图片\";


            var records = new List<RentLendRecord>();


            foreach (var record in records)
            {
                try
                {
                    var property = _propertyService.GetPropertyById(record.PropertyId);

                    if (property == null) continue;
                    if (property.Deleted || property.Off) throw new Exception("资产被删除或核销");

                    var pictures = new List<Picture>();

                    var files = System.IO.Directory.GetFiles(directionName + record.FileId);

                    foreach (var file in files)
                    {
                        var fileName = System.IO.Path.GetFileName(file);
                        var ext = System.IO.Path.GetExtension(file);

                        var fileStream = new FileStream(file, FileMode.Open);
                        var fileBinary = new byte[fileStream.Length];
                        fileStream.Read(fileBinary, 0, fileBinary.Length);

                        var picture = _pictureService.InsertPicture(fileBinary, "image/jpeg", "", "", fileName);
                        var url = _pictureService.GetPictureUrl(picture);
                        pictures.Add(picture);
                    }

                    if (record.Type == 0)
                    {
                        var lend = new PropertyLend
                        {
                            SuggestGovernmentId = record.GovernmentId,
                            LendArea = record.Area,
                            Property = property,
                            Remark = record.Remark,
                            LendTime = Convert.ToDateTime(record.StartDate),
                            ProcessDate = DateTime.Now,
                            State = PropertyApproveState.Start,
                            Name = record.People,
                            Title = property.Name
                        };

                        if (string.IsNullOrEmpty(record.EndDate)) lend.BackTime = Convert.ToDateTime(record.EndDate);

                        _propertyLendService.InsertPropertyLend(lend);

                        foreach (var picture in pictures)
                        {
                            var propertyLendPicture = new PropertyLendPicture
                            {
                                Picture = picture,
                                PropertyLend = lend,
                            };

                            lend.LendPictures.Add(propertyLendPicture);
                        }
                        _propertyLendService.UpdatePropertyLend(lend);

                    }
                    else if (record.Type == 1)
                    {
                        var rent = new PropertyRent
                        {
                            SuggestGovernmentId = record.GovernmentId,
                            RentArea = record.Area,
                            Property = property,
                            Remark = record.Remark,
                            RentTime = Convert.ToDateTime(record.StartDate),
                            BackTime = Convert.ToDateTime(record.EndDate),
                            ProcessDate = DateTime.Now,
                            State = PropertyApproveState.Start,
                            Name = record.People,
                            Title = property.Name,
                            PriceString=record.Money
                        };
                         

                        _propertyRentService.InsertPropertyRent(rent);

                        foreach (var picture in pictures)
                        {
                            var propertyRentPicture = new PropertyRentPicture
                            {
                                Picture = picture,
                                PropertyRent = rent,
                            };

                            rent.RentPictures.Add(propertyRentPicture);
                        }
                        _propertyRentService.UpdatePropertyRent(rent);
                    }
                }
                catch(Exception e)
                {

                }

            }

            return Ok();
        }


        public class Point
        {
            public double lng { get; set; }

            public double lat { get; set; }
        }

        protected class RentLendRecord
        {
            public int PropertyId { get; set; }

            /// <summary>
            /// 0为出借 1为出租
            /// </summary>
            public int Type { get; set; }

            public string PropertyName { get; set; }

            public string People { get; set; }

            public double Area { get; set; }

            public string StartDate { get; set; }

            public string EndDate { get; set; }

            public string Money { get; set; }

            public string Remark { get; set; }

            public int FileId { get; set; }

            public int GovernmentId { get; set; }
        }
    }
}