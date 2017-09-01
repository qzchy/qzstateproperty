namespace QZCHY.Core.Domain.Media
{
    public class File:BaseEntity
    {
        /// <summary>
        /// 文件名
        /// </summary>
        public string Name { get; set; }

        public string Extension { get; set; }

        /// <summary>
        /// Gets or sets the SEO friednly filename of the file
        /// </summary>
        public string SeoFilename { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the file is new
        /// </summary>
        public bool IsNew { get; set; }
    }
}
