#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["OrderBox.Api/OrderBox.Api.csproj", "OrderBox.Api/"]
RUN dotnet restore "OrderBox.Api/OrderBox.Api.csproj"
COPY . .
WORKDIR "/src/OrderBox.Api"
RUN dotnet build "OrderBox.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OrderBox.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OrderBox.Api.dll"]