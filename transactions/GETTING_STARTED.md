# Getting Started

Aquí encontrará una guia de como levantar en un entorno local, considerar los siguientes requisitos

- S.O: Windows / MacOS(x86/arm)
- Docker

## Ejecución del Proyecto

1. Ingresar a la carpeta raíz donde se encuentra el archivo `docker-compose.yml`
```
cd src/Deployments
```

2. Ejecutar el siguiente comando para levantar todos los contenedores:
```
docker-compose up -d  
```

Se comenzaran a descargar y compilar las imagenes del proyecto, que se conforman de la siguiente manera:

- `kafka-ui`: Cliente de kafka para poder tener una interfaz grafica de los mensajes, topicos y consumidores (http://localhost:8180)

- `redis`: Instancia de redis cache para almanecenar los acumulados de las transacciones (http://localhost:6379)

- `transaction-api`: API que permite la creación de las transacciones (http://localhost:8081/swagger)

- `antifraud-api`: API que consume los eventos de transacciones creadas desde kafka (http://localhost:8082/health)

Nota: las imagenes utilizadas en el archivo `docker-compose.yml` pueden variar debido a que el proyecto se desarrollo en un equipo con procesador arm64

## Configuraciones

De cara a la prueba se tiene ya pre-configurado los appsettings de los proyectos suponiendo que se van a ejecutar todos en conjunto con docker-compose, de lo contrario se puedo cambiar el nombre de los contenedores por `localhost`

### transaction-api
```json
{
  "Redis": {
    "Server": "redis:6379"
  },
  "Kafka": {
    "Server": "kafka:29092",
    "ProducerTopic": "validated-transactions-topic",
    "ConsumerGroup": "created-transaction-consumer-group",
    "ConsumerTopic": "created-transactions-topic",
    "Offset": "Earliest"
  },
}
```
### antifraud-api

```json
{
  "ConnectionStrings": {
    "PostgresDb": "Host=postgres;Port=5432;Database=postgres;Username=postgres;Password=postgres;Pooling=true;SSL Mode=Prefer"
  },
  "Kafka": {
    "Server": "kafka:29092",
    "ProducerTopic": "created-transactions-topic",
    "ConsumerGroup": "validated-transaction-consumer-group",
    "ConsumerTopic": "validated-transactions-topic",
    "Offset": "Earliest"
  },
}
```

# Endpoints

## transaction-api

#### Crear una transacción

```
POST http://localhost:8081/api/v1/transactions
```

Petición
```json
{
  "sourceAccountId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "transferAccountId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "transferType": 0,
  "value": 1800
}
```
Respuesta
```json
{
  "transactionId": "84584224-8b28-403f-8f25-cf47c512ece4",
  "sourceAccountId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "transferAccountId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "transferType": 0,
  "value": 1800,
  "status": "Pending",
  "createdAt": "2025-02-16T23:53:56.9587999Z",
  "modifiedAt": "2025-02-16T23:53:56.9588209Z"
}
```

#### Consultar una transacción
```
GET http://localhost:8081/api/v1/transactions/{transactionId}
```

Respuesta
```json
{
  "transactionId": "84584224-8b28-403f-8f25-cf47c512ece4",
  "sourceAccountId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "transferAccountId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "transferType": 0,
  "value": 1800,
  "status": "Approved",
  "createdAt": "2025-02-16T23:53:56.9587999Z",
  "modifiedAt": "2025-02-16T23:53:56.9588209Z"
}
```

#### Salud del api (HealthCheck)
```
GET http://localhost:8081/health
```

Respuesta
```json
{
  "status": "Healthy",
  "totalDuration": "00:00:01.0596583",
  "entries": {
    "kafka": {
      "data": { },
      "duration": "00:00:01.0418767",
      "status": "Healthy",
      "tags": []
    },
    "npgsql": {
      "data": { },
      "duration": "00:00:00.0470451",
      "status": "Healthy",
      "tags": []
    }
  }
}
```
## antifraud-api
#### Salud del api (HealthCheck)
```
GET http://localhost:8082/health
```

Respuesta
```json
{
  "status": "Healthy",
  "totalDuration": "00:00:02.0750645",
  "entries": {
    "kafka": {
      "data": { },
      "duration": "00:00:02.0717604",
      "status": "Healthy",
      "tags": []
    },
    "Redis": {
      "data": { },
      "description": "Redis is reachable",
      "duration": "00:00:00.0616679",
      "status": "Healthy",
      "tags": []
    }
  }
}
```