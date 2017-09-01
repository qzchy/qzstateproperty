using QZCHY.Core;
using QZCHY.Core.Domain.Properties;
using System.Collections.Generic;
using System.Linq;

namespace QZCHY.Services.Property
{
    public interface IGovernmentService
    {
        void DeleteGovernmentUnit(GovernmentUnit government);

        IList<GovernmentUnit> GetAllGeoGovernmentUnits();

        /// <summary>
        /// 获取不同的单位性质
        /// </summary>
        /// <param name="governmentTypes"></param>
        /// <returns></returns>
        IQueryable<GovernmentUnit> GetAllGovernmentUnitsByType(params GovernmentType[] governmentTypes);


        IPagedList<GovernmentUnit> GetAllGovernmentUnits(string search = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false, params PropertySortCondition[] sortConditions);

        IPagedList<GovernmentUnit> GetAllGovernmentUnits(IList<int> governmentType, string search = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false, params PropertySortCondition[] sortConditions);

        /// <summary>
        /// Gets all categories filtered by parent category identifier
        /// </summary>
        /// <param name="parentGovernmentId">Parent government identifier</param>
        /// <param name="exceptGovernmentsWithUsers">是否过滤有账号的单位</param>
        /// <returns>Categories</returns>
        IList<GovernmentUnit> GetAllGovernmentsByParentGovernmentId(int parentGovernmentId, bool exceptGovernmentsWithUsers=false);

        GovernmentUnit GetGovernmentUnitById(int governmentId);

        GovernmentUnit GetGovernmentUnitByName(string governmentName);

        /// <summary>
        /// 名称是否唯一
        /// </summary>
        /// <param name="governmentName"></param>
        /// <returns>true表示唯一</returns>
        bool NameUniqueCheck(string governmentName, int governmentId = 0);

        void InsertGovernmentUnit(GovernmentUnit government);

        void UpdateGovernmentUnit(GovernmentUnit government);

        /// <summary>
        /// 获取子单位的Id集合
        /// </summary>
        /// <param name="governmentId"></param>
        /// <param name="containSelf">结果包含自身id</param>
        /// <returns></returns>
        List<int> GetChildrenGovernmentIds(int governmentId, bool containSelf=true);

        /// <summary>
        /// 是否需要主管部门审批
        /// </summary>
        /// <returns></returns>
        //bool NeedAdminGorvernmentApprove(GovernmentUnit governmentUnit);
    }
}
