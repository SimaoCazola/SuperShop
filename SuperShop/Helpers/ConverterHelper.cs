using SuperShop.Data.Entities;
using SuperShop.Models;
using System;
using System.IO;

namespace SuperShop.Helpers
{
    // Passo 58: Criar uma classe para armazenar os dados e
    // criar as instrucoes ou codigos dos metodos criados no interface de conversao 
    public class ConverterHelper : IConverterHelper
    {
        public Product ToProduct(ProductViewModel model, Guid imageId, bool isNew)
        {
            return new Product
            {
                Id = isNew ? 0 : model.Id,   // Se o valor for true significa que é o Id é novo,
                                             // entao vou colocar zero automaticamente a base de dados vai atribuir um zero, caso contrario meto o ID que existe
                ImageId = imageId,
                IsAvailable = model.IsAvailable,
                LastPurchase = model.LastPurchase,
                LastSale = model.LastSale,
                Name = model.Name,
                Price = model.Price,
                Stock = model.Stock,
                User = model.User
            };
           
        }

        public ProductViewModel ToProductViewModel (Product product)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                IsAvailable = product.IsAvailable,
                LastPurchase = product.LastPurchase,
                LastSale = product.LastSale,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                ImageId = product.ImageId,
                User = product.User

            };
        }
    }
}
