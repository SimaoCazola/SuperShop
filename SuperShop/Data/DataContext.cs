using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Data
{
    public class DataContext: IdentityDbContext<User>
    {
        public DbSet<Product> Products { get; set; } // Tabela dos Produtos

        public DbSet<Order> Orders { get; set; } // Tabela dos Order

        public DbSet<OrderDetail> OrderDetails { get; set; } // Tabela dos OrderDetail

        public DbSet<OrderDetailTemp> OrderDetailsTemp { get; set; } // Tabela dos OrderDetailTemp

        public DbSet<Country> Countries { get; set; } // Tabela dos Paises

        public DbSet<City> Cities { get; set; } // Tabela das cidades
        public DataContext(DbContextOptions<DataContext> options): base(options)
        {
            
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Country>()
                .HasIndex(c => c.Name)
                .IsUnique();


            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");


            modelBuilder.Entity<OrderDetailTemp>()
               .Property(p => p.Price)
               .HasColumnType("decimal(18,2)");


            modelBuilder.Entity<OrderDetail>()
              .Property(p => p.Price)
              .HasColumnType("decimal(18,2)");


            base.OnModelCreating(modelBuilder);
        }

        // O CODIGO ESTA COMENTADO PORQUE VAMOS APLICAR OUTRO CODIGO, MAS CASO NAO QUEIRA APLICAR O OUTRO CODIGO ELA É UTIL
        // HABILITAR A REGRA EM APAGAR EM CASCATA (CASCADE DELETE RULE)
        // Ativar o cascade para apagar os produtos em todas as tabelas assim que foi apagado na web
        // Para codigo fique habilitado é necesario primeiro fazer o migration e mandar a Base de dados abaixo

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    var cascadeFKs = modelBuilder.Model
        //        .GetEntityTypes()
        //        .SelectMany(t => t.GetForeignKeys())
        //        .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

        //    foreach(var fk in cascadeFKs)
        //    {
        //        fk.DeleteBehavior = DeleteBehavior.Restrict;
        //    }

        //    base.OnModelCreating(modelBuilder);

        //}

    }
}
