using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using Service.IServices;

namespace Service.Services;

public class AzureBlobService : IBlobService
{ 
    private readonly IConfiguration _configuration;

    public AzureBlobService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
  public string UploadFileToBlob(string filename, string strFileName, byte[] fileData, string fileMimeType)
        {
            var task = Task.Run(() => this.UploadFileToBlobAsync(filename, strFileName, fileData, fileMimeType));
            task.Wait();
            var fileUrl = task.Result;
            return fileUrl;
        }

        public async void DeleteBlobData(string fileUrl)
        {
            var uriObj = new Uri(fileUrl);
            var BlobName = Path.GetFileName(uriObj.LocalPath);

            var cloudStorageAccount = CloudStorageAccount.Parse(_configuration["AzureBlob:AccessKey"]);
            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            var strContainerName = "uploads";
            var cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);

            var pathPrefix = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd") + "/";
            var blobDirectory = cloudBlobContainer.GetDirectoryReference(pathPrefix);
            // get block blob refarence  
            var blockBlob = blobDirectory.GetBlockBlobReference(BlobName);

            // delete blob from container      
            await blockBlob.DeleteAsync();
        }


        private string GenerateFileName(string filenameP, string fileName)
        {
            var strFileName = string.Empty;
            var strName = fileName.Split('.');
            strFileName = strName[strName.Length - 1] + "/" + filenameP;
            return strFileName;
        }

        public async Task<string> UploadFileToBlobAsync(string filenameP, string strFileName, byte[] fileData, string fileMimeType)
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(_configuration["AzureBlob:AccessKey"]);
            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            var strContainerName = "CimarUploads";
            var cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);
            var fileName = this.GenerateFileName(filenameP, strFileName);

            if (await cloudBlobContainer.CreateIfNotExistsAsync())
            {
                await cloudBlobContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }

            if (fileName == null || fileData == null) return "";
            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(fileName);
            cloudBlockBlob.Properties.ContentType = fileMimeType;
            await cloudBlockBlob.UploadFromByteArrayAsync(fileData, 0, fileData.Length);
            return cloudBlockBlob.Uri.AbsoluteUri;
        }
}