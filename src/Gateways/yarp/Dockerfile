# src/Gateways/yarp/Dockerfile
########################################
# 1) ETAP BUILD
########################################
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1.1) cache’ujemy restore kopiując tylko .sln i .csproj
COPY AuctionSystem.sln ./
COPY Gateways/yarp/yarp.csproj         Gateways/yarp/

# 1.2) restore zależności tylko dla YARP
RUN dotnet restore "Gateways/yarp/yarp.csproj"

# 1.3) kopiujemy resztę kodu i publikujemy
COPY . .
RUN dotnet publish "Gateways/yarp/yarp.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

########################################
# 2) ETAP RUNTIME
########################################
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# 2.1) kopiujemy build artifacts
COPY --from=build /app/publish .

# 2.2) expose i ENTRYPOINT exec-formą
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80
ENTRYPOINT ["dotnet", "yarp.dll"]