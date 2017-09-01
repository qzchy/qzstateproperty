using QZCHY.Core.Domain.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QZCHY.Services.Media
{
    /// <summary>
    /// 文件服务
    /// </summary>
    public interface IFileService
    {
        string GetFileUrl(File file);

        string GetFileUrl(int fileId);

        /// <summary>
        /// Gets a file
        /// </summary>
        /// <param name="fileId">File identifier</param>
        /// <returns>File</returns>
        File GetFileById(int fileId);

        /// <summary>
        /// Deletes a file
        /// </summary>
        /// <param name="file">File</param>
        void DeleteFile(File file);

        /// <summary>
        /// Gets files by property identifier
        /// </summary>
        /// <param name="propertyId">Product identifier</param>
        /// <param name="recordsToReturn">Number of records to return. 0 if you want to get all items</param>
        /// <returns>Files</returns>
        IList<File> GetFilesByPropertyId(int propertyId, int recordsToReturn = 0);

        /// <summary>
        /// Inserts a file
        /// </summary>
        /// <returns>File</returns>
        File InsertFile(byte[] fileBinary, string fileName, string fileExtension, string seoFilename, bool isNew = true);

        /// <summary>
        /// Updates the file
        /// </summary>
        /// <param name="fileId">The file identifier</param>
        /// <param name="fileBinary">The file binary</param>
        /// <param name="mimeType">The file MIME type</param>
        /// <param name="seoFilename">The SEO filename</param>
        /// <param name="altAttribute">"alt" attribute for "img" HTML element</param>
        /// <param name="titleAttribute">"title" attribute for "img" HTML element</param>
        /// <param name="isNew">A value indicating whether the file is new</param>
        /// <returns>File</returns>
        File UpdateFile(int fileId, byte[] fileBinary, string fileName, string fileExtension,
            string seoFilename, bool isNew = true);

        /// <summary>
        /// Updates a SEO filename of a file
        /// </summary>
        /// <param name="fileId">The file identifier</param>
        /// <param name="seoFilename">The SEO filename</param>
        /// <returns>File</returns>
        File SetSeoFilename(int fileId, string seoFilename);
    }
}
