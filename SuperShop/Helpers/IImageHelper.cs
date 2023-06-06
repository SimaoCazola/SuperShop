using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace SuperShop.Helpers
{
    public interface IImageHelper
    {
        // Passo 52: Criar um metodo com 2 parametro que recebe a image e recebe o caminho da imagem
        Task<string> UploadImageAsync(IFormFile imageFile, string  folder);  

    }
}