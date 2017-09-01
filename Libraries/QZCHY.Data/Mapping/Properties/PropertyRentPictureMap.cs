using QZCHY.Core.Domain.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Data.Mapping.Properties
{
    public class PropertyRentPictureMap : EntityTypeConfiguration<PropertyRentPicture>
    {
        public PropertyRentPictureMap()
        {
            this.ToTable("PropertyRent_Picture_Mapping");
            this.HasKey(pp => pp.Id);

            this.HasRequired(pp => pp.Picture)
                .WithMany()
                .HasForeignKey(pp => pp.PictureId);


            this.HasRequired(pp => pp.PropertyRent)
                .WithMany(p => p.RentPictures)
                .HasForeignKey(pp => pp.PropertyRentId);
        }
    }
}
