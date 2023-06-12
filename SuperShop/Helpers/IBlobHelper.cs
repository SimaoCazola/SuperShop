using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace SuperShop.Helpers
{
    public interface IBlobHelper
    {
        Task<Guid> UploadBlobAsync(IFormFile file, string containerName); // metodo para carregar a imagem da web

        Task<Guid> UploadBlobAsync(byte[] file, string containerName); // por exemplo imagens vindas do contentor

        Task<Guid> UploadBlobAsync(string file, string containerName); // carregar a foto de um endereço
    }
}
