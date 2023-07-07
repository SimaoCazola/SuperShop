using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SuperShop.Data;
using SuperShop.Data.Entities;
using SuperShop.Helpers;
using SuperShop.Models;

namespace SuperShop.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _lobHelper;
        private readonly IConverterHelper _converterHelper;

        // Passo 56: Inserir o interface ImageHelper das imagens no construtor do controlador
        public ProductsController(
            IProductRepository productRepository,
            IUserHelper userHelper, IBlobHelper blobHelper, IConverterHelper converterHelper)
        {
            _productRepository = productRepository;
            _userHelper = userHelper;
            _lobHelper = blobHelper;
            _converterHelper = converterHelper;
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
                return new NotFoundViewResult("ProductNotFound");
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return new NotFoundViewResult("ProductNotFound"); // Vou usar a ja a classe not found e dentro do parametro a mensagem que eu quizer mostrar    
            }

            return View(product);
        }

        [Authorize(Roles = "Admin")]
        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }


        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin, Customer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;
                if(model.ImageFile!=null && model.ImageFile.Length > 0)
                {

                    // Passo 49: Resolver nomes repetidos usando o guid: Transferido para um metodo
                    // Passo 50:Inserir o File dentro do caminho e substituindo o filename: Trasferido para um metodo
                    // Passo 57: Definir o caminho ---> Path: Transferido para um metodo
                    imageId = await _lobHelper.UploadBlobAsync(model.ImageFile, "products");
                }

                //criacao da variavel que guarda o a conversao do metodo ToProduct
                var product = _converterHelper.ToProduct(model, imageId, true); // Passo 60: Aplicar o metodo no cotrolador


                //TODO: Modificar para o user que tiver logado
                product.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
                await _productRepository.CreateAsync(product);    // nao e preciso fazer um metodo em baixo para
                return RedirectToAction(nameof(Index));
                                                                  // guardar porque ja esta a guardar no repositorio              
            }
            return View(model);
        }

        // Passo 19: conversao  do product para ToProduct dentro do metodo:Transferido
        // GET: Products/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) 
            {
                return new NotFoundViewResult("ProductNotFound");
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return new NotFoundViewResult("ProductNotFound");
            }


            //Passo 22: Criacao da variavel que guarda a conversaoi do metodo ToProductViewModel: Transferido num metodo
            //Passo 61: Aplicar o metodo da conversao
            var model= _converterHelper.ToProductViewModel(product);
            return View(model);
        }

        // Passo 23: codigo da conversao do product para productviewmodel dentro metodo: Transferido

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
                   Guid imageId = model.ImageId;
                    if(model.ImageFile !=null && model.ImageFile.Length > 0)
                    {


                        ////var guid = Guid.NewGuid().ToString(); // converter um objecto do do tipo Guid e gusrdar numa variavel guid
                        ////var file = $"{guid}.jpg"; // Vou ter o nome do guid e o tipo de imagem

                        imageId = await _lobHelper.UploadBlobAsync(model.ImageFile, "products");

                        //using (var stream = new FileStream(path, FileMode.Create))
                        //{
                        //    await model.ImageFile.CopyToAsync(stream);  // Guardar a imagem no servidor
                        //}
                        //path = $"~/images/products/{file}";
                    }

                    // Passo 32: Fazer a conversao para o metodo ToProduct: Transferido
                    //Passo 62: Aplicar o metodo da conversao
                    var product = _converterHelper.ToProduct(model, imageId, false); // É falso porque não é novo


                    //TODO: Modificar para o user que tiver logado
                    product.User = await _userHelper.GetUserByEmailAsync(this.User.Identity.Name);
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
                return new NotFoundViewResult("ProductNotFound");
            }

            var product = await _productRepository.GetByIdAsync(id.Value);
            if (product == null)
            {
                return new NotFoundViewResult("ProductNotFound");
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

        public IActionResult ProductNotFound()
        {
            return View();  
        }

    }
}
