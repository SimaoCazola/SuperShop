using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data;
using SuperShop.Data.Entities;
using SuperShop.Helpers;
using SuperShop.Models;

namespace SuperShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IUserHelper _userHelper;

        public ProductsController(
            IProductRepository productRepository,
            IUserHelper userHelper)
        {
            _productRepository = productRepository;
            _userHelper = userHelper;
        }

        // GET: Products
        public IActionResult Index()
        {
            return View(_productRepository.GetAll().OrderBy(p=> p.Name)); //Adicionamos o metodo orderby aqui para filtrar as informacoes todas
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                var path = string.Empty;
                if(model.ImageFile!=null && model.ImageFile.Length > 0)
                {
                    // Passo 49: Resolver nomes repetidos usando o guid
                    var guid = Guid.NewGuid().ToString(); // converter um objecto do do tipo Guid e gusrdar numa variavel guid
                    var file = $"{guid}.jpg"; // Vou ter o nome do guid e o tipo de imagem


                    // dar o caminho
                    path = Path.Combine(Directory.GetCurrentDirectory(),
                        "wwwroot\\images\\products", file);  // Passo 50:Inserir o File dentro do caminho e substituindo o filename
                    // Gravar no servidor
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(stream);  // Guardar a imagem no servidor
                    }

                    path = $"~/images/products/{file}"; // Inserir o file aqui tambem
                }

                //criacao da variavel que guarda o a conversao do metodo ToProduct
                var product = this.ToProduct(model, path);


                //TODO: Modificar para o user que tiver logado
                product.User = await _userHelper.GetUserByEmailAsync("cazolasimao@gmail.com");
                await _productRepository.CreateAsync(product);    // nao e preciso fazer um metodo em baixo para
                return RedirectToAction(nameof(Index));
                                                                  // guardar porque ja esta a guardar no repositorio              
            }
            return View(model);
        }

        // Passo 19: conversao  do product para ToProduct dentro do metodo:
        private Product ToProduct(ProductViewModel model, string path)
        {

            return new Product
            {
                Id = model.Id,
                ImageUrl=path,
                IsAvailable=model.IsAvailable,
                LastPurchase=model.LastPurchase,
                LastSale=model.LastSale,
                Name=model.Name,
                Price=model.Price,
                Stock=model.Stock,
                User=model.User
            };
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }


            //Passo 22: Criacao da variavel que guarda a conversaoi do metodo ToProductViewModel
            var model = this.ToProductViewModel(product);
            return View(model);
        }

        // Passo 23: codigo da conversao do product para productviewmodel dentro metodo
        private ProductViewModel ToProductViewModel(Product product)
        {
            return new ProductViewModel
            {
                Id= product.Id,
                IsAvailable = product.IsAvailable,
                LastPurchase=product.LastPurchase,
                LastSale=product.LastSale,
                Name=product.Name,
                Price=product.Price,
                Stock=product.Stock,
                ImageUrl = product.ImageUrl,
                User=product.User
                
            };
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductViewModel model) // Passo 28: Inserir o ProductViewModulo no Parametro e definir a varialve model
        {
          //Passo 29: Apagar o codigo que estava aqui a verificar o Id porque nao faz diferenca

            if (ModelState.IsValid)
            {
                try
                {
                    // Passo 30: Criar o caminho path da Imagem do tipo model
                    var path = model.ImageUrl;
                    // Passo 31: Fazer a verificacao se tem uma imagem selecionada
                    if(model.ImageFile !=null && model.ImageFile.Length > 0)
                    {

                        // Passo 51: Resolver nomes repetidos usando o guid
                        var guid = Guid.NewGuid().ToString(); // converter um objecto do do tipo Guid e gusrdar numa variavel guid
                        var file = $"{guid}.jpg"; // Vou ter o nome do guid e o tipo de imagem

                        path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images\\products",
                            file);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await model.ImageFile.CopyToAsync(stream);  // Guardar a imagem no servidor
                        }
                        path = $"~/images/products/{file}";
                    }

                    // Passo 32: Fazer a conversao para o metodo ToProduct
                    var product=this.ToProduct(model, path);


                    //TODO: Modificar para o user que tiver logado
                    product.User = await _userHelper.GetUserByEmailAsync("cazolasimao@gmail.com");
                    await _productRepository.UpdateAsync(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Passo 33: Adicionar model.Id no parametro do ExistAsync
                    if (!await _productRepository.ExistAsync(model.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // Passo 34: Adicionar model.Id no parametro do view
            return View(model.Id);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
           await _productRepository.DeleteAsync(product);
            return RedirectToAction(nameof(Index));
        }


    }
}
