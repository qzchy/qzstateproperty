using QZCHY.Core;
using QZCHY.Core.Domain.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Services.Property
{
    public interface IPropertyOffService
    {
        IPagedList<QZCHY.Core.Domain.Properties.PropertyOff> GetAllOffRecords(IList<int> governmentIds, string checkState = "unchecked", string search = "", int pageIndex = 0,
            int pageSize = int.MaxValue, params PropertySortCondition[] sortConditions);

        void DeletePropertyOff(PropertyOff p);

        void InsertPropertyOff(PropertyOff p);
        void UpdatePropertyOff(PropertyOff p);

        PropertyOff GetPropertyOffById(int id);

        PropertyOff GetPropertyOffByPropertyId(int property_Id);

        IList<PropertyOff> GetOffsByPropertyId(int id);

        #region Property pictures

        /// <summary>
        /// Deletes a propertyOff picture
        /// </summary>
        /// <param name="propertyOffPicture">Property picture</param>
        void DeletePropertyOffPicture(PropertyOffPicture propertyOffPicture);

        /// <summary>
        /// Gets a propertyOff pictures by propertyOff identifier
        /// </summary>
        /// <param name="propertyOffId">The propertyOff identifier</param>
        /// <returns>Property pictures</returns>
        IList<PropertyOffPicture> GetPropertyOffPicturesByPropertyId(int propertyOffId);

        /// <summary>
        /// Gets a propertyOff picture
        /// </summary>
        /// <param name="propertyOffPictureId">Property picture identifier</param>
        /// <returns>Property picture</returns>
        PropertyOffPicture GetPropertyOffPictureById(int propertyOffPictureId);

        /// <summary>
        /// Inserts a propertyOff picture
        /// </summary>
        /// <param name="propertyOffPicture">Property picture</param>
        void InsertPropertyOffPicture(PropertyOffPicture propertyOffPicture);

        /// <summary>
        /// Updates a propertyOff picture
        /// </summary>
        /// <param name="propertyOffPicture">Property picture</param>
        void UpdatePropertyOffPicture(PropertyOffPicture propertyOffPicture);

        #endregion

        #region Property FILES

        /// <summary>
        /// Deletes a propertyOff file
        /// </summary>
        /// <param name="propertyOffFile">Property picture</param>
        void DeletePropertyOffFile(PropertyOffFile propertyOffFile);

        /// <summary>
        /// Gets a propertyOff files by propertyOff identifier
        /// </summary>
        /// <param name="propertyOffId">The propertyOff identifier</param>
        /// <returns>Property files</returns>
        IList<PropertyOffFile> GetPropertyFilesByPropertyOffId(int propertyOffId);

        /// <summary>
        /// Gets a propertyOff file
        /// </summary>
        /// <param name="propertyFileId">Property file identifier</param>
        /// <returns>Property file</returns>
        PropertyOffFile GetPropertyFileById(int propertyFileId);

        /// <summary>
        /// Inserts a propertyOff file
        /// </summary>
        /// <param name="propertyOffFile">Property file</param>
        void InsertPropertyFile(PropertyOffFile propertyOffFile);

        /// <summary>
        /// Updates a propertyOff file
        /// </summary>
        /// <param name="propertyOffFile">Property file</param>
        void UpdatePropertyFile(PropertyOffFile propertyOffFile);

        #endregion
    }
}
