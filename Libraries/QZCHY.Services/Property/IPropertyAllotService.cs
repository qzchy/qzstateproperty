using QZCHY.Core;
using QZCHY.Core.Domain.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Services.Property
{
    public interface IPropertyAllotService
    {
        IPagedList<QZCHY.Core.Domain.Properties.PropertyAllot> GetAllAllotRecords(IList<int> governmentIds, string checkState = "unchecked", string search = "", int pageIndex = 0,
            int pageSize = int.MaxValue, params PropertySortCondition[] sortConditions);

        void DeletePropertyAllot(PropertyAllot p);

        void InsertPropertyAllot(PropertyAllot p);
        void UpdatePropertyAllot(PropertyAllot p);

        PropertyAllot GetPropertyAllotById(int id);

        IList<PropertyAllot> GetAllotsByPropertyId(int id);

                #region Property pictures

        /// <summary>
        /// Deletes a propertyAllot picture
        /// </summary>
        /// <param name="propertyAllotPicture">Property picture</param>
        void DeletePropertyAllotPicture(PropertyAllotPicture propertyAllotPicture);

        /// <summary>
        /// Gets a propertyAllot pictures by propertyAllot identifier
        /// </summary>
        /// <param name="propertyAllotId">The propertyAllot identifier</param>
        /// <returns>Property pictures</returns>
        IList<PropertyAllotPicture> GetPropertyAllotPicturesByPropertyId(int propertyAllotId);

        /// <summary>
        /// Gets a propertyAllot picture
        /// </summary>
        /// <param name="propertyAllotPictureId">Property picture identifier</param>
        /// <returns>Property picture</returns>
        PropertyAllotPicture GetPropertyAllotPictureById(int propertyAllotPictureId);

        /// <summary>
        /// Inserts a propertyAllot picture
        /// </summary>
        /// <param name="propertyAllotPicture">Property picture</param>
        void InsertPropertyAllotPicture(PropertyAllotPicture propertyAllotPicture);

        /// <summary>
        /// Updates a propertyAllot picture
        /// </summary>
        /// <param name="propertyAllotPicture">Property picture</param>
        void UpdatePropertyAllotPicture(PropertyAllotPicture propertyAllotPicture);

        #endregion

        #region Property FILES

        /// <summary>
        /// Deletes a propertyAllot file
        /// </summary>
        /// <param name="propertyAllotFile">Property picture</param>
        void DeletePropertyAllotFile(PropertyAllotFile propertyAllotFile);

        /// <summary>
        /// Gets a propertyAllot files by propertyAllot identifier
        /// </summary>
        /// <param name="propertyAllotId">The propertyAllot identifier</param>
        /// <returns>Property files</returns>
        IList<PropertyAllotFile> GetPropertyFilesByPropertyAllotId(int propertyAllotId);

        /// <summary>
        /// Gets a propertyAllot file
        /// </summary>
        /// <param name="propertyFileId">Property file identifier</param>
        /// <returns>Property file</returns>
        PropertyAllotFile GetPropertyFileById(int propertyFileId);

        /// <summary>
        /// Inserts a propertyAllot file
        /// </summary>
        /// <param name="propertyAllotFile">Property file</param>
        void InsertPropertyFile(PropertyAllotFile propertyAllotFile);

        /// <summary>
        /// Updates a propertyAllot file
        /// </summary>
        /// <param name="propertyAllotFile">Property file</param>
        void UpdatePropertyFile(PropertyAllotFile propertyAllotFile);

        #endregion
    }
}
