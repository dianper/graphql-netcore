namespace GraphQL.API.Queries
{
    using GraphQL.Core.Entities;
    using GraphQL.Core.Repositories;
    using HotChocolate;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ProductQuery
    {
        public Task<IEnumerable<Product>> GetProductsAsync([Service] IProductRepository productRepository) =>
            productRepository.GetAllAsync();

        public Task<Product> GetProductByIdAsync(string id, [Service] IProductRepository productRepository) =>
            productRepository.GetByIdAsync(id);

        // Add any queries you want here..
        // - Get categories
        // - Get products by category
        // - etc
    }
}
