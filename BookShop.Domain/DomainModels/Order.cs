using BookShop.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Domain.DomainModels
{
    public class Order : BaseEntity
    {
        public string UserId { get; set; }
        public BookShopApplicationUser User { get; set; }
        public virtual ICollection<ProductInOrder> Products { get; set; }
    }
}
