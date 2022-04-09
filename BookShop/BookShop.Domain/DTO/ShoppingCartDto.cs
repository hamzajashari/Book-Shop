using BookShop.Domain.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Domain.DTO
{
    public class ShoppingCartDto
    {
        public List<ProductInShoppingCart> ProductInShoppingCarts { get; set; }
        public double TotalPrice { get; set; }
    }
}
