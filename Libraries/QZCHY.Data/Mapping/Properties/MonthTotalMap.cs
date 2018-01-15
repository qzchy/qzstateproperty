using QZCHY.Core.Domain.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Data.Mapping.Properties
{
   public  class MonthTotalMap:EntityTypeConfiguration<MonthTotal>
    {

        public MonthTotalMap()
        {
            this.ToTable("MonthTotal");
            this.HasKey(m=>m.Id);
            this.Property(m => m.Property_ID).IsRequired();
        }

    }
}
