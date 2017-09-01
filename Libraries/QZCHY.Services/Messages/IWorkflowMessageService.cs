using QZCHY.Core.Domain.AccountUsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Services.Messages
{
    public interface IWorkflowMessageService
    {
        #region AccountUser workflow

        /// <summary>
        /// Sends 'New property' notification message to a store owner
        /// </summary>
        /// <param name="property">AccountUser instance</param>
        /// <returns>Queued email identifier</returns>
        int SendAccountUserRegisteredNotificationMessage(AccountUser property);

        /// <summary>
        /// Sends a welcome message to a property
        /// </summary>
        /// <param name="property">AccountUser instance</param>
        /// <returns>Queued email identifier</returns>
        //int SendAccountUserWelcomeMessage(AccountUser property);

        /// <summary>
        /// Sends an email validation message to a property
        /// </summary>
        /// <param name="property">AccountUser instance</param>
        /// <returns>Queued email identifier</returns>
        //int SendAccountUserEmailValidationMessage(AccountUser property);

        ///// <summary>
        ///// Sends password recovery message to a property
        ///// </summary>
        ///// <param name="property">AccountUser instance</param>
        ///// <returns>Queued email identifier</returns>
        //int SendAccountUserPasswordRecoveryMessage(AccountUser property);

        #endregion
    }
}
