# graphql-netcore

[![.NET](https://img.shields.io/badge/.NET-Core%203.1-512BD4?logo=dotnet)](https://dotnet.microsoft.com/en-us/download/dotnet/3.1)
[![HotChocolate](https://img.shields.io/badge/HotChocolate-v11.0.9-E10098?logo=graphql)](https://chillicream.com/docs/hotchocolate/v11)
[![MongoDB](https://img.shields.io/badge/MongoDB-2.12-47A248?logo=mongodb)](https://www.mongodb.com/)
[![Docker](https://img.shields.io/badge/Docker-ready-2496ED?logo=docker)](https://www.docker.com/)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](LICENSE)

A sample **GraphQL API** built with **ASP.NET Core 3.1** and **HotChocolate v11**, demonstrating Queries, Mutations, Subscriptions, and field Resolvers backed by a **MongoDB** database.

---

## Table of Contents

- [Overview](#overview)
- [Tech Stack](#tech-stack)
- [Architecture](#architecture)
- [How GraphQL is Implemented](#how-graphql-is-implemented)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Running Locally](#running-locally)
  - [Running with Docker](#running-with-docker)
- [Example Queries & Mutations](#example-queries--mutations)
- [Folder Structure](#folder-structure)
- [Notes on Legacy Versions](#notes-on-legacy-versions)
- [Roadmap](#roadmap)

---

## Overview

**graphql-netcore** is a reference project that showcases how to expose a GraphQL API using the [HotChocolate](https://chillicream.com/) framework on top of ASP.NET Core. It covers the four fundamental building blocks of GraphQL:

| Concept | Description |
|---|---|
| **Query** | Read products and categories from MongoDB |
| **Mutation** | Create and remove products; triggers real-time events |
| **Subscription** | Real-time notifications when a product is created or removed |
| **Resolver** | Field-level resolver that resolves a `Category` from a `Product` |

The project uses a clean layered architecture with `Core`, `Infrastructure`, and `API` layers, making it easy to extend with additional domain entities.

---

## Tech Stack

| Technology | Version | Purpose |
|---|---|---|
| .NET Core | 3.1 | Runtime & framework |
| ASP.NET Core | 3.1 | Web host |
| HotChocolate | 11.0.9 | GraphQL server |
| MongoDB.Driver | 2.12.1 | Database driver |
| Docker / Compose | — | Containerisation |
| New Relic | — | Application monitoring (optional) |
| xUnit | 2.4.0 | Unit testing |

---

## Architecture

The solution is split into three projects following the classic **separation of concerns** pattern:

```
graphql-netcore.sln
└── src/
    ├── GraphQL.Core           # Domain entities & repository interfaces
    ├── GraphQL.Infrastructure # MongoDB implementations, context & seeding
    └── GraphQL.API            # ASP.NET Core host, GraphQL schema, Types
```

**Data flow:**

```
Client (GraphQL request)
    │
    ▼
GraphQL.API  ──resolves via──▶  IProductRepository / ICategoryRepository
    │                                         │
    │                                         ▼
    │                              GraphQL.Infrastructure
    │                              (MongoDB CatalogContext)
    └── fires events ──▶  In-Memory Subscription bus (ITopicEventSender)
```

- **GraphQL.Core** defines `Product`, `Category`, `BaseEntity`, and the repository interfaces. It has no dependency on the database driver.
- **GraphQL.Infrastructure** implements the repositories using the MongoDB driver, wires up `CatalogContext`, and seeds the database with sample data on startup.
- **GraphQL.API** registers everything with ASP.NET Core DI, configures HotChocolate, and exposes the GraphQL endpoint at `/api/graphql`.

---

## How GraphQL is Implemented

### Schema Registration (Startup.cs)

HotChocolate v11 uses a code-first, annotation-based approach. The schema is built by chaining type extensions onto root types:

```csharp
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
```

WebSockets are enabled to support subscriptions:

```csharp
app.UseWebSockets();
app.UseEndpoints(endpoints => endpoints.MapGraphQL("/api/graphql"));
```

### Queries

```csharp
[ExtendObjectType(Name = "Query")]
public class ProductQuery
{
    public Task<IEnumerable<Product>> GetProductsAsync([Service] IProductRepository repo) =>
        repo.GetAllAsync();

    public Task<Product> GetProductAsync(string id, [Service] IProductRepository repo) =>
        repo.GetByIdAsync(id);
}
```

### Mutations

Mutations insert or remove a document and publish an event to the subscription bus:

```csharp
[ExtendObjectType(Name = "Mutation")]
public class ProductMutation
{
    public async Task<Product> CreateProductAsync(Product product,
        [Service] IProductRepository repo, [Service] ITopicEventSender sender)
    {
        var result = await repo.InsertAsync(product);
        await sender.SendAsync(nameof(ProductSubscriptions.OnCreateAsync), result);
        return result;
    }
}
```

### Subscriptions

```csharp
[ExtendObjectType(Name = "Subscription")]
public class ProductSubscriptions
{
    [Subscribe][Topic]
    public Task<Product> OnCreateAsync([EventMessage] Product product) =>
        Task.FromResult(product);

    [Subscribe][Topic]
    public Task<string> OnRemoveAsync([EventMessage] string productId) =>
        Task.FromResult(productId);
}
```

### Field Resolver

The `CategoryResolver` is a type extension on `Product` that lazily fetches the related `Category`:

```csharp
[ExtendObjectType(Name = "Category")]
public class CategoryResolver
{
    public Task<Category> GetCategoryAsync(
        [Parent] Product product, [Service] ICategoryRepository repo) =>
        repo.GetByIdAsync(product.CategoryId);
}
```

---

## Getting Started

### Prerequisites

- [.NET Core SDK 3.1](https://dotnet.microsoft.com/en-us/download/dotnet/3.1)
- [MongoDB](https://www.mongodb.com/try/download/community) running on `localhost:27017`  
  *(or use Docker Compose — see below)*
- [Docker](https://www.docker.com/get-started) *(optional)*

### Running Locally

```bash
# 1. Clone the repository
git clone https://github.com/dianper/graphql-netcore.git
cd graphql-netcore

# 2. Start MongoDB (skip if already running)
docker run -d -p 27017:27017 --name catalogdb mongo

# 3. Restore & run the API
cd src/GraphQL.API
dotnet restore
dotnet run
```

The GraphQL endpoint is available at:

```
http://localhost:9000/api/graphql
```

Open the **Banana Cake Pop** (HotChocolate's built-in IDE) in your browser to explore and execute queries interactively.

#### Running Tests

```bash
cd src/GraphQL.API.Tests
dotnet test
```

### Running with Docker

The project ships with a `docker-compose.yml` that starts both **MongoDB** and the **GraphQL API** together:

```bash
# Build & start all services
docker-compose up --build

# Run in detached mode
docker-compose up --build -d
```

Services exposed:

| Service | Host Port | Description |
|---|---|---|
| `graphql.api` | `9006` | GraphQL API (`/api/graphql`) |
| `catalogdb` | `27017` | MongoDB instance |

The API will be reachable at `http://localhost:9006/api/graphql`.

#### Build the image manually

```bash
docker build -t graphqlapi -f src/GraphQL.API/Dockerfile .
docker run -p 9000:9000 \
  -e "MongoDbConfiguration__ConnectionString=mongodb://host.docker.internal:27017" \
  graphqlapi
```

---

## Example Queries & Mutations

Use the Banana Cake Pop IDE or any GraphQL client (e.g. [Insomnia](https://insomnia.rest/), [Postman](https://www.postman.com/)) against `http://localhost:9000/api/graphql`.

### Get all products

```graphql
query {
  products {
    id
    name
    description
    price
    quantity
    category {
      id
      description
    }
  }
}
```

### Get a single product

```graphql
query {
  product(id: "605fbfd4f0d09d08fba6bd80") {
    name
    price
  }
}
```

### Create a product

```graphql
mutation {
  createProduct(product: {
    name: "New Product"
    description: "A brand new product"
    price: 29.99
    quantity: 10
    categoryId: "605fbfdda571444fd7ade05b"
  }) {
    id
    name
    price
  }
}
```

### Remove a product

```graphql
mutation {
  removeProduct(id: "605fbfd4f0d09d08fba6bd80")
}
```

### Subscribe to product creation

```graphql
subscription {
  onCreate {
    id
    name
    price
  }
}
```

### Subscribe to product removal

```graphql
subscription {
  onRemove
}
```

> **Note:** Subscriptions require a WebSocket-capable client. Banana Cake Pop supports this out of the box.

---

## Folder Structure

```
graphql-netcore/
├── docker-compose.yml               # Multi-service Docker Compose definition
├── docker-compose.override.yml      # Local dev overrides (ports, env vars)
├── docker-compose.dcproj            # Visual Studio Docker Compose project
├── graphql-netcore.sln              # Solution file
├── newrelic/                        # New Relic agent binaries (optional monitoring)
└── src/
    ├── GraphQL.API/                 # ASP.NET Core host & GraphQL schema
    │   ├── Configurations/          # Configuration models (MongoDbConfiguration)
    │   ├── Mutations/               # ProductMutation, CategoryMutation
    │   ├── Queries/                 # ProductQuery, CategoryQuery
    │   ├── Resolvers/               # CategoryResolver (field-level resolver)
    │   ├── Subscriptions/           # ProductSubscriptions
    │   ├── Types/                   # ProductType (HotChocolate ObjectType)
    │   ├── Dockerfile               # Container image definition
    │   ├── Program.cs               # Application entry point
    │   ├── Startup.cs               # DI & middleware configuration
    │   └── appsettings.json         # App settings (connection string, etc.)
    │
    ├── GraphQL.Core/                # Domain layer (no framework dependencies)
    │   ├── Entities/                # Product, Category, BaseEntity
    │   └── Repositories/            # IBaseRepository<T>, IProductRepository, ICategoryRepository
    │
    ├── GraphQL.Infrastructure/      # Data access layer
    │   ├── Configurations/          # MongoDbConfiguration binding
    │   ├── Data/                    # CatalogContext, CatalogContextSeed
    │   └── Repositories/            # BaseRepository<T>, ProductRepository, CategoryRepository
    │
    └── GraphQL.API.Tests/           # xUnit integration tests
        └── QueryTests.cs            # Tests for product queries
```

---

## Notes on Legacy Versions

> ⚠️ **This project currently targets .NET Core 3.1 (end-of-life since December 2022) and HotChocolate v11.**

- **.NET Core 3.1** reached end-of-life on **December 13, 2022**. It is no longer receiving security patches. For production workloads, migrating to .NET 8 (LTS) or .NET 9 is strongly recommended.
- **HotChocolate v11** is several major versions behind the current release. Newer versions (v13/v14) introduce breaking changes in the annotation API, schema building, and subscription handling, but offer significant performance and feature improvements.

This repository is kept at its original versions for historical reference and learning purposes.

---

## Roadmap

The following improvements are planned for future iterations:

- [ ] **Upgrade to .NET 8 / .NET 9** — migrate the solution off the end-of-life .NET Core 3.1 runtime
- [ ] **Upgrade HotChocolate to v14** — adopt the latest HC API, pagination helpers, and filtering/sorting middleware
- [ ] **Add CategoryMutation** — complete CRUD operations for categories
- [ ] **Add integration tests** — extend test coverage beyond the current product query test
- [ ] **Add OpenTelemetry support** — replace New Relic with vendor-neutral observability
- [ ] **Add GitHub Actions CI workflow** — automated build, test, and Docker image publishing
