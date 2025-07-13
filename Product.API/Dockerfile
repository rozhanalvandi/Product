FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ../Product.sln ./
COPY ../Product.API/*.csproj ./Product.API/
COPY ../Product.Application/*.csproj ./Product.Application/
COPY ../Product.Infrastructure/*.csproj ./Product.Infrastructure/
COPY ../Product.Domain/*.csproj ./Product.Domain/
COPY ../Product.Tests/*.csproj ./Product.Tests/ 

RUN dotnet restore

COPY ../. ./
WORKDIR /app/Product.API
RUN dotnet publish -c Release -o  /app/out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "Product.API.dll"]
