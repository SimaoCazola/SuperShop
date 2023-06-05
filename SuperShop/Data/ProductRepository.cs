using Microsoft.EntityFrameworkCore;
using SuperShop.Data.Entities;
using System.Linq;

namespace SuperShop.Data
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly DataContext _context;

        // Passo 45: criar um atributo do tipo private readonly DataContext context para ligar as tabelas do user
        public ProductRepository(DataContext context): base(context)
        {
            _context = context;
        }

        // Passo 46: Criar um metodo GetAllWithUsers que vai juntar os produtos e os users
        public IQueryable GetAllWithUsers()
        {
            return _context.Products.Include(p => p.User);
        }
        
    }
}
