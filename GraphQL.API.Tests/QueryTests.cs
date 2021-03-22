namespace GraphQL.API.Tests
{
    using GraphQL.API.Queries;
    using GraphQL.API.Resolvers;
    using GraphQL.API.Services;
    using GraphQL.API.Types;
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
                .AddScoped<IProductService, ProductService>()
                .AddScoped<IPriceService, PriceService>()
                .AddScoped<IPriceConverter, PriceConverter>()
                .AddGraphQL()
                .AddQueryType<Query>()
                .AddType<ProductType>()
                .AddType<PriceResolver>()
                .BuildRequestExecutorAsync();

            // Act
            IExecutionResult result =
                await executor.ExecuteAsync("{ products { description price { formattedPrice } } }");

            dynamic jsonObj =
                JsonConvert.DeserializeObject<dynamic>(result.ToJson(false));

            // Assert
            Assert.Null(jsonObj.errors);
            Assert.NotNull(jsonObj.data);
            Assert.NotEmpty(jsonObj.data.products);
            Assert.NotNull(jsonObj.data.products[0].price);
            Assert.IsType<JValue>(jsonObj.data.products[0].price.formattedPrice);
        }
    }
}
