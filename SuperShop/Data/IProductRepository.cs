using Microsoft.AspNetCore.Mvc.Rendering;
using SuperShop.Data.Entities;
using SuperShop.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Data
{
    public interface IProductRepository:IGenericRepository<Product>
    {

        public IQueryable GetAllWithUsers(); // Passo 47: inserir o metodo criado na classe do repositorio ca na interface


        // Metodo para mostrar a combobox e os produtos disponivel numa combox---> CREATE
        IEnumerable<SelectListItem> GetComboProducts();



    }
}
