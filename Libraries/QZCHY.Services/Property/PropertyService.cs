using System;
using QZCHY.Core;
using QZCHY.Services.Events;
using QZCHY.Core.Data;
using QZCHY.Core.Caching;
using System.Linq;
using QZCHY.Data;
using QZCHY.Services.Property;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;
using QZCHY.Core.Domain.Properties;

namespace QZCHY.Services.Properties
{
    public class PropertyService : IPropertyService
    {
        private const string PROPERTY_BY_ID_KEY = "QZCHY.property.id-{0}";
        private const string PROPERTIES_PATTERN_KEY = "QZCHY.property.";

        private readonly IRepository<QZCHY.Core.Domain.Properties.Property> _propertyRepository;
        private readonly IRepository<QZCHY.Core.Domain.Properties.GovernmentUnit> _governmentUnitRepository;
        private readonly IRepository<PropertyPicture> _propertyPictureRepository;
        private readonly IRepository<PropertyFile> _propertyFileRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;


        public PropertyService(ICacheManager cacheManager,
             IRepository<QZCHY.Core.Domain.Properties.Property> propertyRepository, IRepository<QZCHY.Core.Domain.Properties.GovernmentUnit> governmentUnitRepository,
             IRepository<PropertyPicture> propertyPictureRepository, IRepository<PropertyFile> propertyFileRepository,
        IEventPublisher eventPublisher)
        {

            this._cacheManager = cacheManager;

            _propertyRepository = propertyRepository;
            _governmentUnitRepository = governmentUnitRepository;

            _propertyPictureRepository = propertyPictureRepository;
            _propertyFileRepository = propertyFileRepository;

            this._eventPublisher = eventPublisher;
        }

        public void GetProperties(string wkt)
        {
            //var extent = DbGeography.FromText(wkt);

            //var p1 = _propertyRepository.Table.Where(p => p.Location.Intersects(extent));
        }

        public void DeleteProperty(QZCHY.Core.Domain.Properties.Property property)
        {
            if (property == null)
                throw new ArgumentNullException("property is null");

            property.Deleted = true;
            UpdateProperty(property);
        }

        public IList<QZCHY.Core.Domain.Properties.Property> GetAllProperties()
        {
            var query = _propertyRepository.Table;

            query = query.Where(m => !m.Deleted);

            // query = query.Where(s => s.Published);

            return query.ToList();
        }

        public IQueryable<QZCHY.Core.Domain.Properties.Property> GetPropertiesByGovernmentId(IList<int> governmentIds)
        {
            var query = _propertyRepository.Table;
            query = query.Where(p =>!p.Deleted && governmentIds.Contains(p.Government.Id));
            return query;
        }

        public IQueryable<QZCHY.Core.Domain.Properties.Property> GetAllProperties(IList<int> governmentIds, bool showHidden = false)
        {
            var query = from p in _propertyRepository.Table.AsNoTracking()
                        select p;

            if (governmentIds.Count > 0) query = query.Where(p => governmentIds.Contains(p.Government.Id));
            Expression<Func<QZCHY.Core.Domain.Properties.Property, bool>> expression = p => !p.Deleted;

            if (!showHidden) expression = expression.And(p => p.Published && !p.Off); 

            query = query.Where(expression);

            return query;
        }

        public IPagedList<QZCHY.Core.Domain.Properties.Property> GetAllProperties(IList<int> governmentIds,string search = "", int pageIndex = 0, int pageSize = int.MaxValue,
            bool showHidden = true, PropertyAdvanceConditionRequest advanceCondition = null, params PropertySortCondition[] sortConditions)
        {
            var query = _propertyRepository.Table.AsNoTracking();

            if (governmentIds.Count > 0) query = query.Where(p => governmentIds.Contains(p.Government.Id));

            Expression<Func<QZCHY.Core.Domain.Properties.Property, bool>> expression = p => !p.Deleted;

            if (!showHidden) expression = expression.And(p => p.Published && !p.Off);

            //字符串查询
            if (!string.IsNullOrEmpty(search))
            {
                expression = expression.And(p => p.Name.Contains(search) || p.Address.Contains(search));
                //query = query.Where(e => e.Name.Contains(search) || e.Address.Contains(search));
            }

            //高级搜索条件
            if (advanceCondition != null)
            {
                if (advanceCondition.GovernmentId > 0)
                {
                    if (advanceCondition.SerachParentGovernement)
                    {
                        var ids = _governmentUnitRepository.TableNoTracking.Where(g => g.ParentGovernmentId == advanceCondition.GovernmentId || g.Id == advanceCondition.GovernmentId).Select(g => g.Id).ToArray();

                        expression = expression.And(p => ids.Contains(p.Government.Id));
                    }
                    else
                    {
                        expression = expression.And(p => p.Government.Id == advanceCondition.GovernmentId);
                    }
                }

                if (advanceCondition.GovernmentTypes.Count != 0)
                    expression = expression.And(p => advanceCondition.GovernmentTypes.Contains((int)p.Government.GovernmentType));
                //query = query.Where(s => advanceCondition.PropertyType.Contains((int)s.Government.GovernmentType));

                #region 资产类别和建筑面积、土地面积挂钩
                Expression<Func<QZCHY.Core.Domain.Properties.Property, bool>> houseAndLandExpression = null;

                #region 建筑面积区间集合
                Expression<Func<QZCHY.Core.Domain.Properties.Property, bool>> constructRangeExpression = null;
                if (advanceCondition.ConstructArea.Count > 0)
                {
                    #region 遍历区间集合
                    foreach (var range in advanceCondition.ConstructArea)
                    {
                        var min = range[0];
                        var max = range[1];

                        if (max == 0) max = int.MaxValue;

                        if (min >= max) continue;

                        if (constructRangeExpression == null) constructRangeExpression = p => p.ConstructArea >= min && p.ConstructArea <= max;
                        else constructRangeExpression = constructRangeExpression.Or(p => p.ConstructArea >= min && p.ConstructArea <= max);
                    }
                    #endregion                 
                }

                #endregion

                #region 土地面积区间集合
                Expression<Func<QZCHY.Core.Domain.Properties.Property, bool>> landRangeExpression = null;

                if (advanceCondition.LandArea.Count > 0)
                {
                    #region 遍历区间集合
                    foreach (var range in advanceCondition.LandArea)
                    {
                        var min = range[0];
                        var max = range[1];

                        if (max == 0) max = int.MaxValue;

                        if (min >= max) continue;

                        if (landRangeExpression == null) landRangeExpression = p => p.LandArea >= min && p.LandArea <= max;
                        else landRangeExpression = landRangeExpression.Or(p => p.LandArea >= min && p.LandArea <= max);
                    }
                    #endregion                 
                }
                #endregion

                if (advanceCondition.PropertyType.Count == 0)
                {
                    if (landRangeExpression != null)
                    {
                        if (houseAndLandExpression == null) houseAndLandExpression = landRangeExpression;
                        else houseAndLandExpression = houseAndLandExpression.Or(landRangeExpression);
                    }
                }
                else
                {

                    #region 房屋及房屋面积、土地面积
                    if (advanceCondition.PropertyType.Contains(0))
                    {
                        Expression<Func<QZCHY.Core.Domain.Properties.Property, bool>> xpression = p => p.PropertyType == QZCHY.Core.Domain.Properties.PropertyType.House;

                        if (constructRangeExpression != null)
                            xpression = xpression.And(constructRangeExpression);

                        if (landRangeExpression != null)
                            xpression = xpression.And(landRangeExpression);

                        houseAndLandExpression = xpression;
                    }
                    #endregion

                    #region 土地及土地面积
                    if (advanceCondition.PropertyType.Contains(1) || advanceCondition.PropertyType.Contains(2))
                    {
                        if (advanceCondition.PropertyType.Contains(0)) advanceCondition.PropertyType.Remove(0);
                        Expression<Func<QZCHY.Core.Domain.Properties.Property, bool>> xpression = p => advanceCondition.PropertyType.Contains((int)p.PropertyType);

                        if (landRangeExpression != null)
                            xpression = xpression.And(landRangeExpression);

                        if (houseAndLandExpression == null) houseAndLandExpression = xpression;
                        else houseAndLandExpression = houseAndLandExpression.Or(xpression);
                    }
                    #endregion
                }

                if (houseAndLandExpression != null) expression = expression.And(houseAndLandExpression);
                #endregion

                if (advanceCondition.Region.Count != 0)
                    expression = expression.And(p => advanceCondition.Region.Contains((int)p.Region));

                #region 土地证书情况
                if (!(advanceCondition.Certificate_Both && advanceCondition.Certificate_Construct &&
                            advanceCondition.Certificate_Land && advanceCondition.Certificate_None))
                {
                    Expression<Func<QZCHY.Core.Domain.Properties.Property, bool>> ex = null;
                    if (advanceCondition.Certificate_Both) ex = p => p.HasConstructID && p.HasLandID;
                    if (advanceCondition.Certificate_Construct)
                    {
                        if (ex == null) ex = p => p.HasConstructID && !p.HasLandID;
                        else ex = ex.Or(p => p.HasConstructID && !p.HasLandID);
                    }
                    if (advanceCondition.Certificate_Land)
                    {
                        if (ex == null) ex = p => !p.HasConstructID && p.HasLandID;
                        else ex = ex.Or(p => !p.HasConstructID && p.HasLandID);
                    }
                    if (advanceCondition.Certificate_None)
                    {
                        if (ex == null) ex = p => !p.HasConstructID && !p.HasLandID;
                        else ex = ex.Or(p => !p.HasConstructID && !p.HasLandID);
                    }

                    if (ex != null) expression = expression.And(ex);
                }
                #endregion

                #region 使用现状
                if (!(advanceCondition.Current_Self && advanceCondition.Current_Rent && advanceCondition.Current_Lend && advanceCondition.Current_Idle))
                {
                    Expression<Func<QZCHY.Core.Domain.Properties.Property, bool>> ex = null;
                    if (advanceCondition.Current_Self) ex = p => p.CurrentUse_Self > 0;
                    if (advanceCondition.Current_Rent)
                    {
                        if (ex == null) ex = p => p.CurrentUse_Rent > 0;
                        else ex = ex.Or(p => p.CurrentUse_Rent > 0);
                    }
                    if (advanceCondition.Current_Lend)
                    {
                        if (ex == null) ex = p => p.CurrentUse_Lend > 0;
                        else ex = ex.Or(p => p.CurrentUse_Lend > 0);
                    }
                    if (advanceCondition.Current_Idle)
                    {
                        if (ex == null) ex = p => p.CurrentUse_Idle > 0;
                        else ex = ex.Or(p => p.CurrentUse_Idle > 0);
                    }

                    if (ex != null) expression = expression.And(ex);
                }
                #endregion

                if (advanceCondition.NextStep.Count != 0)
                    expression = expression.And(p => advanceCondition.NextStep.Contains((int)p.NextStepUsage));

                #region 价格区间集合
                if (advanceCondition.Price.Count > 0)
                {
                    foreach (var range in advanceCondition.Price)
                    {
                        var min = range[0];
                        var max = range[1];

                        if (max == 0) max = int.MaxValue;

                        if (min >= max) continue;

                        expression = expression.And(p => p.Price >= min && p.Price <= max);
                    }
                }
                #endregion

                #region 价格区间集合
                if (advanceCondition.Price.Count > 0)
                {
                    foreach (var range in advanceCondition.Price)
                    {
                        var min = range[0];
                        var max = range[1];

                        if (max == 0) max = int.MaxValue;

                        if (min >= max) continue;

                        expression = expression.And(p => p.Price >= min && p.Price <= max);
                    }
                }
                #endregion

                if (advanceCondition.LifeTime.Count == 2)
                {
                    var min = advanceCondition.LifeTime[0];
                    var max = advanceCondition.LifeTime[1];

                    expression = expression.And(p => p.LifeTime >= min && p.LifeTime <= max);
                }

                //if (advanceCondition.GetedDate.Count == 2)
                //{
                //    var min = advanceCondition.GetedDate[0];
                //    var max = advanceCondition.GetedDate[1];

                //    expression = expression.And(p => p.GetedDate >= new DateTime(min, 1, 1) && p.GetedDate <= new DateTime(max, 12, 31));
                //}

                //范围过滤
                if (advanceCondition.Extent != null)
                    expression = expression.And(p => !advanceCondition.Extent.Intersects(p.Location));
            }

            query = query.Where(expression);

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

            var properties = new PagedList<QZCHY.Core.Domain.Properties.Property>(query, pageIndex, pageSize);
            return properties;
        }

        public QZCHY.Core.Domain.Properties.Property GetPropertyById(int propertyId)
        {
            if (propertyId == 0) return null;

            string key = string.Format(PROPERTY_BY_ID_KEY, propertyId);
            return _cacheManager.Get(key, () => _propertyRepository.GetById(propertyId));
        }

        public void InsertProperty(QZCHY.Core.Domain.Properties.Property property)
        {
            if (property == null)
                throw new ArgumentNullException("property is null");

            _propertyRepository.Insert(property);

            //cache
            _cacheManager.RemoveByPattern(PROPERTIES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(property);
        }

        /// <summary>
        /// 名称是否唯一
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns>true表示唯一</returns>
        public bool NameUniqueCheck(string propertyName)
        {
            var query = _propertyRepository.Table;
            query = query.Where(c => !c.Deleted);

            if (!String.IsNullOrWhiteSpace(propertyName))
            {
                var property = query.Where(c => c.Name == propertyName).FirstOrDefault();
                return property == null;
            }
            else return true;
        }

        public void UpdateProperty(QZCHY.Core.Domain.Properties.Property property)
        {
            if (property == null)
                throw new ArgumentNullException("property is null");

            _propertyRepository.Update(property);

            //cache
            _cacheManager.RemoveByPattern(PROPERTIES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(property);
        }

        public IList<Core.Domain.Properties.Property> GetPropertyProcess(int governmentId)
        {
            var query = from c in _propertyRepository.Table
                        where c.Government.Id == governmentId && c.Deleted == false && c.Locked == false//&&c.Published==true
                        select c;
            var properties = query.ToList();
            return properties;
        }

        /// <summary>
        /// 获取可处理的资产
        /// </summary>
        /// <param name="governmentIds"></param>
        /// <returns></returns>
        public IList<QZCHY.Core.Domain.Properties.Property> GetProcessProperties(IList<int> governmentIds)
        {
            var query = from p in _propertyRepository.TableNoTracking
                        where !p.Deleted && !p.Locked && p.Published && governmentIds.Contains(p.Government.Id)
                        orderby p.CreatedOn descending
                        select p;

            return query.ToList();
        }

        public IList<Core.Domain.Properties.Property> GetAllProcessProperties(IList<int> governmentIds, string search = "", PropertyAdvanceConditionRequest advanceCondition = null,  params PropertySortCondition[] sortConditions)
        {
            var query = _propertyRepository.Table.AsNoTracking();

            query = query.Where(p => !p.Deleted && governmentIds.Contains(p.Government.Id));

            Expression<Func<QZCHY.Core.Domain.Properties.Property, bool>> expression = p => !p.Deleted && !p.Locked && p.Published ;
             

            //字符串查询
            if (!string.IsNullOrEmpty(search))
            {
                expression = expression.And(p => p.Name.Contains(search) || p.Address.Contains(search));
                //query = query.Where(e => e.Name.Contains(search) || e.Address.Contains(search));
            }

            //高级搜索条件
            if (advanceCondition != null) 
            {
                if (advanceCondition.GovernmentId > 0)
                {
                    if (advanceCondition.SerachParentGovernement)
                    {
                        var ids = _governmentUnitRepository.TableNoTracking.Where(g => g.ParentGovernmentId == advanceCondition.GovernmentId || g.Id == advanceCondition.GovernmentId).Select(g => g.Id).ToArray();

                        expression = expression.And(p => ids.Contains(p.Government.Id));
                    }
                    else
                    {
                        expression = expression.And(p => p.Government.Id == advanceCondition.GovernmentId);
                    }
                }

                if (advanceCondition.GovernmentTypes.Count != 0)
                    expression = expression.And(p => advanceCondition.GovernmentTypes.Contains((int)p.Government.GovernmentType));
                //query = query.Where(s => advanceCondition.PropertyType.Contains((int)s.Government.GovernmentType));

                #region 资产类别和建筑面积、土地面积挂钩
                Expression<Func<QZCHY.Core.Domain.Properties.Property, bool>> houseAndLandExpression = null;

                #region 建筑面积区间集合
                Expression<Func<QZCHY.Core.Domain.Properties.Property, bool>> constructRangeExpression = null;
                if (advanceCondition.ConstructArea.Count > 0)
                {
                    #region 遍历区间集合
                    foreach (var range in advanceCondition.ConstructArea)
                    {
                        var min = range[0];
                        var max = range[1];

                        if (max == 0) max = int.MaxValue;

                        if (min >= max) continue;

                        if (constructRangeExpression == null) constructRangeExpression = p => p.ConstructArea >= min && p.ConstructArea <= max;
                        else constructRangeExpression = constructRangeExpression.Or(p => p.ConstructArea >= min && p.ConstructArea <= max);
                    }
                    #endregion                 
                }

                #endregion

                #region 土地面积区间集合
                Expression<Func<QZCHY.Core.Domain.Properties.Property, bool>> landRangeExpression = null;

                if (advanceCondition.LandArea.Count > 0)
                {
                    #region 遍历区间集合
                    foreach (var range in advanceCondition.LandArea)
                    {
                        var min = range[0];
                        var max = range[1];

                        if (max == 0) max = int.MaxValue;

                        if (min >= max) continue;

                        if (landRangeExpression == null) landRangeExpression = p => p.LandArea >= min && p.LandArea <= max;
                        else landRangeExpression = landRangeExpression.Or(p => p.LandArea >= min && p.LandArea <= max);
                    }
                    #endregion                 
                }
                #endregion

                if (advanceCondition.PropertyType.Count == 0)
                {
                    if (landRangeExpression != null)
                    {
                        if (houseAndLandExpression == null) houseAndLandExpression = landRangeExpression;
                        else houseAndLandExpression = houseAndLandExpression.Or(landRangeExpression);
                    }
                }
                else
                {

                    #region 房屋及房屋面积、土地面积
                    if (advanceCondition.PropertyType.Contains(0))
                    {
                        Expression<Func<QZCHY.Core.Domain.Properties.Property, bool>> xpression = p => p.PropertyType == QZCHY.Core.Domain.Properties.PropertyType.House;

                        if (constructRangeExpression != null)
                            xpression = xpression.And(constructRangeExpression);

                        if (landRangeExpression != null)
                            xpression = xpression.And(landRangeExpression);

                        houseAndLandExpression = xpression;
                    }
                    #endregion

                    #region 土地及土地面积
                    if (advanceCondition.PropertyType.Contains(1) || advanceCondition.PropertyType.Contains(2))
                    {
                        if (advanceCondition.PropertyType.Contains(0)) advanceCondition.PropertyType.Remove(0);
                        Expression<Func<QZCHY.Core.Domain.Properties.Property, bool>> xpression = p => advanceCondition.PropertyType.Contains((int)p.PropertyType);

                        if (landRangeExpression != null)
                            xpression = xpression.And(landRangeExpression);

                        if (houseAndLandExpression == null) houseAndLandExpression = xpression;
                        else houseAndLandExpression = houseAndLandExpression.Or(xpression);
                    }
                    #endregion
                }

                if (houseAndLandExpression != null) expression = expression.And(houseAndLandExpression);
                #endregion

                if (advanceCondition.Region.Count != 0)
                    expression = expression.And(p => advanceCondition.Region.Contains((int)p.Region));

                #region 土地证书情况
                if (!(advanceCondition.Certificate_Both && advanceCondition.Certificate_Construct &&
                            advanceCondition.Certificate_Land && advanceCondition.Certificate_None))
                {
                    Expression<Func<QZCHY.Core.Domain.Properties.Property, bool>> ex = null;
                    if (advanceCondition.Certificate_Both) ex = p => p.HasConstructID && p.HasLandID;
                    if (advanceCondition.Certificate_Construct)
                    {
                        if (ex == null) ex = p => p.HasConstructID && !p.HasLandID;
                        else ex = ex.Or(p => p.HasConstructID && !p.HasLandID);
                    }
                    if (advanceCondition.Certificate_Land)
                    {
                        if (ex == null) ex = p => !p.HasConstructID && p.HasLandID;
                        else ex = ex.Or(p => !p.HasConstructID && p.HasLandID);
                    }
                    if (advanceCondition.Certificate_None)
                    {
                        if (ex == null) ex = p => !p.HasConstructID && !p.HasLandID;
                        else ex = ex.Or(p => !p.HasConstructID && !p.HasLandID);
                    }

                    if (ex != null) expression = expression.And(ex);
                }
                #endregion

                #region 使用现状
                if (!(advanceCondition.Current_Self && advanceCondition.Current_Rent && advanceCondition.Current_Lend && advanceCondition.Current_Idle))
                {
                    Expression<Func<QZCHY.Core.Domain.Properties.Property, bool>> ex = null;
                    if (advanceCondition.Current_Self) ex = p => p.CurrentUse_Self > 0;
                    if (advanceCondition.Current_Rent)
                    {
                        if (ex == null) ex = p => p.CurrentUse_Rent > 0;
                        else ex = ex.Or(p => p.CurrentUse_Rent > 0);
                    }
                    if (advanceCondition.Current_Lend)
                    {
                        if (ex == null) ex = p => p.CurrentUse_Lend > 0;
                        else ex = ex.Or(p => p.CurrentUse_Lend > 0);
                    }
                    if (advanceCondition.Current_Idle)
                    {
                        if (ex == null) ex = p => p.CurrentUse_Idle > 0;
                        else ex = ex.Or(p => p.CurrentUse_Idle > 0);
                    }

                    if (ex != null) expression = expression.And(ex);
                }
                #endregion

                if (advanceCondition.NextStep.Count != 0)
                    expression = expression.And(p => advanceCondition.NextStep.Contains((int)p.NextStepUsage));

                #region 价格区间集合
                if (advanceCondition.Price.Count > 0)
                {
                    foreach (var range in advanceCondition.Price)
                    {
                        var min = range[0];
                        var max = range[1];

                        if (max == 0) max = int.MaxValue;

                        if (min >= max) continue;

                        expression = expression.And(p => p.Price >= min && p.Price <= max);
                    }
                }
                #endregion

                #region 价格区间集合
                if (advanceCondition.Price.Count > 0)
                {
                    foreach (var range in advanceCondition.Price)
                    {
                        var min = range[0];
                        var max = range[1];

                        if (max == 0) max = int.MaxValue;

                        if (min >= max) continue;

                        expression = expression.And(p => p.Price >= min && p.Price <= max);
                    }
                }
                #endregion

                if (advanceCondition.LifeTime.Count == 2)
                {
                    var min = advanceCondition.LifeTime[0];
                    var max = advanceCondition.LifeTime[1];

                    expression = expression.And(p => p.LifeTime >= min && p.LifeTime <= max);
                }

                //if (advanceCondition.GetedDate.Count == 2)
                //{
                //    var min = advanceCondition.GetedDate[0];
                //    var max = advanceCondition.GetedDate[1];

                //    expression = expression.And(p => p.GetedDate >= new DateTime(min, 1, 1) && p.GetedDate <= new DateTime(max, 12, 31));
                //}

                //范围过滤
                if (advanceCondition.Extent != null)
                    expression = expression.And(p => !advanceCondition.Extent.Intersects(p.Location));
            }

            query = query.Where(expression);

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

            var properties = query.ToList();
            return properties;
        }

        public void DeletePropertyPicture(PropertyPicture propertyPicture)
        {
            if (propertyPicture == null)
                throw new ArgumentNullException("propertyPicture");

            _propertyPictureRepository.Delete(propertyPicture);

            //event notification
            _eventPublisher.EntityDeleted(propertyPicture);
        }

        public IList<PropertyPicture> GetPropertyPicturesByPropertyId(int propertyId)
        {
            var query = from sp in _propertyPictureRepository.Table
                        where sp.PropertyId == propertyId
                        orderby sp.DisplayOrder
                        select sp;

            var propertyPictures = query.ToList();
            return propertyPictures;
        }

        public PropertyPicture GetPropertyPictureById(int propertyPictureId)
        {
            if (propertyPictureId == 0)
                return null;

            return _propertyPictureRepository.GetById(propertyPictureId);
        }

        public void InsertPropertyPicture(PropertyPicture propertyPicture)
        {
            if (propertyPicture == null)
                throw new ArgumentNullException("propertyPicture");

            _propertyPictureRepository.Insert(propertyPicture);

            //event notification
            _eventPublisher.EntityInserted(propertyPicture);
        }

        public void UpdatePropertyPicture(PropertyPicture propertyPicture)
        {
            if (propertyPicture == null)
                throw new ArgumentNullException("propertyPicture");

            _propertyPictureRepository.Update(propertyPicture);

            //event notification
            _eventPublisher.EntityUpdated(propertyPicture);
        }

        public void DeletePropertyFile(PropertyFile propertyFile)
        {
            if (propertyFile == null)
                throw new ArgumentNullException("propertyFile");

            _propertyFileRepository.Delete(propertyFile);

            //event notification
            _eventPublisher.EntityDeleted(propertyFile);
        }

        public IList<PropertyFile> GetPropertyFilesByPropertyId(int propertyId)
        {
            var query = from sp in _propertyFileRepository.Table
                        where sp.PropertyId == propertyId
                        orderby sp.DisplayOrder
                        select sp;

            var propertyFiles = query.ToList();
            return propertyFiles;
        }

        public PropertyFile GetPropertyFileById(int propertyFileId)
        {
            if (propertyFileId == 0)
                return null;

            return _propertyFileRepository.GetById(propertyFileId);
        }

        public void InsertPropertyFile(PropertyFile propertyFile)
        {
            if (propertyFile == null)
                throw new ArgumentNullException("propertyFile");

            _propertyFileRepository.Insert(propertyFile);

            //event notification
            _eventPublisher.EntityInserted(propertyFile);
        }

        public void UpdatePropertyFile(PropertyFile propertyFile)
        {
            if (propertyFile == null)
                throw new ArgumentNullException("propertyFile");

            _propertyFileRepository.Update(propertyFile);

            //event notification
            _eventPublisher.EntityUpdated(propertyFile);
        }

    }
}