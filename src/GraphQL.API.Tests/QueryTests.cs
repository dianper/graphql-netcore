namespace GraphQL.API.Tests
{
    using GraphQL.API.Queries;
    using GraphQL.API.Resolvers;
    using GraphQL.API.Types;
    using GraphQL.Core.Repositories;
    using GraphQL.Infrastructure.Configurations;
    using GraphQL.Infrastructure.Data;
    using GraphQL.Infrastructure.Repositories;
    using HotChocolate;
    using HotChocolate.Execution;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Xunit;

    public class QueryTests
    {
        [Fact]
        public async void Query_ReturnsProducts()
        {
            // Arrange
            IRequestExecutor executor =
                await new ServiceCollection()
                .AddSingleton(sp => new MongoDbConfiguration()) // Should be mocked
                .AddSingleton<ICatalogContext, CatalogContext>() // Should be mocked
                .AddScoped<ICategoryRepository, CategoryRepository>() // Should be mocked
                .AddScoped<IProductRepository, ProductRepository>() // Should be mocked
                .AddGraphQL()
                .AddQueryType<ProductQuery>()
                .AddType<ProductType>()
                .AddType<CategoryResolver>()
                .BuildRequestExecutorAsync();

            // Act
            IExecutionResult result = await executor.ExecuteAsync("{ products { description price } }");

            dynamic jsonObj = JsonConvert.DeserializeObject<dynamic>(result.ToJson(false));

            // Assert
            Assert.Null(jsonObj.errors);
            Assert.NotNull(jsonObj.data);
            Assert.NotEmpty(jsonObj.data.products);
        }
    }
}
