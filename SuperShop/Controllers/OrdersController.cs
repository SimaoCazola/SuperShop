using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuperShop.Data;
using SuperShop.Models;
using System;
using System.Threading.Tasks;

namespace SuperShop.Controllers
{
    [Authorize] // aqui estou autorizar apenas users registrados a executar essa accao
    public class OrdersController : Controller
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public OrdersController(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        // IAction do Tipo Create para mostrar os orders
        public async Task<IActionResult> Index()
        {
            // Guardar na variavel model todas informacoes ja com os codigos para mostrar na view
            var model = await _orderRepository.GetOrderAsync(this.User.Identity.Name);
            return View(model);
        }

        // IAction do Tipo CREATE para mostrar os ordersDetails
        public async Task<IActionResult> Create()
        {
            // Guardar na variavel model todas informacoes ja com os codigos para mostrar na view
            var model = await _orderRepository.GetDetailTempsAsync(this.User.Identity.Name);
            return View(model);
        }

        // Mostrar a view do botao AddProduct da Combox--> CREATE
        public IActionResult AddProduct()
        {
            var model = new AddItemViewModel
            {
                Quantity = 1,
                Products = _productRepository.GetComboProducts()
            };
            return View(model);
        }

        // Executar as funcoes do botao AddProduct da Combox--> POST
        [HttpPost]
        public async Task<IActionResult> AddProduct(AddItemViewModel model)
        {
            /*Verficar primeiro se o model e valido*/
            if (ModelState.IsValid)
            {
                await _orderRepository.AddItemToOrderAsync(model, this.User.Identity.Name);   
                return RedirectToAction("Create");
            }
            return View(model); 
        }


        // POST do Delete
        public async Task<IActionResult> DeleteItem(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            await _orderRepository.DeleteDetailTempAsync(id.Value);
            return RedirectToAction("Create");
        }


        // POST Increase
        public async Task<IActionResult> Increase(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            await _orderRepository.ModifyOrderDetailTempQuantityAsync(id.Value,1);
            return RedirectToAction("Create");
        }


        // POST Decrease
        public async Task<IActionResult> Decrease(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            await _orderRepository.ModifyOrderDetailTempQuantityAsync(id.Value, -1);
            return RedirectToAction("Create");
        }

        // POST CONFIRM 
        public async Task<IActionResult> ConfirmOrder()
        {
            var response= await _orderRepository.ConfirmOrderAsync(this.User.Identity.Name);
            if (response)
            {
                return RedirectToAction("Index");
            }
            return RedirectToAction("Create");
        }

        // GET DELIVERY VIEW PARA APARECER A VIEW
        
        public async Task<IActionResult> Deliver (int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _orderRepository.GetOrderAsync(id.Value);
            if (order == null)
            {
                return NotFound();
            }

            var model = new DeliveryViewModel
            {
                Id = order.Id,
                DeliverDate = DateTime.Today
            };

            return View(model);
        }

        // POST DELIVERY VIEW PARA FAZER O SUBMIT DO QUE ESTIVER NO FORMULARIO
        [HttpPost]
        public async Task<IActionResult> Deliver(DeliveryViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _orderRepository.DeliverOrder(model);
                return RedirectToAction("Index");
            }

            return View();
        }


    }
}
