using System.IO;
using FluentFTP;
using System.Threading.Tasks;

namespace ArchiveDocuments.Services
{
    public interface IFtpService
    {
        bool DownloadFile(string path, out byte[] bytes);
        Task<byte[]> DownloadFileAsync(string path);
        FtpStatus UploadFile(byte[] bytes, string remotePath);
        FtpStatus UploadFile(Stream stream, string remotePath);
        Task<FtpStatus> UploadFileAsync(byte[] bytes, string remotePath);
        Task<FtpStatus> UploadFileAsync(Stream stream, string remotePath);
    }
}
