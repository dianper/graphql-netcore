namespace GraphQL.Infrastructure.Data
{
    using GraphQL.Core.Entities;
    using MongoDB.Driver;

    public interface ICatalogContext
    {
        IMongoCollection<Category> Categories { get; }
        IMongoCollection<Product> Products { get; }
    }
}