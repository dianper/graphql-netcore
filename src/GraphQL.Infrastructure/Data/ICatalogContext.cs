namespace GraphQL.Infrastructure.Data
{
    using MongoDB.Driver;

    public interface ICatalogContext
    {
        IMongoCollection<T> GetCollection<T>(string name);
    }
}