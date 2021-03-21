namespace GraphQL.API.Data
{
    using GraphQL.API.Models;
    using System.Threading.Tasks;

    public interface IPriceService
    {
        Task<Price> GetPriceAsync(int productId);
    }
}
