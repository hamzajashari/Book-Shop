using EShop.Domain.DomainModels;
using EShop.Domain.Identity;
using EShop.Repository;
using EShop.Repository.Interface;
using EShop.Services.Interface;
using EShop.Web.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Xunit;

namespace ProductService.Test
{
    public class ProductIntegrationTestController {

        FakeDatabase _fakeDatabase;
        List<Product> _products;
        Guid[] _guIdList;
        public ProductIntegrationTestController()
        {
            _fakeDatabase = new FakeDatabase();
            _products = _fakeDatabase.JustGenericProducts(10);
            _guIdList = _fakeDatabase.list;
           
        }


        [Fact]
        public void getAllProductsTest()
        {
            //Arrange
            Mock<IRepository<Product>> _mockProductRepo = new Mock<IRepository<Product>>();
            _mockProductRepo.Setup(m => m.GetAll()).Returns(_products);

            Mock<IProductService> _mockProductService = new Mock<IProductService>();
            _mockProductService.Setup(m => m.GetAllProducts()).Returns(_mockProductRepo.Object.GetAll().ToList());


            var controller = new ProductsController(_mockProductService.Object);

            IActionResult actionResult = controller.Index();
            ViewResult okResult = actionResult as ViewResult;

            Assert.Equal(okResult.Model, _products);
        }

        [Fact]
        public void getAllProductByIdTest_GET()
        {
            //Arrange
            Mock<IRepository<Product>> _mockProductRepo = new Mock<IRepository<Product>>();
            var firstElementId =_guIdList[0];
           
            _mockProductRepo.Setup(m => m.Get(firstElementId)).Returns(_products.SingleOrDefault(el => el.Id.Equals(firstElementId)));

            Mock<IProductService> _mockProductService = new Mock<IProductService>();
            _mockProductService.Setup(m => m.GetDetailsForProduct(firstElementId)).Returns(_mockProductRepo.Object.Get(firstElementId));

            var controller = new ProductsController(_mockProductService.Object);

            IActionResult actionResult = controller.Details(firstElementId);
            ViewResult okResult = actionResult as ViewResult;

            Assert.Equal(okResult.Model, _products.SingleOrDefault(el => el.Id.Equals(firstElementId)));
        }

        [Fact]
        public void deleteByIdTest_POST()
        {
            //Arrange
            Mock<IRepository<Product>> _mockProductRepo = new Mock<IRepository<Product>>();
            var firstElementId = _guIdList[0];
            Product product = _products.SingleOrDefault(el => el.Id.Equals(firstElementId));


            _mockProductRepo.Setup(m => m.Delete(product)).Verifiable(); 
            Mock<IProductService> _mockProductService = new Mock<IProductService>();
            _mockProductService.Setup(m => m.DeleteProduct(firstElementId)).Verifiable();

            var controller = new ProductsController(_mockProductService.Object);

            IActionResult actionResult = controller.DeleteConfirmed(firstElementId);
            RedirectToActionResult okResult = actionResult as RedirectToActionResult;
            Assert.Equal("Index",okResult.ActionName);

        }

        [Fact]
        public void CheckIfDeleteReturnsNotFoundOnNullProduct_GET()
        {
          
            Mock<IProductService> _mockProductService = new Mock<IProductService>();
            var firstElementId = _guIdList[0];

            var controller = new ProductsController(_mockProductService.Object);

            var result = controller.Delete(null) as NotFoundResult;

            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);

        }

        [Fact]
        public void CheckDeleteForInvalidProductId_GET()
        {
            var randomId = new Guid();
            Mock<IProductService> _mockProductService = new Mock<IProductService>();
            

            var controller = new ProductsController(_mockProductService.Object);

            var result = controller.Delete(randomId) as NotFoundResult;

            Assert.Equal(404, result.StatusCode);

        }


        [Fact]
        public void TestCreateProductWithInvalidModelState_GET()
        {
            var thirdProductId = _guIdList[2];
            Product product =  _products.First(p => p.Id.Equals(thirdProductId));
            Mock<IProductService> _mockProductService = new Mock<IProductService>();


            var controller = new ProductsController(_mockProductService.Object);

            controller.ModelState.AddModelError("testError", "This is an intentional ModelState error");

            ViewResult result = controller.Create(product) as ViewResult;

            Assert.Equal(result.Model, product);

        }

        public void TestAddProductToCard_Get()
        {
            var thirdProductId = _guIdList[2];
            Product product = _products.First(p => p.Id.Equals(thirdProductId));

            Mock<IProductService> _mockProductService = new Mock<IProductService>();
            Mock<IUserRepository> _mockUserRepository = new Mock<IUserRepository>();
            Mock<IRepository<Product>> _mockProductRepo = new Mock<IRepository<Product>>();


            _mockProductRepo.Setup(m => m.Get(thirdProductId)).Returns(_products.SingleOrDefault(el => el.Id.Equals(thirdProductId)));
            _mockProductService.Setup(u => u.GetDetailsForProduct(thirdProductId)).Returns(_mockProductRepo.Object.Get(thirdProductId));

            _mockUserRepository.Setup(u => u.Get(It.IsAny<string>())).Returns(new EShopApplicationUser()
            {
                FirstName = "PanchiTest",
                LastName = "KrangoTest",
                Address = "AddressTest",
                UserCart = new ShoppingCart()
            });

            // var userId = .FindFirstValue(ClaimTypes.NameIdentifier);

            // _mockProductService.Setup(m => m.AddToShoppingCart(product,userId)).Verifiable();

            //TO BE IMPLEMENTED
            var controller = new ProductsController(_mockProductService.Object);
            Assert.True(true);

        }


        [Fact]
        public void editProductById()
        {
            Mock<IRepository<Product>> _mockProductRepo = new Mock<IRepository<Product>>();
            var secondElementId = _guIdList[1];

            Product product = _products.SingleOrDefault(el => el.Id.Equals(secondElementId));

            _mockProductRepo.Setup(m => m.Update(It.IsAny<Product>())).Callback<Product>(prod =>
            {
                prod.ProductPrice = 250;
                _products.Remove(product);
                _products.Add(prod);
            });



            Mock<IProductService> _mockProductService = new Mock<IProductService>();
            _mockProductService.Setup(m => m.UpdateExistingProduct(It.IsAny<Product>())).Callback<Product>(
                prod => {
                    _mockProductRepo.Object.Update(prod);
        });
            var controller = new ProductsController(_mockProductService.Object);

            IActionResult actionResult = controller.Edit(secondElementId,product);
            RedirectToActionResult okResult = actionResult as RedirectToActionResult;

            Assert.Equal("Index",okResult.ActionName);
        }




    }
}
