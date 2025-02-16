FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
ARG BUILD_CONFIGURATION=Release

COPY ../../../Services/TransactionService/TransactionService.Api/TransactionService.Api.csproj /TransactionService.Api/.
COPY ../../../Services/TransactionService/TransactionService.Application/TransactionService.Application.csproj /TransactionService.Application/.
COPY ../../../Services/TransactionService/TransactionService.Domain/TransactionService.Domain.csproj /TransactionService.Domain/.
COPY ../../../Services/TransactionService/TransactionService.Infrastructure/TransactionService.Infrastructure.csproj /TransactionService.Infrastructure/.

COPY ../../../Shared/Common.Messaging.Contracts/Common.Messaging.Contracts.csproj /Shared/Common.Messaging.Contracts/.
COPY ../../../Shared/Common.Messaging.Core/Common.Messaging.Core.csproj /Shared/Common.Messaging.Core/.
COPY ../../../Shared/Common.Messaging.Kafka/Common.Messaging.Kafka.csproj /Shared/Common.Messaging.Kafka/.

RUN dotnet restore "/TransactionService.Api/TransactionService.Api.csproj"

COPY ../../../ ./

WORKDIR /src/Services/TransactionService/TransactionService.Api
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /out ./

ENTRYPOINT ["dotnet", "TransactionService.Api.dll"]

