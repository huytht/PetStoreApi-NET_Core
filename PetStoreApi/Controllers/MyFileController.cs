using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace PetStoreApi.Controllers
{
    [ApiController]
    [Route("api/myfile")]
    public class MyFileController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;

        public MyFileController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet("images/{fileName}")]
        public FileResult getImageFile(string fileName)
        {
            var path = Path.Combine("", _hostingEnvironment.ContentRootPath + "/Images/" + fileName);

             return File(System.IO.File.ReadAllBytes(path), "image/png");
        }
    }
}
