using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using BookShop.Domain.DTO;
using BookShop.Domain.DomainModels;
using BookShop.Repository;
using BookShop.Domain.Identity;
using BookShop.Services.Interface;
using BookShop.Services.Implementation;

namespace BookShop.Web.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: Products
        public IActionResult Index()
        {
            var allProducts = this._productService.GetAllProducts();
            return View(allProducts);
        }
        //GET za AddProductToCard
        public IActionResult AddProductToCard(Guid? id)
        {
            var model = this._productService.GetShoppingCartInfo(id);
            return View(model);
        }
        //POST ZA ADDPRODUCTTOCARD
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


        // GET: Products/Details/5
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

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Products/Edit/5
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

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // POST: Products/Delete/5
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
