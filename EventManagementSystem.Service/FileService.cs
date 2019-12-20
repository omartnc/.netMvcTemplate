using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Web;
using EventManagementSystem.Helper;
using File = EventManagementSystem.Entity.Model.File.File;

namespace EventManagementSystem.Service
{
    public class FileService : BaseService
    {
        public FileService()
        {

        }

        public void Add(File item)
        {
            FileRepository.Add(item);
        }

        public ParameterService ParameterService { get; set; } = new ParameterService();
        public File UploadFile(Image image, string fileName, string tags = "", string uploadPath = "")
        {
            var rootPath = HttpContext.Current.Server.MapPath("~/");
            var datePrefix = ParameterService.GetValueByKey("UploadFolderDatePrefix");
            var datePath = DateTime.Now.ToString(datePrefix, CultureInfo.InvariantCulture);
            if (string.IsNullOrEmpty(uploadPath))
                uploadPath = ParameterService.GetValueByKey("FileUploadFolder");
            var folderPath = Path.Combine(rootPath.Trim('\\'), uploadPath.Trim('\\'), datePath.Trim('\\'));
            string fullPath;

            Directory.CreateDirectory(folderPath);
            do
            {
                fullPath = Path.Combine(folderPath.Trim('\\'), GenarateFileName(fileName).Trim('\\'));
            } while (System.IO.File.Exists(fullPath));

            image.Save(fullPath);
            var file = new File
            {
                Path = "/" + fullPath.Replace(rootPath, "").Replace("\\", "/"),
                UploadedTime = DateTime.Now,
                Tags = tags
            };
            Add(file);

            return file;
        }
        public File UploadFile(HttpPostedFileBase postedFile, string tags = "", string uploadPath = "")
        {
            var rootPath = HttpContext.Current.Server.MapPath("~/");
            var datePrefix = ParameterService.GetValueByKey("UploadFolderDatePrefix");
            var datePath = DateTime.Now.ToString(datePrefix, CultureInfo.InvariantCulture);

            if (string.IsNullOrEmpty(uploadPath))
                uploadPath = ParameterService.GetValueByKey("FileUploadFolder");
            var folderPath = Path.Combine(rootPath.Trim('\\'), uploadPath.Trim('\\'), datePath.Trim('\\'));
            string fullPath;

            Directory.CreateDirectory(folderPath);
            do
            {
                fullPath = Path.Combine(folderPath.Trim('\\'), GenarateFileName(postedFile.FileName).Trim('\\'));
            } while (System.IO.File.Exists(fullPath));

            postedFile.SaveAs(fullPath);
            var file = new File
            {
                Path = "/" + fullPath.Replace(rootPath, "").Replace("\\", "/"),
                UploadedTime = DateTime.Now,
                Tags = tags
            };
            Add(file);

            return file;
        }

        private string GenarateFileName(string fullFileName)
        {
            var guidPrefix = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);
            var fileName = Path.GetFileNameWithoutExtension(fullFileName).ToUrlFriendlyLower();
            var fileExtenstion = Path.GetExtension(fullFileName);
            var filePath = $"{guidPrefix}-{fileName}{fileExtenstion}";
            return filePath;
        }
    }
}