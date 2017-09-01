using QZCHY.Core;
using QZCHY.Core.Domain.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Services.Property
{
   public interface IPropertyLendService
    {
        IPagedList<QZCHY.Core.Domain.Properties.PropertyLend> GetAllLendRecords(IList<int> governmentIds, string checkState = "unchecked", string search = "", int pageIndex = 0,
    int pageSize = int.MaxValue, params PropertySortCondition[] sortConditions);

        void DeletePropertyLend(PropertyLend p);

        void InsertPropertyLend(PropertyLend p);
        void UpdatePropertyLend(PropertyLend p);

        PropertyLend GetPropertyLendById(int id);

        IList<PropertyLend> GetLendsByPropertyId(int id);

        #region Property pictures

        /// <summary>
        /// Deletes a propertyLend picture
        /// </summary>
        /// <param name="propertyLendPicture">Property picture</param>
        void DeletePropertyLendPicture(PropertyLendPicture propertyLendPicture);

        /// <summary>
        /// Gets a propertyLend pictures by propertyLend identifier
        /// </summary>
        /// <param name="propertyLendId">The propertyLend identifier</param>
        /// <returns>Property pictures</returns>
        IList<PropertyLendPicture> GetPropertyLendPicturesByPropertyId(int propertyLendId);

        /// <summary>
        /// Gets a propertyLend picture
        /// </summary>
        /// <param name="propertyLendPictureId">Property picture identifier</param>
        /// <returns>Property picture</returns>
        PropertyLendPicture GetPropertyLendPictureById(int propertyLendPictureId);

        /// <summary>
        /// Inserts a propertyLend picture
        /// </summary>
        /// <param name="propertyLendPicture">Property picture</param>
        void InsertPropertyLendPicture(PropertyLendPicture propertyLendPicture);

        /// <summary>
        /// Updates a propertyLend picture
        /// </summary>
        /// <param name="propertyLendPicture">Property picture</param>
        void UpdatePropertyLendPicture(PropertyLendPicture propertyLendPicture);

        #endregion

        #region Property FILES

        /// <summary>
        /// Deletes a propertyLend file
        /// </summary>
        /// <param name="propertyLendFile">Property picture</param>
        void DeletePropertyLendFile(PropertyLendFile propertyLendFile);

        /// <summary>
        /// Gets a propertyLend files by propertyLend identifier
        /// </summary>
        /// <param name="propertyLendId">The propertyLend identifier</param>
        /// <returns>Property files</returns>
        IList<PropertyLendFile> GetPropertyFilesByPropertyLendId(int propertyLendId);

        /// <summary>
        /// Gets a propertyLend file
        /// </summary>
        /// <param name="propertyFileId">Property file identifier</param>
        /// <returns>Property file</returns>
        PropertyLendFile GetPropertyFileById(int propertyFileId);

        /// <summary>
        /// Inserts a propertyLend file
        /// </summary>
        /// <param name="propertyLendFile">Property file</param>
        void InsertPropertyFile(PropertyLendFile propertyLendFile);

        /// <summary>
        /// Updates a propertyLend file
        /// </summary>
        /// <param name="propertyLendFile">Property file</param>
        void UpdatePropertyFile(PropertyLendFile propertyLendFile);

        #endregion
    }
}
