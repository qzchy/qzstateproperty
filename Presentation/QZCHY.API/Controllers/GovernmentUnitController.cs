using QZCHY.API.Models.Properties;
using QZCHY.Core;
using QZCHY.Core.Domain.Properties;
using QZCHY.Services.AccountUsers;
using QZCHY.Services.Logging;
using QZCHY.Services.Property;
using QZCHY.Web.Api.Extensions;
using QZCHY.Web.Framework.Controllers;
using QZCHY.Web.Framework.Response;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;

namespace QZCHY.API.Controllers
{
    [RoutePrefix("Systemmanage/Governments")]
    public class GovernmentUnitController : BaseAdminApiController
    {
        private readonly IGovernmentService _governmentService;
        private readonly IAccountUserService _accountUserService;
        private readonly IAccountUserActivityService _accountUserActivityService;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;

        public GovernmentUnitController(
            IAccountUserService accountUserService,
        IGovernmentService governmentService,
            IAccountUserActivityService accountUserActivityService,
             IWebHelper webHelper,
            IWorkContext workContext)

        {
            _accountUserService = accountUserService;
            _accountUserActivityService = accountUserActivityService;
            _governmentService = governmentService;

            _webHelper = webHelper;
            _workContext = workContext;
        }

        /// <summary>
        /// 获取子类别生成目录树
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [NonAction]
        private IList<TreeList<GovernmentUnitModel>> LoadChildrenCategories(int parentId)
        {

            var treeResponse = new List<TreeList<GovernmentUnitModel>>();

            //获取所有一级节点
            var governments = _governmentService.GetAllGovernmentsByParentGovernmentId(parentId, false);

            foreach (var government in governments)
            {
                var obj = new TreeList<GovernmentUnitModel>()
                {
                    Label = government.Name ,
                    Children = LoadChildrenCategories(government.Id),
                    Data = government.ToModel()
                };

                treeResponse.Add(obj);
            }

            return treeResponse;
        }

        /// <summary>
        /// 根据目录从属关系生成select子项
        /// </summary>
        /// <param name="list"></param>
        /// <param name="governments"></param>
        /// <param name="seperator"></param>
        /// <param name="breadCrumb"></param>
        /// <param name="showHidden"></param>
        [NonAction]
        private void GetFormattedBreadCrumbSelectListItems(IList<SelectListItem> list, IList<GovernmentUnit> governments, string seperator, string breadCrumb = "", bool showHidden = false)
        {
            foreach (var government in governments)
            {
                list.Add(new SelectListItem()
                {
                    Label = government.Name,
                    Value = government.Id
                });

                var subCategories = _governmentService.GetAllGovernmentsByParentGovernmentId(government.Id);
                string tempBreadCrumbText = string.IsNullOrEmpty(breadCrumb) ? government.Name : string.Format("{0} {1} {2}", breadCrumb, seperator, government.Name);

                if (subCategories.Count > 0)
                    GetFormattedBreadCrumbSelectListItems(list, subCategories, seperator, tempBreadCrumbText, showHidden);
                else
                {
                    var selectItem = new SelectListItem()
                    {
                        Label = tempBreadCrumbText,
                        Value = government.Id
                    };
                    list.Add(selectItem);
                }
            }
        }

        /// <summary>
        /// 生成下拉选择集合
        /// </summary>
        /// <param name="showHidden"></param>
        /// <returns></returns>
        [Route("SelectList")]
        public IHttpActionResult GetGovernmentList()
        {
            var response = new List<SelectListItem>();

            //获取根节点
            var governments = _governmentService.GetAllGovernmentsByParentGovernmentId(0);
            //GetFormattedBreadCrumbSelectListItems(response, rootCategories, ">>", string.Empty);

            foreach(var government in governments)
            {
                var selectItem = new SelectListItem()
                {
                    Label = government.Name,
                    Value = government.Id
                };
                response.Add(selectItem);
            }

            return Ok(response);
        }

        [Route("TreeView")]
        public IHttpActionResult GetGovernmentTreeView()
        {
            var response = LoadChildrenCategories(0);

            return Ok(response);
        }

        [HttpGet]
        [Route("Autocomplete/{governmentName}")]
        public IHttpActionResult AutocompleteByName(string governmentName)
        {
            var governments = _governmentService.GetAllGovernmentUnits(governmentName, 0, 15, true);

            var response = new ListResponse<SimpleGovernmentUnitModel>();
            response.Data = governments.Select(g => {
                return new SimpleGovernmentUnitModel
                {
                    Id = g.Id,
                    Name = g.Name
                };
            });

           
            //activity log
            _accountUserActivityService.InsertActivity("AutocompleteByName", "查询 名称包含 {0} 的单位", governmentName);

            return Ok(response);
        }

        /// <summary>
        /// 类别名称唯一性检查，商铺类别、商铺类自定义类别内要求唯一        
        /// </summary>
        /// <param name="name"></param>
        /// <param name="storeId"></param>
        /// <returns>返回为true，表示不可用</returns>
        [HttpGet]
        [Route("Unique/{name}")]
        public IHttpActionResult UniqueCheck(string name, int storeId = 0)
        {
            var result = !_governmentService.NameUniqueCheck(name);

            return Ok(result);
        }
        [HttpGet]
        [Route("CurrentUser")]
        public IHttpActionResult GetCurrentGoverment()
        {
            var currentGoverment = _workContext.CurrentAccountUser.Government.ToModel();
            return Ok(currentGoverment);
        }
        [HttpGet]
        [Route("{governmentId}")]
        public IHttpActionResult Get(int governmentId)
        {
            var government = _governmentService.GetGovernmentUnitById(governmentId);
            if (government == null || government.Deleted)
                return NotFound();

            var model = government.ToModel();

            var parentGovernemnt = _governmentService.GetGovernmentUnitById(model.ParentGovernmentId);
            if (parentGovernemnt != null) model.ParentGovernment = parentGovernemnt.Name;

            var childrenGovernments = _governmentService.GetAllGovernmentsByParentGovernmentId(model.Id);
            if(childrenGovernments!=null)
            {
                model.ChildrenGorvernments = childrenGovernments.Select(g =>
                {
                    return new SimpleGovernmentUnitModel
                    {
                        Id = g.Id,
                        Name = g.Name
                    };
                }).ToList();
            }


            //activity log
            _accountUserActivityService.InsertActivity("GetGovernemntInfo", "获取 名为 {0} 的单位信息", government.Name);

            return Ok(model);
        }



        [HttpGet]
        [Route("children/{governmentId}")]
        public IHttpActionResult GetChildren(int governmentId)
        {
            var governments = _governmentService.GetAllGovernmentsByParentGovernmentId(governmentId);
            var response = new ListResponse<SimpleGovernmentUnitModel>();
            response.Data = governments.Select(g => {
                return new SimpleGovernmentUnitModel
                {
                    Id = g.Id,
                    Name = g.Name
                };
            });

            //activity log
            _accountUserActivityService.InsertActivity("GetChildrenGovernments", "获取 id为 {0} 的单位的下级单位", governmentId);

            return Ok(response);
        }

        [HttpGet]
        [Route("currentAccount/children/SelectList")]
        public IHttpActionResult GetCurrentAccountChidlrenGovernemnts()
        {
            var currentAccount = _workContext.CurrentAccountUser;

            var response = new List<SelectListItem>();

            //获取根节点
            var governmentId = currentAccount.Government.Id;

            var governments = _governmentService.GetAllGovernmentsByParentGovernmentId(governmentId);
           
            foreach (var government in governments)
            {
                var selectItem = new SelectListItem()
                {
                    Label = government.Name,
                    Value = government.Id
                };
                response.Add(selectItem);
            }

            return Ok(response);
        }

        /// <summary>
        /// 创建对象
        /// </summary>
        /// <param name="governmentUnitModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public IHttpActionResult Create(GovernmentUnitModel governmentUnitModel)
        {
            var government = governmentUnitModel.ToEntity();

            if (ModelState.IsValid)
            {
                _governmentService.InsertGovernmentUnit(government);

                //activity log
                _accountUserActivityService.InsertActivity("AddNewGovernment", "新增名为 {0} 的单位", government.Name);

                //SuccessNotification(_localizationService.GetResource("Admin.Catalog.Categories.Added"));

                return Ok(government.ToModel());
            }
            else return BadRequest("类别模型数据错误");
        }

        [HttpPut]
        [Route("{id}")]
        public IHttpActionResult Update(int id, GovernmentUnitModel governmentUnitModel)
        {

            var government = _governmentService.GetGovernmentUnitById(id);
            if (government == null || government.Deleted) return NotFound();

            government = governmentUnitModel.ToEntity(government);

            _governmentService.UpdateGovernmentUnit(government);
            _accountUserActivityService.InsertActivity("UpdateGovernment", "更新名为 {0} 的单位", government.Name);
            return Ok(government.ToModel());
        }

        /// <summary>
        /// 删除类别，单个删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {

            var government = _governmentService.GetGovernmentUnitById(id);
            if (government == null) return NotFound();

            _governmentService.DeleteGovernmentUnit(government);

            //活动日志
            _accountUserActivityService.InsertActivity("DeleteGovernment", "删除名为 {0} 的单位，及其所属单位", government.Name);

            _accountUserService.DeleteGovernmentUsers(government);

            //活动日志
            _accountUserActivityService.InsertActivity("DeleteGovernmentAccountUsers", "删除名为 {0} 的单位下的所有账户", government.Name);

            //通知
            //SuccessNotification(_localizationService.GetResource("Admin.Catalog.Categories.Deleted"));

            return Ok();
        }


        /// <summary>
        /// 地图中的搜索
        /// </summary>
        /// <param name="query"></param>
        /// <param name="sort"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="showHidden"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("geo")]
        public IHttpActionResult GetAllInMap(PropertyAdvanceConditionModel advance)
        {
            //初始化排序条件
            var sortConditions = PropertySortCondition.Instance(advance.Sort);

            var governmentTypes = new List<int>();
            if (advance.Government.IsCompany) governmentTypes.Add((int)GovernmentType.Government);
            if (advance.Government.IsInstitution) governmentTypes.Add((int)GovernmentType.Institution);
            if (advance.Government.IsCompany) governmentTypes.Add((int)GovernmentType.Company);

            if (governmentTypes.Count == 3) governmentTypes = new List<int>();

            //高级搜索参数设置

            var governments = _governmentService.GetAllGovernmentUnits(governmentTypes,advance.Query, 0, int.MaxValue, true, sortConditions);

            var response = new ListResponse<GeoGorvernmentModel>
            {
                Time = advance.Time,
                Paging = new Paging
                {
                    PageIndex = 0,
                    PageSize = int.MaxValue,
                    Total = governments.TotalCount,
                    FilterCount = string.IsNullOrEmpty(advance.Query) ? governments.TotalCount : governments.Count,
                },
                Data = governments.Select(s =>
                {
                    var geoGovernment = s.ToGeoModel();
                    return geoGovernment;
                })
            };

            //activity log
            _accountUserActivityService.InsertActivity("GetpropertyList", "获取单位列表信息");

            return Ok(response);
        }

        /// <summary>
        /// 获取地图大数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("geo/bigdata")]
        public IHttpActionResult GetMapBigData()
        {
            var governments = _governmentService.GetAllGovernmentUnits();

            var mapGovernments = governments.Select(p => {
                return p.ToGeoModel();
            });

            //activity log
            _accountUserActivityService.InsertActivity("GetGeopropertyList", "地图获取单位列表信息");

            return Ok(mapGovernments);
        }

        /// <summary>
        /// 搜索联想提示
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pageSize"></param>
        /// <param name="showHidden"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("geo/suggestion")]
        public IHttpActionResult Suggestion(string query = "", long time = 0, int pageSize = 10, bool showHidden = true)
        {

            var properties = _governmentService.GetAllGovernmentUnits(query, 0, pageSize, showHidden);

            var response = new ListResponse<GeoGorvernmentModel>
            {
                Time = time,
                Paging = new Paging
                {
                    PageIndex = 0,
                    PageSize = int.MaxValue,
                    Total = properties.TotalCount,
                    FilterCount = properties.TotalCount,
                },
                Data = properties.Select(s =>
                {
                    return s.ToGeoModel();
                })
            };

            //activity log
            _accountUserActivityService.InsertActivity("GovernmentSuggestion", "单位信息关键字联想");

            return Ok(response);
        }

    }
}
