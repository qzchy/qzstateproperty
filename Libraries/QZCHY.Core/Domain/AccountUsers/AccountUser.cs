using Microsoft.AspNet.Identity;
using QZCHY.Core.Domain.Common;
using QZCHY.Core.Domain.Properties;
using System;
using System.Collections.Generic;

namespace QZCHY.Core.Domain.AccountUsers
{

    public class AccountUser : BaseEntity, IUser<int>
    {
        private ICollection<AccountUserRole> _customerRoles;

        public AccountUser()
        {
            this.AccountUserGuid = Guid.NewGuid();          
        }
        
        public string UserName { get; set; }

        public string NickName { get; set; }

        public bool Active { get; set; }

        public Guid AccountUserGuid { get; set; }
           
        public string LastIpAddress { get; set; }

        public DateTime? LastActivityDate { get; set; }

        public DateTime? LastLoginDate { get; set; }        

        public string Password { get; set; }

        public int PasswordFormatId { get;  set; }

        public PasswordFormat PasswordFormat
        {
            get { return (PasswordFormat)PasswordFormatId; }
            set { this.PasswordFormatId = (int)value; }
        }

        public string PasswordSalt { get; set; }

        public bool IsSystemAccount { get; set; }

        public string SystemName { get; set; }

        public int DisplayOrder { get; set; }

        public string Remark { get; set; }

        /// <summary>
        /// Gets or sets the customer roles
        /// </summary>
        public virtual ICollection<AccountUserRole> AccountUserRoles
        {
            get { return _customerRoles ?? (_customerRoles = new List<AccountUserRole>()); }
            protected set { _customerRoles = value; }
        }

        /// <summary>
        /// 所属单位
        /// </summary>
        public virtual GovernmentUnit Government { get; set; }
    }
}
