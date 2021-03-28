namespace GraphQL.Infrastructure.Repositories
{
    using GraphQL.Core.Entities;
    using GraphQL.Core.Repositories;
    using GraphQL.Infrastructure.Data;
    using MongoDB.Driver;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext catalogContext;

        public ProductRepository(ICatalogContext catalogContext)
        {
            this.catalogContext = catalogContext ?? throw new ArgumentNullException(nameof(catalogContext));
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await this.catalogContext.Products.Find(_ => true).ToListAsync();
        }

        public async Task<Product> GetByIdAsync(string id)
        {
            var filter = Builders<Product>.Filter.Eq(_ => _.Id, id);

            return await this.catalogContext.Products.Find(filter).FirstOrDefaultAsync();
        }
    }
}
