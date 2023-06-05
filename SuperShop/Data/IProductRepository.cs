using SuperShop.Data.Entities;
using System.Linq;

namespace SuperShop.Data
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        public IQueryable GetAllWithUsers(); // Passo 47: inserir o metodo criado na classe do repositorio ca na interface
    }
}
