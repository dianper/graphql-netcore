namespace GraphQL.API.Resolvers
{
    using GraphQL.Core.Entities;
    using GraphQL.Core.Repositories;
    using HotChocolate;
    using HotChocolate.Types;
    using System.Threading.Tasks;

    [ExtendObjectType(Name = "Category")]
    public class CategoryResolver
    {
        public Task<Category> GetCategoryAsync([Parent] Product product, [Service] ICategoryRepository categoryRepository) => categoryRepository.GetById(product.CategoryId);
    }
}
