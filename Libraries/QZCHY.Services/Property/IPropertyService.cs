using QZCHY.Core;
using QZCHY.Core.Domain.Properties;
using System.Collections.Generic;
using System.Linq;

namespace QZCHY.Services.Property
{
    public interface IPropertyService
    {
        /// <summary>
        /// Delete a property
        /// </summary>
        /// <param name="property">Property</param>
        void DeleteProperty(QZCHY.Core.Domain.Properties.Property property);

        /// <summary>
        /// Gets a property
        /// </summary>
        /// <param name="propertyId">Property identifier</param>
        /// <returns>A property</returns>
        QZCHY.Core.Domain.Properties.Property GetPropertyById(int propertyId);

        IList<QZCHY.Core.Domain.Properties.Property> GetAllProperties();

        /// <summary>
        /// 获取资产列表
        /// </summary>
        /// <param name="search"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="showHidden"></param>
        /// <param name="sortConditions"></param>
        /// <returns></returns>
        IPagedList<QZCHY.Core.Domain.Properties.Property> GetAllProperties(IList<int> governmentIds, string search = "",int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false, PropertyAdvanceConditionRequest advanceCondition=null, params PropertySortCondition[] sortConditions);


        IList<QZCHY.Core.Domain.Properties.Property> GetAllProcessProperties(IList<int> governmentIds, string search = "", PropertyAdvanceConditionRequest advanceCondition = null, params PropertySortCondition[] sortConditions);

        IQueryable<QZCHY.Core.Domain.Properties.Property> GetAllProperties(IList<int> governmentIds, bool showHidden = true);

        IQueryable<QZCHY.Core.Domain.Properties.Property> GetPropertiesByGovernmentId(IList<int> governmentIds);

        IList<QZCHY.Core.Domain.Properties.Property> GetPropertiesByGId(int id);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="property"></param>
        void UpdateProperty(QZCHY.Core.Domain.Properties.Property property);

        /// <summary>
        /// Insert a property
        /// </summary>
        /// <param name="property">Property</param>
        void InsertProperty(QZCHY.Core.Domain.Properties.Property property);

        /// <summary>
        /// 名称唯一性检查
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>Picture
        bool NameUniqueCheck(string propertyName);

        IList<QZCHY.Core.Domain.Properties.Property> GetPropertyProcess(int governmentId);

        /// <summary>
        /// 获取所有可审批的资产
        /// </summary>
        /// <returns></returns>
        IList<QZCHY.Core.Domain.Properties.Property> GetProcessProperties(IList<int> governmentIds);

        #region Property pictures

        /// <summary>
        /// Deletes a property picture
        /// </summary>
        /// <param name="propertyPicture">Property picture</param>
        void DeletePropertyPicture(PropertyPicture propertyPicture);

        /// <summary>
        /// Gets a property pictures by property identifier
        /// </summary>
        /// <param name="propertyId">The property identifier</param>
        /// <returns>Property pictures</returns>
        IList<PropertyPicture> GetPropertyPicturesByPropertyId(int propertyId);

        /// <summary>
        /// Gets a property picture
        /// </summary>
        /// <param name="propertyPictureId">Property picture identifier</param>
        /// <returns>Property picture</returns>
        PropertyPicture GetPropertyPictureById(int propertyPictureId);

        /// <summary>
        /// Inserts a property picture
        /// </summary>
        /// <param name="propertyPicture">Property picture</param>
        void InsertPropertyPicture(PropertyPicture propertyPicture);

        /// <summary>
        /// Updates a property picture
        /// </summary>
        /// <param name="propertyPicture">Property picture</param>
        void UpdatePropertyPicture(PropertyPicture propertyPicture);

        #endregion

        #region Property FILES

        /// <summary>
        /// Deletes a property file
        /// </summary>
        /// <param name="propertyFile">Property picture</param>
        void DeletePropertyFile(PropertyFile propertyFile);

        /// <summary>
        /// Gets a property files by property identifier
        /// </summary>
        /// <param name="propertyId">The property identifier</param>
        /// <returns>Property files</returns>
        IList<PropertyFile> GetPropertyFilesByPropertyId(int propertyId);

        /// <summary>
        /// Gets a property file
        /// </summary>
        /// <param name="propertyFileId">Property file identifier</param>
        /// <returns>Property file</returns>
        PropertyFile GetPropertyFileById(int propertyFileId);

        /// <summary>
        /// Inserts a property file
        /// </summary>
        /// <param name="propertyFile">Property file</param>
        void InsertPropertyFile(PropertyFile propertyFile);

        /// <summary>
        /// Updates a property file
        /// </summary>
        /// <param name="propertyFile">Property file</param>
        void UpdatePropertyFile(PropertyFile propertyFile);

        #endregion
         
    }
}
