using QZCHY.Core;
using QZCHY.Core.Caching;
using QZCHY.Core.Data;
using QZCHY.Core.Domain.Properties;
using QZCHY.Data;
using QZCHY.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QZCHY.Services.Property
{
    public class GovernmentService:IGovernmentService
    {
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : category ID
        /// </remarks>
        private const string GOVERNMENTUNITS_BY_ID_KEY = "QZCHY.government.id-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : parent category ID
        /// {1} : show hidden records?
        /// </remarks>
        private const string GOVERNMENTUNITS_BY_PARENT_GOVERNMENTUNIT_ID_KEY = "QZCHY.government.byparent-{0}-{1}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string GOVERNMENTUNITS_PATTERN_KEY = "QZCHY.government.";

        private readonly IRepository<GovernmentUnit> _governmentUnitRepository;       
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;

        public GovernmentService(ICacheManager cacheManager, IRepository<GovernmentUnit> governmentUnitRepository,
             IEventPublisher eventPublisher)
        {

            this._cacheManager = cacheManager;             
            _governmentUnitRepository = governmentUnitRepository;

            this._eventPublisher = eventPublisher;
        }

        /// <summary>
        /// 删除店铺
        /// </summary>
        /// <param name="governmentUnit"></param>
        public void DeleteGovernmentUnit(GovernmentUnit governmentUnit)
        {
            if (governmentUnit == null)
                throw new ArgumentNullException("governmentUnit is null");

            governmentUnit.Deleted = true;
            UpdateGovernmentUnit(governmentUnit);

            //父节点单位删除或，子节点单位一并删除
            var subGovernments = GetAllGovernmentsByParentGovernmentId(governmentUnit.Id);
            foreach (var subGovernment in subGovernments)
            {
                subGovernment.Deleted = true;
                UpdateGovernmentUnit(subGovernment);
            }
        }

        public IList<GovernmentUnit> GetAllGeoGovernmentUnits()
        {
            var query = _governmentUnitRepository.Table;

            query = query.Where(m => !m.Deleted && m.Location != null);


            query = query.Sort(new PropertySortCondition[1] {
                    new PropertySortCondition("DisplayOrder", System.ComponentModel.ListSortDirection.Ascending)
                });

            return query.ToList();
        }

        /// <summary>
        /// 获取不同的单位性质
        /// </summary>
        /// <param name="governmentTypes"></param>
        /// <returns></returns>
        public IQueryable<GovernmentUnit> GetAllGovernmentUnitsByType(params GovernmentType[] governmentTypes)
        {
            var query = _governmentUnitRepository.TableNoTracking;
            query = query.Where(g => !g.Deleted);
            query = query.Where(g => governmentTypes.Contains(g.GovernmentType));

            return query;
        }

        /// <summary>
        /// 获取商家列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="showHidden"></param>
        /// <param name="sortConditions"></param>
        /// <returns></returns>
        public IPagedList<GovernmentUnit> GetAllGovernmentUnits(string search = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false, params PropertySortCondition[] sortConditions)
        {
            var query = _governmentUnitRepository.Table;

            query = query.Where(m => !m.Deleted);
             

            //实现查询
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(e => e.Name.Contains(search) || e.Address.Contains(search) || e.Tel.Contains(search) || e.Person.Contains(search));
            }

            if (sortConditions != null && sortConditions.Length != 0)
            {
                query = query.Sort(sortConditions);
            }
            else
            {
                query = query.Sort(new PropertySortCondition[1] {
                    new PropertySortCondition("DisplayOrder", System.ComponentModel.ListSortDirection.Ascending)
                });
            }

            var governmentUnits = new PagedList<GovernmentUnit>(query, pageIndex, pageSize);
            return governmentUnits;
        }

        public IPagedList<GovernmentUnit> GetAllGovernmentUnits(IList<int> governmentType,string search = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false, params PropertySortCondition[] sortConditions)
        {
            var query = _governmentUnitRepository.Table;

            query = query.Where(m => !m.Deleted);


            //实现查询
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(e => e.Name.Contains(search) || e.Address.Contains(search) || e.Tel.Contains(search) || e.Person.Contains(search));
            }

            if (governmentType.Count != 0)
                query = query.Where(g => governmentType.Contains((int)g.GovernmentType));

            if (sortConditions != null && sortConditions.Length != 0)
            {
                query = query.Sort(sortConditions);
            }
            else
            {
                query = query.Sort(new PropertySortCondition[1] {
                    new PropertySortCondition("DisplayOrder", System.ComponentModel.ListSortDirection.Ascending)
                });
            }

            var governmentUnits = new PagedList<GovernmentUnit>(query, pageIndex, pageSize);
            return governmentUnits;
        }


        /// <summary>
        /// Gets all categories filtered by parent category identifier
        /// </summary>
        /// <param name="parentGovernmentUnitId">Parent category identifier</param>
        /// <param name="exceptGovernmentsWithUsers">不获取有账号的单位</param>
        /// <returns>Categories</returns>
        public virtual IList<GovernmentUnit> GetAllGovernmentsByParentGovernmentId(int parentGovernmentUnitId,bool exceptGovernmentsWithUsers=false)
        {
            string key = string.Format(GOVERNMENTUNITS_BY_PARENT_GOVERNMENTUNIT_ID_KEY, parentGovernmentUnitId, 0);
            return _cacheManager.Get(key, () =>
            {
                var query = _governmentUnitRepository.Table;
                query = query.Where(c => !c.Deleted&& c.ParentGovernmentId == parentGovernmentUnitId);
                if (exceptGovernmentsWithUsers) query = query.Where(g => g.Users.Count == 0);
                query = query.OrderBy(c => c.DisplayOrder);

                //if (!showHidden && (!_catalogSettings.IgnoreAcl || !_catalogSettings.IgnoreStoreLimitations))
                //{
                //    if (!_catalogSettings.IgnoreAcl)
                //    {

                //        query = from c in query
                //                group c by c.Id
                //                into cGroup
                //                orderby cGroup.Key
                //                select cGroup.FirstOrDefault();
                //        query = query.OrderBy(c => c.DisplayOrder);
                //    }
                //}

                var categories = query.ToList();
                return categories;
            });

        }

        public GovernmentUnit GetGovernmentUnitById(int governmentUnitId)
        {
            if (governmentUnitId == 0) return null;

            return _governmentUnitRepository.GetById(governmentUnitId);

            //string key = string.Format(GOVERNMENTUNITS_BY_ID_KEY, governmentUnitId);
            //return _cacheManager.Get(key, () =>);
        }

        public GovernmentUnit GetGovernmentUnitByName(string governmentUnitName)
        {
            var query = _governmentUnitRepository.Table;

            query = query.Where(m => !m.Deleted&& m.Name.Contains(governmentUnitName));

            return query.FirstOrDefault();
        }

        /// <summary>
        /// 名称是否唯一
        /// </summary>
        /// <param name="governmentUnitName"></param>
        /// <returns>true表示唯一</returns>
        public bool NameUniqueCheck(string governmentUnitName, int governmentUnitId = 0)
        {
            var query = _governmentUnitRepository.Table;
            query = query.Where(c => !c.Deleted);

            if (!String.IsNullOrWhiteSpace(governmentUnitName))
            {
                var governmentUnit = query.Where(c => c.Name == governmentUnitName).FirstOrDefault();
                if (governmentUnit == null) return true;
                else
                    return governmentUnit.Id == governmentUnitId;

            }
            else return true;
        }

        public void InsertGovernmentUnit(GovernmentUnit governmentUnit)
        {
            if (governmentUnit == null)
                throw new ArgumentNullException("governmentUnit");

            //插入前名称唯一性判断
            //if (!NameUniqueCheck(governmentUnit.Name)) throw new ArgumentException(string.Format("商家名称 {0} 已经存在", governmentUnit.Name));

            _governmentUnitRepository.Insert(governmentUnit);

            //cache
            _cacheManager.RemoveByPattern(GOVERNMENTUNITS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(governmentUnit);
        }

        public void UpdateGovernmentUnit(GovernmentUnit governmentUnit)
        {
            if (governmentUnit == null)
                throw new ArgumentNullException("governmentUnit");

            _governmentUnitRepository.Update(governmentUnit);

            //cache
            _cacheManager.RemoveByPattern(GOVERNMENTUNITS_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(governmentUnit);
        }

        /// <summary>
        /// 获取子单位的Id集合
        /// </summary>
        /// <param name="governmentId"></param>
        /// <param name="containSelf">结果包含自身id</param>
        /// <returns></returns>
        public List<int> GetChildrenGovernmentIds(int governmentId, bool containSelf=true)
        {
            var idList = new List<int>();
            if (governmentId == 0) return new List<int>(); ;
            if (containSelf) idList.Add(governmentId);

            var query = _governmentUnitRepository.Table;
            query = query.Where(c => c.ParentGovernmentId == governmentId);
            query = query.Where(c => !c.Deleted);
            query = query.OrderBy(c => c.DisplayOrder);

            idList.AddRange(query.Select(p => p.Id).ToList());

            return idList;
        }

        /// <summary>
        /// 当前用户部门是否需要主管部门审批
        /// </summary>
        /// <param name="governmentUnit"></param>
        /// <returns></returns>
        //public bool NeedAdminGorvernmentApprove(GovernmentUnit governmentUnit)
        //{
        //    if(governmentUnit.Users.)
        //}
    }
}
