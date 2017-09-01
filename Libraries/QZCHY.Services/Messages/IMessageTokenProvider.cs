using QZCHY.Core;
using QZCHY.Core.Domain.AccountUsers;
using QZCHY.Core.Domain.Messages;
using QZCHY.Services.Events;
using System.Collections.Generic;

namespace QZCHY.Services.Messages
{
    public partial interface IMessageTokenProvider
    {


        void AddStoreTokens(IList<Token> tokens, EmailAccount emailAccount);

        void AddAccountUserTokens(IList<Token> tokens, AccountUser property);
    }
}