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
using Newtonsoft.Json;

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

                    if (government == null) sb.AppendLine("产权单位为空；");
                    else property.Government = government;

                    switch (type)
                    {
                        case "房屋": property.PropertyType = PropertyType.House; break;
                        case "土地": property.PropertyType = PropertyType.Land; break;
                        case "对应房屋土地": property.PropertyType = PropertyType.LandUnderHouse; break;
                        default: property.PropertyType = PropertyType.Others; break;
                    }

                    if (string.IsNullOrEmpty(name)) { sb.AppendLine("名称为空；"); property.Name = "无名称"; }
                    else property.Name = name;

                    if (string.IsNullOrEmpty(address)) { sb.AppendLine("地址为空；"); }
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
                            sb.AppendLine("取得日期格式不正确；");
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
                        sb.AppendLine("下一步操作有重复；");
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
                            wktsb.AppendLine(string.Format("{0} {1},", point.lng, point.lat));
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
                            sb.AppendLine("坐标格式有误，无法生成几何图形；");

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
                    resultSb.AppendLine(string.Format("序号为 {0} 的要素异常，错误为：{1}\n", id,
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
            return Ok("closed");

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

            var geo1 = "[{ \"lat\":28.940045200288296,\"lng\":118.96144151687622},{ \"lat\":28.94018467515707,\"lng\":118.96187603473663},{ \"lat\":28.940077386796474,\"lng\":118.96191895008087},{ \"lat\":28.93993254750967,\"lng\":118.96150052547455}]";
            var geo2 = "[{ \"lat\":28.939831210300326,\"lng\":118.96154344081879},{ \"lat\":28.93994922749698,\"lng\":118.96196186542511},{ \"lat\":28.939836574718356,\"lng\":118.96201550960541},{ \"lat\":28.93969709984958,\"lng\":118.96159172058105}]";
            var geo3 = "[{\"lat\":28.939562989398837,\"lng\":118.9616346359253},{\"lat\":28.93969709984958,\"lng\":118.96206378936768},{\"lat\":28.939595175907016,\"lng\":118.96211206912994},{\"lat\":28.93946106545627,\"lng\":118.9616721868515}]";
            var geo4 = "[{\"lat\":28.94019599072635,\"lng\":118.96196186542511},{\"lat\":28.940324736759067,\"lng\":118.96239638328552},{\"lat\":28.94022817723453,\"lng\":118.96244466304779},{\"lat\":28.940094066783786,\"lng\":118.96201014518738}]";
            var geo5 = "[{\"lat\":28.939943863078952,\"lng\":118.96204769611359},{\"lat\":28.940094066783786,\"lng\":118.96249830722809},{\"lat\":28.93998141400516,\"lng\":118.96254122257233},{\"lat\":28.939852667972445,\"lng\":118.96209597587585}]";
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
                        wktsb.AppendLine(string.Format("{0} {1},", point.lng, point.lat));
                    }

                    wkt = string.Format("POLYGON(({0}))", wktsb.ToString().TrimEnd(','));
                    property.WKT = wkt;
                    SqlGeography sqlGeography = SqlGeography.Parse(wkt).MakeValid();
                    var graphy = DbGeography.FromText(sqlGeography.ToString());
                    property.Extent = graphy;
                    property.Locked = true;

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
            catch (Exception e)
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
            StringBuilder sb = new StringBuilder();
            //拷贝图片
            var targetPath = System.Web.HttpContext.Current.Server.MapPath("~/Content/images/");
            var directionName = @"C:\import\";

            // var json = "[{\"PropertyId\":\"36392\",\"People\":\"衢州港诚机电产品制造有限公司\",\"Area\":\"90\",\"StartDate\":\"2017年5月15日\",\"EndDate\":\"2018年5月14日\",\"Money\":\"36000\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"Remark\":\"王菲13867024990\",\"FileId\":\"0\"},{\"PropertyId\":\"36392\",\"People\":\"衢州港诚机电产品制造有限公司\",\"Area\":\"300\",\"StartDate\":\"2017年3月16日\",\"EndDate\":\"2018年3月15日\",\"Money\":\"10800\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"Remark\":\"王菲13867024990\",\"FileId\":\"0\"},{\"PropertyId\":\"36392\",\"People\":\"浙江好彩印刷包装有限公司\",\"Area\":\"30\",\"StartDate\":\"2017年3月2日\",\"EndDate\":\"2018年3月1日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"Remark\":\"王850687713575668598\",\"FileId\":\"88\"},{\"PropertyId\":\"36392\",\"People\":\"衢州峰仔食品有限公司\",\"Area\":\"180\",\"StartDate\":\"2017年9月4日\",\"EndDate\":\"2018年3月3日\",\"Money\":\"10800\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"Remark\":\"杨薛13967012213\",\"FileId\":\"0\"},{\"PropertyId\":\"38903\",\"People\":\"金瑞泓科技（衢州）有限公司\",\"Area\":\"70.73\",\"StartDate\":\"2017年9月12\",\"EndDate\":\"2018年9月11日\",\"Money\":\"7200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"Remark\":\"联系人：汪红燕13957000034\",\"FileId\":\"128\"},{\"PropertyId\":\"38911\",\"People\":\"金瑞泓科技（衢州）有限公司\",\"Area\":\"70.73\",\"StartDate\":\"2017年9月12\",\"EndDate\":\"2018年9月11日\",\"Money\":\"8400\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"Remark\":\"联系人：汪红燕13957000034\",\"FileId\":\"128\"},{\"PropertyId\":\"38919\",\"People\":\"金瑞泓科技（衢州）有限公司\",\"Area\":\"70.73\",\"StartDate\":\"2017年9月12\",\"EndDate\":\"2018年9月11日\",\"Money\":\"8400\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"Remark\":\"联系人：汪红燕13957000034\",\"FileId\":\"128\"},{\"PropertyId\":\"38927\",\"People\":\"金瑞泓科技（衢州）有限公司\",\"Area\":\"70.73\",\"StartDate\":\"2017年9月12\",\"EndDate\":\"2018年9月11日\",\"Money\":\"8400\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"Remark\":\"联系人：汪红燕13957000034\",\"FileId\":\"128\"},{\"PropertyId\":\"38935\",\"People\":\"金瑞泓科技（衢州）有限公司\",\"Area\":\"70.73\",\"StartDate\":\"2017年9月12\",\"EndDate\":\"2018年9月11日\",\"Money\":\"7200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"Remark\":\"联系人：汪红燕13957000034\",\"FileId\":\"128\"},{\"PropertyId\":\"38906\",\"People\":\"金瑞泓科技（衢州）有限公司\",\"Area\":\"69.86\",\"StartDate\":\"2017年9月12\",\"EndDate\":\"2018年9月11日\",\"Money\":\"7200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"Remark\":\"联系人：汪红燕13957000034\",\"FileId\":\"128\"},{\"PropertyId\":\"38914\",\"People\":\"金瑞泓科技（衢州）有限公司\",\"Area\":\"69.86\",\"StartDate\":\"2017年9月12\",\"EndDate\":\"2018年9月11日\",\"Money\":\"8400\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"Remark\":\"联系人：汪红燕13957000034\",\"FileId\":\"128\"},{\"PropertyId\":\"38922\",\"People\":\"金瑞泓科技（衢州）有限公司\",\"Area\":\"69.86\",\"StartDate\":\"2017年9月12\",\"EndDate\":\"2018年9月11日\",\"Money\":\"8400\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"Remark\":\"联系人：汪红燕13957000034\",\"FileId\":\"128\"},{\"PropertyId\":\"38930\",\"People\":\"金瑞泓科技（衢州）有限公司\",\"Area\":\"69.86\",\"StartDate\":\"2017年9月12\",\"EndDate\":\"2018年9月11日\",\"Money\":\"8400\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"Remark\":\"联系人：汪红燕13957000034\",\"FileId\":\"128\"},{\"PropertyId\":\"38938\",\"People\":\"金瑞泓科技（衢州）有限公司\",\"Area\":\"69.86\",\"StartDate\":\"2017年9月12\",\"EndDate\":\"2018年9月11日\",\"Money\":\"7200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"Remark\":\"联系人：汪红燕13957000034\",\"FileId\":\"128\"},{\"PropertyId\":\"38907\",\"People\":\"金瑞泓科技（衢州）有限公司\",\"Area\":\"69.86\",\"StartDate\":\"2017年9月12\",\"EndDate\":\"2018年9月11日\",\"Money\":\"7200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"Remark\":\"联系人：汪红燕13957000034\",\"FileId\":\"128\"},{\"PropertyId\":\"38915\",\"People\":\"金瑞泓科技（衢州）有限公司\",\"Area\":\"69.86\",\"StartDate\":\"2017年9月12\",\"EndDate\":\"2018年9月11日\",\"Money\":\"8400\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"Remark\":\"联系人：汪红燕13957000034\",\"FileId\":\"128\"},{\"PropertyId\":\"38923\",\"People\":\"金瑞泓科技（衢州）有限公司\",\"Area\":\"69.86\",\"StartDate\":\"2017年9月12\",\"EndDate\":\"2018年9月11日\",\"Money\":\"8400\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"Remark\":\"联系人：汪红燕13957000034\",\"FileId\":\"128\"},{\"PropertyId\":\"38931\",\"People\":\"金瑞泓科技（衢州）有限公司\",\"Area\":\"69.86\",\"StartDate\":\"2017年9月12\",\"EndDate\":\"2018年9月11日\",\"Money\":\"8400\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"Remark\":\"联系人：汪红燕13957000034\",\"FileId\":\"128\"},{\"PropertyId\":\"38939\",\"People\":\"金瑞泓科技（衢州）有限公司\",\"Area\":\"69.86\",\"StartDate\":\"2017年9月12\",\"EndDate\":\"2018年9月11日\",\"Money\":\"7200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"Remark\":\"联系人：汪红燕13957000034\",\"FileId\":\"128\"},{\"PropertyId\":\"38910\",\"People\":\"金瑞泓科技（衢州）有限公司\",\"Area\":\"70.73\",\"StartDate\":\"2017年9月12\",\"EndDate\":\"2018年9月11日\",\"Money\":\"7200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"Remark\":\"联系人：汪红燕13957000034\",\"FileId\":\"128\"},{\"PropertyId\":\"38918\",\"People\":\"金瑞泓科技（衢州）有限公司\",\"Area\":\"70.73\",\"StartDate\":\"2017年9月12\",\"EndDate\":\"2018年9月11日\",\"Money\":\"8400\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"Remark\":\"联系人：汪红燕13957000034\",\"FileId\":\"128\"},{\"PropertyId\":\"36629\",\"People\":\"金瑞泓科技（衢州）有限公司\",\"Area\":\"81.28\",\"StartDate\":\"2017年9月12\",\"EndDate\":\"2018年9月11日\",\"Money\":\"10200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"Remark\":\"联系人：汪红燕13957000034\",\"FileId\":\"128\"},{\"PropertyId\":\"36631\",\"People\":\"金瑞泓科技（衢州）有限公司\",\"Area\":\"83\",\"StartDate\":\"2017年9月12\",\"EndDate\":\"2018年9月11日\",\"Money\":\"10200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"Remark\":\"联系人：汪红燕13957000034\",\"FileId\":\"128\"},{\"PropertyId\":\"36630\",\"People\":\"金瑞泓科技（衢州）有限公司\",\"Area\":\"81.59\",\"StartDate\":\"2017年9月12\",\"EndDate\":\"2018年9月11日\",\"Money\":\"10200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"Remark\":\"联系人：汪红燕13957000034\",\"FileId\":\"128\"},{\"PropertyId\":\"38904\",\"People\":\"浙江金维克家庭用品科技有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年9月18日\",\"EndDate\":\"2018年9月17日\",\"Money\":\"6000\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"127\"},{\"PropertyId\":\"38912\",\"People\":\"浙江金维克家庭用品科技有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年9月18日\",\"EndDate\":\"2018年9月17日\",\"Money\":\"7200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"127\"},{\"PropertyId\":\"38920\",\"People\":\"浙江金维克家庭用品科技有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年9月18日\",\"EndDate\":\"2018年9月17日\",\"Money\":\"7200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"127\"},{\"PropertyId\":\"38928\",\"People\":\"浙江金维克家庭用品科技有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年9月18日\",\"EndDate\":\"2018年9月17日\",\"Money\":\"7200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"127\"},{\"PropertyId\":\"38936\",\"People\":\"浙江金维克家庭用品科技有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年9月18日\",\"EndDate\":\"2018年9月17日\",\"Money\":\"6000\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"127\"},{\"PropertyId\":\"38905\",\"People\":\"浙江金维克家庭用品科技有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年9月18日\",\"EndDate\":\"2018年9月17日\",\"Money\":\"6000\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"127\"},{\"PropertyId\":\"38913\",\"People\":\"浙江金维克家庭用品科技有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年9月18日\",\"EndDate\":\"2018年9月17日\",\"Money\":\"7200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"127\"},{\"PropertyId\":\"38921\",\"People\":\"浙江金维克家庭用品科技有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年9月18日\",\"EndDate\":\"2018年9月17日\",\"Money\":\"7200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"127\"},{\"PropertyId\":\"38929\",\"People\":\"浙江金维克家庭用品科技有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年9月18日\",\"EndDate\":\"2018年9月17日\",\"Money\":\"7200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"127\"},{\"PropertyId\":\"38937\",\"People\":\"浙江金维克家庭用品科技有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年9月18日\",\"EndDate\":\"2018年9月17日\",\"Money\":\"6000\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"127\"},{\"PropertyId\":\"38910\",\"People\":\"浙江金维克家庭用品科技有限公司\",\"Area\":\"70.73\",\"StartDate\":\"2017年9月18日\",\"EndDate\":\"2018年9月17日\",\"Money\":\"7200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"127\"},{\"PropertyId\":\"38918\",\"People\":\"浙江金维克家庭用品科技有限公司\",\"Area\":\"70.73\",\"StartDate\":\"2017年9月18日\",\"EndDate\":\"2018年9月17日\",\"Money\":\"8400\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"127\"},{\"PropertyId\":\"38926\",\"People\":\"浙江金维克家庭用品科技有限公司\",\"Area\":\"70.73\",\"StartDate\":\"2017年9月18日\",\"EndDate\":\"2018年9月17日\",\"Money\":\"8400\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"127\"},{\"PropertyId\":\"38934\",\"People\":\"浙江金维克家庭用品科技有限公司\",\"Area\":\"70.73\",\"StartDate\":\"2017年9月18日\",\"EndDate\":\"2018年9月17日\",\"Money\":\"8400\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"127\"},{\"PropertyId\":\"38942\",\"People\":\"浙江金维克家庭用品科技有限公司\",\"Area\":\"70.73\",\"StartDate\":\"2017年9月18日\",\"EndDate\":\"2018年9月17日\",\"Money\":\"7200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"127\"},{\"PropertyId\":\"38909\",\"People\":\"浙江金维克家庭用品科技有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年9月18日\",\"EndDate\":\"2018年9月17日\",\"Money\":\"6000\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"127\"},{\"PropertyId\":\"38917\",\"People\":\"浙江金维克家庭用品科技有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年9月18日\",\"EndDate\":\"2018年9月17日\",\"Money\":\"7200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"127\"},{\"PropertyId\":\"38925\",\"People\":\"浙江金维克家庭用品科技有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年9月18日\",\"EndDate\":\"2018年9月17日\",\"Money\":\"7200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"127\"},{\"PropertyId\":\"38933\",\"People\":\"浙江金维克家庭用品科技有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年9月18日\",\"EndDate\":\"2018年9月17日\",\"Money\":\"7200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"127\"},{\"PropertyId\":\"38941\",\"People\":\"浙江金维克家庭用品科技有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年9月18日\",\"EndDate\":\"2018年9月17日\",\"Money\":\"6000\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"127\"},{\"PropertyId\":\"36628\",\"People\":\"浙江金维克家庭用品科技有限公司\",\"Area\":\"81.83\",\"StartDate\":\"2017年9月18日\",\"EndDate\":\"2018年9月17日\",\"Money\":\"10200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"127\"},{\"PropertyId\":\"38983\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"70.73\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"38984\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"38985\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"38986\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"69.96\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"38987\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"69.96\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"38988\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"38989\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"38990\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"70.73\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"38991\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"70.73\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"38992\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"38993\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"38994\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"69.96\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"38995\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"69.96\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"38996\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"38997\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"38998\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"70.73\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"38999\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"70.73\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39000\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39001\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39002\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"69.96\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39003\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"69.96\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39004\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39005\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39006\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"70.73\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39007\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"70.73\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39008\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39009\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39010\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"69.96\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39011\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"69.96\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39012\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39013\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39014\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"70.73\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39015\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"70.73\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39016\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39017\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39018\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"69.96\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39019\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"69.96\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39020\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39021\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39022\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"70.73\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"4200\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39103\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"70.73\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39104\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39105\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39106\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"69.96\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39107\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"69.96\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39108\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39109\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39110\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"70.73\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39111\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"70.73\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39112\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39113\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39114\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"69.96\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39115\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"69.96\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39116\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39117\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39118\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"70.73\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39119\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"70.73\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39120\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39121\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39122\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"69.96\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39123\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"69.96\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39124\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39125\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39126\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"70.73\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39127\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"70.73\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39128\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39129\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39130\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"69.96\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39131\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"69.96\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39132\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39133\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39134\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"70.73\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39135\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"70.73\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39136\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39137\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39138\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"69.96\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39139\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"69.96\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39140\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39141\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"54.15\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},{\"PropertyId\":\"39142\",\"People\":\"晓星氨纶（衢州）有限公司\",\"Area\":\"70.73\",\"StartDate\":\"2017年7月3日\",\"EndDate\":\"2018年7月2日\",\"Money\":\"3600\",\"Type\":\"1\",\"GovernmentId\":\"155\",\"FileId\":\"126\"},]";

            var json = "[{\"PropertyId\":\"36520\",\"People\":\"衢州市公安局衢州经济开发区分局\",\"Area\":\"1272\",\"StartDate\":\"2014年12月11日\",\"Money\":\"0\",\"Type\":\"0\",\"GovernmentId\":\"155\",\"Remark\":\"自用收回;联系人：陈连宝13957029161\",\"FileId\":\"0\"}]";
            var records = JsonConvert.DeserializeObject<List<RentLendRecord>>(json);


            foreach (var record in records)
            {
                try
                {
                    var property = _propertyService.GetPropertyById(record.PropertyId);

                    if (property == null) continue;
                    if (property.Deleted || property.Off) throw new Exception("资产被删除或核销");

                    var pictures = new List<Picture>();

                    if (!string.IsNullOrEmpty(record.FileId) && record.FileId!="0")
                    {
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
                            fileStream.Dispose();
                        }
                    }

                    if (record.Type == 0)
                    {
                        #region 资产出借
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

                        if (!string.IsNullOrEmpty(record.EndDate)) lend.BackTime = Convert.ToDateTime(record.EndDate);

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
                        #endregion

                    }
                    else if (record.Type == 1)
                    {
                        #region 资产出租
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
                            PriceString = record.Money
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
                        #endregion
                    }
                }
                catch (Exception e)
                {
                    sb.AppendLine(string.Format("编号为{0}，出现错误：{1}", record.PropertyId, e.Message));
                }
            }

            return Ok(sb.ToString());
        }

        [HttpGet]
        [Route("Temp")]
        public IHttpActionResult Temp()
        {
            return Ok("closed");
            int scucessCount = 0, errorCount = 0;
            StringBuilder sb = new StringBuilder();
            var g = _governmentService.GetGovernmentUnitByName("汇盛公司");
            //拷贝图片
            var targetPath = System.Web.HttpContext.Current.Server.MapPath("~/Content/images/");
            var directionName = @"C:\房产证图片";

            var directions = System.IO.Directory.GetDirectories(directionName);

            foreach (var direction in directions)
            {
                try
                {
                    var files = System.IO.Directory.GetFiles(direction);
                    if (files == null || files.Count() == 0) continue;

                    string[] dirName = direction.Split('\\'); var id = dirName[dirName.Length - 1];

                    var property = _propertyService.GetPropertyById(int.Parse(id));
                    if (property == null) continue;
                    if (property.Deleted || property.Off) throw new Exception("资产被删除或核销");

                    var pictures = new List<Picture>();

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

                    if (property.Published)
                    {
                        #region 变更操作

                        var logoPicture = property.Pictures.Where(pp => pp.IsLogo).FirstOrDefault();

                        #region 赋值
                        var copyProperty = new CopyProperty
                        {
                            Address = property.Address,
                            ConstructArea = property.ConstructArea,
                            ConstructId = property.ConstructId,
                            CurrentUse_Idle = property.CurrentUse_Idle,
                            CurrentUse_Rent = property.CurrentUse_Rent,
                            CurrentUse_Lend = property.CurrentUse_Lend,
                            CurrentUse_Self = property.CurrentUse_Self,
                            Deleted = property.Deleted,
                            DisplayOrder = property.DisplayOrder,
                            Description = property.Description,
                            EstateId = property.EstateId,
                            Extent = property.Extent != null ? property.Extent.AsText() : "",
                            FileIds = string.Join("_", property.Files.Select(p => p.FileId).ToArray()),
                            GetedDate = property.GetedDate,
                            Government_Id = property.Government.Id,
                            HasConstructID = property.HasConstructID,
                            HasLandID = property.HasLandID,
                            LandArea = property.LandArea,
                            LandId = property.LandId,
                            LandNature = property.LandNature,
                            LifeTime = property.LifeTime,
                            Location = property.Location.AsText(),
                            LogoPicture_Id = logoPicture != null ? logoPicture.PictureId : 0,
                            Name = property.Name,
                            NextStepUsage = property.NextStepUsage,
                            Off = property.Off,
                            Price = property.Price,
                            PrictureIds = string.Join("_", property.Pictures.Select(p => p.PictureId).ToArray()),
                            PropertyID = property.PropertyID,
                            PropertyNature = property.PropertyNature,
                            PropertyType = property.PropertyType,
                            Property_Id = property.Id,
                            Published = property.Published,
                            Region = property.Region,
                            UsedPeople = property.UsedPeople
                        };
                        #endregion 

                        copyProperty.PrictureIds += (string.IsNullOrEmpty(copyProperty.PrictureIds) ? "" : "_") + string.Join("_", pictures.Select(p => p.Id).ToArray());
                        _copyPropertyService.InsertCopyProperty(copyProperty);

                        //添加一个资产编辑申请
                        var propertyEdit = new PropertyEdit()
                        {
                            Property = property,
                            Title = property.Name,
                            State = PropertyApproveState.Start,
                            ProcessDate = DateTime.Now,
                            SuggestGovernmentId = g.Id
                        };

                        propertyEdit.CopyProperty_Id = copyProperty.Id;

                        _propertyEditService.InsertPropertyEdit(propertyEdit);

                        //锁定property
                        property.Locked = true;
                        _propertyService.UpdateProperty(property);

                        #endregion
                    }
                    else
                    {
                        foreach (var picture in pictures)
                        {
                            var propertyPicture = new PropertyPicture {
                                Picture = picture,
                                Property = property,
                                IsLogo = false
                            };

                            property.Pictures.Add(propertyPicture);
                        }

                        _propertyService.UpdateProperty(property);
                    }
                    sb.AppendLine(string.Format("id 为 {0} 的资产 添加附件添加成功\r\n", id));

                    scucessCount++;
                }
                catch (Exception e)
                {
                    sb.AppendLine(string.Format("id 为 {0} 的资产 添加附件过程中出错\r\n", direction, e.Message));
                    errorCount++;
                }

            }

            sb.AppendLine(string.Format("共处理{0}个，成功{1}个，失败{2}个", scucessCount + errorCount, scucessCount, errorCount));

            return Ok(sb.ToString());
        }

        [HttpGet]
        [Route("Temp0")]
        public IHttpActionResult Temp0()
        {
            //return Ok("closed");
            int scucessCount = 0, errorCount = 0, allCcount = 0;
            StringBuilder sb = new StringBuilder();
            var g = _governmentService.GetGovernmentUnitByName("汇盛公司");
            //拷贝图片
            var targetPath = System.Web.HttpContext.Current.Server.MapPath("~/Content/images/");
            var directionName = @"C:\封面";

            var directions = System.IO.Directory.GetDirectories(directionName);
            //获取汇盛公司所有资产
            var properties = _propertyService.GetPropertiesByGovernmentId(new List<int>() { g.Id });

            foreach (var direction in directions)
            {
                var files = System.IO.Directory.GetFiles(direction);
                if (files == null || files.Count() == 0) continue;

                string[] dirName = direction.Split('\\');
                var address = dirName[dirName.Length - 1];

                var address1 = address.Substring(0, address.IndexOf('（'));
                var count = address.Substring(address.IndexOf('（') + 1, address.IndexOf('）') - address.IndexOf('（') - 1);

                var targetProperties = properties.Where(p => p.Address.StartsWith(address1));
                var ids = targetProperties.Select(p => p.Id).ToList();
                if (count == targetProperties.Count().ToString())
                {
                    allCcount += targetProperties.Count();
                    sb.AppendLine(string.Format("开始更新{0}的资产，查询数目为{1}，当前累计数目为{2},成功数目{3}，失败数目{4}", address, targetProperties.Count(), allCcount, scucessCount, errorCount));
                    foreach (var file in files)
                    {
                        var fileName = System.IO.Path.GetFileName(file);
                        var ext = System.IO.Path.GetExtension(file);

                        var fileStream = new FileStream(file, FileMode.Open);
                        var fileBinary = new byte[fileStream.Length];
                        fileStream.Read(fileBinary, 0, fileBinary.Length);

                        var picture = _pictureService.InsertPicture(fileBinary, "image/jpeg", "", "", fileName);
                        var url = _pictureService.GetPictureUrl(picture);


                        foreach (var id in ids)
                        {
                            var property = _propertyService.GetPropertyById(id);
                            if (property == null) continue;
                            if (property.Deleted || property.Off) throw new Exception("资产被删除或核销");

                            try
                            {
                                if (property.Published)
                                {

                                    #region 变更操作
                                    var startEditsCount = _propertyEditService.GetPropertyEditByPropertyId(id).Count(e => e.State == PropertyApproveState.Start);
                                    if (startEditsCount == 0)
                                    {
                                        #region 新增一个变更操作

                                        #region 赋值
                                        var copyProperty = new CopyProperty
                                        {
                                            Address = property.Address,
                                            ConstructArea = property.ConstructArea,
                                            ConstructId = property.ConstructId,
                                            CurrentUse_Idle = property.CurrentUse_Idle,
                                            CurrentUse_Rent = property.CurrentUse_Rent,
                                            CurrentUse_Lend = property.CurrentUse_Lend,
                                            CurrentUse_Self = property.CurrentUse_Self,
                                            Deleted = property.Deleted,
                                            DisplayOrder = property.DisplayOrder,
                                            Description = property.Description,
                                            EstateId = property.EstateId,
                                            Extent = property.Extent != null ? property.Extent.AsText() : "",
                                            FileIds = string.Join("_", property.Files.Select(p => p.FileId).ToArray()),
                                            GetedDate = property.GetedDate,
                                            Government_Id = property.Government.Id,
                                            HasConstructID = property.HasConstructID,
                                            HasLandID = property.HasLandID,
                                            LandArea = property.LandArea,
                                            LandId = property.LandId,
                                            LandNature = property.LandNature,
                                            LifeTime = property.LifeTime,
                                            Location = property.Location.AsText(),
                                            LogoPicture_Id = picture.Id,
                                            Name = property.Name,
                                            NextStepUsage = property.NextStepUsage,
                                            Off = property.Off,
                                            Price = property.Price,
                                            PrictureIds = string.Join("_", property.Pictures.Select(p => p.PictureId).ToArray()),
                                            PropertyID = property.PropertyID,
                                            PropertyNature = property.PropertyNature,
                                            PropertyType = property.PropertyType,
                                            Property_Id = property.Id,
                                            Published = property.Published,
                                            Region = property.Region,
                                            UsedPeople = property.UsedPeople
                                        };
                                        #endregion

                                        _copyPropertyService.InsertCopyProperty(copyProperty);

                                        //添加一个资产编辑申请
                                        var propertyEdit = new PropertyEdit()
                                        {
                                            Property = property,
                                            Title = property.Name,
                                            State = PropertyApproveState.Start,
                                            ProcessDate = DateTime.Now,
                                            SuggestGovernmentId = g.Id
                                        };

                                        propertyEdit.CopyProperty_Id = copyProperty.Id;

                                        _propertyEditService.InsertPropertyEdit(propertyEdit);

                                        //锁定property
                                        property.Locked = true;
                                        _propertyService.UpdateProperty(property);
                                        #endregion
                                    }
                                    else if (startEditsCount == 1)
                                    {
                                        //查找是否已经有一个变更记录
                                        var edit = _propertyEditService.GetPropertyEditByPropertyId(id).Where(e => e.State == PropertyApproveState.Start).SingleOrDefault();
                                        if (edit.State == PropertyApproveState.Start)
                                        {
                                            var copy = _copyPropertyService.GetCopyPropertyById(edit.CopyProperty_Id);

                                            copy.LogoPicture_Id = picture.Id;
                                            _copyPropertyService.UpdateCopyProperty(copy);


                                            ////edit.State = PropertyApproveState.DepartmentApprove;
                                            ////_propertyEditService.UpdatePropertyEdit(edit);
                                        }
                                    }
                                    #endregion
                                }
                                else
                                {
                                    PropertyPicture logoPicture = null;

                                    if (property.Pictures.Count > 0) logoPicture = property.Pictures.Where(pp => pp.IsLogo).FirstOrDefault();

                                    if (logoPicture != null)
                                    {
                                        logoPicture.Picture = picture;
                                        _propertyService.UpdatePropertyPicture(logoPicture);
                                    }
                                    else
                                    {
                                        logoPicture = new PropertyPicture
                                        {
                                            Picture = picture,
                                            Property = property,
                                            IsLogo = true
                                        };

                                        _propertyService.InsertPropertyPicture(logoPicture);
                                    }

                                    var newCreate = _propertyNewCreateService.GetPropertyNewCreateByPropertyId(property.Id);
                                    if (property.Pictures.Where(pp => !pp.IsLogo).Count() > 0) newCreate.State = PropertyApproveState.DepartmentApprove;
                                    _propertyNewCreateService.UpdatePropertyNewCreate(newCreate);
                                }
                                sb.AppendLine(string.Format("id 为 {0} 的资产 添加附件添加成功\r\n", property.Address));

                                scucessCount++;
                            }
                            catch (Exception e)
                            {
                                sb.AppendLine(string.Format("id 为 {0} 的资产 添加附件过程中出错\r\n", direction, e.Message));
                                errorCount++;
                            }


                        }
                    }

                }


            }

            sb.AppendLine(string.Format("共处理{0}个，成功{1}个，失败{2}个", scucessCount + errorCount, scucessCount, errorCount));

            return Ok(sb.ToString());
        }

        [HttpGet]
        [Route("Temp1")]
        public IHttpActionResult Temp1()
        {

            return Ok("closed");

            StringBuilder sb = new StringBuilder();
            var properties = new List<Property>()
            {
                #region data
                            new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢01号",Address="衢州市后垄张新村1幢01号",ConstructArea=6.68,LandArea=1.32,LandId="2014 - 10964",ConstructId="14109620",PropertyNature="国有",LandNature="国有",Price= 1 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 6.68    },
    new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢02号",Address="衢州市后垄张新村1幢02号",ConstructArea=6.34,LandArea=1.25,LandId="2014 - 10963",ConstructId="14109621",PropertyNature="国有",LandNature="国有",Price= 1 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 6.34    },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢03号",Address="衢州市后垄张新村1幢03号",ConstructArea=6.68,LandArea=1.32,LandId="2014 - 10962",ConstructId="14109622",PropertyNature="国有",LandNature="国有",Price= 1 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 6.68    },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢04号",Address="衢州市后垄张新村1幢04号",ConstructArea=6.01,LandArea=1.19,LandId="2014 - 10961",ConstructId="14109623",PropertyNature="国有",LandNature="国有",Price= 1 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 6.01    },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢05号",Address="衢州市后垄张新村1幢05号",ConstructArea=5.68,LandArea=1.12,LandId="2014 - 10960",ConstructId="14109624",PropertyNature="国有",LandNature="国有",Price= 1 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 5.68    },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢06号",Address="衢州市后垄张新村1幢06号",ConstructArea=6.01,LandArea=1.19,LandId="2014 - 10959",ConstructId="14109625",PropertyNature="国有",LandNature="国有",Price= 1 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 6.01    },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢07号",Address="衢州市后垄张新村1幢07号",ConstructArea=6.01,LandArea=1.19,LandId="2014 - 10958",ConstructId="14109626",PropertyNature="国有",LandNature="国有",Price= 1 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 6.01    },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢08号",Address="衢州市后垄张新村1幢08号",ConstructArea=6.68,LandArea=1.32,LandId="2014 - 10957",ConstructId="14109627",PropertyNature="国有",LandNature="国有",Price= 1 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 6.68    },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢09号",Address="衢州市后垄张新村1幢09号",ConstructArea=6.68,LandArea=1.32,LandId="2014 - 10999",ConstructId="14109628",PropertyNature="国有",LandNature="国有",Price= 1 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 6.68    },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢10号",Address="衢州市后垄张新村1幢10号",ConstructArea=6.68,LandArea=1.32,LandId="2014 - 10998",ConstructId="14109629",PropertyNature="国有",LandNature="国有",Price= 1 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 6.68    },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢11号",Address="衢州市后垄张新村1幢11号",ConstructArea=6.68,LandArea=1.32,LandId="2014 - 10997",ConstructId="14109630",PropertyNature="国有",LandNature="国有",Price= 1 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 6.68    },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢12号",Address="衢州市后垄张新村1幢12号",ConstructArea=6.01,LandArea=1.19,LandId="2014 - 10996",ConstructId="14109631",PropertyNature="国有",LandNature="国有",Price= 1 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 6.01    },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢13号",Address="衢州市后垄张新村1幢13号",ConstructArea=6.01,LandArea=1.19,LandId="2014 - 10995",ConstructId="14109632",PropertyNature="国有",LandNature="国有",Price= 1 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 6.01    },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢14号",Address="衢州市后垄张新村1幢14号",ConstructArea=6.01,LandArea=1.19,LandId="2014 - 10994",ConstructId="14109633",PropertyNature="国有",LandNature="国有",Price= 1 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 6.01    },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢15号",Address="衢州市后垄张新村1幢15号",ConstructArea=6.01,LandArea=1.19,LandId="2014 - 10993",ConstructId="14109634",PropertyNature="国有",LandNature="国有",Price= 1 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 6.01    },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢16号",Address="衢州市后垄张新村1幢16号",ConstructArea=6.68,LandArea=1.32,LandId="2014 - 10992",ConstructId="14109635",PropertyNature="国有",LandNature="国有",Price= 1 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 6.68    },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢17号",Address="衢州市后垄张新村1幢17号",ConstructArea=6.68,LandArea=1.32,LandId="2014 - 10990",ConstructId="14109636",PropertyNature="国有",LandNature="国有",Price= 1 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 6.68    },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢18号",Address="衢州市后垄张新村1幢18号",ConstructArea=6.68,LandArea=1.32,LandId="2014 - 10989",ConstructId="14109637",PropertyNature="国有",LandNature="国有",Price= 1 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 6.68    },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢19号",Address="衢州市后垄张新村1幢19号",ConstructArea=6.68,LandArea=1.32,LandId="2014 - 10988",ConstructId="14109638",PropertyNature="国有",LandNature="国有",Price= 1 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 6.68    },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢20号",Address="衢州市后垄张新村1幢20号",ConstructArea=6.01,LandArea=1.19,LandId="2014 - 10987",ConstructId="14109639",PropertyNature="国有",LandNature="国有",Price= 1 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 6.01    },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢21号",Address="衢州市后垄张新村1幢21号",ConstructArea=6.01,LandArea=1.19,LandId="2014 - 10986",ConstructId="14109640",PropertyNature="国有",LandNature="国有",Price= 1 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 6.01    },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢22号",Address="衢州市后垄张新村1幢22号",ConstructArea=6.01,LandArea=1.19,LandId="2014 - 10985",ConstructId="14109641",PropertyNature="国有",LandNature="国有",Price= 1 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 6.01    },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢23号",Address="衢州市后垄张新村1幢23号",ConstructArea=6.01,LandArea=1.19,LandId="2014 - 10984",ConstructId="14109642",PropertyNature="国有",LandNature="国有",Price= 1 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 6.01    },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢24号",Address="衢州市后垄张新村1幢24号",ConstructArea=6.68,LandArea=1.32,LandId="2014 - 10981",ConstructId="14109643",PropertyNature="国有",LandNature="国有",Price= 1 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 6.68    },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢25号",Address="衢州市后垄张新村1幢25号",ConstructArea=6.68,LandArea=1.32,LandId="2014 - 10980",ConstructId="14109644",PropertyNature="国有",LandNature="国有",Price= 1 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 6.68    },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢27号",Address="衢州市后垄张新村1幢27号",ConstructArea=6.68,LandArea=1.32,LandId="2014 - 10979",ConstructId="14109646",PropertyNature="国有",LandNature="国有",Price= 1 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 6.68    },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢28号",Address="衢州市后垄张新村1幢28号",ConstructArea=6.01,LandArea=1.19,LandId="2014 - 10978",ConstructId="14109647",PropertyNature="国有",LandNature="国有",Price= 1 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 6.01    },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢29号",Address="衢州市后垄张新村1幢29号",ConstructArea=6.01,LandArea=1.19,LandId="2014 - 10977",ConstructId="14109648",PropertyNature="国有",LandNature="国有",Price= 1 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 6.01    },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢30号",Address="衢州市后垄张新村1幢30号",ConstructArea=6.34,LandArea=1.25,LandId="2014 - 10976",ConstructId="14109649",PropertyNature="国有",LandNature="国有",Price= 1 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 6.34    },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢35号",Address="衢州市后垄张新村1幢35号",ConstructArea=6.68,LandArea=1.32,LandId="2014 - 10975",ConstructId="14109654",PropertyNature="国有",LandNature="国有",Price= 1 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 6.68    },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢1 - 1号一层、二层",Address="衢州市后垄张新村1幢1 - 1号一层、二层",ConstructArea=186.87,LandArea=19.64 / 17.29,LandId="2014 - 10930            2014 - 10931  ",ConstructId="14109717",PropertyNature="国有",LandNature="国有",Price= 20,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 186.87  },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢1 - 2号一层、二层",Address="衢州市后垄张新村1幢1 - 2号一层、二层",ConstructArea=220.94,LandArea=20.46 / 23.21,LandId="2014 - 10932            2014 - 10934  ",ConstructId="14109713",PropertyNature="国有",LandNature="国有",Price= 24,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 220.94  },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢1 - 3号一层、二层",Address="衢州市后垄张新村1幢1 - 3号一层、二层",ConstructArea=161.2 ,LandArea=17.08 / 14.78,LandId="2014 - 10956            2014 - 10955  ",ConstructId="14109619",PropertyNature="国有",LandNature="国有",Price= 18,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 161.2   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢1 - 4号一层、二层",Address="衢州市后垄张新村1幢1 - 4号一层、二层",ConstructArea=204.92,LandArea=19.54 / 20.97,LandId="2014 - 10953            2014 - 10954  ",ConstructId="14109618",PropertyNature="国有",LandNature="国有",Price= 22,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 204.92  },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢1 - 5号一层、二层",Address="衢州市后垄张新村1幢1 - 5号一层、二层",ConstructArea=263.41,LandArea=27.78 / 24.29,LandId="2014 - 10952            2014 - 10951  ",ConstructId="14109615",PropertyNature="国有",LandNature="国有",Price= 29,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 263.41  },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢1 - 6号一层、二层",Address="衢州市后垄张新村1幢1 - 6号一层、二层",ConstructArea=209.84,LandArea=21.94 / 19.54,LandId="2014 - 10950            2014 - 10949  ",ConstructId="14109616",PropertyNature="国有",LandNature="国有",Price= 23,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 209.84  },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢1 - 7号一层、二层",Address="衢州市后垄张新村1幢1 - 7号一层、二层",ConstructArea=258.49,LandArea=26.80 / 24.29,LandId="2014 - 10948            2014 - 10947  ",ConstructId="14109715",PropertyNature="国有",LandNature="国有",Price= 28,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 258.49  },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢1 - 8号一层、二层",Address="衢州市后垄张新村1幢1 - 8号一层、二层",ConstructArea=209.84,LandArea=21.94 / 19.54,LandId="2014 - 10946            2014 - 10945  ",ConstructId="14109714",PropertyNature="国有",LandNature="国有",Price= 23,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 209.84  },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢1 - 9号一层、二层",Address="衢州市后垄张新村1幢1 - 9号一层、二层",ConstructArea=186.87,LandArea=19.64 / 17.29,LandId="2014 - 10944            2014 - 10943  ",ConstructId="14109716",PropertyNature="国有",LandNature="国有",Price= 20,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 186.87  },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢1 - 10号一层、二层",Address="衢州市后垄张新村1幢1 - 10号一层、二层",ConstructArea=186.87,LandArea=19.64 / 17.29,LandId="2014 - 10942            2014 - 10941  ",ConstructId="14109614",PropertyNature="国有",LandNature="国有",Price= 20,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="",CurrentUse_Self= 186.87  },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢1单元301室",Address="衢州市后垄张新村1幢1单元301室",ConstructArea=92.98 ,LandArea=18.38,LandId="2014 - 11109",ConstructId="14109655",PropertyNature="国有",LandNature="国有",Price= 10,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 92.98   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢1单元302室",Address="衢州市后垄张新村1幢1单元302室",ConstructArea=64.48 ,LandArea=12.74,LandId="2014 - 11108",ConstructId="14109656",PropertyNature="国有",LandNature="国有",Price= 7 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="",CurrentUse_Self= 64.48   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢1单元401室",Address="衢州市后垄张新村1幢1单元401室",ConstructArea=92.98 ,LandArea=18.38,LandId="2014 - 11026",ConstructId="14109657",PropertyNature="国有",LandNature="国有",Price= 10,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="",CurrentUse_Self= 92.98   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢1单元402室",Address="衢州市后垄张新村1幢1单元402室",ConstructArea=64.48 ,LandArea=12.74,LandId="2014 - 11024",ConstructId="14109658",PropertyNature="国有",LandNature="国有",Price= 7 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="",CurrentUse_Self= 64.48   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢1单元501室",Address="衢州市后垄张新村1幢1单元501室",ConstructArea=84.5,LandArea=16.7,LandId="2014 - 11022",ConstructId="14109659",PropertyNature="国有",LandNature="国有",Price= 9 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="",CurrentUse_Self= 84.5    },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢1单元502室",Address="衢州市后垄张新村1幢1单元502室",ConstructArea=59.97 ,LandArea=11.85,LandId="2014 - 11021",ConstructId="14109660",PropertyNature="国有",LandNature="国有",Price= 7 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="",CurrentUse_Self= 59.97   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢1单元甲501室",Address="衢州市后垄张新村1幢1单元甲501室",ConstructArea=65.07 ,LandArea=12.86,LandId="2014 - 11020",ConstructId="14109661",PropertyNature="国有",LandNature="国有",Price= 7 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="",CurrentUse_Self= 65.07   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢1单元甲502室",Address="衢州市后垄张新村1幢1单元甲502室",ConstructArea=41.23 ,LandArea=8.15,LandId="2014 - 11019",ConstructId="14109662",PropertyNature="国有",LandNature="国有",Price= 4 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="",CurrentUse_Self= 41.23   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢2单元301室",Address="衢州市后垄张新村1幢2单元301室",ConstructArea=68.58 ,LandArea=13.56,LandId="2014 - 11064",ConstructId="14109663",PropertyNature="国有",LandNature="国有",Price= 7 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 68.58   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢2单元302室",Address="衢州市后垄张新村1幢2单元302室",ConstructArea=58.78 ,LandArea=11.62,LandId="2014 - 11122",ConstructId="14109664",PropertyNature="国有",LandNature="国有",Price= 6 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 58.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢2单元303室",Address="衢州市后垄张新村1幢2单元303室",ConstructArea=65.71 ,LandArea=12.99,LandId="2014 - 11121",ConstructId="14109665",PropertyNature="国有",LandNature="国有",Price= 7 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 65.71   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢2单元304室",Address="衢州市后垄张新村1幢2单元304室",ConstructArea=58.78 ,LandArea=11.62,LandId="2014 - 11119",ConstructId="14109666",PropertyNature="国有",LandNature="国有",Price= 6 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 58.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢2单元401室",Address="衢州市后垄张新村1幢2单元401室",ConstructArea=68.58 ,LandArea=13.56,LandId="2014 - 11118",ConstructId="14109668",PropertyNature="国有",LandNature="国有",Price= 7 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 68.58   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢2单元402室",Address="衢州市后垄张新村1幢2单元402室",ConstructArea=58.78 ,LandArea=11.62,LandId="2014 - 11117",ConstructId="14109667",PropertyNature="国有",LandNature="国有",Price= 6 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 58.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢2单元403室",Address="衢州市后垄张新村1幢2单元403室",ConstructArea=65.71 ,LandArea=12.99,LandId="2014 - 11116",ConstructId="14109669",PropertyNature="国有",LandNature="国有",Price= 7 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 65.71   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢2单元404室",Address="衢州市后垄张新村1幢2单元404室",ConstructArea=58.78 ,LandArea=11.62,LandId="2014 - 11115",ConstructId="14109670",PropertyNature="国有",LandNature="国有",Price= 6 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 58.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢2单元501室",Address="衢州市后垄张新村1幢2单元501室",ConstructArea=60.79 ,LandArea=12.02,LandId="2014 - 11114",ConstructId="14109671",PropertyNature="国有",LandNature="国有",Price= 7 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 60.79   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢2单元502室",Address="衢州市后垄张新村1幢2单元502室",ConstructArea=58.78 ,LandArea=11.62,LandId="2014 - 11113",ConstructId="14109672",PropertyNature="国有",LandNature="国有",Price= 6 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 58.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢2单元503室",Address="衢州市后垄张新村1幢2单元503室",ConstructArea=60.79 ,LandArea=12.02,LandId="2014 - 11111",ConstructId="14109673",PropertyNature="国有",LandNature="国有",Price= 7 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 60.79   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢2单元504室",Address="衢州市后垄张新村1幢2单元504室",ConstructArea=58.78 ,LandArea=11.62,LandId="2014 - 11110",ConstructId="14109674",PropertyNature="国有",LandNature="国有",Price= 6 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 58.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢2单元甲501室",Address="衢州市后垄张新村1幢2单元甲501室",ConstructArea=88.44 ,LandArea=17.48,LandId="2014 - 11107",ConstructId="14109675",PropertyNature="国有",LandNature="国有",Price= 10,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 88.44   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢2单元甲502室",Address="衢州市后垄张新村1幢2单元甲502室",ConstructArea=88.44 ,LandArea=17.48,LandId="2014 - 11106",ConstructId="14109676",PropertyNature="国有",LandNature="国有",Price= 10,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 88.44   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢3单元301室",Address="衢州市后垄张新村1幢3单元301室",ConstructArea=69.1,LandArea=13.66,LandId="2014 - 11105",ConstructId="14109677",PropertyNature="国有",LandNature="国有",Price= 8 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 69.1    },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢3单元302室",Address="衢州市后垄张新村1幢3单元302室",ConstructArea=58.78 ,LandArea=11.62,LandId="2014 - 10969",ConstructId="14109678",PropertyNature="国有",LandNature="国有",Price= 6 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 58.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢3单元303室",Address="衢州市后垄张新村1幢3单元303室",ConstructArea=69.1,LandArea=13.66,LandId="2014 - 10967",ConstructId="14109679",PropertyNature="国有",LandNature="国有",Price= 8 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 69.1    },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢3单元304室",Address="衢州市后垄张新村1幢3单元304室",ConstructArea=58.78 ,LandArea=11.62,LandId="2014 - 10971",ConstructId="14109680",PropertyNature="国有",LandNature="国有",Price= 6 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 58.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢3单元401室",Address="衢州市后垄张新村1幢3单元401室",ConstructArea=69.1,LandArea=13.66,LandId="2014 - 10972",ConstructId="14109681",PropertyNature="国有",LandNature="国有",Price= 8 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 69.1    },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢3单元402室",Address="衢州市后垄张新村1幢3单元402室",ConstructArea=58.78 ,LandArea=11.62,LandId="2014 - 10970",ConstructId="14109682",PropertyNature="国有",LandNature="国有",Price= 6 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 58.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢3单元403室",Address="衢州市后垄张新村1幢3单元403室",ConstructArea=69.1,LandArea=13.66,LandId="2014 - 10968",ConstructId="14109683",PropertyNature="国有",LandNature="国有",Price= 8 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 69.1    },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢3单元404室",Address="衢州市后垄张新村1幢3单元404室",ConstructArea=58.78 ,LandArea=11.62,LandId="2014 - 10966",ConstructId="14109684",PropertyNature="国有",LandNature="国有",Price= 6 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 58.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢3单元501室",Address="衢州市后垄张新村1幢3单元501室",ConstructArea=58.17 ,LandArea=11.5,LandId="2014 - 10965",ConstructId="14109685",PropertyNature="国有",LandNature="国有",Price= 6 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 58.17   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢3单元502室",Address="衢州市后垄张新村1幢3单元502室",ConstructArea=58.78 ,LandArea=11.62,LandId="2014 - 10974",ConstructId="14109686",PropertyNature="国有",LandNature="国有",Price= 6 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 58.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢3单元503室",Address="衢州市后垄张新村1幢3单元503室",ConstructArea=58.17 ,LandArea=11.5,LandId="2014 - 11034",ConstructId="14109687",PropertyNature="国有",LandNature="国有",Price= 6 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 58.17   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢3单元504室",Address="衢州市后垄张新村1幢3单元504室",ConstructArea=58.78 ,LandArea=11.62,LandId="2014 - 11031",ConstructId="14109688",PropertyNature="国有",LandNature="国有",Price= 6 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 58.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢3单元甲501室",Address="衢州市后垄张新村1幢3单元甲501室",ConstructArea=77.82 ,LandArea=15.38,LandId="2014 - 11101",ConstructId="14109689",PropertyNature="国有",LandNature="国有",Price= 8 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 77.82   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢3单元甲502室",Address="衢州市后垄张新村1幢3单元甲502室",ConstructArea=87.08 ,LandArea=17.21,LandId="2014 - 11102",ConstructId="14109690",PropertyNature="国有",LandNature="国有",Price= 9 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 87.08   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢4单元302室",Address="衢州市后垄张新村1幢4单元302室",ConstructArea=58.78 ,LandArea=11.62,LandId="2014 - 11103",ConstructId="14109692",PropertyNature="国有",LandNature="国有",Price= 6 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 58.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢4单元303室",Address="衢州市后垄张新村1幢4单元303室",ConstructArea=69.26 ,LandArea=13.69,LandId="2014 - 11104",ConstructId="14109693",PropertyNature="国有",LandNature="国有",Price= 8 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 69.26   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢4单元304室",Address="衢州市后垄张新村1幢4单元304室",ConstructArea=58.78 ,LandArea=11.62,LandId="2014 - 11033",ConstructId="14109694",PropertyNature="国有",LandNature="国有",Price= 6 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 58.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢4单元401室",Address="衢州市后垄张新村1幢4单元401室",ConstructArea=65.71 ,LandArea=12.99,LandId="2014 - 11029",ConstructId="14109695",PropertyNature="国有",LandNature="国有",Price= 7 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 65.71   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢4单元402室",Address="衢州市后垄张新村1幢4单元402室",ConstructArea=58.78 ,LandArea=11.62,LandId="2014 - 11028",ConstructId="14109696",PropertyNature="国有",LandNature="国有",Price= 6 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 58.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢4单元403室",Address="衢州市后垄张新村1幢4单元403室",ConstructArea=69.26 ,LandArea=13.69,LandId="2014 - 11027",ConstructId="14109697",PropertyNature="国有",LandNature="国有",Price= 8 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 69.26   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢4单元404室",Address="衢州市后垄张新村1幢4单元404室",ConstructArea=58.78 ,LandArea=11.62,LandId="2014 - 11025",ConstructId="14109698",PropertyNature="国有",LandNature="国有",Price= 6 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 58.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢4单元501室",Address="衢州市后垄张新村1幢4单元501室",ConstructArea=60.79 ,LandArea=12.02,LandId="2014 - 10973",ConstructId="14109699",PropertyNature="国有",LandNature="国有",Price= 7 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 60.79   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢4单元502室",Address="衢州市后垄张新村1幢4单元502室",ConstructArea=58.78 ,LandArea=11.62,LandId="2014 - 11069",ConstructId="14109700",PropertyNature="国有",LandNature="国有",Price= 6 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 58.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢4单元503室",Address="衢州市后垄张新村1幢4单元503室",ConstructArea=60.79 ,LandArea=12.02,LandId="2014 - 11070",ConstructId="14109701",PropertyNature="国有",LandNature="国有",Price= 7 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 60.79   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢4单元504室",Address="衢州市后垄张新村1幢4单元504室",ConstructArea=58.78 ,LandArea=11.62,LandId="2014 - 11075",ConstructId="14109702",PropertyNature="国有",LandNature="国有",Price= 6 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 58.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢4单元甲501室",Address="衢州市后垄张新村1幢4单元甲501室",ConstructArea=88.44 ,LandArea=17.48,LandId="2014 - 11068",ConstructId="14109703",PropertyNature="国有",LandNature="国有",Price= 10,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 88.44   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢4单元甲502室",Address="衢州市后垄张新村1幢4单元甲502室",ConstructArea=88.44 ,LandArea=17.48,LandId="2014 - 11074",ConstructId="14109704",PropertyNature="国有",LandNature="国有",Price= 10,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 88.44   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢5单元402室",Address="衢州市后垄张新村1幢5单元402室",ConstructArea=92.98 ,LandArea=18.38,LandId="2014 - 11073",ConstructId="14109708",PropertyNature="国有",LandNature="国有",Price= 10,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 92.98   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢5单元501室",Address="衢州市后垄张新村1幢5单元501室",ConstructArea=84.27 ,LandArea=16.66,LandId="2014 - 11072",ConstructId="14109709",PropertyNature="国有",LandNature="国有",Price= 9 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 84.27   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢5单元甲501室",Address="衢州市后垄张新村1幢5单元甲501室",ConstructArea=65.07 ,LandArea=12.86,LandId="2014 - 11076",ConstructId="14109711",PropertyNature="国有",LandNature="国有",Price= 7 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 65.07   },
new Property {PropertyType= PropertyType.House, Name="衢州市后垄张新村1幢5单元甲502室",Address="衢州市后垄张新村1幢5单元甲502室",ConstructArea=65.07 ,LandArea=12.86,LandId="2014 - 11077",ConstructId="14109712",PropertyNature="国有",LandNature="国有",Price= 7 ,GetedDate=Convert.ToDateTime(" 2014.9.11 "),LifeTime= 70,UsedPeople="汇盛",CurrentUse_Self= 65.07   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑13幢1单元202",Address="衢州市碧桂园小区凤翔苑13幢1单元202",ConstructArea=83.13 ,LandArea=6.47,LandId="衢州国用(2016)第07009  ",ConstructId="衢房权证衢州市字第16156967",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 83.13   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑13幢1单元203 ",Address=" 衢州市碧桂园小区凤翔苑13幢1单元203 ",ConstructArea=81.64 ,LandArea=6.35,LandId="衢州国用(2016)第06762  ",ConstructId="衢房权证衢州市字第16156570",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.64   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑13幢1单元303 ",Address=" 衢州市碧桂园小区凤翔苑13幢1单元303 ",ConstructArea=81.32 ,LandArea=6.33,LandId="衢州国用(2016)第06749  ",ConstructId="衢房权证衢州市字第16156617",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.32   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑13幢1单元403 ",Address=" 衢州市碧桂园小区凤翔苑13幢1单元403 ",ConstructArea=81.32 ,LandArea=6.33,LandId="衢州国用(2016)第06757  ",ConstructId="衢房权证衢州市字第16156654",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.32   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑13幢1单元903 ",Address=" 衢州市碧桂园小区凤翔苑13幢1单元903 ",ConstructArea=81.32 ,LandArea=6.33,LandId="衢州国用(2016)第06760  ",ConstructId="衢房权证衢州市字第16156569",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.32   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑13幢1单元1103",Address=" 衢州市碧桂园小区凤翔苑13幢1单元1103",ConstructArea=81.32 ,LandArea=6.33,LandId="衢州国用(2016)第06774  ",ConstructId="衢房权证衢州市字第16156612",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.32   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑13幢1单元1303",Address=" 衢州市碧桂园小区凤翔苑13幢1单元1303",ConstructArea=81.32 ,LandArea=6.33,LandId="衢州国用(2016)第06849  ",ConstructId="衢房权证衢州市字第16156766",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.32   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑13幢1单元1402",Address=" 衢州市碧桂园小区凤翔苑13幢1单元1402",ConstructArea=82.81 ,LandArea=6.44,LandId="衢州国用(2016)第06848  ",ConstructId="衢房权证衢州市字第16156764",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.81   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑13幢1单元1403",Address=" 衢州市碧桂园小区凤翔苑13幢1单元1403",ConstructArea=81.32 ,LandArea=6.33,LandId="衢州国用(2016)第07002  ",ConstructId="衢房权证衢州市字第16156962",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.32   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑13幢1单元1703",Address=" 衢州市碧桂园小区凤翔苑13幢1单元1703",ConstructArea=81.32 ,LandArea=6.33,LandId="衢州国用(2016)第06847  ",ConstructId="衢房权证衢州市字第16156765",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.32   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑13幢1单元1803",Address=" 衢州市碧桂园小区凤翔苑13幢1单元1803",ConstructArea=81.32 ,LandArea=6.33,LandId="衢州国用(2016)第06700  ",ConstructId="衢房权证衢州市字第16156742",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.32   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑13幢1单元1903",Address=" 衢州市碧桂园小区凤翔苑13幢1单元1903",ConstructArea=81.32 ,LandArea=6.33,LandId="衢州国用(2016)第06702  ",ConstructId="衢房权证衢州市字第16156738",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.32   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑13幢1单元2002",Address=" 衢州市碧桂园小区凤翔苑13幢1单元2002",ConstructArea=82.81 ,LandArea=6.44,LandId="衢州国用(2016)第07004  ",ConstructId="衢房权证衢州市字第16156963",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.81   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑13幢1单元2003",Address=" 衢州市碧桂园小区凤翔苑13幢1单元2003",ConstructArea=81.32 ,LandArea=6.33,LandId="衢州国用(2016)第06925  ",ConstructId="衢房权证衢州市字第16156780",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.32   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑13幢1单元2102",Address=" 衢州市碧桂园小区凤翔苑13幢1单元2102",ConstructArea=82.81 ,LandArea=6.44,LandId="衢州国用(2016)第07005  ",ConstructId="衢房权证衢州市字第16156964",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.81   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑13幢1单元2103",Address=" 衢州市碧桂园小区凤翔苑13幢1单元2103",ConstructArea=81.32 ,LandArea=6.33,LandId="衢州国用(2016)第07007  ",ConstructId="衢房权证衢州市字第16156965",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.32   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑13幢1单元2202",Address=" 衢州市碧桂园小区凤翔苑13幢1单元2202",ConstructArea=83.13 ,LandArea=6.47,LandId="衢州国用(2016)第07008  ",ConstructId="衢房权证衢州市字第16156966",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 83.13   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑13幢1单元2203",Address=" 衢州市碧桂园小区凤翔苑13幢1单元2203",ConstructArea=81.64 ,LandArea=6.35,LandId="衢州国用(2016)第07010  ",ConstructId="衢房权证衢州市字第16156968",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.64   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑13幢1单元2302",Address=" 衢州市碧桂园小区凤翔苑13幢1单元2302",ConstructArea=82.81 ,LandArea=6.44,LandId="衢州国用(2016)第07011  ",ConstructId="衢房权证衢州市字第16156969",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.81   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑13幢1单元2303",Address=" 衢州市碧桂园小区凤翔苑13幢1单元2303",ConstructArea=81.32 ,LandArea=6.33,LandId="衢州国用(2016)第07012  ",ConstructId="衢房权证衢州市字第16156970",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.32   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑13幢1单元2401",Address=" 衢州市碧桂园小区凤翔苑13幢1单元2401",ConstructArea=100.29,LandArea=7.8,LandId="衢州国用(2016)第07014  ",ConstructId="衢房权证衢州市字第16156971",PropertyNature="国有",LandNature=" 国有",Price= 48,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 100.29  },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑13幢1单元2402",Address=" 衢州市碧桂园小区凤翔苑13幢1单元2402",ConstructArea=79.25 ,LandArea=6.16,LandId="衢州国用(2016)第06999  ",ConstructId="衢房权证衢州市字第16156960",PropertyNature="国有",LandNature=" 国有",Price= 38,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 79.25   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑13幢1单元2403",Address=" 衢州市碧桂园小区凤翔苑13幢1单元2403",ConstructArea=77.76 ,LandArea=6.05,LandId="衢州国用(2016)第07001  ",ConstructId="衢房权证衢州市字第16156961",PropertyNature="国有",LandNature=" 国有",Price= 37,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 77.76   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑13幢1单元2404",Address=" 衢州市碧桂园小区凤翔苑13幢1单元2404",ConstructArea=123.73,LandArea=9.62,LandId="衢州国用(2016)第06851  ",ConstructId="衢房权证衢州市字第16156767",PropertyNature="国有",LandNature=" 国有",Price= 59,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 123.73  },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元201 ",Address=" 衢州市碧桂园小区凤翔苑14幢1单元201 ",ConstructArea=89.17 ,LandArea=8.47,LandId="衢州国用(2016)第06853  ",ConstructId="衢房权证衢州市字第16156823",PropertyNature="国有",LandNature=" 国有",Price= 42,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 89.17   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元202 ",Address=" 衢州市碧桂园小区凤翔苑14幢1单元202 ",ConstructArea=83.09 ,LandArea=7.89,LandId="衢州国用(2016)第06894  ",ConstructId="衢房权证衢州市字第16156835",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 83.09   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元203 ",Address=" 衢州市碧桂园小区凤翔苑14幢1单元203 ",ConstructArea=81.6,LandArea=7.75,LandId="衢州国用(2016)第07006  ",ConstructId="衢房权证衢州市字第16157433",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.6    },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元204 ",Address=" 衢州市碧桂园小区凤翔苑14幢1单元204 ",ConstructArea=112.32,LandArea=10.67,LandId="衢州国用(2016)第06892  ",ConstructId="衢房权证衢州市字第16156824",PropertyNature="国有",LandNature=" 国有",Price= 53,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 112.32  },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元301 ",Address=" 衢州市碧桂园小区凤翔苑14幢1单元301 ",ConstructArea=88.4,LandArea=8.4,LandId="衢州国用(2016)第06889  ",ConstructId="衢房权证衢州市字第16156808",PropertyNature="国有",LandNature=" 国有",Price= 42,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 88.4    },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元302 ",Address=" 衢州市碧桂园小区凤翔苑14幢1单元302 ",ConstructArea=82.77 ,LandArea=7.86,LandId="衢州国用(2016)第06883  ",ConstructId="衢房权证衢州市字第16156804",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.77   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元303 ",Address=" 衢州市碧桂园小区凤翔苑14幢1单元303 ",ConstructArea=81.28 ,LandArea=7.72,LandId="衢州国用(2016)第07073  ",ConstructId="衢房权证衢州市字第16157415",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.28   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元304 ",Address=" 衢州市碧桂园小区凤翔苑14幢1单元304 ",ConstructArea=111.51,LandArea=10.59,LandId="衢州国用(2016)第06882  ",ConstructId="衢房权证衢州市字第16156805",PropertyNature="国有",LandNature=" 国有",Price= 53,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 111.51  },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元401 ",Address=" 衢州市碧桂园小区凤翔苑14幢1单元401 ",ConstructArea=88.4,LandArea=8.4,LandId="衢州国用(2016)第06678  ",ConstructId="衢房权证衢州市字第16156796",PropertyNature="国有",LandNature=" 国有",Price= 42,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 88.4    },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元402 ",Address=" 衢州市碧桂园小区凤翔苑14幢1单元402 ",ConstructArea=82.77 ,LandArea=7.86,LandId="衢州国用(2016)第06846  ",ConstructId="衢房权证衢州市字第16156810",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.77   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元403 ",Address=" 衢州市碧桂园小区凤翔苑14幢1单元403 ",ConstructArea=81.28 ,LandArea=7.72,LandId="衢州国用(2016)第06987  ",ConstructId="衢房权证衢州市字第16157436",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.28   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元404 ",Address=" 衢州市碧桂园小区凤翔苑14幢1单元404 ",ConstructArea=111.51,LandArea=10.59,LandId="衢州国用(2016)第06683  ",ConstructId="衢房权证衢州市字第16156900",PropertyNature="国有",LandNature=" 国有",Price= 53,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 111.51  },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元501 ",Address=" 衢州市碧桂园小区凤翔苑14幢1单元501 ",ConstructArea=88.4,LandArea=8.4,LandId="衢州国用(2016)第06689  ",ConstructId="衢房权证衢州市字第16156905",PropertyNature="国有",LandNature=" 国有",Price= 42,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 88.4    },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元502 ",Address=" 衢州市碧桂园小区凤翔苑14幢1单元502 ",ConstructArea=82.77 ,LandArea=7.86,LandId="衢州国用(2016)第06685  ",ConstructId="衢房权证衢州市字第16156903",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.77   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元503 ",Address=" 衢州市碧桂园小区凤翔苑14幢1单元503 ",ConstructArea=81.28 ,LandArea=7.72,LandId="衢州国用(2016)第06998  ",ConstructId="衢房权证衢州市字第16157430",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.28   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元504 ",Address=" 衢州市碧桂园小区凤翔苑14幢1单元504 ",ConstructArea=111.51,LandArea=10.59,LandId="衢州国用(2016)第07000  ",ConstructId="衢房权证衢州市字第16157431",PropertyNature="国有",LandNature=" 国有",Price= 53,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 111.51  },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元601 ",Address=" 衢州市碧桂园小区凤翔苑14幢1单元601 ",ConstructArea=88.4,LandArea=8.4,LandId="衢州国用(2016)第07003  ",ConstructId="衢房权证衢州市字第16157432",PropertyNature="国有",LandNature=" 国有",Price= 42,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 88.4    },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元602 ",Address=" 衢州市碧桂园小区凤翔苑14幢1单元602 ",ConstructArea=82.77 ,LandArea=7.86,LandId="衢州国用(2016)第07400  ",ConstructId="衢房权证衢州市字第16157207",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.77   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元603 ",Address=" 衢州市碧桂园小区凤翔苑14幢1单元603 ",ConstructArea=81.28 ,LandArea=7.72,LandId="衢州国用(2016)第06956  ",ConstructId="衢房权证衢州市字第16157362",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.28   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元604 ",Address=" 衢州市碧桂园小区凤翔苑14幢1单元604 ",ConstructArea=111.51,LandArea=10.59,LandId="衢州国用(2016)第06955  ",ConstructId="衢房权证衢州市字第16157363",PropertyNature="国有",LandNature=" 国有",Price= 53,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 111.51  },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元701 ",Address=" 衢州市碧桂园小区凤翔苑14幢1单元701 ",ConstructArea=88.4,LandArea=8.4,LandId="衢州国用(2016)第06810  ",ConstructId="衢房权证衢州市字第16156656",PropertyNature="国有",LandNature=" 国有",Price= 52,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 88.4    },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元702 ",Address=" 衢州市碧桂园小区凤翔苑14幢1单元702 ",ConstructArea=82.77 ,LandArea=7.86,LandId="衢州国用(2016)第06841  ",ConstructId="衢房权证衢州市字第16156812",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.77   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元703 ",Address=" 衢州市碧桂园小区凤翔苑14幢1单元703 ",ConstructArea=81.28 ,LandArea=7.72,LandId="衢州国用(2016)第06680  ",ConstructId="衢房权证衢州市字第16156797",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.28   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元704 ",Address=" 衢州市碧桂园小区凤翔苑14幢1单元704 ",ConstructArea=111.51,LandArea=10.59,LandId="衢州国用(2016)第06821  ",ConstructId="衢房权证衢州市字第16157409",PropertyNature="国有",LandNature=" 国有",Price= 53,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 111.51  },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元801 ",Address=" 衢州市碧桂园小区凤翔苑14幢1单元801 ",ConstructArea=88.4,LandArea=8.4,LandId="衢州国用(2016)第06957  ",ConstructId="衢房权证衢州市字第16157361",PropertyNature="国有",LandNature=" 国有",Price= 42,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 88.4    },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元802 ",Address=" 衢州市碧桂园小区凤翔苑14幢1单元802 ",ConstructArea=82.77 ,LandArea=7.86,LandId="衢州国用(2016)第07116  ",ConstructId="衢房权证衢州市字第16156984",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.77   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元803 ",Address=" 衢州市碧桂园小区凤翔苑14幢1单元803 ",ConstructArea=81.28 ,LandArea=7.72,LandId="衢州国用(2016)第06884  ",ConstructId="衢房权证衢州市字第16156809",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.28   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元804 ",Address=" 衢州市碧桂园小区凤翔苑14幢1单元804 ",ConstructArea=111.51,LandArea=10.59,LandId="衢州国用(2016)第06902  ",ConstructId="衢房权证衢州市字第16156831",PropertyNature="国有",LandNature=" 国有",Price= 53,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 111.51  },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元901 ",Address=" 衢州市碧桂园小区凤翔苑14幢1单元901 ",ConstructArea=88.4,LandArea=8.4,LandId="衢州国用(2016)第06675  ",ConstructId="衢房权证衢州市字第16156899",PropertyNature="国有",LandNature=" 国有",Price= 42,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 88.4    },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元902 ",Address=" 衢州市碧桂园小区凤翔苑14幢1单元902 ",ConstructArea=82.77 ,LandArea=7.86,LandId="衢州国用(2016)第06856  ",ConstructId="衢房权证衢州市字第16156820",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.77   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元903 ",Address=" 衢州市碧桂园小区凤翔苑14幢1单元903 ",ConstructArea=81.28 ,LandArea=7.72,LandId="衢州国用(2016)第06811  ",ConstructId="衢房权证衢州市字第16156685",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.28   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元1002",Address=" 衢州市碧桂园小区凤翔苑14幢1单元1002",ConstructArea=82.77 ,LandArea=7.86,LandId="衢州国用(2016)第07118  ",ConstructId="衢房权证衢州市字第16156986",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.77   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元1003",Address=" 衢州市碧桂园小区凤翔苑14幢1单元1003",ConstructArea=81.28 ,LandArea=7.72,LandId="衢州国用(2016)第06893  ",ConstructId="衢房权证衢州市字第16156834",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.28   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元1102",Address=" 衢州市碧桂园小区凤翔苑14幢1单元1102",ConstructArea=82.77 ,LandArea=7.86,LandId="衢州国用(2016)第06813  ",ConstructId="衢房权证衢州市字第16156717",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.77   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元1103",Address=" 衢州市碧桂园小区凤翔苑14幢1单元1103",ConstructArea=81.28 ,LandArea=7.72,LandId="衢州国用(2016)第06735  ",ConstructId="衢房权证衢州市字第16156730",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.28   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元1202",Address=" 衢州市碧桂园小区凤翔苑14幢1单元1202",ConstructArea=82.77 ,LandArea=7.86,LandId="衢州国用(2016)第06734  ",ConstructId="衢房权证衢州市字第16156733",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.77   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元1203",Address=" 衢州市碧桂园小区凤翔苑14幢1单元1203",ConstructArea=81.28 ,LandArea=7.72,LandId="衢州国用(2016)第06845  ",ConstructId="衢房权证衢州市字第16156811",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.28   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元1301",Address=" 衢州市碧桂园小区凤翔苑14幢1单元1301",ConstructArea=88.4,LandArea=8.4,LandId="衢州国用(2016)第06809  ",ConstructId="衢房权证衢州市字第16156690",PropertyNature="国有",LandNature=" 国有",Price= 42,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 88.4    },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元1302",Address=" 衢州市碧桂园小区凤翔苑14幢1单元1302",ConstructArea=82.77 ,LandArea=7.86,LandId="衢州国用(2016)第06812  ",ConstructId="衢房权证衢州市字第16156681",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.77   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元1303",Address=" 衢州市碧桂园小区凤翔苑14幢1单元1303",ConstructArea=81.28 ,LandArea=7.72,LandId="衢州国用(2016)第07117  ",ConstructId="衢房权证衢州市字第16156985",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.28   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元1402",Address=" 衢州市碧桂园小区凤翔苑14幢1单元1402",ConstructArea=82.77 ,LandArea=7.86,LandId="衢州国用(2016)第06805  ",ConstructId="衢房权证衢州市字第16156655",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.77   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元1403",Address=" 衢州市碧桂园小区凤翔苑14幢1单元1403",ConstructArea=81.28 ,LandArea=7.72,LandId="衢州国用(2016)第06814  ",ConstructId="衢房权证衢州市字第16156657",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.28   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元1502",Address=" 衢州市碧桂园小区凤翔苑14幢1单元1502",ConstructArea=82.77 ,LandArea=7.86,LandId="衢州国用(2016)第06817  ",ConstructId="衢房权证衢州市字第16156668",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.77   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元1503",Address=" 衢州市碧桂园小区凤翔苑14幢1单元1503",ConstructArea=81.28 ,LandArea=7.72,LandId="衢州国用(2016)第06822  ",ConstructId="衢房权证衢州市字第16156680",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.28   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元1602",Address=" 衢州市碧桂园小区凤翔苑14幢1单元1602",ConstructArea=82.77 ,LandArea=7.86,LandId="衢州国用(2016)第06986  ",ConstructId="衢房权证衢州市字第16157437",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.77   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元1603",Address=" 衢州市碧桂园小区凤翔苑14幢1单元1603",ConstructArea=81.28 ,LandArea=7.72,LandId="衢州国用(2016)第06816  ",ConstructId="衢房权证衢州市字第16156667",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.28   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元1702",Address=" 衢州市碧桂园小区凤翔苑14幢1单元1702",ConstructArea=82.77 ,LandArea=7.86,LandId="衢州国用(2016)第06815  ",ConstructId="衢房权证衢州市字第16156664",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.77   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元1703",Address=" 衢州市碧桂园小区凤翔苑14幢1单元1703",ConstructArea=81.28 ,LandArea=7.72,LandId="衢州国用(2016)第06996  ",ConstructId="衢房权证衢州市字第16157429",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.28   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元1802",Address=" 衢州市碧桂园小区凤翔苑14幢1单元1802",ConstructArea=82.77 ,LandArea=7.86,LandId="衢州国用(2016)第06988  ",ConstructId="衢房权证衢州市字第16157423",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.77   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元1803",Address=" 衢州市碧桂园小区凤翔苑14幢1单元1803",ConstructArea=81.28 ,LandArea=7.72,LandId="衢州国用(2016)第06989  ",ConstructId="衢房权证衢州市字第16157424",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.28   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元1902",Address=" 衢州市碧桂园小区凤翔苑14幢1单元1902",ConstructArea=82.77 ,LandArea=7.86,LandId="衢州国用(2016)第06995  ",ConstructId="衢房权证衢州市字第16157428",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.77   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元1903",Address=" 衢州市碧桂园小区凤翔苑14幢1单元1903",ConstructArea=81.28 ,LandArea=7.72,LandId="衢州国用(2016)第06993  ",ConstructId="衢房权证衢州市字第16157427",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.28   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元2002",Address=" 衢州市碧桂园小区凤翔苑14幢1单元2002",ConstructArea=82.77 ,LandArea=7.86,LandId="衢州国用(2016)第06991  ",ConstructId="衢房权证衢州市字第16157426",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.77   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元2003",Address=" 衢州市碧桂园小区凤翔苑14幢1单元2003",ConstructArea=81.28 ,LandArea=7.72,LandId="衢州国用(2016)第06959  ",ConstructId="衢房权证衢州市字第16157367",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.28   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元2101",Address=" 衢州市碧桂园小区凤翔苑14幢1单元2101",ConstructArea=88.4,LandArea=8.4,LandId="衢州国用(2016)第06960  ",ConstructId="衢房权证衢州市字第16157368",PropertyNature="国有",LandNature=" 国有",Price= 42,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 88.4    },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元2102",Address=" 衢州市碧桂园小区凤翔苑14幢1单元2102",ConstructArea=82.77 ,LandArea=7.86,LandId="衢州国用(2016)第06961  ",ConstructId="衢房权证衢州市字第16157370",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.77   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元2103",Address=" 衢州市碧桂园小区凤翔苑14幢1单元2103",ConstructArea=81.28 ,LandArea=7.72,LandId="衢州国用(2016)第06818  ",ConstructId="衢房权证衢州市字第16156819",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.28   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元2202",Address=" 衢州市碧桂园小区凤翔苑14幢1单元2202",ConstructArea=83.09 ,LandArea=7.89,LandId="衢州国用(2016)第06990  ",ConstructId="衢房权证衢州市字第16157425",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 83.09   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元2203",Address=" 衢州市碧桂园小区凤翔苑14幢1单元2203",ConstructArea=81.6,LandArea=7.75,LandId="衢州国用(2016)第06962  ",ConstructId="衢房权证衢州市字第16157371",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.6    },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元2302",Address=" 衢州市碧桂园小区凤翔苑14幢1单元2302",ConstructArea=82.77 ,LandArea=7.86,LandId="衢州国用(2016)第06958  ",ConstructId="衢房权证衢州市字第16157366",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.77   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元2303",Address=" 衢州市碧桂园小区凤翔苑14幢1单元2303",ConstructArea=81.28 ,LandArea=7.72,LandId="衢州国用(2016)第06819  ",ConstructId="衢房权证衢州市字第16156815",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.28   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元2402",Address=" 衢州市碧桂园小区凤翔苑14幢1单元2402",ConstructArea=79.21 ,LandArea=7.53,LandId="衢州国用(2016)第06820  ",ConstructId="衢房权证衢州市字第16156814",PropertyNature="国有",LandNature=" 国有",Price= 38,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 79.21   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑14幢1单元2403",Address=" 衢州市碧桂园小区凤翔苑14幢1单元2403",ConstructArea=77.72 ,LandArea=7.38,LandId="衢州国用(2016)第06966  ",ConstructId="衢房权证衢州市字第16157204",PropertyNature="国有",LandNature=" 国有",Price= 37,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 77.72   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元201 ",Address=" 衢州市碧桂园小区凤翔苑15幢1单元201 ",ConstructArea=89.18 ,LandArea=8.15,LandId="衢州国用(2016)第07115  ",ConstructId="衢房权证衢州市字第16156983",PropertyNature="国有",LandNature=" 国有",Price= 42,GetedDate=Convert.ToDateTime(" 2016.6.3  "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 89.18   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元202 ",Address=" 衢州市碧桂园小区凤翔苑15幢1单元202 ",ConstructArea=83.09 ,LandArea=7.59,LandId="衢州国用(2016)第08326  ",ConstructId="衢房权证衢州市字第16158452",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.6.17 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 83.09   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元203 ",Address=" 衢州市碧桂园小区凤翔苑15幢1单元203 ",ConstructArea=81.61 ,LandArea=7.45,LandId="衢州国用(2016)第06808  ",ConstructId="衢房权证衢州市字第16156695",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.6.17 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.61   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元204 ",Address=" 衢州市碧桂园小区凤翔苑15幢1单元204 ",ConstructArea=112.34,LandArea=10.26,LandId="衢州国用(2016)第07076  ",ConstructId="衢房权证衢州市字第16157417",PropertyNature="国有",LandNature=" 国有",Price= 53,GetedDate=Convert.ToDateTime(" 2016.6.12 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 112.34  },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元301 ",Address=" 衢州市碧桂园小区凤翔苑15幢1单元301 ",ConstructArea=88.41 ,LandArea=8.08,LandId="衢州国用(2016)第06784  ",ConstructId="衢房权证衢州市字第16156897",PropertyNature="国有",LandNature=" 国有",Price= 42,GetedDate=Convert.ToDateTime(" 2016.6.2  "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 88.41   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元302 ",Address=" 衢州市碧桂园小区凤翔苑15幢1单元302 ",ConstructArea=82.78 ,LandArea=7.56,LandId="衢州国用(2016)第07106  ",ConstructId="衢房权证衢州市字第16158593",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元303 ",Address=" 衢州市碧桂园小区凤翔苑15幢1单元303 ",ConstructArea=81.29 ,LandArea=7.43,LandId="衢州国用(2016)第07107  ",ConstructId="衢房权证衢州市字第16156909",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.29   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元304 ",Address=" 衢州市碧桂园小区凤翔苑15幢1单元304 ",ConstructArea=111.52,LandArea=10.19,LandId="衢州国用(2016)第07110  ",ConstructId="衢房权证衢州市字第16156790",PropertyNature="国有",LandNature=" 国有",Price= 53,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 111.52  },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元401 ",Address=" 衢州市碧桂园小区凤翔苑15幢1单元401 ",ConstructArea=88.41 ,LandArea=8.08,LandId="衢州国用(2016)第06807  ",ConstructId="衢房权证衢州市字第16156716",PropertyNature="国有",LandNature=" 国有",Price= 42,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 88.41   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元402 ",Address=" 衢州市碧桂园小区凤翔苑15幢1单元402 ",ConstructArea=82.78 ,LandArea=7.56,LandId="衢州国用(2016)第08389  ",ConstructId="衢房权证衢州市字第16158596",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元403 ",Address=" 衢州市碧桂园小区凤翔苑15幢1单元403 ",ConstructArea=81.29 ,LandArea=7.43,LandId="衢州国用(2016)第07065  ",ConstructId="衢房权证衢州市字第16157372",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.29   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元404 ",Address=" 衢州市碧桂园小区凤翔苑15幢1单元404 ",ConstructArea=111.52,LandArea=10.19,LandId="衢州国用(2016)第07111  ",ConstructId="衢房权证衢州市字第16156789",PropertyNature="国有",LandNature=" 国有",Price= 53,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 111.52  },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元501 ",Address=" 衢州市碧桂园小区凤翔苑15幢1单元501 ",ConstructArea=88.41 ,LandArea=8.08,LandId="衢州国用(2016)第07109  ",ConstructId="衢房权证衢州市字第16156906",PropertyNature="国有",LandNature=" 国有",Price= 42,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 88.41   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元502 ",Address=" 衢州市碧桂园小区凤翔苑15幢1单元502 ",ConstructArea=82.78 ,LandArea=7.56,LandId="衢州国用(2016)第07715  ",ConstructId="衢房权证衢州市字第16158594",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元503 ",Address=" 衢州市碧桂园小区凤翔苑15幢1单元503 ",ConstructArea=81.29 ,LandArea=7.43,LandId="衢州国用(2016)第07097  ",ConstructId="衢房权证衢州市字第16156921",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.29   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元504 ",Address=" 衢州市碧桂园小区凤翔苑15幢1单元504 ",ConstructArea=111.52,LandArea=10.19,LandId="衢州国用(2016)第07100  ",ConstructId="衢房权证衢州市字第16156920",PropertyNature="国有",LandNature=" 国有",Price= 53,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 111.52  },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元601 ",Address=" 衢州市碧桂园小区凤翔苑15幢1单元601 ",ConstructArea=88.41 ,LandArea=8.08,LandId="衢州国用(2016)第07102  ",ConstructId="衢房权证衢州市字第16156916",PropertyNature="国有",LandNature=" 国有",Price= 42,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 88.41   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元602 ",Address=" 衢州市碧桂园小区凤翔苑15幢1单元602 ",ConstructArea=82.78 ,LandArea=7.56,LandId="衢州国用(2016)第08331  ",ConstructId="衢房权证衢州市字第16158449",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元603 ",Address=" 衢州市碧桂园小区凤翔苑15幢1单元603 ",ConstructArea=81.29 ,LandArea=7.43,LandId="衢州国用(2016)第06804  ",ConstructId="衢房权证衢州市字第16156888",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.29   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元604 ",Address=" 衢州市碧桂园小区凤翔苑15幢1单元604 ",ConstructArea=111.52,LandArea=10.19,LandId="衢州国用(2016)第06823  ",ConstructId="衢房权证衢州市字第16157408",PropertyNature="国有",LandNature=" 国有",Price= 53,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 111.52  },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元701 ",Address=" 衢州市碧桂园小区凤翔苑15幢1单元701 ",ConstructArea=88.41 ,LandArea=8.08,LandId="衢州国用(2016)第06824  ",ConstructId="衢房权证衢州市字第16157407",PropertyNature="国有",LandNature=" 国有",Price= 42,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 88.41   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元702 ",Address=" 衢州市碧桂园小区凤翔苑15幢1单元702 ",ConstructArea=82.78 ,LandArea=7.56,LandId="衢州国用(2016)第08329  ",ConstructId="衢房权证衢州市字第16158446",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元703 ",Address=" 衢州市碧桂园小区凤翔苑15幢1单元703 ",ConstructArea=81.29 ,LandArea=7.43,LandId="衢州国用(2016)第07086  ",ConstructId="衢房权证衢州市字第16157443",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.29   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元704 ",Address=" 衢州市碧桂园小区凤翔苑15幢1单元704 ",ConstructArea=111.52,LandArea=10.19,LandId="衢州国用(2016)第07070  ",ConstructId="衢房权证衢州市字第16157413",PropertyNature="国有",LandNature=" 国有",Price= 53,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 111.52  },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元801 ",Address=" 衢州市碧桂园小区凤翔苑15幢1单元801 ",ConstructArea=88.41 ,LandArea=8.08,LandId="衢州国用(2016)第06855  ",ConstructId="衢房权证衢州市字第16156821",PropertyNature="国有",LandNature=" 国有",Price= 42,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 88.41   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元802 ",Address=" 衢州市碧桂园小区凤翔苑15幢1单元802 ",ConstructArea=82.78 ,LandArea=7.56,LandId="衢州国用(2016)第08372  ",ConstructId="衢房权证衢州市字第16158598",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元803 ",Address=" 衢州市碧桂园小区凤翔苑15幢1单元803 ",ConstructArea=81.29 ,LandArea=7.43,LandId="衢州国用(2016)第06854  ",ConstructId="衢房权证衢州市字第16156822",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.29   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元804 ",Address=" 衢州市碧桂园小区凤翔苑15幢1单元804 ",ConstructArea=111.52,LandArea=10.19,LandId="衢州国用(2016)第06886  ",ConstructId="衢房权证衢州市字第16156852",PropertyNature="国有",LandNature=" 国有",Price= 53,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 111.52  },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元901 ",Address=" 衢州市碧桂园小区凤翔苑15幢1单元901 ",ConstructArea=88.41 ,LandArea=8.08,LandId="衢州国用(2016)第07071  ",ConstructId="衢房权证衢州市字第16157412",PropertyNature="国有",LandNature=" 国有",Price= 42,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 88.41   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元902 ",Address=" 衢州市碧桂园小区凤翔苑15幢1单元902 ",ConstructArea=82.78 ,LandArea=7.56,LandId="衢州国用(2016)第08325  ",ConstructId="衢房权证衢州市字第16158453",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元903 ",Address=" 衢州市碧桂园小区凤翔苑15幢1单元903 ",ConstructArea=81.29 ,LandArea=7.43,LandId="衢州国用(2016)第06792  ",ConstructId="衢房权证衢州市字第16156892",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.29   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元904 ",Address=" 衢州市碧桂园小区凤翔苑15幢1单元904 ",ConstructArea=111.52,LandArea=10.19,LandId="衢州国用(2016)第07072  ",ConstructId="衢房权证衢州市字第16157416",PropertyNature="国有",LandNature=" 国有",Price= 53,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 111.52  },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1001",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1001",ConstructArea=88.41 ,LandArea=8.08,LandId="衢州国用(2016)第07069  ",ConstructId="衢房权证衢州市字第16157403",PropertyNature="国有",LandNature=" 国有",Price= 42,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 88.41   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1002",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1002",ConstructArea=82.78 ,LandArea=7.56,LandId="衢州国用(2016)第08374  ",ConstructId="衢房权证衢州市字第16158599",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1003",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1003",ConstructArea=81.29 ,LandArea=7.43,LandId="衢州国用(2016)第06903  ",ConstructId="衢房权证衢州市字第16156850",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.29   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1004",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1004",ConstructArea=111.52,LandArea=10.19,LandId="衢州国用(2016)第07066  ",ConstructId="衢房权证衢州市字第16157405",PropertyNature="国有",LandNature=" 国有",Price= 53,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 111.52  },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1101",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1101",ConstructArea=88.41 ,LandArea=8.08,LandId="衢州国用(2016)第06899  ",ConstructId="衢房权证衢州市字第16156832",PropertyNature="国有",LandNature=" 国有",Price= 42,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 88.41   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1102",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1102",ConstructArea=82.78 ,LandArea=7.56,LandId="衢州国用(2016)第06789  ",ConstructId="衢房权证衢州市字第16158600",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1103",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1103",ConstructArea=81.29 ,LandArea=7.43,LandId="衢州国用(2016)第06786  ",ConstructId="衢房权证衢州市字第16156894",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.29   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1104",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1104",ConstructArea=111.52,LandArea=10.19,LandId="衢州国用(2016)第06909  ",ConstructId="衢房权证衢州市字第16156861",PropertyNature="国有",LandNature=" 国有",Price= 53,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 111.52  },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1201",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1201",ConstructArea=88.41 ,LandArea=8.08,LandId="衢州国用(2016)第07068  ",ConstructId="衢房权证衢州市字第16157404",PropertyNature="国有",LandNature=" 国有",Price= 42,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 88.41   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1202",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1202",ConstructArea=82.78 ,LandArea=7.56,LandId="衢州国用(2016)第08332  ",ConstructId="衢房权证衢州市字第16158450",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1203",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1203",ConstructArea=81.29 ,LandArea=7.43,LandId="衢州国用(2016)第06794  ",ConstructId="衢房权证衢州市字第16156867",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.29   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1204",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1204",ConstructArea=111.52,LandArea=10.19,LandId="衢州国用(2016)第06791  ",ConstructId="衢房权证衢州市字第16156889",PropertyNature="国有",LandNature=" 国有",Price= 53,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 111.52  },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1301",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1301",ConstructArea=88.41 ,LandArea=8.08,LandId="衢州国用(2016)第06797  ",ConstructId="衢房权证衢州市字第16156869",PropertyNature="国有",LandNature=" 国有",Price= 42,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 88.41   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1302",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1302",ConstructArea=82.78 ,LandArea=7.56,LandId="衢州国用(2016)第06795  ",ConstructId="衢房权证衢州市字第16158601",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1303",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1303",ConstructArea=81.29 ,LandArea=7.43,LandId="衢州国用(2016)第06787  ",ConstructId="衢房权证衢州市字第16156895",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.29   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1304",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1304",ConstructArea=111.52,LandArea=10.19,LandId="衢州国用(2016)第06788  ",ConstructId="衢房权证衢州市字第16156868",PropertyNature="国有",LandNature=" 国有",Price= 53,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 111.52  },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1401",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1401",ConstructArea=88.41 ,LandArea=8.08,LandId="衢州国用(2016)第06799  ",ConstructId="衢房权证衢州市字第16156877",PropertyNature="国有",LandNature=" 国有",Price= 42,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 88.41   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1402",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1402",ConstructArea=82.78 ,LandArea=7.56,LandId="衢州国用(2016)第08376  ",ConstructId="衢房权证衢州市字第16158602",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1403",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1403",ConstructArea=81.29 ,LandArea=7.43,LandId="衢州国用(2016)第06798  ",ConstructId="衢房权证衢州市字第16156870",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.29   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1404",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1404",ConstructArea=111.52,LandArea=10.19,LandId="衢州国用(2016)第06785  ",ConstructId="衢房权证衢州市字第16156896",PropertyNature="国有",LandNature=" 国有",Price= 53,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 111.52  },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1501",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1501",ConstructArea=88.41 ,LandArea=8.08,LandId="衢州国用(2016)第06891  ",ConstructId="衢房权证衢州市字第16156807",PropertyNature="国有",LandNature=" 国有",Price= 42,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 88.41   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1502",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1502",ConstructArea=82.78 ,LandArea=7.56,LandId="衢州国用(2016)第06790  ",ConstructId="衢房权证衢州市字第16158603",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1503",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1503",ConstructArea=81.29 ,LandArea=7.43,LandId="衢州国用(2016)第06793  ",ConstructId="衢房权证衢州市字第16156865",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.29   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1504",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1504",ConstructArea=111.52,LandArea=10.19,LandId="衢州国用(2016)第07113  ",ConstructId="衢房权证衢州市字第16156785",PropertyNature="国有",LandNature=" 国有",Price= 53,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 111.52  },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1601",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1601",ConstructArea=88.41 ,LandArea=8.08,LandId="衢州国用(2016)第06908  ",ConstructId="衢房权证衢州市字第16156858",PropertyNature="国有",LandNature=" 国有",Price= 42,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 88.41   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1602",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1602",ConstructArea=82.78 ,LandArea=7.56,LandId="衢州国用(2016)第08377  ",ConstructId="衢房权证衢州市字第16158604",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1603",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1603",ConstructArea=81.29 ,LandArea=7.43,LandId="衢州国用(2016)第06906  ",ConstructId="衢房权证衢州市字第16156857",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.29   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1604",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1604",ConstructArea=111.52,LandArea=10.19,LandId="衢州国用(2016)第06900  ",ConstructId="衢房权证衢州市字第16156854",PropertyNature="国有",LandNature=" 国有",Price= 53,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 111.52  },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1701",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1701",ConstructArea=88.41 ,LandArea=8.08,LandId="衢州国用(2016)第06901  ",ConstructId="衢房权证衢州市字第16156855",PropertyNature="国有",LandNature=" 国有",Price= 42,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 88.41   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1702",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1702",ConstructArea=82.78 ,LandArea=7.56,LandId="衢州国用(2016)第08373  ",ConstructId="衢房权证衢州市字第16158605",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1703",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1703",ConstructArea=81.29 ,LandArea=7.43,LandId="衢州国用(2016)第07149  ",ConstructId="衢房权证衢州市字第16157000",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.29   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1704",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1704",ConstructArea=111.52,LandArea=10.19,LandId="衢州国用(2016)第06904  ",ConstructId="衢房权证衢州市字第16156856",PropertyNature="国有",LandNature=" 国有",Price= 52,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 111.52  },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1801",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1801",ConstructArea=88.41 ,LandArea=8.08,LandId="衢州国用(2016)第07119  ",ConstructId="衢房权证衢州市字第16156784",PropertyNature="国有",LandNature=" 国有",Price= 42,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 88.41   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1802",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1802",ConstructArea=82.78 ,LandArea=7.56,LandId="衢州国用(2016)第08388  ",ConstructId="衢房权证衢州市字第16158595",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1803",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1803",ConstructArea=81.29 ,LandArea=7.43,LandId="衢州国用(2016)第06910  ",ConstructId="衢房权证衢州市字第16156862",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.29   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1804",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1804",ConstructArea=111.52,LandArea=10.19,LandId="衢州国用(2016)第06890  ",ConstructId="衢房权证衢州市字第16156828",PropertyNature="国有",LandNature=" 国有",Price= 53,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 111.52  },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1901",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1901",ConstructArea=88.41 ,LandArea=8.08,LandId="衢州国用(2016)第06895  ",ConstructId="衢房权证衢州市字第16156827",PropertyNature="国有",LandNature=" 国有",Price= 42,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 88.41   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1902",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1902",ConstructArea=82.78 ,LandArea=7.56,LandId="衢州国用(2016)第08385  ",ConstructId="衢房权证衢州市字第16158606",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1903",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1903",ConstructArea=81.29 ,LandArea=7.43,LandId="衢州国用(2016)第06802  ",ConstructId="衢房权证衢州市字第16156885",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.29   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元1904",Address=" 衢州市碧桂园小区凤翔苑15幢1单元1904",ConstructArea=111.52,LandArea=10.19,LandId="衢州国用(2016)第06825  ",ConstructId="衢房权证衢州市字第16157406",PropertyNature="国有",LandNature=" 国有",Price= 53,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 111.52  },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元2001",Address=" 衢州市碧桂园小区凤翔苑15幢1单元2001",ConstructArea=88.41 ,LandArea=8.08,LandId="衢州国用(2016)第06827  ",ConstructId="衢房权证衢州市字第16157410",PropertyNature="国有",LandNature=" 国有",Price= 42,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 88.41   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元2002",Address=" 衢州市碧桂园小区凤翔苑15幢1单元2002",ConstructArea=82.78 ,LandArea=7.56,LandId="衢州国用(2016)第08390  ",ConstructId="衢房权证衢州市字第16158607",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元2003",Address=" 衢州市碧桂园小区凤翔苑15幢1单元2003",ConstructArea=81.29 ,LandArea=7.43,LandId="衢州国用(2016)第06826  ",ConstructId="衢房权证衢州市字第16157411",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.29   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元2004",Address=" 衢州市碧桂园小区凤翔苑15幢1单元2004",ConstructArea=111.52,LandArea=10.19,LandId="衢州国用(2016)第06800  ",ConstructId="衢房权证衢州市字第16156880",PropertyNature="国有",LandNature=" 国有",Price= 53,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 111.52  },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元2101",Address=" 衢州市碧桂园小区凤翔苑15幢1单元2101",ConstructArea=88.41 ,LandArea=8.08,LandId="衢州国用(2016)第06828  ",ConstructId="衢房权证衢州市字第16157358",PropertyNature="国有",LandNature=" 国有",Price= 42,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 88.41   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元2102",Address=" 衢州市碧桂园小区凤翔苑15幢1单元2102",ConstructArea=82.78 ,LandArea=7.56,LandId="衢州国用(2016)第08330  ",ConstructId="衢房权证衢州市字第16158448",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元2103",Address=" 衢州市碧桂园小区凤翔苑15幢1单元2103",ConstructArea=81.29 ,LandArea=7.43,LandId="衢州国用(2016)第07085  ",ConstructId="衢房权证衢州市字第16157442",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.29   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元2104",Address=" 衢州市碧桂园小区凤翔苑15幢1单元2104",ConstructArea=111.52,LandArea=10.19,LandId="衢州国用(2016)第07089  ",ConstructId="衢房权证衢州市字第16157441",PropertyNature="国有",LandNature=" 国有",Price= 53,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 111.52  },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元2201",Address=" 衢州市碧桂园小区凤翔苑15幢1单元2201",ConstructArea=89.18 ,LandArea=8.15,LandId="衢州国用(2016)第07087  ",ConstructId="衢房权证衢州市字第16157458",PropertyNature="国有",LandNature=" 国有",Price= 42,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 89.18   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元2202",Address=" 衢州市碧桂园小区凤翔苑15幢1单元2202",ConstructArea=83.09 ,LandArea=7.59,LandId="衢州国用(2016)第08328  ",ConstructId="衢房权证衢州市字第16158445",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 83.09   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元2203",Address=" 衢州市碧桂园小区凤翔苑15幢1单元2203",ConstructArea=81.61 ,LandArea=7.45,LandId="衢州国用(2016)第06635  ",ConstructId="衢房权证衢州市字第16156898",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.61   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元2204",Address=" 衢州市碧桂园小区凤翔苑15幢1单元2204",ConstructArea=112.34,LandArea=10.26,LandId="衢州国用(2016)第07075  ",ConstructId="衢房权证衢州市字第16157414",PropertyNature="国有",LandNature=" 国有",Price= 53,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 112.34  },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元2301",Address=" 衢州市碧桂园小区凤翔苑15幢1单元2301",ConstructArea=88.41 ,LandArea=8.08,LandId="衢州国用(2016)第07152  ",ConstructId="衢房权证衢州市字第16156982",PropertyNature="国有",LandNature=" 国有",Price= 42,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 88.41   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元2302",Address=" 衢州市碧桂园小区凤翔苑15幢1单元2302",ConstructArea=82.78 ,LandArea=7.56,LandId="衢州国用(2016)第08327  ",ConstructId="衢房权证衢州市字第16158451",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 82.78   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元2303",Address=" 衢州市碧桂园小区凤翔苑15幢1单元2303",ConstructArea=81.29 ,LandArea=7.43,LandId="衢州国用(2016)第07120  ",ConstructId="衢房权证衢州市字第16156987",PropertyNature="国有",LandNature=" 国有",Price= 39,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 81.29   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元2304",Address=" 衢州市碧桂园小区凤翔苑15幢1单元2304",ConstructArea=111.52,LandArea=10.19,LandId="衢州国用(2016)第06732  ",ConstructId="衢房权证衢州市字第16156735",PropertyNature="国有",LandNature=" 国有",Price= 53,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 111.52  },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元2401",Address=" 衢州市碧桂园小区凤翔苑15幢1单元2401",ConstructArea=100.25,LandArea=9.16,LandId="衢州国用(2016)第07090  ",ConstructId="衢房权证衢州市字第16157440",PropertyNature="国有",LandNature=" 国有",Price= 48,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 100.25  },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元2402",Address=" 衢州市碧桂园小区凤翔苑15幢1单元2402",ConstructArea=79.22 ,LandArea=7.24,LandId="衢州国用(2016)第06896  ",ConstructId="衢房权证衢州市字第16158597",PropertyNature="国有",LandNature=" 国有",Price= 38,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 79.22   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元2403",Address=" 衢州市碧桂园小区凤翔苑15幢1单元2403",ConstructArea=77.73 ,LandArea=7.1,LandId="衢州国用(2016)第06897  ",ConstructId="衢房权证衢州市字第16156853",PropertyNature="国有",LandNature=" 国有",Price=  39.00,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 77.73   },
new Property {PropertyType= PropertyType.House, Name="衢州市碧桂园小区凤翔苑15幢1单元2404",Address=" 衢州市碧桂园小区凤翔苑15幢1单元2404",ConstructArea=123.7 ,LandArea=11.3,LandId="衢州国用(2016)第06888  ",ConstructId="衢房权证衢州市字第16156833",PropertyNature="国有",LandNature=" 国有",Price=  59.00,GetedDate=Convert.ToDateTime(" 2016.5.31 "),LifeTime= 70,UsedPeople=" 汇盛",CurrentUse_Self= 123.7   },
                #endregion
            };
            var targetProperties = _propertyService.GetPropertiesByGovernmentId(new List<int>() { 155 });
            foreach (var property in properties)
            {
                try
                {
                    var pp = targetProperties.Where(p => p.Address == property.Address);



                    if (pp.Count() == 1)
                    {
                        var tp = pp.FirstOrDefault();

                        if (tp.Name == property.Name
                            && tp.Address == property.Address
                            //&& tp.ConstructArea == property.ConstructArea
                            //&& tp.LandArea == property.LandArea
                            //&& tp.ConstructId == property.ConstructId
                            //&& tp.LandId == property.LandId
                            //&& tp.LandNature == property.LandNature
                            //&& tp.PropertyNature == property.PropertyNature
                            && tp.Price == property.Price
                            && tp.LifeTime == property.LifeTime
                            && tp.UsedPeople == property.UsedPeople
                            //&& tp.CurrentUse_Self == property.CurrentUse_Self
                            //&& tp.GetedDate == property.GetedDate
                            )
                        {

                        }
                        else
                        {
                            sb.AppendLine(tp.Name);
                        }
                    }
                    else
                    {
                        sb.AppendLine(property.Address);
                    }
                }
                catch (Exception e)
                {
                    sb.AppendLine(e.Message);
                }

            }

            return Ok(sb.ToString());
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

            public string FileId { get; set; }

            public int GovernmentId { get; set; }
        }
    }
}