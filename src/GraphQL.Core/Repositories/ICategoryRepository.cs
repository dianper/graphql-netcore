namespace GraphQL.Core.Repositories
{
    using GraphQL.Core.Entities;
    using System.Threading.Tasks;

    public interface ICategoryRepository
    {
        Task<Category> GetById(string id);
    }
}
