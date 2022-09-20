using PetStoreApi.Constants;
using PetStoreApi.Helpers;

namespace PetStoreApi.Services.Repositories
{
    public class FileRepository : IFileRepository
    {
		private readonly IWebHostEnvironment _hostingEnvironment;

		public FileRepository(IWebHostEnvironment hostingEnvironment)
		{
			_hostingEnvironment = hostingEnvironment;
		}

        public string Upload(string fileName, IFormFile file)
        {
			try
			{
				FileInfo fileInfo = new FileInfo(file.FileName);
				var newFileName = HelperFunction.normalizeUri(fileName) + "-" + DateTime.Now.TimeOfDay.Milliseconds + fileInfo.Extension;
				var path = Path.Combine("", _hostingEnvironment.ContentRootPath + "/Images/" + newFileName);
				using (var stream = new FileStream(path, FileMode.Create))
				{
					file.CopyTo(stream);
				}
				return FileConstant.FILE_URL_PATH + newFileName;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
        }
    }
}
