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
        /// Gets a value indicating whether customer is in a certain customer role
        /// </summary>
        /// <param name="customer">AccountUser</param>
        /// <param name="customerRoleSystemName">AccountUser role system name</param>
        /// <param name="onlyActiveAccountUserRoles">A value indicating whether we should look only in active customer roles</param>
        /// <returns>Result</returns>
        public static bool IsInAccountUserRole(this AccountUser customer,
            string customerRoleSystemName, bool onlyActiveAccountUserRoles = true)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            if (String.IsNullOrEmpty(customerRoleSystemName))
                throw new ArgumentNullException("customerRoleSystemName");

            var result = customer.AccountUserRoles
                .FirstOrDefault(cr => (!onlyActiveAccountUserRoles || cr.Active) && (cr.SystemName == customerRoleSystemName)) != null;
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether customer a search engine
        /// </summary>
        /// <param name="customer">AccountUser</param>
        /// <returns>Result</returns>
        public static bool IsSearchEngineAccount(this AccountUser customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            if (!customer.IsSystemAccount || String.IsNullOrEmpty(customer.SystemName))
                return false;

            var result = customer.SystemName.Equals(SystemAccountUserNames.SearchEngine, StringComparison.InvariantCultureIgnoreCase);
            return result;
        }

        /// <summary>
        /// Gets a value indicating whether the customer is a built-in record for background tasks
        /// </summary>
        /// <param name="customer">AccountUser</param>
        /// <returns>Result</returns>
        public static bool IsBackgroundTaskAccount(this AccountUser customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            if (!customer.IsSystemAccount || String.IsNullOrEmpty(customer.SystemName))
                return false;

            var result = customer.SystemName.Equals(SystemAccountUserNames.BackgroundTask, StringComparison.InvariantCultureIgnoreCase);
            return result;
        }

        /// <summary>
        /// 客户是否为管理员
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="onlyActiveAccountUserRoles"></param>
        /// <returns></returns>
        public static bool IsAdmin(this AccountUser customer, bool onlyActiveAccountUserRoles = true)
        {
            return IsInAccountUserRole(customer, SystemAccountUserRoleNames.Administrators, onlyActiveAccountUserRoles);
        }

        /// <summary>
        /// Gets a value indicating whether customer is registered
        /// </summary>
        /// <param name="customer">AccountUser</param>
        /// <param name="onlyActiveAccountUserRoles">A value indicating whether we should look only in active customer roles</param>
        /// <returns>Result</returns>
        public static bool IsRegistered(this AccountUser customer, bool onlyActiveAccountUserRoles = true)
        {
            return IsInAccountUserRole(customer, SystemAccountUserRoleNames.Registered, onlyActiveAccountUserRoles);
        }
    }
}
