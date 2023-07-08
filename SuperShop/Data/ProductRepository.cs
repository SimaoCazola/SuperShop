using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data.Entities;
using System.Collections.Generic;
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

        // criar fazer o codigo do metodo que vai comandar a combox
        public IEnumerable<SelectListItem> GetComboProducts()
        {
            // Criar a listagem dentro da combox 
            var list = _context.Products.Select(p => new SelectListItem
            {
                Text = p.Name,
                Value= p.Id.ToString(),

            }).ToList();

            // Inserir um texto padrao na combox--> "Select a product"
            list.Insert(0, new SelectListItem
            {
                Text = "(Select a product)",
                Value= "0"
            });

            return list;
        }
    }
}
