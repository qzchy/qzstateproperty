using QZCHY.API.Models.Media;
using QZCHY.Services.Configuration;
using QZCHY.Services.Media;
using QZCHY.Web.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace QZCHY.API.Controllers
{
    [RoutePrefix("Media")]
    public class MediaController: BaseAdminApiController
    {
        private readonly IPictureService _pictureService;
        private readonly ISettingService _settingService;
        private readonly IFileService _fileService;

        public MediaController(IPictureService pictureService, IFileService fileService,
            Services.Configuration.ISettingService settingService)
        {
            _pictureService = pictureService;
            _fileService = fileService;
            _settingService = settingService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Pictures/Upload")]
        public IHttpActionResult PicturesUpload(int size = 200)
        {
            var httpRequest = HttpContext.Current.Request;

            var response = new List<MediaModel>();

            if (httpRequest.Files.Count > 0)
            {
                #region 遍历文件
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filename = postedFile.FileName;
                    var contentType = postedFile.ContentType;
                    var stream = postedFile.InputStream;

                    // NOTE: To store in memory use postedFile.InputStream
                    var fileBinary = new byte[stream.Length];
                    stream.Read(fileBinary, 0, fileBinary.Length);

                    var fileExtension = Path.GetExtension(filename);
                    if (!string.IsNullOrEmpty(fileExtension))
                        fileExtension = fileExtension.ToLower();
                    //contentType is not always available 
                    //that's why we manually update it here
                    //http://www.sfsu.edu/training/mimetype.htm
                    if (String.IsNullOrEmpty(contentType))
                    {
                        switch (fileExtension)
                        {
                            case ".bmp":
                                contentType = "image/bmp";
                                break;
                            case ".gif":
                                contentType = "image/gif";
                                break;
                            case ".jpeg":
                            case ".jpg":
                            case ".jpe":
                            case ".jfif":
                            case ".pjpeg":
                            case ".pjp":
                                contentType = "image/jpeg";
                                break;
                            case ".png":
                                contentType = "image/png";
                                break;
                            case ".tiff":
                            case ".tif":
                                contentType = "image/tiff";
                                break;
                            default:
                                break;
                        }
                    }

                    var picture = _pictureService.InsertPicture(fileBinary, contentType, "", "", filename);

                    response.Add(new MediaModel
                    {
                        Id = picture.Id,
                        Url = _pictureService.GetPictureUrl(picture)
                    });
                }
                #endregion           
                return Ok(response);
            }
            return BadRequest("无文件上传");

        }


        [AllowAnonymous]
        [HttpPost]
        [Route("Files/Upload")]
        public IHttpActionResult FilesUpload()
        {
            var httpRequest = HttpContext.Current.Request;

            var response = new List<MediaModel>();

            if (httpRequest.Files.Count > 0)
            {
                #region 遍历文件
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];
                    var filename = postedFile.FileName;
                    var contentType = postedFile.ContentType;
                    var stream = postedFile.InputStream;

                    // NOTE: To store in memory use postedFile.InputStream
                    var fileBinary = new byte[stream.Length];
                    stream.Read(fileBinary, 0, fileBinary.Length);

                    var fileExtension = Path.GetExtension(filename);
                    if (!string.IsNullOrEmpty(fileExtension))
                        fileExtension = fileExtension.ToLower();
                    //contentType is not always available 
                    //that's why we manually update it here
                    //http://www.sfsu.edu/training/mimetype.htm

                    var f = _fileService.InsertFile(fileBinary, filename, fileExtension, null);

                    response.Add(new MediaModel
                    {
                        Id = f.Id,
                        Url = _fileService.GetFileUrl(f)
                    });
                }
                #endregion           
                return Ok(response);
            }
            return BadRequest("无文件上传");
        }
    }
}