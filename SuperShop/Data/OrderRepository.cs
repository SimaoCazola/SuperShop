using Microsoft.EntityFrameworkCore;
using SuperShop.Data.Entities;
using SuperShop.Helpers;
using SuperShop.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Data
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public OrderRepository(DataContext context, IUserHelper userHelper) : base (context)
        {
            _context = context;
            _userHelper = userHelper;
        }


        // Codigo do Metodo para Adicionar no carrinho os Items escolhidos na combox--->POST 
        public async Task AddItemToOrderAsync(AddItemViewModel model, string userName)
        {
          /*Verificar o utilizador esta vazio que esta na web*/
          var user= await _userHelper.GetUserByEmailAsync(userName);
            if (user == null)
            {
                return;
            }

            /*Verificar se o produto esta vazio que esta na web*/
            var product= await _context.Products.FindAsync(model.ProductId); 
            if (product == null)
            {
                return;
            }

            /*Variavel que guarda o produto e o utilizador*/
            var orderDetailTemp = await _context.OrderDetailsTemp
                .Where(odt => odt.User == user && odt.Product==product)
                .FirstOrDefaultAsync();

            /*Caso a variavel orderDetailTemp estiver vazio, entao vamos criar um novo detalhe da encomenda */
            if (orderDetailTemp == null)
            {
                orderDetailTemp = new OrderDetailTemp
                {
                    Price = product.Price,
                    Product = product,
                    Quantity = model.Quantity,
                    User = user
                };

                /*Adicionar o objecto novo*/
                _context.OrderDetailsTemp.Add(orderDetailTemp);

            }
            else // caso nao estiver vazio, vamos executar o seguinte
            {
                orderDetailTemp.Quantity += model.Quantity;
                _context.OrderDetailsTemp.Update(orderDetailTemp); // fazer o update
            }

            await _context.SaveChangesAsync();
        }


        //Codigo para Metodo para CONFIRMAR a encomenda escolhida na web---> POST
        public async Task<bool> ConfirmOrderAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);
            if (user == null) 
            {
                return false;
            }

            var orderTmps = await _context.OrderDetailsTemp
                .Include(o => o.Product)
                .Where(o => o.User == user)
                .ToListAsync();
            if(orderTmps==null || orderTmps.Count == 0)
            {
                return false;
            }

            var details = orderTmps.Select(o => new OrderDetail
            {
                Price=o.Price,
                Product=o.Product,
                Quantity=o.Quantity

            }).ToList();

            var order = new Order
            {
                OrderDate = DateTime.UtcNow, // busca a data do servidor
                User = user,
                Items = details
            };
            await CreateAsync(order);
            _context.OrderDetailsTemp.RemoveRange(orderTmps);
            await _context.SaveChangesAsync();
            return true;
          
        }


        // Codigo do Metodo para APAGAR a encomenda escolhida na web---> POST
        public async Task DeleteDetailTempAsync(int id)
        {
          var orderDetailTemp = await _context.OrderDetailsTemp.FindAsync(id);
            if (orderDetailTemp == null)
            {
                return;
            }

            _context.OrderDetailsTemp.Remove(orderDetailTemp);
            await _context.SaveChangesAsync();
        }



        // Metodo Que retorna a data de entrega na view
        public async Task DeliverOrder(DeliveryViewModel model)
        {
            var order = await _context.Orders.FindAsync(model.Id);
            if(order == null)
            {
                return;
            }

            order.DeliverDate = model.DeliverDate;   // Guardar na variavel order a data do modelo
            _context.Orders.Update(order);          // Atualiza na memoria
            await _context.SaveChangesAsync();      // Guardar na base de dados
        }

        public async Task<IQueryable<OrderDetailTemp>> GetDetailTempsAsync(string userName)
        {
            var user = await _userHelper.GetUserByEmailAsync(userName);

            if (user == null)
            {
                return null;
            }
            return _context.OrderDetailsTemp
                .Include(p => p.Product)
                .Where(o => o.User == user)
                .OrderBy(o => o.Product.Name);
        }


        // Metodo para calcular as encomendas (Orders)
        public async Task<IQueryable<Order>> GetOrderAsync(string userName)
        {
            // Metodo que retorna o utilizador atual, vamos guardar esse metodo na variavel user.
            var user = await _userHelper.GetUserByEmailAsync(userName);

            // validacao que verifica se o utilizador esta vazio ou nao
            if (user == null)
            {
                return null;
            }

            // Validacao que verifica se o utilizador em funcao é Admin
            if (await _userHelper.IsUserInRoleAsync(user, "Admin"))
            {

                // Codigo que retorna as tabelas, neste caso tabela encomenda, inclui os itens, tambem inlui o produto, e ordena em descendente por data. Ou seja retorna as 3 tabelas se for o Admin
                return _context.Orders
                .Include(o => o.User)
                .Include(o => o.Items)
                .ThenInclude(p => p.Product)
                .OrderByDescending(o => o.OrderDate); 
            }

            // Codigo que retorna as tabelas quando não for o Admin. Acrestamos o where simplesmente para definir o user que estiver logado.
            return _context.Orders
            .Include(o => o.Items)
            .ThenInclude(p => p.Product)
            .Where(o => o.User == user)
            .OrderByDescending(o => o.OrderDate);
        }




        // Codigo do Metodo Para mostrar a encomenda por ID do delivery---> CREATE
        public async Task<Order> GetOrderAsync(int id)
        {
            return await _context.Orders.FindAsync(id); 
        }


        // Codigo do Metodo para modificar a encomenda escolhida na web---> POST
        public async Task ModifyOrderDetailTempQuantityAsync(int id, double quantity)
        {
            /*Guardar os detalhes da encomenda na variavel para verificar se esta nulo ou nao*/
            var orderDetailTemp = await _context.OrderDetailsTemp.FindAsync(id);
            if (orderDetailTemp == null)
            {
                return;
            }
            orderDetailTemp.Quantity += quantity;
            if(orderDetailTemp.Quantity > 0)
            {
                _context.OrderDetailsTemp.Update(orderDetailTemp);
                await _context.SaveChangesAsync();
            }
            
        }
    }
}
