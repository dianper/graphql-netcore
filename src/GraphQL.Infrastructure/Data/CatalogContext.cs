namespace GraphQL.Infrastructure.Data
{
    using GraphQL.Core.Entities;
    using GraphQL.Infrastructure.Configurations;
    using MongoDB.Driver;

    public class CatalogContext : ICatalogContext
    {
        private const string ProductCollectionName = "Products";
        private const string CategoryCollectionName = "Categories";

        public CatalogContext(MongoDbConfiguration mongoDbConfiguration)
        {
            var client = new MongoClient(mongoDbConfiguration.ConnectionString);
            var database = client.GetDatabase(mongoDbConfiguration.Database);

            this.Categories = database.GetCollection<Category>(CategoryCollectionName);
            this.Products = database.GetCollection<Product>(ProductCollectionName);

            CatalogContextSeed.SeedData(this.Categories, this.Products);
        }

        public IMongoCollection<Category> Categories { get; }
        public IMongoCollection<Product> Products { get; }
    }
}
