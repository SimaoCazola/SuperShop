using SuperShop.Data.Entities;
using SuperShop.Models;
using System;

namespace SuperShop.Helpers
{
    // Passo 57: Criar um interface para obter os metodos de conversao
    public interface IConverterHelper
    {
        Product ToProduct(ProductViewModel model, Guid imageId, bool isNew);
        ProductViewModel ToProductViewModel (Product product);

    }
}
