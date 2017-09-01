using QZCHY.Core.Domain.AccountUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Services.AccountUsers
{
    public class AccountUserRegistrationRequest
    {
        public AccountUser AccountUser { get; set; }

        public string Email { get; set; }

        public string MobilePhone { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public PasswordFormat PasswordFormat { get; set; }

        public bool IsApproved { get; set; }

        public AccountUserRegistrationRequest(AccountUser customer ,string email,string mobilePhone,string username,string password,PasswordFormat passwordFormat,
            bool isApproved)
        {
            this.AccountUser = customer;
            this.Email = email;
            this.MobilePhone = mobilePhone;
            this.Username = username;
            this.Password = password;
            this.PasswordFormat = passwordFormat;
            this.IsApproved = IsApproved;
        }
    }
}
