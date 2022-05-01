using EShop.Domain.DomainModels;
using EShop.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductService.Test
{
    public class FakeDatabase
    {

        public  Guid[] list { get; set; }

        private List<Product> productList { get; set; }

        public FakeDatabase()
        {
            productList = new List<Product>();

            list = new Guid[]
            {        
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            };
        }

        



        public List<Product> JustGenericProducts(int numberOfProducts)
        {
           
            for (int i = 0; i < numberOfProducts; i++)
            {
                
                var p = new Product()
                { 
                    Id =this.list[i], ProductName = "TestProduct" + i, ProductDescription = "This is product test no. "+i, ProductImage = "https://media.gettyimages.com/photos/stack-of-books-picture-id157482029?s=612x612", ProductPrice = i*100 
                 };
                
                this.productList.Add(p);
            }

            return productList;
        }


    }
}