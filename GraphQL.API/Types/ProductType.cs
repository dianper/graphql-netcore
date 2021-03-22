namespace GraphQL.API.Types
{
    using GraphQL.API.Models;
    using GraphQL.API.Resolvers;
    using HotChocolate.Types;

    public class ProductType : ObjectType<Product>
    {
        protected override void Configure(IObjectTypeDescriptor<Product> descriptor)
        {
            descriptor.Field(_ => _.Id);
            descriptor.Field(_ => _.Name);
            descriptor.Field(_ => _.Quantity);
            descriptor.Field(_ => _.Created);
            descriptor.Field(_ => _.Description);
            descriptor.Field<PriceResolver>(_ => _.GetPriceAsync(default, default));
        }
    }
}
