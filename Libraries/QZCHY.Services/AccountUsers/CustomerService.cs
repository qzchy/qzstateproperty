using System;
using System.Collections.Generic;
using System.Linq;
using QZCHY.Core.Data;
using QZCHY.Data;
using QZCHY.Core.Caching;
using QZCHY.Services.Events;
using QZCHY.Core.Domain.Common;
using QZCHY.Core;
using QZCHY.Core.Domain.AccountUsers;

namespace QZCHY.Services.AccountUsers
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public class AccountUserService : IAccountUserService
    {

        #region Constants

        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : show hidden records?
        /// </remarks>
        private const string CUSTOMERROLES_ALL_KEY = "QM.customerrole.all-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : system name
        /// </remarks>
        private const string CUSTOMERROLES_BY_SYSTEMNAME_KEY = "QM.customerrole.systemname-{0}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string CUSTOMERROLES_PATTERN_KEY = "QM.customerrole.";

        #endregion

        #region Fields

        private readonly IRepository<AccountUser> _customerRepository;
        private readonly IRepository<AccountUserRole> _customerRoleRepository;
        private readonly IRepository<GenericAttribute> _gaRepository;
        private readonly IDataProvider _dataProvider;
        private readonly IDbContext _dbContext;
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly AccountUserSettings _customerSettings;
        private readonly CommonSettings _commonSettings;
        
        #endregion

        #region 构造函数

        public AccountUserService(ICacheManager cacheManager,
            IRepository<AccountUser> customerRepository,
            IRepository<AccountUserRole> customerRoleRepository,
            IRepository<GenericAttribute> gaRepository,
            //IRepository<Order> orderRepository,
            //IRepository<ForumPost> forumPostRepository,
            //IRepository<ForumTopic> forumTopicRepository,
            //IRepository<BlogComment> blogCommentRepository,
            //IRepository<NewsComment> newsCommentRepository,
            //IRepository<PollVotingRecord> pollVotingRecordRepository,
            //IRepository<ProductReview> productReviewRepository,
            //IRepository<ProductReviewHelpfulness> productReviewHelpfulnessRepository,
            //IGenericAttributeService genericAttributeService,
            IDataProvider dataProvider,
            IDbContext dbContext,
            IEventPublisher eventPublisher,
        AccountUserSettings customerSettings,
            CommonSettings commonSettings)
        {
            this._cacheManager = cacheManager;
            this._customerRepository = customerRepository;
            this._customerRoleRepository = customerRoleRepository;
            this._gaRepository = gaRepository;
            //this._orderRepository = orderRepository;
            //this._forumPostRepository = forumPostRepository;
            //this._forumTopicRepository = forumTopicRepository;
            //this._blogCommentRepository = blogCommentRepository;
            //this._newsCommentRepository = newsCommentRepository;
            //this._pollVotingRecordRepository = pollVotingRecordRepository;
            //this._productReviewRepository = productReviewRepository;
            //this._productReviewHelpfulnessRepository = productReviewHelpfulnessRepository;
            //this._genericAttributeService = genericAttributeService;
            this._dataProvider = dataProvider;
            this._dbContext = dbContext;
            this._eventPublisher = eventPublisher;

            this._customerSettings = customerSettings;
            this._commonSettings = commonSettings;
        }

        #endregion

        #region IAccountUser 实现
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="customer"></param>
        public void DeleteAccountUser(AccountUser customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            if (customer.IsSystemAccount)
                throw new QZCHYException(string.Format("System customer account ({0}) could not be deleted", customer.SystemName));

            customer.Deleted = true;

            if (_customerSettings.SuffixDeletedAccountUsers) customer.UserName += "-DELETED";

            UpdateAccountUser(customer);
        }

        /// <summary>
        /// 通过id获取用户
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public AccountUser GetAccountUserById(int customerId)
        {
            if (customerId == 0)
                return null;

            return _customerRepository.GetById(customerId);
        }

        public virtual IList<AccountUser> GetAccountUsersByIds(int[] customerIds)
        {
            if (customerIds == null || customerIds.Length == 0)
                return new List<AccountUser>();

            var query = from c in _customerRepository.Table
                        where customerIds.Contains(c.Id)
                        select c;
            var customers = query.ToList();
            //sort by passed identifiers
            var sortedAccountUsers = new List<AccountUser>();
            foreach (int id in customerIds)
            {
                var customer = customers.Find(x => x.Id == id);
                if (customer != null)
                    sortedAccountUsers.Add(customer);
            }
            return sortedAccountUsers;
        }


        public AccountUser GetAccountUserByGuid(Guid customerGuid)
        {
            if (customerGuid == Guid.Empty)
                return null;

            var query = from c in _customerRepository.Table
                        orderby c.Id
                        where c.AccountUserGuid == customerGuid
                        select c;

            var customer = query.FirstOrDefault();
            return customer;
        }

        public AccountUser GetAccountUserBySystemName(string systemName)
        {
            if (string.IsNullOrWhiteSpace(systemName))
                return null;

            var query = from c in _customerRepository.Table
                        orderby c.Id
                        where c.SystemName == systemName
                        select c;
            var customer = query.FirstOrDefault();
            return customer;
        }

        /// <summary>
        /// Get customer by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>AccountUser</returns>
        public virtual AccountUser GetAccountUserByUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
                return null;

            var query = from c in _customerRepository.Table
                        orderby c.Id
                        where c.UserName == username
                        select c;

            var customer = query.FirstOrDefault();
            return customer;
        }

        public AccountUser GetAccountUserByAccount(string account)
        {
            if (string.IsNullOrWhiteSpace(account))
                throw new ArgumentNullException("账户");

            AccountUser customer = null;

            customer = GetAccountUserByUsername(account);

            return customer;
        }

        /// <summary>
        /// 新增客户
        /// </summary>
        /// <param name="customer"></param>
        public void InsertAccountUser(AccountUser customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            _customerRepository.Insert(customer);

            //event notification
            _eventPublisher.EntityInserted(customer);
        }

        public AccountUser InsertGuestAccountUser()
        {
            var customer = new AccountUser
            {
                AccountUserGuid = Guid.NewGuid(),
                Active = true,
                CreatedOn = DateTime.Now,
                LastActivityDate = DateTime.Now,
            };

            //add to 'Guests' role
            var guestRole = GetAccountUserRoleBySystemName(SystemAccountUserRoleNames.Guests);
            if (guestRole == null)
                throw new QZCHYException("角色 '访客' 不能被加载");
            customer.AccountUserRoles.Add(guestRole);

            _customerRepository.Insert(customer);

            return customer;
        }

        public void UpdateAccountUser(AccountUser customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            _customerRepository.Update(customer);

            //event notification
            _eventPublisher.EntityUpdated(customer);
        } 
        #endregion

        #region AccountUser roles

        /// <summary>
        /// Delete a customer role
        /// </summary>
        /// <param name="customerRole">AccountUser role</param>
        public virtual void DeleteAccountUserRole(AccountUserRole customerRole)
        {
            if (customerRole == null)
                throw new ArgumentNullException("customerRole");

            if (customerRole.IsSystemRole)
                throw new QZCHYException("System role could not be deleted");

            _customerRoleRepository.Delete(customerRole);

            _cacheManager.RemoveByPattern(CUSTOMERROLES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(customerRole);
        }

        /// <summary>
        /// Gets a customer role
        /// </summary>
        /// <param name="customerRoleId">AccountUser role identifier</param>
        /// <returns>AccountUser role</returns>
        public virtual AccountUserRole GetAccountUserRoleById(int customerRoleId)
        {
            if (customerRoleId == 0)
                return null;

            return _customerRoleRepository.GetById(customerRoleId);
        }

        /// <summary>
        /// Gets a customer role
        /// </summary>
        /// <param name="systemName">AccountUser role system name</param>
        /// <returns>AccountUser role</returns>
        public virtual AccountUserRole GetAccountUserRoleBySystemName(string systemName)
        {
            if (String.IsNullOrWhiteSpace(systemName))
                return null;

            string key = string.Format(CUSTOMERROLES_BY_SYSTEMNAME_KEY, systemName);
            return _cacheManager.Get(key, () =>
            {
                var query = from cr in _customerRoleRepository.Table
                            orderby cr.Id
                            where cr.SystemName == systemName
                            select cr;
                var customerRole = query.FirstOrDefault();
                return customerRole;
            });
        }

        /// <summary>
        /// Gets all customer roles
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>AccountUser roles</returns>
        public virtual IList<AccountUserRole> GetAllAccountUserRoles(bool showHidden = false)
        {
            string key = string.Format(CUSTOMERROLES_ALL_KEY, showHidden);
            return _cacheManager.Get(key, () =>
        {
                var query = from cr in _customerRoleRepository.Table
                            orderby cr.Name
                            where (showHidden || cr.Active)
                            select cr;
                var customerRoles = query.ToList();
                return customerRoles;
            });
        }

        /// <summary>
        /// Inserts a customer role
        /// </summary>
        /// <param name="customerRole">AccountUser role</param>
        public virtual void InsertAccountUserRole(AccountUserRole customerRole)
        {
            if (customerRole == null)
                throw new ArgumentNullException("customerRole");

            _customerRoleRepository.Insert(customerRole);

            _cacheManager.RemoveByPattern(CUSTOMERROLES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(customerRole);
        }

        /// <summary>
        /// Updates the customer role
        /// </summary>
        /// <param name="customerRole">AccountUser role</param>
        public virtual void UpdateAccountUserRole(AccountUserRole customerRole)
        {
            if (customerRole == null)
                throw new ArgumentNullException("customerRole");

            _customerRoleRepository.Update(customerRole);

            _cacheManager.RemoveByPattern(CUSTOMERROLES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(customerRole);
        }

        #endregion
    }
}
