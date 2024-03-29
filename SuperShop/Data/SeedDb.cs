﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data.Entities;
using SuperShop.Helpers;
using System;
using System.Collections.Generic;
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
            await _context.Database.MigrateAsync(); // Cria a migracao e a base de dados ao mesmo tempo
            /*await _context.Database.EnsureCreatedAsync();*/ // Cria a base de dados caso ainda nao estiver criada

            await _userHelper.CheckRoleAsync("Admin");
            await _userHelper.CheckRoleAsync("Customer");

            // Adicionar Paises, e cidades nas tabelas para arrancar compo defaulty
            if(!_context.Countries.Any())
            {
                var cities = new List<City>();
                cities.Add(new City {Name = "Lisboa"});
                cities.Add(new City { Name = "Porto" });
                cities.Add(new City { Name = "Faro" });

                _context.Countries.Add(new Country
                {
                    Cities = cities,
                    Name = "Portugal"
                });

                //Gravamos na base de dados
                await _context.SaveChangesAsync();
            }

            var user = await _userHelper.GetUserByEmailAsync("cazolasimao@gmail.com");

            if(user == null)
            {
                user = new User
                {
                    FirstName = "Simao",
                    LastName = "Cazola",
                    Email = "cazolasimao@gmail.com",
                    UserName = "cazolasimao@gmail.com",
                    PhoneNumber = "914105858",
                    Address = "Rua Jau 33",
                    CityId = _context.Countries.First().Cities.FirstOrDefault().Id,
                    City = _context.Countries.FirstOrDefault().Cities.FirstOrDefault()
                };
                var result= await _userHelper.AddUserAsync(user, "123456");
                if(result!=IdentityResult.Success)
                {
                    throw new InvalidOperationException("User not created in Seed");
                }
                await _userHelper.AddUserToRoleAsync(user, "Admin");
                var token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);
            }

            var isInRole = await _userHelper.IsUserInRoleAsync(user, "Admin");
            if(isInRole)
            {
                await _userHelper.AddUserToRoleAsync(user, "Admin");
            }

            if (!_context.Products.Any())
            {
                AddProduct("Camisa Vermelha", user);
                AddProduct("Camisa amarela", user);
                AddProduct("Camisa Castanha", user);
                AddProduct("Casaco Vermelho", user);
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
