
using RepositoryPatternSample.ClientModels.Base;
using RepositoryPatternSample.ClientModels.Models.Auth.Authenticate;
using RepositoryPatternSample.ClientModels.Models.Utilities;
using Microsoft.AspNetCore.Http;
using System.Drawing;
using System.Drawing.Imaging;

namespace RepositoryPatternSample.Services.Base.Helpers
{
    public static class HelperMethods
    {
        public static object PreparePaginatedResponse<T>(List<T> data, int currentPage, int totalElements, int totalPages) where T : class
        {
            return new
            { 
                Collection = data,
                TotalElements = totalElements,
                CurrentPage = currentPage,
                TotalPages = totalPages
            };
        }

        public static async Task<ResponseModel> SaveImage(ImageFile imageFile, string rootPath, string folderName, string? name)
        {
            try
            {
                var response = new ResponseModel
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    Status = "Success",
                    Message = "Image Saved!",
                    Data = "",
                };
                var relativeFilePath = Path.Combine(StringResources.IMAGE_PATH, @folderName);
                var uploads = Path.Combine(rootPath, relativeFilePath);

                if (imageFile != null)
                {
                    var file = new FileInfo(imageFile.Name);
                    var fileExtention = file.Extension;
                    var fileName = $"{name}{fileExtention}";
                    var uniqueFileName = GetUniqueFileName(fileName);
                    var filePath = Path.Combine(uploads, uniqueFileName);
                    string base64String = imageFile.File.Substring(imageFile.File.IndexOf("base64,") + "base64,".Length);
                    byte[] bytes = Convert.FromBase64String(base64String);

                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        Image image = Image.FromStream(ms);
                        var jhf = ImageFormat.Webp;
                        image.Save("im", ImageFormat.Webp);
                    }

                    response.Data = Path.Combine(relativeFilePath, uniqueFileName);
                }

                return response;
            }

            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Status = "Failed",
                    Message = "Failed to save image!",
                    Data = ex.Message
                };
            }
        }

        public static string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return string.Concat(Path.GetFileNameWithoutExtension(fileName)
                                , "_"
                                , Guid.NewGuid().ToString().AsSpan(0, 4)
                                , Path.GetExtension(fileName));
        }

        internal static Task<ResponseModel> SaveImage(string attachmentUrl, string webRootPath, string path, string v)
        {
            throw new NotImplementedException();
        }
    }
}
