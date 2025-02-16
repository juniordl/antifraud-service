FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
ARG BUILD_CONFIGURATION=Release

COPY ../../../Services/AntiFraudService/AntiFraudService.Api/AntiFraudService.Api.csproj /AntiFraudService.Api/.
COPY ../../../Services/AntiFraudService/AntiFraudService.Application/AntiFraudService.Application.csproj /AntiFraudService.Application/.
COPY ../../../Services/AntiFraudService/AntiFraudService.Infrastructure/AntiFraudService.Infrastructure.csproj /AntiFraudService.Infrastructure/.

COPY ../../../Shared/Common.Messaging.Contracts/Common.Messaging.Contracts.csproj /Shared/Common.Messaging.Contracts/.
COPY ../../../Shared/Common.Messaging.Core/Common.Messaging.Core.csproj /Shared/Common.Messaging.Core/.
COPY ../../../Shared/Common.Messaging.Kafka/Common.Messaging.Kafka.csproj /Shared/Common.Messaging.Kafka/.

RUN dotnet restore "/AntiFraudService.Api/AntiFraudService.Api.csproj"

COPY ../../../ ./

WORKDIR /src/Services/AntiFraudService/AntiFraudService.Api
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out ./

ENTRYPOINT ["dotnet", "AntiFraudService.Api.dll"]

