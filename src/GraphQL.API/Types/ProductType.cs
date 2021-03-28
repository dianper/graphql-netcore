namespace GraphQL.API.Types
{
    using GraphQL.API.Resolvers;
    using GraphQL.Core.Entities;
    using HotChocolate.Types;

    public class ProductType : ObjectType<Product>
    {
        protected override void Configure(IObjectTypeDescriptor<Product> descriptor)
        {
            descriptor.Field(_ => _.Id);
            descriptor.Field(_ => _.CategoryId);
            descriptor.Field(_ => _.Name);
            descriptor.Field(_ => _.Description);
            descriptor.Field(_ => _.Price);
            descriptor.Field(_ => _.Quantity);
            descriptor.Field<CategoryResolver>(_ => _.GetCategoryAsync(default, default));
        }
    }
}
