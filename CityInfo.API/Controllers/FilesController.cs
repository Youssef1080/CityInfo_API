using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.IO;

namespace CityInfo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly FileExtensionContentTypeProvider fileExtensionContentTypeProvider;

        public FilesController(FileExtensionContentTypeProvider fileExtensionContentTypeProvider)
        {
            this.fileExtensionContentTypeProvider = fileExtensionContentTypeProvider
                ?? throw new System.ArgumentNullException(nameof(fileExtensionContentTypeProvider));
        }

        [HttpGet]
        public ActionResult GetFile()
        {
            string filePath = "German Made Simple- Learn to Speak and Understand German Quickly and Easily ( PDFDrive ).pdf";

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            if (!fileExtensionContentTypeProvider.TryGetContentType(filePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var bytes = System.IO.File.ReadAllBytes(filePath);

            return File(bytes, contentType, Path.GetFileName(filePath));
        }
    }
}