using QZCHY.Core.Domain.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Core.Domain.Properties
{
    public class PropertyAllotFile : BaseEntity
    {
        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
        public int PropertyAllotId { get; set; }

        /// <summary>
        /// Gets or sets the file identifier
        /// </summary>
        public int FileId { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets the file
        /// </summary>
        public virtual File File { get; set; }

        /// <summary>
        /// Gets the product
        /// </summary>
        public virtual PropertyAllot PropertyAllot { get; set; }
    }
}
