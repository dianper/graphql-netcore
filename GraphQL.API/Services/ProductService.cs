namespace GraphQL.API.Services
{
    using GraphQL.API.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProductService : IProductService
    {
        private readonly IList<Product> products;

        public ProductService()
        {
            this.products = new List<Product>
            {
                new Product {
                    Created = DateTime.Now,
                    Description = "Description One",
                    Id = 1,
                    Name = "Product Name 1",
                    Quantity = 10
                },
                new Product {
                    Created = DateTime.Now.AddDays(1),
                    Description = "Description Two",
                    Id = 2,
                    Name = "Product Name 2",
                    Quantity = 20
                },
            };
        }

        public Task<IEnumerable<Product>> GetProductsAsync()
        {
            return Task.FromResult(this.products.AsEnumerable());
        }
    }
}
