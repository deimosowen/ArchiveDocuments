using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using FluentFTP;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ArchiveDocuments.Models;

namespace ArchiveDocuments.Services
{
    public class FtpService : IFtpService
    {
        private readonly FtpClient _ftpClient;
        private readonly ILogger<FtpService> _logger;

        public FtpService(IOptions<FtpSettingsModel> appIdentitySettingsAccessor, ILogger<FtpService> logger)
        {
            var ftpSettingsModel = appIdentitySettingsAccessor.Value;
            _logger = logger;
            _ftpClient = new FtpClient(ftpSettingsModel.Server, ftpSettingsModel.Port, ftpSettingsModel.Login, ftpSettingsModel.Password);
        }

        public bool DownloadFile(string path, out byte[] bytes)
        {
            try
            {
                _ftpClient.Connect();
                return _ftpClient.Download(out bytes, path);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.InnerException?.Message);
                bytes = null;
                return false;
            }
            finally
            {
                _ftpClient.Disconnect();
            }
        }
        public async Task<byte[]> DownloadFileAsync(string path)
        {
            try
            {
                await _ftpClient.ConnectAsync();
                return await _ftpClient.DownloadAsync(path, CancellationToken.None);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.InnerException?.Message);
                return null;
            }
            finally
            {
                await _ftpClient.DisconnectAsync();
            }
        }
        public FtpStatus UploadFile(byte[] bytes, string remotePath)
        {
            try
            {
                _ftpClient.Connect();
                return _ftpClient.Upload(bytes, remotePath, FtpRemoteExists.Overwrite, true);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.InnerException?.Message);
                return FtpStatus.Failed;
            }
            finally
            {
                _ftpClient.Disconnect();
            }
        }
        public FtpStatus UploadFile(Stream stream, string remotePath)
        {
            try
            {
                _ftpClient.Connect();
                return _ftpClient.Upload(stream, remotePath, FtpRemoteExists.Overwrite, true);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.InnerException?.Message);
                return FtpStatus.Failed;
            }
            finally
            {
                _ftpClient.Disconnect();
            }
        }
        public async Task<FtpStatus> UploadFileAsync(byte[] bytes, string remotePath)
        {
            try
            {
                await _ftpClient.ConnectAsync();
                return await _ftpClient.UploadAsync(bytes, remotePath, FtpRemoteExists.Overwrite, true);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.InnerException?.Message);
                return FtpStatus.Failed;
            }
            finally
            {
                await _ftpClient.DisconnectAsync();
            }
        }

        public async Task<FtpStatus> UploadFileAsync(Stream stream, string remotePath)
        {
            try
            {
                await _ftpClient.ConnectAsync();
                return await _ftpClient.UploadAsync(stream, remotePath, FtpRemoteExists.Overwrite, true);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.InnerException?.Message);
                return FtpStatus.Failed;
            }
            finally
            {
                await _ftpClient.DisconnectAsync();
            }
        }
    }
}
