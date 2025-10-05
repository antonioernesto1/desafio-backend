using DnsClient.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MotorcycleRental.Application.Interfaces;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Infrastructure.Storage
{
    internal class LocalStorageService : IStorageService
    {
        private readonly string _basePath;
        public LocalStorageService(IConfiguration configuration)
        {
            _basePath = configuration["Storage:Local:BasePath"] ?? Path.Combine(Directory.GetCurrentDirectory(), "uploads");

            if (!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }
        }
        public async Task<string> SaveBase64FileAsync(string fileName, string base64Content, string? folder = null)
        {
            var base64Data = base64Content.Contains(",")
               ? base64Content.Split(',')[1]
               : base64Content;

            var fileContent = Convert.FromBase64String(base64Data);
            return await SaveFileAsync(fileName, fileContent, folder);
        }

        public async Task<string> SaveFileAsync(string fileName, byte[] fileContent, string? folder = null)
        {
            var folderPath = string.IsNullOrEmpty(folder)
                ? _basePath
                : Path.Combine(_basePath, folder);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}.png";
            var fullPath = Path.Combine(folderPath, uniqueFileName);

            await File.WriteAllBytesAsync(fullPath, fileContent);

            var relativePath = string.IsNullOrEmpty(folder)
                ? uniqueFileName
                : Path.Combine(folder, uniqueFileName);

            return relativePath;
        }

        public Task<bool> DeleteAsync(string path)
        {
            var fullPath = Path.Combine(_basePath, path);
            
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);

                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}
