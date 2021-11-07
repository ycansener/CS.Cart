#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["CS.Cart.API/CS.Cart.API.csproj", "CS.Cart.API/"]
RUN dotnet restore "CS.Cart.API/CS.Cart.API.csproj"
COPY . .
WORKDIR "/src/CS.Cart.API"
RUN dotnet build "CS.Cart.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CS.Cart.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CS.Cart.API.dll"]