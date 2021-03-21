namespace GraphQL.API.Resolvers
{
    using GraphQL.API.Data;
    using GraphQL.API.Models;
    using HotChocolate;
    using HotChocolate.Types;
    using System.Threading.Tasks;

    [ExtendObjectType(Name = "Price")]
    public class PriceResolver
    {
        public Task<Price> GetPriceAsync([Parent] Product product, [Service] IPriceService priceService) => priceService.GetPriceAsync(product.Id);
    }
}
