namespace GraphQL.API
{
    using GraphQL.API.Configurations;
    using GraphQL.API.Mutations;
    using GraphQL.API.Queries;
    using GraphQL.API.Resolvers;
    using GraphQL.API.Subscriptions;
    using GraphQL.API.Types;
    using GraphQL.Core.Repositories;
    using GraphQL.Infrastructure.Data;
    using GraphQL.Infrastructure.Repositories;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    public class Startup
    {
        private readonly ApiConfiguration apiConfiguration;

        public Startup(IConfiguration configuration)
        {
            this.apiConfiguration = configuration.Get<ApiConfiguration>();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Configurations
            services.AddSingleton(this.apiConfiguration.MongoDbConfiguration);

            // Repositories
            services.AddSingleton<ICatalogContext, CatalogContext>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            // GraphQL
            services
                .AddGraphQLServer()
                .AddQueryType(d => d.Name("Query"))
                    .AddTypeExtension<ProductQuery>()
                    .AddTypeExtension<CategoryQuery>()
                .AddMutationType(d => d.Name("Mutation"))
                    .AddTypeExtension<ProductMutation>()
                    .AddTypeExtension<CategoryMutation>()
                .AddSubscriptionType(d => d.Name("Subscription"))
                    .AddTypeExtension<ProductSubscriptions>()
                .AddType<ProductType>()
                .AddType<CategoryResolver>()
                .AddInMemorySubscriptions();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseWebSockets();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL("/api/graphql");
            });
        }
    }
}
