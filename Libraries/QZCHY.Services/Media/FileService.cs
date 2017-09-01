using ImageResizer;
using QZCHY.Core;
using QZCHY.Core.Data;
using QZCHY.Core.Domain.Media;
using QZCHY.Data;
using QZCHY.Services.Configuration;
using QZCHY.Services.Events;
using QZCHY.Services.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using File = QZCHY.Core.Domain.Media.File;

namespace QZCHY.Services.Media
{
    /// <summary>
    /// 图片服务
    /// </summary>
    public partial class FileService : IFileService
    {
        #region 常量

        private const int MULTIPLE_THUMB_DIRECTORIES_LENGTH = 3;

        #endregion

        #region Fields

        private static readonly object s_lock = new object();

        private readonly IRepository<File> _fileRepository;
        private readonly ISettingService _settingService;
        private readonly IWebHelper _webHelper;
        private readonly ILogger _logger;
        private readonly IDbContext _dbContext;
        private readonly IEventPublisher _eventPublisher;
        private readonly MediaSettings _mediaSettings;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="fileRepository">File repository</param>
        /// <param name="fileFileRepository">Product file repository</param>
        /// <param name="settingService">Setting service</param>
        /// <param name="webHelper">Web helper</param>
        /// <param name="logger">Logger</param>
        /// <param name="dbContext">Database context</param>
        /// <param name="eventPublisher">Event publisher</param>
        /// <param name="mediaSettings">Media settings</param>
        public FileService(IRepository<File> fileRepository,
            ISettingService settingService,
            IWebHelper webHelper,
            ILogger logger,
            IDbContext dbContext,
            IEventPublisher eventPublisher,
            MediaSettings mediaSettings)
        {
            this._fileRepository = fileRepository;
            this._settingService = settingService;
            this._webHelper = webHelper;
            this._logger = logger;
            this._dbContext = dbContext;
            this._eventPublisher = eventPublisher;
            this._mediaSettings = mediaSettings;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Loads a file from file
        /// </summary>
        /// <param name="fileId">File identifier</param>
        /// <param name="path">MIME type</param>
        /// <returns>File binary</returns>
        protected virtual byte[] LoadFileFromLocation(int fileId, string filePath)
        {
            if (!System.IO.File.Exists(filePath))
                return new byte[0];
            return System.IO.File.ReadAllBytes(filePath);
        }

        /// <summary>
        /// Save file on file system
        /// </summary>
        /// <param name="fileId">File identifier</param>
        /// <param name="fileBinary">File binary</param>
        /// <param name="fileExtension">扩展名</param>
        protected virtual void SaveFileInSystem(int fileId, byte[] fileBinary, string fileExtension)
        {
            string fileName = string.Format("{0}_0{1}", fileId.ToString("0000000"), fileExtension);
            System.IO.File.WriteAllBytes(GetFileLocalPath(fileName), fileBinary);
        }

        /// <summary>
        /// Delete a file on file system
        /// </summary>
        /// <param name="file">File</param>
        protected virtual void DeleteFileOnFileSystem(File file)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            string fileName = string.Format("{0}_0{1}", file.Id.ToString("0000000"), file.Extension);
            string filePath = GetFileLocalPath(fileName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        /// <summary>
        /// Get file local path. Used when images stored on file system (not in the database)
        /// </summary>
        /// <param name="fileName">Filename</param>
        /// <param name="filesDirectoryPath">Directory path with images; if null, then default one is used</param>
        /// <returns>Local file path</returns>
        protected virtual string GetFileLocalPath(string fileName, string filesDirectoryPath = null)
        {
            if (String.IsNullOrEmpty(filesDirectoryPath))
            {
                filesDirectoryPath = _webHelper.MapPath("~/content/files/");
            }
            if (!Directory.Exists(filesDirectoryPath)) Directory.CreateDirectory(filesDirectoryPath);
            var filePath = Path.Combine(filesDirectoryPath, fileName);
            return filePath;
        }


        #endregion

        #region Getting file local path/URL methods
        /// <summary>
        /// Get a file URL
        /// </summary>
        /// <param name="file">File instance</param>
        /// <returns>File URL</returns>
        public virtual string GetFileUrl(int fileId)
        {
            var url = _webHelper.GetStoreLocation() + "content/files/";
            var file = GetFileById(fileId);

            return GetFileUrl(file);
        }

        /// <summary>
        /// Get a file URL
        /// </summary>
        /// <param name="file">File instance</param>
        /// <returns>File URL</returns>
        public virtual string GetFileUrl(File file)
        {
            var url = _webHelper.GetStoreLocation() + "content/files/";

            byte[] fileBinary = null;
            if (file != null)
                fileBinary = LoadFileBinary(file);
            if (file == null || fileBinary == null || fileBinary.Length == 0)
            {
                string fileName = string.Format("{0}_0{1}", file.Id.ToString("0000000"), file.Extension);
                url += fileName;
                return url;
            }

            return url;
        }

        /// <summary>
        /// Gets the loaded file binary depending on file storage settings
        /// </summary>
        /// <param name="file">File</param>
        /// <returns>File binary</returns>
        public virtual byte[] LoadFileBinary(File file)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            var result = LoadFileFromLocation(file.Id, file.Extension);
            return result;
        } 

        #endregion

        #region CRUD methods

        /// <summary>
        /// Gets a file
        /// </summary>
        /// <param name="fileId">File identifier</param>
        /// <returns>File</returns>
        public virtual File GetFileById(int fileId)
        {
            if (fileId == 0)
                return null;

            return _fileRepository.GetById(fileId);
        }

        /// <summary>
        /// Deletes a file
        /// </summary>
        /// <param name="file">File</param>
        public virtual void DeleteFile(File file)
        {
            if (file == null)
                throw new ArgumentNullException("file");

            //delete from file system
            DeleteFileOnFileSystem(file);

            //delete from database
            _fileRepository.Delete(file);

            //event notification
            _eventPublisher.EntityDeleted(file);
        }

        /// <summary>
        /// Gets a collection of files
        /// </summary>
        /// <param name="pageIndex">Current page</param>
        /// <param name="pageSize">Items on each page</param>
        /// <returns>Paged list of files</returns>
        public virtual IPagedList<File> GetFiles(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = from p in _fileRepository.Table
                        orderby p.Id descending
                        select p;
            var pics = new PagedList<File>(query, pageIndex, pageSize);
            return pics;
        }


        /// <summary>
        /// Gets files by file identifier
        /// </summary>
        /// <param name="fileId">Product identifier</param>
        /// <param name="recordsToReturn">Number of records to return. 0 if you want to get all items</param>
        /// <returns>Files</returns>
        public virtual IList<File> GetFilesByPropertyId(int fileId, int recordsToReturn = 0)
        {
            if (fileId == 0)
                return new List<File>();

            var query = from p in _fileRepository.Table
                        //join pp in _fileFileRepository.Table on p.Id equals pp.FileId
                        //orderby p.DisplayOrder
                        //where pp.ProductId == fileId
                        select p;

            if (recordsToReturn > 0)
                query = query.Take(recordsToReturn);

            var pics = query.ToList();
            return pics;
        }

        /// <summary>
        /// Inserts a file
        /// </summary>
        /// <returns>File</returns>
        public virtual File InsertFile(byte[] fileBinary, string fileName,string fileExtension, string seoFilename, bool isNew = true)
        {
            fileName = CommonHelper.EnsureNotNull(fileName);
            fileName = CommonHelper.EnsureMaximumLength(fileName, 20);

            seoFilename = CommonHelper.EnsureMaximumLength(seoFilename, 100);

            var file = new File
            {
                Name=fileName,
                Extension=fileExtension,
                SeoFilename = seoFilename,
                IsNew = isNew,
            };
            _fileRepository.Insert(file);
         
            //保存文件
            SaveFileInSystem(file.Id, fileBinary, fileExtension);

            //event notification
            _eventPublisher.EntityInserted(file);

            return file;
        }

        /// <summary>
        /// Updates the file
        /// </summary>
        /// <param name="fileId">The file identifier</param>
        /// <param name="fileBinary">The file binary</param>
        /// <param name="fileExtension">The file MIME type</param>
        /// <param name="seoFilename">The SEO filename</param>
        /// <param name="altAttribute">"alt" attribute for "img" HTML element</param>
        /// <param name="titleAttribute">"title" attribute for "img" HTML element</param>
        /// <param name="isNew">A value indicating whether the file is new</param>
        /// <param name="validateBinary">A value indicating whether to validated provided file binary</param>
        /// <returns>File</returns>
        public virtual File UpdateFile(int fileId, byte[] fileBinary, string fileName, string fileExtension,
            string seoFilename,bool isNew = true)
        {
            fileExtension = CommonHelper.EnsureNotNull(fileExtension);
            fileExtension = CommonHelper.EnsureMaximumLength(fileExtension, 20);
            seoFilename = CommonHelper.EnsureMaximumLength(seoFilename, 100);

            var file = GetFileById(fileId);
            if (file == null)
                return null;
            
            file.Extension = fileExtension;
            file.SeoFilename = seoFilename;
            file.Name = fileName;
            file.IsNew = isNew;

            _fileRepository.Update(file);

           SaveFileInSystem(file.Id, fileBinary, fileExtension);

            //event notification
            _eventPublisher.EntityUpdated(file);

            return file;
        }

        /// <summary>
        /// Updates a SEO filename of a file
        /// </summary>
        /// <param name="fileId">The file identifier</param>
        /// <param name="seoFilename">The SEO filename</param>
        /// <returns>File</returns>
        public virtual File SetSeoFilename(int fileId, string seoFilename)
        {
            var file = GetFileById(fileId);
            if (file == null)
                throw new ArgumentException("No file found with the specified id");

            //update if it has been changed
            if (seoFilename != file.SeoFilename)
            {
                //update file
                file = UpdateFile(file.Id,
                    LoadFileBinary(file),
                    file.Name,
                    file.Extension,
                    seoFilename,
                    true);
            }
            return file;
        }


        #endregion
    }
}
