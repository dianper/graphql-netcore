FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["src/GraphQL.API/GraphQL.API.csproj", "src/GraphQL.API/"]
COPY ["src/GraphQL.Infrastructure/GraphQL.Infrastructure.csproj", "src/GraphQL.Infrastructure/"]
COPY ["src/GraphQL.Core/GraphQL.Core.csproj", "src/GraphQL.Core/"]
RUN dotnet restore "src/GraphQL.API/GraphQL.API.csproj"
COPY . .
WORKDIR "/src/src/GraphQL.API"
RUN dotnet build "GraphQL.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GraphQL.API.csproj" -c Release -r debian-x64 -o /app/publish

ENV CORECLR_ENABLE_PROFILING="1" \
CORECLR_PROFILER="{36032161-FFC0-4B61-B559-F6C5D41BAE5A}" \
CORECLR_NEWRELIC_HOME="/usr/local/newrelic-netcore20-agent" \
CORECLR_PROFILER_PATH="/usr/local/newrelic-netcore20-agent/libNewRelicProfiler.so" \
NEW_RELIC_LICENSE_KEY="KEY" \
NEW_RELIC_APP_NAME="graphqlapi"

WORKDIR app

ARG NewRelic=./newrelic
COPY $NewRelic ./newrelic
COPY $NewRelic ./newrelic

RUN dpkg -i ./newrelic/newrelic-netcore20-agent*.deb

ENV ASPNETCORE_URLS http://+:9000
EXPOSE 9000

WORKDIR /app/publish
ENTRYPOINT ["dotnet", "./GraphQL.API.dll"]
