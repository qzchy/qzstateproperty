using System;
using System.Collections.Generic;
using QZCHY.Core.Domain.AccountUsers;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace QZCHY.Services.AccountUsers
{
    public interface IAccountUserService 
    {
        /// <summary>
        /// Delete a customer
        /// </summary>
        /// <param name="customer">AccountUser</param>
        void DeleteAccountUser(AccountUser customer);

        /// <summary>
        /// Gets a customer
        /// </summary>
        /// <param name="customerId">AccountUser identifier</param>
        /// <returns>A customer</returns>
        AccountUser GetAccountUserById(int customerId);

        /// <summary>
        /// Get customers by identifiers
        /// </summary>
        /// <param name="customerIds">AccountUser identifiers</param>
        /// <returns>AccountUsers</returns>
        IList<AccountUser> GetAccountUsersByIds(int[] customerIds);

        /// <summary>
        /// Gets a customer by GUID
        /// </summary>
        /// <param name="customerGuid">AccountUser GUID</param>
        /// <returns>A customer</returns>
        AccountUser GetAccountUserByGuid(Guid customerGuid);

        /// <summary>
        /// Get customer by system role
        /// </summary>
        /// <param name="systemName">System name</param>
        /// <returns>AccountUser</returns>
        AccountUser GetAccountUserBySystemName(string systemName);

        /// <summary>
        /// Get customer by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>AccountUser</returns>
        AccountUser GetAccountUserByUsername(string username);

        AccountUser GetAccountUserByAccount(string username);

        void UpdateAccountUser(AccountUser customer);

        /// <summary>
        /// Insert a guest customer
        /// </summary>
        /// <returns>AccountUser</returns>
        AccountUser InsertGuestAccountUser();

        /// <summary>
        /// Insert a customer
        /// </summary>
        /// <param name="customer">AccountUser</param>
        void InsertAccountUser(AccountUser customer);

        #region AccountUser roles

        /// <summary>
        /// Delete a customer role
        /// </summary>
        /// <param name="customerRole">AccountUser role</param>
        void DeleteAccountUserRole(AccountUserRole customerRole);

        /// <summary>
        /// Gets a customer role
        /// </summary>
        /// <param name="customerRoleId">AccountUser role identifier</param>
        /// <returns>AccountUser role</returns>
        AccountUserRole GetAccountUserRoleById(int customerRoleId);

        /// <summary>
        /// Gets a customer role
        /// </summary>
        /// <param name="systemName">AccountUser role system name</param>
        /// <returns>AccountUser role</returns>
        AccountUserRole GetAccountUserRoleBySystemName(string systemName);

        /// <summary>
        /// Gets all customer roles
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>AccountUser roles</returns>
        IList<AccountUserRole> GetAllAccountUserRoles(bool showHidden = false);

        /// <summary>
        /// Inserts a customer role
        /// </summary>
        /// <param name="customerRole">AccountUser role</param>
        void InsertAccountUserRole(AccountUserRole customerRole);

        /// <summary>
        /// Updates the customer role
        /// </summary>
        /// <param name="customerRole">AccountUser role</param>
        void UpdateAccountUserRole(AccountUserRole customerRole);

        #endregion
    }
}
