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
using QZCHY.Core.Domain.Properties;

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
        private const string CUSTOMERROLES_ALL_KEY = "QM.accountUserrole.all-{0}";
        /// <summary>
        /// Key for caching
        /// </summary>
        /// <remarks>
        /// {0} : system name
        /// </remarks>
        private const string CUSTOMERROLES_BY_SYSTEMNAME_KEY = "QM.accountUserrole.systemname-{0}";
        /// <summary>
        /// Key pattern to clear cache
        /// </summary>
        private const string CUSTOMERROLES_PATTERN_KEY = "QM.accountUserrole.";

        #endregion

        #region Fields

        private readonly IRepository<AccountUser> _accountUserRepository;
        private readonly IRepository<AccountUserRole> _accountUserRoleRepository;
        private readonly IRepository<GenericAttribute> _gaRepository;
        private readonly IDataProvider _dataProvider;
        private readonly IDbContext _dbContext;
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly AccountUserSettings _accountUserSettings;
        private readonly CommonSettings _commonSettings;
        
        #endregion

        #region 构造函数

        public AccountUserService(ICacheManager cacheManager,
            IRepository<AccountUser> accountUserRepository,
            IRepository<AccountUserRole> accountUserRoleRepository,
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
        AccountUserSettings accountUserSettings,
            CommonSettings commonSettings)
        {
            this._cacheManager = cacheManager;
            this._accountUserRepository = accountUserRepository;
            this._accountUserRoleRepository = accountUserRoleRepository;
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

            this._accountUserSettings = accountUserSettings;
            this._commonSettings = commonSettings;
        }

        #endregion

        #region IAccountUser 实现
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="accountUser"></param>
        public void DeleteAccountUser(AccountUser accountUser)
        {
            if (accountUser == null)
                throw new ArgumentNullException("accountUser");

            if (accountUser.IsSystemAccount)
                throw new QZCHYException(string.Format("System accountUser account ({0}) could not be deleted", accountUser.SystemName));

            accountUser.Deleted = true;          

            if (_accountUserSettings.SuffixDeletedAccountUsers) accountUser.UserName += "-DELETED";

            UpdateAccountUser(accountUser);
        }

        public void DeleteGovernmentUsers(GovernmentUnit governmentUnit)
        {
            if(governmentUnit==null )
                throw new ArgumentNullException("要删除的部门不存在");

            if(governmentUnit.Deleted)
            {
                var query = from au in _accountUserRepository.Table
                            where au.Government.Id == governmentUnit.Id
                            select au;

                var accountUsers = query.ToList();

               foreach(var accountUser in accountUsers)
                {
                    DeleteAccountUser(accountUser);
                }
            }
        }

        /// <summary>
        /// 通过id获取用户
        /// </summary>
        /// <param name="accountUserId"></param>
        /// <returns></returns>
        public AccountUser GetAccountUserById(int accountUserId)
        {
            if (accountUserId == 0)
                return null;

            return _accountUserRepository.GetById(accountUserId);
        }

        public virtual IList<AccountUser> GetAccountUsersByIds(int[] accountUserIds)
        {
            if (accountUserIds == null || accountUserIds.Length == 0)
                return new List<AccountUser>();

            var query = from c in _accountUserRepository.Table
                        where accountUserIds.Contains(c.Id) && !c.Deleted
                        select c;
            var accountUsers = query.ToList();
            //sort by passed identifiers
            var sortedAccountUsers = new List<AccountUser>();
            foreach (int id in accountUserIds)
            {
                var accountUser = accountUsers.Find(x => x.Id == id);
                if (accountUser != null)
                    sortedAccountUsers.Add(accountUser);
            }
            return sortedAccountUsers;
        }


        public AccountUser GetAccountUserByGuid(Guid accountUserGuid)
        {
            if (accountUserGuid == Guid.Empty)
                return null;

            var query = from c in _accountUserRepository.Table
                        orderby c.Id
                        where c.AccountUserGuid == accountUserGuid && !c.Deleted
                        select c;

            var accountUser = query.FirstOrDefault();
            return accountUser;
        }

        public AccountUser GetAccountUserBySystemName(string systemName)
        {
            if (string.IsNullOrWhiteSpace(systemName))
                return null;

            var query = from c in _accountUserRepository.Table
                        orderby c.Id
                        where c.SystemName == systemName && !c.Deleted
                        select c;
            var accountUser = query.FirstOrDefault();
            return accountUser;
        }

        /// <summary>
        /// Get accountUser by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>AccountUser</returns>
        public virtual AccountUser GetAccountUserByUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
                return null;

            var query = from c in _accountUserRepository.Table
                        orderby c.Id
                        where c.UserName == username && !c.Deleted
                        select c;

            var accountUser = query.FirstOrDefault();
            return accountUser;
        }

        /// <summary>
        /// 新增客户
        /// </summary>
        /// <param name="accountUser"></param>
        public void InsertAccountUser(AccountUser accountUser)
        {
            if (accountUser == null)
                throw new ArgumentNullException("accountUser");

            _accountUserRepository.Insert(accountUser);

            //event notification
            _eventPublisher.EntityInserted(accountUser);
        }

        public IPagedList<AccountUser> GetAllAccountUsers(string search = "", int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false, params PropertySortCondition[] sortConditions)
        {
            var query = _accountUserRepository.Table;

            query = query.Where(m => !m.Deleted);           

            //实现查询
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(e => e.UserName.Contains(search) || e.NickName.Contains(search) || e.Government.Name.Contains(search));
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

            var accountUsers = new PagedList<AccountUser>(query, pageIndex, pageSize);
            return accountUsers;
        }


        public void UpdateAccountUser(AccountUser accountUser)
        {
            if (accountUser == null)
                throw new ArgumentNullException("accountUser");

            _accountUserRepository.Update(accountUser);

            //event notification
            _eventPublisher.EntityUpdated(accountUser);
        }

        /// <summary>
        /// 名称唯一性检查
        /// </summary>
        /// <param name="accountUserName"></param>
        /// <returns></returns>
        public bool NameUniqueCheck(string accountUserName, int accountUserId = 0)
        {
            var query = _accountUserRepository.Table;
            query = query.Where(c => !c.Deleted);

            if (!String.IsNullOrWhiteSpace(accountUserName))
            {
                var accountUser = query.Where(c => c.UserName == accountUserName).FirstOrDefault();
                if (accountUser == null) return true;
                else
                    return accountUser.Id == accountUserId;
            }
            else return true;
        }

        #endregion

        #region AccountUser roles

        /// <summary>
        /// Delete a accountUser role
        /// </summary>
        /// <param name="accountUserRole">AccountUser role</param>
        public virtual void DeleteAccountUserRole(AccountUserRole accountUserRole)
        {
            if (accountUserRole == null)
                throw new ArgumentNullException("accountUserRole");

            if (accountUserRole.IsSystemRole)
                throw new QZCHYException("System role could not be deleted");

            _accountUserRoleRepository.Delete(accountUserRole);

            _cacheManager.RemoveByPattern(CUSTOMERROLES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(accountUserRole);
        }

        /// <summary>
        /// Gets a accountUser role
        /// </summary>
        /// <param name="accountUserRoleId">AccountUser role identifier</param>
        /// <returns>AccountUser role</returns>
        public virtual AccountUserRole GetAccountUserRoleById(int accountUserRoleId)
        {
            if (accountUserRoleId == 0)
                return null;

            return _accountUserRoleRepository.GetById(accountUserRoleId);
        }

        /// <summary>
        /// Gets a accountUser role
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
                var query = from cr in _accountUserRoleRepository.Table
                            orderby cr.Id
                            where cr.SystemName == systemName
                            select cr;
                var accountUserRole = query.FirstOrDefault();
                return accountUserRole;
            });
        }

        /// <summary>
        /// Gets all accountUser roles
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>AccountUser roles</returns>
        public virtual IList<AccountUserRole> GetAllAccountUserRoles(bool showHidden = false)
        {
            string key = string.Format(CUSTOMERROLES_ALL_KEY, showHidden);
            return _cacheManager.Get(key, () =>
        {
                var query = from cr in _accountUserRoleRepository.Table
                            orderby cr.Name
                            where (showHidden || cr.Active)
                            select cr;
                var accountUserRoles = query.ToList();
                return accountUserRoles;
            });
        }

        /// <summary>
        /// Inserts a accountUser role
        /// </summary>
        /// <param name="accountUserRole">AccountUser role</param>
        public virtual void InsertAccountUserRole(AccountUserRole accountUserRole)
        {
            if (accountUserRole == null)
                throw new ArgumentNullException("accountUserRole");

            _accountUserRoleRepository.Insert(accountUserRole);

            _cacheManager.RemoveByPattern(CUSTOMERROLES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(accountUserRole);
        }

        /// <summary>
        /// Updates the accountUser role
        /// </summary>
        /// <param name="accountUserRole">AccountUser role</param>
        public virtual void UpdateAccountUserRole(AccountUserRole accountUserRole)
        {
            if (accountUserRole == null)
                throw new ArgumentNullException("accountUserRole");

            _accountUserRoleRepository.Update(accountUserRole);

            _cacheManager.RemoveByPattern(CUSTOMERROLES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(accountUserRole);
        }



        #endregion
    }
}
