using QZCHY.Core.Domain.AccountUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Services.AccountUsers
{
    public interface IAccountUserRegistrationService
    {
        /// <summary>
        /// 验证用户名和密码
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<AccountUser> ValidateAccountUserAsync(string account, string password);

        /// <summary>
        /// 验证微信小应用用户名密码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Task<AccountUser> ValidateWechatAppAccountUserAsync(string code);

        /// <summary>
        /// 用户名可以是手机、邮件以及用户名
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        AccountUserLoginResults ValidateAccountUser(string account, string password);

        /// <summary>
        /// 用户注册请求
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        AccountUserRegistrationResult RegisterAccountUser(AccountUserRegistrationRequest request);
    }
}
