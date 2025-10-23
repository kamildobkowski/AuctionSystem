FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY AuctionSystem.sln ./

COPY Services/Identity/Identity.Api/Identity.Api.csproj             Services/Identity/Identity.Api/
COPY Services/Identity/Identity.Domain/Identity.Domain.csproj       Services/Identity/Identity.Domain/
COPY Services/Identity/Identity.Application/Identity.Application.csproj Services/Identity/Identity.Application/
COPY Services/Identity/Identity.Infrastructure/Identity.Infrastructure.csproj Services/Identity/Identity.Infrastructure/
COPY Services/Shared/Shared.Base/Shared.Base.csproj                 Services/Shared/Shared.Base/
COPY Services/Shared/Shared.Events/Shared.Events.csproj             Services/Shared/Shared.Events/

RUN dotnet restore "Services/Identity/Identity.Api/Identity.Api.csproj"

COPY . .
RUN dotnet publish "Services/Identity/Identity.Api/Identity.Api.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Identity.Api.dll"]