using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperShop.Data;
using System.Threading.Tasks;

namespace SuperShop.Controllers
{
    [Authorize] // aqui estou autorizar apenas users registrados a executar essa accao
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrdersController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        // IAction do Tipo Create
        public async Task<IActionResult> Index()
        {
            // Guardar na variavel model todas informacoes ja com os codigos para mostrar na view
            var model = await _orderRepository.GetOrderAsync(this.User.Identity.Name);
            return View(model);
        }
    }
}
