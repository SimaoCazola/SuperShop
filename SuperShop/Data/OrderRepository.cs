using Microsoft.EntityFrameworkCore;
using SuperShop.Data.Entities;
using SuperShop.Helpers;
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

    }
}
