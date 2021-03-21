namespace GraphQL.API.Data
{
    using GraphQL.API.Models;
    using System.Threading.Tasks;

    public class PriceService : IPriceService
    {
        public Task<Price> GetPriceAsync(int productId)
        {
            return Task.FromResult(new Price { InitialPrice = 10 + productId, FinalPrice = 15 * productId, SalesPrice = 13 / productId });
        }
    }
}
