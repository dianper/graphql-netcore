namespace GraphQL.API.Mutations
{
    using GraphQL.Core.Entities;
    using GraphQL.Core.Repositories;
    using HotChocolate;
    using HotChocolate.Types;
    using System.Threading.Tasks;

    [ExtendObjectType(Name = "Mutation")]
    public class ProductMutation
    {
        public Task<Product> CreateProductAsync(Product product, [Service] IProductRepository productRepository) =>
            productRepository.InsertAsync(product);

        public Task<bool> RemoveProductAsync(string id, [Service] IProductRepository productRepository) =>
            productRepository.RemoveAsync(id);
    }
}
