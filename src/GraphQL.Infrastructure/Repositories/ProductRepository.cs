namespace GraphQL.Infrastructure.Repositories
{
    using GraphQL.Core.Entities;
    using GraphQL.Core.Repositories;
    using GraphQL.Infrastructure.Data;

    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(ICatalogContext catalogContext) : base(catalogContext)
        {

        }
    }
}
