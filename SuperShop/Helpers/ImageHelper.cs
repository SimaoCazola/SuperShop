using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Threading.Tasks;

namespace SuperShop.Helpers
{
    // Passo 53: Criar uma classe ImageHelper e implenatar o interface
    public class ImageHelper : IImageHelper
    {
        public async Task<string> UploadImageAsync(IFormFile imageFile, string folder)
        {
            // Passo 54: Resolver nomes repetidos usando o guid
            string guid = Guid.NewGuid().ToString(); // converter um objecto do do tipo Guid e gusrdar numa variavel guid
            string file = $"{guid}.jpg";

            string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images\\{folder}",
            file);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);  // Guardar a imagem no servidor
            }
            return $"~/images/{folder}/{file}";
        }

    }
}
