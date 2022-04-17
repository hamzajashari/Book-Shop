﻿using BookShop.Domain.DomainModels;
using BookShop.Domain.DTO;
using BookShop.Repository.Interface;
using BookShop.Services.Interface;
using EShop.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Core.Persistence.Repositories;

namespace BookShop.Services.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductInShoppingCart> _productInShoppingCartRepository;
        private readonly IUserRepository _userRepository;
        public ProductService(IRepository<Product> productRepository, IUserRepository userRepository, IRepository<ProductInShoppingCart> productInShoppingCartRepository)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
            _productInShoppingCartRepository = productInShoppingCartRepository;
        }
        public bool AddToShoppingCart(AddToShoppingCardDto item, string userID)
        {
            var user = this._userRepository.Get(userID);
            var userShoppingCard = user.UserCart;
            if (item.ProductId != null && userShoppingCard != null)
            {
                var product = this.GetDetailsForProduct(item.ProductId);
                if (product != null)
                {
                    ProductInShoppingCart itemToAdd = new ProductInShoppingCart
                    {
                        Id = Guid.NewGuid(),
                        Product = product,
                        ProductId = product.Id,
                        ShoppingCart = userShoppingCard,
                        ShoppingCartId = userShoppingCard.Id,
                        Quantity = item.Quantity
                    };
                    this._productInShoppingCartRepository.Insert(itemToAdd);
                    return true;
                }
                return false;
            }
            return false;
        }

        public void CreateNewProduct(Product p)
        {
            this._productRepository.Insert(p);
        }

        public void DeleteProduct(Guid id)
        {
            var product = this.GetDetailsForProduct(id);
            this._productRepository.Delete(product);
        }

        public List<Product> GetAllProducts()
        {
            return this._productRepository.GetAll().ToList();
        }

        public Product GetDetailsForProduct(Guid? id)
        {
            return this._productRepository.Get(id);
        }

        public AddToShoppingCardDto GetShoppingCartInfo(Guid? id)
        {
            var product = this.GetDetailsForProduct(id);
            AddToShoppingCardDto model = new AddToShoppingCardDto
            {
                SelectedProduct = product,
                ProductId = product.Id,
                Quantity = 1
            };
            return model;


        }

        public void UpdateExistingProduct(Product p)
        {
            this._productRepository.Update(p);
        }
    }
}