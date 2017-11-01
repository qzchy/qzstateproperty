using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Services.ExportImport
{
    public interface IExportManager
    {
        /// <summary>
        /// Export products to XLSX
        /// </summary>
        /// <param name="stream">Stream</param>
        /// <param name="properties">Properties</param>
        void ExportPropertyToXlsx(Stream stream, IList<QZCHY.Core.Domain.Properties.Property> properties, IList<string> headers);
    }
}