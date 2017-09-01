using QZCHY.Core.Domain.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Services.Property
{
    public interface ICopyPropertyService
    {
        void DeleteCopyProperty(CopyProperty p);

        void InsertCopyProperty(CopyProperty p);

        void UpdateCopyProperty(CopyProperty p);

        CopyProperty GetCopyPropertyById(int id);
        CopyProperty GetCopyPropertyByPropertyId(int property_id);

    }
}
