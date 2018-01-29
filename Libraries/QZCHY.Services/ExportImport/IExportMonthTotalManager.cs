using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Services.ExportImport
{
   public  interface IExportMonthTotalManager
    {

        void ExportMonthTotal(Stream stream,List<QZCHY.Core.Domain.Properties.Property> properties);
    }
}
