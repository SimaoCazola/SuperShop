using SuperShop.Data.Entities;
using SuperShop.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SuperShop.Data
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
        // metodo que serve para devolve (Mostrar) todas as encomendas feitas por um user(Utilizador);
        Task<IQueryable<Order>> GetOrderAsync(string userName);



        Task<IQueryable<OrderDetailTemp>> GetDetailTempsAsync(string userName);


        // Metodo para Adicionar no carrinho os Items escolhidos na combox--->POST
        Task AddItemToOrderAsync(AddItemViewModel model, string userName);


        // Metodo para modificar a encomenda escolhida na web---> POST
        Task ModifyOrderDetailTempQuantityAsync(int id, double quantity);


        // Metodo para APAGAR a encomenda escolhida na web---> POST
        Task DeleteDetailTempAsync(int id);


        // Metodo para CONFIRMAR a encomenda escolhida na web---> POST
        Task<bool> ConfirmOrderAsync(string userName);

        // Metodo para Mostrar a data de entrega---> 
        Task DeliverOrder(DeliveryViewModel model);

        // Metodo Para mostrar a encomenda por ID---> CREATE
        Task<Order> GetOrderAsync(int id);  



    }
}
