namespace GraphQL.API.Services
{
    using GraphQL.API.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync();
    }
}
