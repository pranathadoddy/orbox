#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Orderbox.Mvc/Orderbox.Mvc.csproj", "Orderbox.Mvc/"]
COPY ["Orderbox.Repository/Orderbox.Repository.csproj", "Orderbox.Repository/"]
COPY ["Orderbox.DataAccess/Orderbox.DataAccess.csproj", "Orderbox.DataAccess/"]
COPY ["Orderbox.Dto/Orderbox.Dto.csproj", "Orderbox.Dto/"]
COPY ["Framework.Dto/Framework.Dto.csproj", "Framework.Dto/"]
COPY ["Framework.Repository/Framework.Repository.csproj", "Framework.Repository/"]
COPY ["Framework.RepositoryContract/Framework.RepositoryContract.csproj", "Framework.RepositoryContract/"]
COPY ["Framework.Core/Framework.Core.csproj", "Framework.Core/"]
COPY ["Orderbox.RepositoryContract/Orderbox.RepositoryContract.csproj", "Orderbox.RepositoryContract/"]
COPY ["Orderbox.Service/Orderbox.Service.csproj", "Orderbox.Service/"]
COPY ["Orderbox.ServiceContract/Orderbox.ServiceContract.csproj", "Orderbox.ServiceContract/"]
COPY ["Framework.ServiceContract/Framework.ServiceContract.csproj", "Framework.ServiceContract/"]
COPY ["Framework.Service/Framework.Service.csproj", "Framework.Service/"]
COPY ["Orderbox.Core/Orderbox.Core.csproj", "Orderbox.Core/"]
COPY ["Framework.Application/Framework.Application.csproj", "Framework.Application/"]
RUN dotnet restore "Orderbox.Mvc/Orderbox.Mvc.csproj"
COPY . .
WORKDIR "/src/Orderbox.Mvc"
RUN dotnet build "Orderbox.Mvc.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Orderbox.Mvc.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Orderbox.Mvc.dll"]