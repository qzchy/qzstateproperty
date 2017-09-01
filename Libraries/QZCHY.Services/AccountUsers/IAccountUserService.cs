using System;
using System.Collections.Generic;
using QZCHY.Core.Domain.AccountUsers;
using QZCHY.Core;
using QZCHY.Core.Domain.Properties;

namespace QZCHY.Services.AccountUsers
{
    public interface IAccountUserService 
    {
        /// <summary>
        /// Delete a customer
        /// </summary>
        /// <param name="customer">AccountUser</param>
        void DeleteAccountUser(AccountUser customer);

        void DeleteGovernmentUsers(GovernmentUnit governmentUnit);

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

        /// <summary>
        /// 获取所有的用户账号
        /// </summary>
        /// <param name="search"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="showHidden"></param>
        /// <param name="sortConditions"></param>
        /// <returns></returns>
        IPagedList<AccountUser> GetAllAccountUsers(string search = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false, params PropertySortCondition[] sortConditions);


        void UpdateAccountUser(AccountUser customer);

        /// <summary>
        /// Insert a customer
        /// </summary>
        /// <param name="customer">AccountUser</param>
        void InsertAccountUser(AccountUser customer);

        /// <summary>
        /// 名称是否唯一
        /// </summary>
        /// <param name="accountUserName"></param>
        /// <returns>true表示唯一</returns>
        bool NameUniqueCheck(string accountUserName,int accountUserId=0);

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
