using QZCHY.Core.Domain.AccountUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Services.AccountUsers
{
    public static class AccountUserExtensions
    {
        /// <summary>
        /// Gets a value indicating whether property is in a certain property role
        /// </summary>
        /// <param name="property">AccountUser</param>
        /// <param name="customerRoleSystemName">AccountUser role system name</param>
        /// <param name="onlyActiveAccountUserRoles">A value indicating whether we should look only in active property roles</param>
        /// <returns>Result</returns>
        public static bool IsInAccountUserRole(this AccountUser property,
            string customerRoleSystemName, bool onlyActiveAccountUserRoles = true)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            if (String.IsNullOrEmpty(customerRoleSystemName))
                throw new ArgumentNullException("customerRoleSystemName");

            var result = property.AccountUserRoles
                .FirstOrDefault(cr => (!onlyActiveAccountUserRoles || cr.Active) && (cr.SystemName == customerRoleSystemName)) != null;
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether property a search engine
        /// </summary>
        /// <param name="property">AccountUser</param>
        /// <returns>Result</returns>
        public static bool IsSearchEngineAccount(this AccountUser property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            if (!property.IsSystemAccount || String.IsNullOrEmpty(property.SystemName))
                return false;

            var result = property.SystemName.Equals(SystemAccountUserNames.SearchEngine, StringComparison.InvariantCultureIgnoreCase);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether the property is a built-in record for background tasks
        /// </summary>
        /// <param name="property">AccountUser</param>
        /// <returns>Result</returns>
        public static bool IsBackgroundTaskAccount(this AccountUser property)
        {
            if (property == null)
                throw new ArgumentNullException("property");

            if (!property.IsSystemAccount || String.IsNullOrEmpty(property.SystemName))
                return false;

            var result = property.SystemName.Equals(SystemAccountUserNames.BackgroundTask, StringComparison.InvariantCultureIgnoreCase);
            return result;
        }

        /// <summary>
        /// 客户是否为管理员
        /// </summary>
        /// <param name="property"></param>
        /// <param name="onlyActiveAccountUserRoles"></param>
        /// <returns></returns>
        public static bool IsAdmin(this AccountUser property, bool onlyActiveAccountUserRoles = true)
        {
            return IsInAccountUserRole(property, SystemAccountUserRoleNames.Administrators, onlyActiveAccountUserRoles);
        }

        /// <summary>
        /// Gets a value indicating whether property is registered
        /// </summary>
        /// <param name="property">AccountUser</param>
        /// <param name="onlyActiveAccountUserRoles">A value indicating whether we should look only in active property roles</param>
        /// <returns>Result</returns>
        public static bool IsRegistered(this AccountUser property, bool onlyActiveAccountUserRoles = true)
        {
            return IsInAccountUserRole(property, SystemAccountUserRoleNames.Registered, onlyActiveAccountUserRoles);
        }

        /// <summary>
        /// 是否行政事业审批员
        /// </summary>
        /// <param name="property"></param>
        /// <param name="onlyActiveAccountUserRoles"></param>
        /// <returns></returns>
        public static bool IsGovAuditor(this AccountUser property, bool onlyActiveAccountUserRoles = true)
        {
            return IsInAccountUserRole(property, SystemAccountUserRoleNames.GovAuditor, onlyActiveAccountUserRoles);
        }

        /// <summary>
        /// 是否为国有资产审批员
        /// </summary>
        /// <param name="property"></param>
        /// <param name="onlyActiveAccountUserRoles"></param>
        /// <returns></returns>
        public static bool IsStateOwnerAuditor(this AccountUser property, bool onlyActiveAccountUserRoles = true)
        {
            return IsInAccountUserRole(property, SystemAccountUserRoleNames.StateOwnerAuditor, onlyActiveAccountUserRoles);
        }
    }
}
