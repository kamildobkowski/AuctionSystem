FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY AuctionSystem.sln ./

COPY Services/Auctions/Auctions.Api/Auctions.Api.csproj             Services/Auctions/Auctions.Api/
COPY Services/Auctions/Auctions.Domain/Auctions.Domain.csproj       Services/Auctions/Auctions.Domain/
COPY Services/Auctions/Auctions.Application/Auctions.Application.csproj Services/Auctions/Auctions.Application/
COPY Services/Auctions/Auctions.Infrastructure/Auctions.Infrastructure.csproj Services/Auctions/Auctions.Infrastructure/
COPY Services/Shared/Shared.Base/Shared.Base.csproj                 Services/Shared/Shared.Base/
COPY Services/Shared/Shared.Events/Shared.Events.csproj             Services/Shared/Shared.Events/

RUN dotnet restore "Services/Auctions/Auctions.Api/Auctions.Api.csproj"

COPY . .
RUN dotnet publish "Services/Auctions/Auctions.Api/Auctions.Api.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Auctions.Api.dll"]