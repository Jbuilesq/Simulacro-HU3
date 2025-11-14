FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["productosApi.Api/productosApi.Api.csproj", "productosApi.Api/"]
COPY ["productosApi.Application/productosApi.Application.csproj", "productosApi.Application/"]
COPY ["productosApi.Domain/productosApi.Domain.csproj", "productosApi.Domain/"]
COPY ["productosApi.Infrastructure/productosApi.Infrastructure.csproj", "productosApi.Infrastructure/"]
RUN dotnet restore "productosApi.Api/productosApi.Api.csproj"
COPY . .
WORKDIR "/src/productosApi.Api"
RUN dotnet build "./productosApi.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./productosApi.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "productosApi.Api.dll"]
