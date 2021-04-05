namespace GraphQL.API.Subscriptions
{
    using GraphQL.Core.Entities;
    using HotChocolate;
    using HotChocolate.Types;
    using System.Threading.Tasks;

    [ExtendObjectType(Name = "Subscription")]
    public class ProductSubscriptions
    {
        [Subscribe]
        [Topic]
        public Task<Product> OnCreateAsync([EventMessage] Product product) =>
            Task.FromResult(product);

        [Subscribe]
        [Topic]
        public Task<string> OnRemoveAsync([EventMessage] string productId) =>
            Task.FromResult(productId);
    }
}
