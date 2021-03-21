namespace GraphQL.API.Models
{
    using System;

    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Quantity { get; set; }

        public Price Price { get; set; }

        public DateTime Created { get; set; }
    }
}
