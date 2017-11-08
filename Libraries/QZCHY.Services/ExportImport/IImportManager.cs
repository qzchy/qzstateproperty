using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Services.ExportImport
{
    public interface IImportManager
    {
        /// <summary>
        /// Import products from XLSX file
        /// </summary>
        /// <param name="stream">Stream</param>
        ImportResponse ImportProductsFromXlsx(Stream stream,string path);
    }
}
