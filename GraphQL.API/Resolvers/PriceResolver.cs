namespace GraphQL.API.Resolvers
{
    using GraphQL.API.Models;
    using GraphQL.API.Services;
    using HotChocolate;
    using HotChocolate.Types;
    using System.Threading.Tasks;

    [ExtendObjectType(Name = "Price")]
    public class PriceResolver
    {
        public Task<Price> GetPriceAsync([Parent] Product product, [Service] IPriceService priceService) => priceService.GetPriceAsync(product.Id);
    }
}
