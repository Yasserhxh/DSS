using System.Threading.Tasks;

namespace Service.IServices
{
    public interface IBlobService
    {
        string UploadFileToBlob(string filename, string folder, byte[] fileData, string mimeType);
        Task<string> UploadFileToBlobAsync(string filenameP, string strFileName, byte[] fileData, string fileMimeType);
    }
}