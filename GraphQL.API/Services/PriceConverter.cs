namespace GraphQL.API.Services
{
    using System.Threading.Tasks;

    public class PriceConverter : IPriceConverter
    {
        public Task<string> ConvertAsync(decimal price)
        {
            return Task.FromResult(price.ToString());
        }
    }
}
