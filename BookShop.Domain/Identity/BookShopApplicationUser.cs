using BookShop.Domain.DomainModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Domain.Identity
{
    public class BookShopApplicationUser : IdentityUser /*preoptovaruvanje na klasata*/

    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public virtual ShoppingCart UserCart { get; set; }

    }
}
