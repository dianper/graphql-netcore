namespace GraphQL.Core.Repositories
{
    using GraphQL.Core.Entities;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product> GetByIdAsync(string id);
    }
}
