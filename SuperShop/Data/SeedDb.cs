﻿using Microsoft.AspNetCore.Identity;
using SuperShop.Data.Entities;
using SuperShop.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private Random _random; 
        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _random = new Random(); 
        }

        public async Task SeedAsync()
        {
            await _context.Database.EnsureCreatedAsync(); // Cria a base de dados caso ainda nao estiver criada

            var user = await _userHelper.GetUserByEmailAsync("cazolasimao@gmail.com");

            if(user == null)
            {
                user = new User
                {
                    FirstName = "Simao",
                    LastName= "Cazola",
                    Email="cazolasimao@gmail.com",
                    UserName = "cazolasimao@gmail.com",
                    PhoneNumber="914105858"
                };
                var result= await _userHelper.AddUserAsync(user, "123456");
                if(result!=IdentityResult.Success)
                {
                    throw new InvalidOperationException("User not created");
                }
            }

            if (!_context.Products.Any())
            {
                AddProduct("iPhone", user);
                AddProduct("Huawei", user);
                AddProduct("iPad", user);
                AddProduct("SumSung", user);
                await _context.SaveChangesAsync();
            }
        }

        private void AddProduct(string name, User user)
        {
            _context.Products.Add(new Product
            {
                Name = name,    
                Price=_random.Next(1000),
                IsAvailable=true,   
                Stock=_random.Next(100),
                User=user
            });
        }
    }
}
