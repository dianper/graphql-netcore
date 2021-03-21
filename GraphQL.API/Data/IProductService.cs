namespace GraphQL.API.Data
{
    using GraphQL.API.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProductService
    {
        Task<IEnumerable<Product>> GetProductsAsync();
    }
}
