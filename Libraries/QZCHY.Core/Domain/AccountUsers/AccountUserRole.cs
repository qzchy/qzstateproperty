using Microsoft.AspNet.Identity;
using QZCHY.Core.Domain.Security;
using System.Collections.Generic;

namespace QZCHY.Core.Domain.AccountUsers
{
    /// <summary>
    /// Represents a customer role
    /// </summary>
    public partial class AccountUserRole : BaseEntity, IRole<int>
    {
        //private ICollection<PermissionRecord> _permissionRecords;

        /// <summary>
        /// Gets or sets the customer role name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer role is active
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the customer role is system
        /// </summary>
        public bool IsSystemRole { get; set; }

        /// <summary>
        /// Gets or sets the customer role system name
        /// </summary>
        public string SystemName { get; set; }
    }
}
