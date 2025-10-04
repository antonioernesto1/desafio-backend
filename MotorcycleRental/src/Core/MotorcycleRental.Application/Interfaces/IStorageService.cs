using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorcycleRental.Application.Interfaces
{
    public interface IStorageService
    {
        Task<string> SaveBase64FileAsync(string fileName, string base64Content, string? folder = null);
        Task<string> SaveFileAsync(string fileName, byte[] fileContent, string? folder = null);
    }
}
