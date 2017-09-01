using QZCHY.Core.Domain.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Data.Mapping.Properties
{
    public class PropertyOffPictureMap : EntityTypeConfiguration<PropertyOffPicture>
    {
        public PropertyOffPictureMap()
        {
            this.ToTable("PropertyOff_Picture_Mapping");
            this.HasKey(pp => pp.Id);

            this.HasRequired(pp => pp.Picture)
                .WithMany()
                .HasForeignKey(pp => pp.PictureId);


            this.HasRequired(pp => pp.PropertyOff)
                .WithMany(p => p.OffPictures)
                .HasForeignKey(pp => pp.PropertyOffId);
        }
    }
}
