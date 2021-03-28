namespace GraphQL.API.Queries
{
    using GraphQL.Core.Entities;
    using GraphQL.Core.Repositories;
    using HotChocolate;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class Query
    {
        public Task<IEnumerable<Product>> GetProductsAsync([Service] IProductRepository productRepository)
        {
            return productRepository.GetAllAsync();
        }
    }
}
