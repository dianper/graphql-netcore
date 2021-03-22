namespace GraphQL.API.Queries
{
    using GraphQL.API.Models;
    using GraphQL.API.Services;
    using HotChocolate;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class Query
    {
        public Task<IEnumerable<Product>> GetProductsAsync([Service] IProductService productService) => productService.GetProductsAsync();
    }
}
