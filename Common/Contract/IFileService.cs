using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contract
{
    public interface IFileService
    {
        Task<bool> IsConnectionReadyAsync();

        Task<bool> GetFileAsync(string fileFullName);

        Task<bool> SaveFileAsync(string uploadeFileFullName, string fileUploadFolder);

        Task<bool> DeleteFileAsync(string fileFullName);
    }
}
