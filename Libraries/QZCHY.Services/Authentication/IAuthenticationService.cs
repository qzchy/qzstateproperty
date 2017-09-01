using QZCHY.Core.Domain.AccountUsers;

namespace QZCHY.Services.Authentication
{
    /// <summary>
    /// Authentication service interface
    /// </summary>
    public partial interface IAuthenticationService
    {
        //void SignIn(AccountUser property, bool createPersistentCookie);

        void SignOut();

        AccountUser GetAuthenticatedAccountUser();
    }
}
