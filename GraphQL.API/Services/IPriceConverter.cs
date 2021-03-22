namespace GraphQL.API.Services
{
    using System.Threading.Tasks;

    public interface IPriceConverter
    {
        Task<string> ConvertAsync(decimal price);
    }
}
