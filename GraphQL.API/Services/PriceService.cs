namespace GraphQL.API.Services
{
    using GraphQL.API.Models;
    using System.Threading.Tasks;

    public class PriceService : IPriceService
    {
        private readonly IPriceConverter priceConverter;

        public PriceService(IPriceConverter priceConverter)
        {
            this.priceConverter = priceConverter;
        }

        public async Task<Price> GetPriceAsync(int productId)
        {
            var price = new Price
            {
                InitialPrice = 10 + productId,
                FinalPrice = 15 * productId,
                SalesPrice = 13 / productId
            };

            price.FormattedPrice = await this.priceConverter.ConvertAsync(price.SalesPrice);

            return await Task.FromResult(price);
        }
    }
}
