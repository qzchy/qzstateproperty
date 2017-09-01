using FluentValidation.Attributes;
using QZCHY.API.Validators.AccountUsers;
using QZCHY.Core.Domain.AccountUsers;
using QZCHY.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace QZCHY.API.Models.AccountUsers
{
    public class AccountUserModel : BaseQMEntityModel
    {
 
        public string UserName { get; set; }

        public string NickName { get; set; }

        /// <summary>
        /// 是否初始化密码
        /// </summary>
        public bool InitPassword { get; set; }       

        public int GovernmentId { get; set; }

        public string GovernmentName { get; set; }

        public bool Active { get; set; }

        public bool IsAdministrator { get; set; }

        public string RoleName { get; set; }

        public string LastIpAddress { get; set; }

        public string LastActivityDate { get; set; }

        public string LastLoginDate { get; set; }

        public string Remark { get; set; }

        public string RoleList { get
            {
                if (this.AccountUserRoles == null) return "";
                StringBuilder sb = new StringBuilder();
                foreach (var role in this.AccountUserRoles)
                {
                    sb.Append(role.Name + ",");
                }

                return sb.ToString().TrimEnd(',');
            }
        }

        public ICollection<AccountUserRoleModel> AccountUserRoles { get; set; }
    }
}