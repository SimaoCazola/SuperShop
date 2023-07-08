using SuperShop.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Data
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        // metodo que serve para devolve (Mostrar) todas as encomendas feitas por um user(Utilizador);
        Task<IQueryable<Order>> GetOrderAsync(string userName);

    }
}
