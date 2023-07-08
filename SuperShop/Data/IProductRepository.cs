using Microsoft.AspNetCore.Mvc.Rendering;
using SuperShop.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace SuperShop.Data
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        public IQueryable GetAllWithUsers(); // Passo 47: inserir o metodo criado na classe do repositorio ca na interface

        IEnumerable<SelectListItem> GetComboProducts();
    }
}
