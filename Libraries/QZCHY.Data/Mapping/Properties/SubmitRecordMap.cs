using QZCHY.Core.Domain.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Data.Mapping.Properties
{
   public class SubmitRecordMap:EntityTypeConfiguration<SubmitRecord>
    {

        public SubmitRecordMap()
        {
            this.ToTable("SubmitRecord");
            this.HasKey(m => m.Id);
            this.Property(m => m.Goverment_ID).IsRequired();
        }
    }
}
