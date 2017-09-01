using QZCHY.Core.Domain.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Data.Mapping.Properties
{
    public class PropertyLendPictureMap : EntityTypeConfiguration<PropertyLendPicture>
    {
        public PropertyLendPictureMap()
        {
            this.ToTable("PropertyLend_Picture_Mapping");
            this.HasKey(pp => pp.Id);

            this.HasRequired(pp => pp.Picture)
                .WithMany()
                .HasForeignKey(pp => pp.PictureId);


            this.HasRequired(pp => pp.PropertyLend)
                .WithMany(p => p.LendPictures)
                .HasForeignKey(pp => pp.PropertyLendId);
        }
    }
}
