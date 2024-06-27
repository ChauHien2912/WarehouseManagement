using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WareHouseManagement.Repository.Services.IServices
{
    public interface IMediaService
    {
        Task<string> UploadAnImage(IFormFile file, string pathFolder, string nameOfImg);
        Task<string> UploadAVideo(IFormFile file, string pathFolder, string nameOfImg);
    }
}
