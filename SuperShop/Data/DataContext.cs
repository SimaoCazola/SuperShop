using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SuperShop.Data
{
    public class DataContext: IdentityDbContext<User>
    {
        public DbSet<Product> Products { get; set; } // Tabela dos Produtos

        public DbSet<Order> Orders { get; set; } // Tabela dos Order

        public DbSet<OrderDetail> OrderDetails { get; set; } // Tabela dos OrderDetail

        public DbSet<OrderDetailTemp> OrderDetailsTemp { get; set; } // Tabela dos OrderDetailTemp

        public DataContext(DbContextOptions<DataContext> options): base(options)
        {
            
        }

       
    }
}
