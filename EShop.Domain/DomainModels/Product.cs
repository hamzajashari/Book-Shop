using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace EShop.Domain.DomainModels
{
    public class Product : BaseEntity
    {
        
        [Required]
        public string ProductName { get; set; }
        [Required]
        public string ProductImage { get; set; }
        [Required]
        public string ProductDescription { get; set; }
        [Required]
        public int Rating{ get; set; }
        [Required]
        public int ProductPrice { get; set; }
        public virtual ICollection<ProductInShoppingCart> ProductInShoppingCarts { get; set; }
        public virtual ICollection<ProductInOrder> Orders { get; set; }
        public Product()
        {

        }

        public override bool Equals(object obj)
        {
            return obj is Product product &&
                   Id.Equals(product.Id) &&
                   ProductName == product.ProductName &&
                   ProductImage == product.ProductImage &&
                   ProductDescription == product.ProductDescription &&
                   Rating == product.Rating &&
                   ProductPrice == product.ProductPrice &&
                   EqualityComparer<ICollection<ProductInShoppingCart>>.Default.Equals(ProductInShoppingCarts, product.ProductInShoppingCarts) &&
                   EqualityComparer<ICollection<ProductInOrder>>.Default.Equals(Orders, product.Orders);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, ProductName, ProductImage, ProductDescription, Rating, ProductPrice, ProductInShoppingCarts, Orders);
        }
    }
}
