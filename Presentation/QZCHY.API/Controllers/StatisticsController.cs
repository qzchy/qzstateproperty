using QZCHY.API.Models.Statistics;
using QZCHY.Core;
using QZCHY.Core.Domain.Properties;
using QZCHY.Services.AccountUsers;
using QZCHY.Services.Logging;
using QZCHY.Services.Property;
using QZCHY.Web.Framework.Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;

namespace QZCHY.API.Controllers
{
    [RoutePrefix("Statistics")]
    public class StatisticsController : BaseAdminApiController
    {
        private readonly IPropertyService _propertyService;
        private readonly IGovernmentService _governmentService;
        private readonly IAccountUserService _accountUserService;
        private readonly IAccountUserActivityService _accountUserActivityService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;

        public StatisticsController(
            IPropertyService propertyService,
            IAccountUserService accountUserService,
        IGovernmentService governmentService,
            IAccountUserActivityService accountUserActivityService,
             IWebHelper webHelper,
            IWorkContext workContext)
        {
            _propertyService = propertyService;
            _accountUserService = accountUserService;
            _accountUserActivityService = accountUserActivityService;
            _governmentService = governmentService;

            _webHelper = webHelper;
            _workContext = workContext;
        }

        [Route("Ovewview")]
        [HttpGet]
        public IHttpActionResult OverviewStatistics(bool showSelf = false, bool includeChildren = true)
        {
            var currentUser = _workContext.CurrentAccountUser;

            var properties = _propertyService.GetAllProperties(currentUser.IsAdmin() && !showSelf ? 0 : currentUser.Government.Id,false);
            var constructProperties = properties.Where(p => p.PropertyType == PropertyType.House);
            var constructLandProperties = properties.Where(p => p.PropertyType == PropertyType.LandUnderHouse);
            var landProperties = properties.Where(p => p.PropertyType == PropertyType.Land);

            var response = new OverviewStatistics();
            response.TotalCount = properties.Count();
            response.LandCount = landProperties.Count();
            response.ConstructCount = response.TotalCount - response.LandCount;

            response.TotalPrice = properties.Select(p => p.Price).Sum();
            response.LandPrice = response.LandCount > 0 ? landProperties.Select(p => p.Price).Sum() : 0;
            response.ConstructPrice = response.TotalPrice - response.LandPrice;

            response.ConstructArea = constructProperties.Count() > 0 ? constructProperties.Select(p => p.ConstructArea).Sum() : 0;

            response.ConstrcutLandArea = constructProperties.Count() > 0 ? constructProperties.Select(p => p.LandArea).Sum() : 0;
            response.ConstrcutLandArea += constructLandProperties.Count() > 0 ? constructLandProperties.Select(p => p.LandArea).Sum() : 0;

            response.LandArea = response.LandCount > 0 ? landProperties.Select(p => p.LandArea).Sum() : 0;
            return Ok(response);
        }

        /// <summary>
        /// 按区域、单位性质进行柱状堆叠图
        /// </summary>
        /// <param name="showSelf">是否查询全部资产</param>
        /// <param name="includeChildren">是否包括下级单位资产</param>
        /// <param name="propertyType">资产类别</param>
        /// <param name="statistics">统计字段，默认统计数量</param>
        /// <returns></returns>
        [Route("Group")]
        [HttpGet]
        public IHttpActionResult GroupStatistics(bool showSelf = false, bool includeChildren = true, int propertyType = 0, string statistics = "Count")
        {
            var response = new List<ArrayList>();

            var currentUser = _workContext.CurrentAccountUser;
            var properties = _propertyService.GetAllProperties(currentUser.IsAdmin() && !showSelf ? 0 : currentUser.Government.Id,false);
            //var c1 = properties.Count();
            var statisticsProperties = propertyType == 0 ? properties.Where(p => p.PropertyType != PropertyType.Land) :
                properties.Where(p => p.PropertyType == PropertyType.Land);
            //var c2 = statisticsProperties.Count();
            //var governTypes= enum
            foreach (GovernmentType gType in System.Enum.GetValues(typeof(GovernmentType)))
            {
                ArrayList array = new ArrayList();

                var governmentTypeStackProperties = statisticsProperties.Where(p => p.Government.GovernmentType == gType);
                //var c3 = governmentTypeStackProperties.Count();
                foreach (Region region in System.Enum.GetValues(typeof(Region)))
                {
                    var governmentTypeStackRegionProperties = governmentTypeStackProperties.Where(p => p.Region == region);
                    //var c4 = governmentTypeStackRegionProperties.Count();
                    var propertyObjectType = typeof(Property);
                  
                    if (statistics == "Count") array.Add(governmentTypeStackRegionProperties.Count());
                    else
                    {
                        if (propertyObjectType.GetProperty(statistics) == null ||
                          !(propertyObjectType.GetProperty(statistics).PropertyType.Equals(typeof(int)) ||
                          propertyObjectType.GetProperty(statistics).PropertyType.Equals(typeof(float)) ||
                          propertyObjectType.GetProperty(statistics).PropertyType.Equals(typeof(double)) ||
                          propertyObjectType.GetProperty(statistics).PropertyType.Equals(typeof(decimal))))

                            throw new QZCHYException("统计字段无效");


                        switch (statistics.ToLower())
                        {
                            case "constructarea":
                                array.Add(governmentTypeStackRegionProperties.Count() != 0 ?
                                    governmentTypeStackRegionProperties.Select(p => p.ConstructArea).Sum() : 0);
                                break;
                            case "landarea":
                                array.Add(governmentTypeStackRegionProperties.Count() != 0 ?
                                    governmentTypeStackRegionProperties.Select(p => p.LandArea).Sum() : 0);
                                break;
                            case "price":
                                array.Add(governmentTypeStackRegionProperties.Count() != 0 ?
                                    governmentTypeStackRegionProperties.Select(p => p.Price).Sum() : 0);
                                break;
                            default:
                                throw new QZCHYException("统计字段无效");
                        }
                    }
                }

                response.Add(array);
            }

            return Ok(response);
        }

        /// <summary>
        /// 当前使用统计
        /// </summary>
        /// <param name="showSelf"></param>
        /// <param name="includeChildren"></param>
        /// <param name="land"></param>
        /// <returns></returns>
        [Route("CurrentUsage")]
        [HttpGet]
        public IHttpActionResult CurrentUsageStatistics(bool showSelf = false, bool includeChildren = true)
        {
            var response = new List<dynamic>();

            var currentUser = _workContext.CurrentAccountUser;
            var properties = _propertyService.GetAllProperties(currentUser.IsAdmin() && !showSelf ? 0 : currentUser.Government.Id, false);

            var constructProperties = properties.Where(p => p.PropertyType != PropertyType.Land);
            var landProperties = properties.Where(p => p.PropertyType == PropertyType.Land);

            IList<PieChartItemlModel> list = new List<PieChartItemlModel>();           
            PieChartItemlModel pieDataItem = new PieChartItemlModel();  //饼图统计类型

            pieDataItem = new PieChartItemlModel();
            pieDataItem.Name = "自用";
            pieDataItem.Value = constructProperties.Count() == 0 ? 0 : constructProperties.Select(p => p.CurrentUse_Self).Sum();
            list.Add(pieDataItem);

            pieDataItem = new PieChartItemlModel();
            pieDataItem.Name = "出借";
            pieDataItem.Value = constructProperties.Count() == 0 ? 0 : constructProperties.Select(p => p.CurrentUse_Lend).Sum();
            list.Add(pieDataItem);

            pieDataItem = new PieChartItemlModel();
            pieDataItem.Name = "出租";
            pieDataItem.Value = constructProperties.Count() == 0 ? 0 : constructProperties.Select(p => p.CurrentUse_Rent).Sum();
            list.Add(pieDataItem);

            pieDataItem = new PieChartItemlModel();
            pieDataItem.Name = "闲置";
            pieDataItem.Value = constructProperties.Count() == 0 ? 0 : constructProperties.Select(p => p.CurrentUse_Idle).Sum();
            list.Add(pieDataItem);

            response.Add(new PieChartModel() { Data = list });

            list = new List<PieChartItemlModel>();
            pieDataItem = new PieChartItemlModel();  //饼图统计类型

            pieDataItem = new PieChartItemlModel();
            pieDataItem.Name = "自用";
            pieDataItem.Value = landProperties.Count() == 0 ? 0 : landProperties.Select(p => p.CurrentUse_Self).Sum();
            list.Add(pieDataItem);

            pieDataItem = new PieChartItemlModel();
            pieDataItem.Name = "出借";
            pieDataItem.Value = landProperties.Count() == 0 ? 0 : landProperties.Select(p => p.CurrentUse_Lend).Sum();
            list.Add(pieDataItem);

            pieDataItem = new PieChartItemlModel();
            pieDataItem.Name = "出租";
            pieDataItem.Value = landProperties.Count() == 0 ? 0 : landProperties.Select(p => p.CurrentUse_Rent).Sum();
            list.Add(pieDataItem);

            pieDataItem = new PieChartItemlModel();
            pieDataItem.Name = "闲置";
            pieDataItem.Value = landProperties.Count() == 0 ? 0 : landProperties.Select(p => p.CurrentUse_Idle).Sum();
            list.Add(pieDataItem);

            response.Add(new PieChartModel() { Data = list });

            return Ok(response);
        }

    }
}
