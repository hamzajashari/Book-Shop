using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using EShop.Domain.DTO;
using EShop.Domain.DomainModels;
using EShop.Repository;
using EShop.Domain.Identity;
using EShop.Services.Interface;


namespace EShop.Web.Controllers
{
    
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: Products
        [Route("/products")]
        public IActionResult Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var allProducts = this._productService.GetAllProducts();
                return View(allProducts);
            }

        }
        [Route("/products/{id}")]
        public IActionResult AddProductToCard(Guid? id)
        {
            var model =this._productService.GetShoppingCartInfo(id);
            return View(model);
        }

        [Route("/products/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddProductToCard([Bind("ProductId", "Quantity")] AddToShoppingCardDto item)
        {   
            
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = this._productService.AddToShoppingCart(item, userId);
            if (result)
            { 
                return RedirectToAction("Index", "Products");
            }
            return View(item);
        }
        [Route("/products/details/{id}")]
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = this._productService.GetDetailsForProduct(id);
                
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [Route("/products/create")]
        public IActionResult Create()
        {
            return View();
        }


        [Route("/products/create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,ProductName,ProductImage,ProductDescription,Rating,ProductPrice")] Product product)
        {
            if (ModelState.IsValid)
            {
               
                this._productService.CreateNewProduct(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [Route("/products/edit/{p}")]
        public IActionResult Edit(Guid? p)
        {
            if (p == null)
            {
                return NotFound();
            }

            var product = this._productService.GetDetailsForProduct(p);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [Route("/products/edit/{p}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, [Bind("Id,ProductName,ProductImage,ProductDescription,Rating,ProductPrice")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    this._productService.UpdateExistingProduct(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        // GET: Products/Delete/5
        [Route("/products/delete/{id}")]
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = this._productService.GetDetailsForProduct(id);
                
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [Route("/products/delete/{id}")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            this._productService.DeleteProduct(id);
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(Guid id)
        {
            return this._productService.GetDetailsForProduct(id) != null;
        }
    }
}
