using QZCHY.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QZCHY.API.Models.Properties
{

    public class PropertyPictureModel : BaseQMEntityModel
    {
        public int PictureId { get; set; }

        public int PropertyId { get; set; }

        public int DisplayOrder { get; set; }

        /// <summary>
        /// 图片SEO名称
        /// </summary>
        public string SeoFilename { get; set; }

        /// <summary>
        /// Gets or sets the "alt" attribute for "img" HTML element. If empty, then a default rule will be used (e.g. property name)
        /// </summary>
        public string Alt { get; set; }

        /// <summary>
        /// Gets or sets the "title" attribute for "img" HTML element. If empty, then a default rule will be used (e.g. property name)
        /// </summary>
        public string Title { get; set; }

        public string Href { get; set; }

        public bool IsLogo { get; set; }
    }

    public class PropertyFileModel : BaseQMEntityModel
    {
        public int FileId { get; set; }

        public int PropertyId { get; set; }

        /// <summary>
        /// Gets or sets the "title" attribute for "img" HTML element. If empty, then a default rule will be used (e.g. property name)
        /// </summary>
        public string Title { get; set; }

        public string Src { get; set; }

        public int Percentage { get; set; }

        public bool Uploaded { get; set; }
    }

    public class PropertyAllotPictureModel : BaseQMEntityModel
    {
        public int PictureId { get; set; }

        public int PropertyAllotId { get; set; }

        public int DisplayOrder { get; set; }

        /// <summary>
        /// 图片SEO名称
        /// </summary>
        public string SeoFilename { get; set; }

        /// <summary>
        /// Gets or sets the "alt" attribute for "img" HTML element. If empty, then a default rule will be used (e.g. property name)
        /// </summary>
        public string Alt { get; set; }

        /// <summary>
        /// Gets or sets the "title" attribute for "img" HTML element. If empty, then a default rule will be used (e.g. property name)
        /// </summary>
        public string Title { get; set; }

        public string Href { get; set; }
    }

    public class PropertyAllotFileModel : BaseQMEntityModel
    {
        public int FileId { get; set; }

        public int PropertyAllotId { get; set; }

        /// <summary>
        /// Gets or sets the "title" attribute for "img" HTML element. If empty, then a default rule will be used (e.g. property name)
        /// </summary>
        public string Title { get; set; }

        public string Src { get; set; }

        public int Percentage { get; set; }

        public bool Uploaded { get; set; }
    }

    public class PropertyLendPictureModel : BaseQMEntityModel
    {
        public int PictureId { get; set; }

        public int PropertyLendId { get; set; }

        public int DisplayOrder { get; set; }

        /// <summary>
        /// 图片SEO名称
        /// </summary>
        public string SeoFilename { get; set; }

        /// <summary>
        /// Gets or sets the "alt" attribute for "img" HTML element. If empty, then a default rule will be used (e.g. property name)
        /// </summary>
        public string Alt { get; set; }

        /// <summary>
        /// Gets or sets the "title" attribute for "img" HTML element. If empty, then a default rule will be used (e.g. property name)
        /// </summary>
        public string Title { get; set; }

        public string Href { get; set; }
    }

    public class PropertyLendFileModel : BaseQMEntityModel
    {
        public int FileId { get; set; }

        public int PropertyId { get; set; }

        /// <summary>
        /// Gets or sets the "title" attribute for "img" HTML element. If empty, then a default rule will be used (e.g. property name)
        /// </summary>
        public string Title { get; set; }

        public string Src { get; set; }

        public int Percentage { get; set; }

        public bool Uploaded { get; set; }
    }

    public class PropertyRentPictureModel : BaseQMEntityModel
    {
        public int PictureId { get; set; }

        public int PropertyRentId { get; set; }

        public int DisplayOrder { get; set; }

        /// <summary>
        /// 图片SEO名称
        /// </summary>
        public string SeoFilename { get; set; }

        /// <summary>
        /// Gets or sets the "alt" attribute for "img" HTML element. If empty, then a default rule will be used (e.g. property name)
        /// </summary>
        public string Alt { get; set; }

        /// <summary>
        /// Gets or sets the "title" attribute for "img" HTML element. If empty, then a default rule will be used (e.g. property name)
        /// </summary>
        public string Title { get; set; }

        public string Href { get; set; }

    }

    public class PropertyRentFileModel : BaseQMEntityModel
    {
        public int FileId { get; set; }

        public int PropertyRentId { get; set; }

        /// <summary>
        /// Gets or sets the "title" attribute for "img" HTML element. If empty, then a default rule will be used (e.g. property name)
        /// </summary>
        public string Title { get; set; }

        public string Src { get; set; }

        public int Percentage { get; set; }

        public bool Uploaded { get; set; }
    }

    public class PropertyOffPictureModel : BaseQMEntityModel
    {
        public int PictureId { get; set; }

        public int PropertyRentId { get; set; }

        public int DisplayOrder { get; set; }

        /// <summary>
        /// 图片SEO名称
        /// </summary>
        public string SeoFilename { get; set; }

        /// <summary>
        /// Gets or sets the "alt" attribute for "img" HTML element. If empty, then a default rule will be used (e.g. property name)
        /// </summary>
        public string Alt { get; set; }

        /// <summary>
        /// Gets or sets the "title" attribute for "img" HTML element. If empty, then a default rule will be used (e.g. property name)
        /// </summary>
        public string Title { get; set; }

        public string Href { get; set; }

    }

    public class PropertyOffFileModel : BaseQMEntityModel
    {
        public int FileId { get; set; }

        public int PropertyRentId { get; set; }

        /// <summary>
        /// Gets or sets the "title" attribute for "img" HTML element. If empty, then a default rule will be used (e.g. property name)
        /// </summary>
        public string Title { get; set; }

        public string Src { get; set; }

        public int Percentage { get; set; }

        public bool Uploaded { get; set; }
    }
}