# VernyykodTestApp

Микросервисное приложение на .NET 8 для регистрации пользователей, аутентификации по JWT и управления избранными валютами. Курсы валют периодически загружаются с сайта ЦБ РФ.

## Содержание

- [Возможности](#возможности)
- [Архитектура](#архитектура)
- [Структура решения](#структура-решения)
- [Сервисы](#сервисы)
- [База данных](#база-данных)
- [API](#api)
- [Запуск через Docker Compose](#запуск-через-docker-compose)
- [Локальный запуск без Docker](#локальный-запуск-без-docker)
- [Конфигурация](#конфигурация)
- [Тесты](#тесты)
- [Технологии](#технологии)

## Возможности

- Регистрация и вход пользователя с выдачей JWT
- Выход с отзывом токена (`jti` сохраняется в `revoked_token`)
- Добавление / удаление / получение избранных валют пользователя
- Фоновый парсинг курсов валют с ЦБ РФ (XML daily)
- Единая точка входа через API Gateway (YARP) со сводным Swagger UI

## Архитектура

Проект построен по принципам **Clean Architecture** и **CQRS**:

| Слой | Назначение |
|------|------------|
| **Api / ConsoleApp** | Точка входа, HTTP, DI, middleware |
| **Application** | Request handlers (use cases), DTO, интерфейсы репозиториев |
| **Domain** | Агрегаты, value objects, доменная логика |
| **Infrastructure** | EF Core, репозитории, внешние интеграции |

Разделение read/write:

- отдельные DbContext для чтения и записи (где это применимо);
- write-операции работают с доменными агрегатами;
- read-операции проецируют данные сразу в DTO.

Общий код вынесен в **Shared** (JWT, исключения, unit of work, swagger-регистрация, общие сущности).

```
Клиент
  │
  ▼
ApiGateway (YARP + Swagger UI)  :7730
  ├── /api/auth/**            → UserService
  └── /api/user-currencies/** → FinanceService
        │
        ▼
   PostgreSQL (vernyykod)
        ▲
        │
CurrencyParserService (фоновый парсер ЦБ РФ)
MigrationService (одноразовый запуск миграций)
```

## Структура решения

Solution: `src/VernyykodTestApp.sln`

```
src/
├── ApiGateway/
│   └── ApiGateway.Api/
├── UserService/
│   ├── UserService.Api/
│   ├── UserService.Application/
│   ├── UserService.Domain/
│   └── UserService.Infrastructure/
├── FinanceService/
│   ├── FinanceService.Api/
│   ├── FinanceService.Application/
│   ├── FinanceService.Domain/
│   └── FinanceService.Infrastructure/
├── CurrencyParserService/
│   ├── CurrencyParserService.ConsoleApp/
│   ├── CurrencyParserService.Application/
│   └── CurrencyParserService.Infrastructure/
├── MigrationService/
│   └── MigrationService.ConsoleApp/
├── Shared/
│   ├── Shared.Api/
│   ├── Shared.Application/
│   ├── Shared.Domain/
│   └── Shared.Infrastructure/
└── Tests/
    └── UnitTests/
```

## Сервисы

### ApiGateway

- Reverse proxy на **YARP**
- Агрегированный Swagger UI:
  - User Service: `/user/swagger/v1/swagger.json`
  - Finance Service: `/finance/swagger/v1/swagger.json`
- Локальный порт: `http://localhost:7730`
- Корень `/` редиректит на `/swagger`

### UserService

Аутентификация и управление пользователями.

| Метод | Путь | Auth | Описание |
|-------|------|------|----------|
| `POST` | `/api/auth/register` | нет | Регистрация, возвращает JWT |
| `POST` | `/api/auth/login` | нет | Вход, возвращает JWT |
| `POST` | `/api/auth/logout` | JWT | Отзыв текущего токена |

Локальный порт: `http://localhost:7731`

### FinanceService

Избранные валюты текущего пользователя (user id берётся из JWT).

| Метод | Путь | Auth | Описание |
|-------|------|------|----------|
| `GET` | `/api/user-currencies` | JWT | Список избранных валют с курсами |
| `POST` | `/api/user-currencies/{currencyId}` | JWT | Добавить валюту в избранное |
| `DELETE` | `/api/user-currencies/{currencyId}` | JWT | Удалить валюту из избранного |

Локальный порт: `http://localhost:7732`

### CurrencyParserService

Фоновый worker:

1. Запрашивает XML курсов с ЦБ РФ (`https://www.cbr.ru/scripts/XML_daily.asp`)
2. Обновляет таблицу `currency`
3. Ждёт интервал из `currencyParserBackgroundService:delayInSeconds` (по умолчанию 3600 сек) и повторяет цикл

### MigrationService

Одноразовое консольное приложение на **FluentMigrator**:

1. Создаёт БД `vernyykod`, если её ещё нет
2. Применяет все миграции
3. Завершает работу

В Docker Compose запускается **до** остальных сервисов и не перезапускается (`restart: "no"`).

## База данных

PostgreSQL, БД `vernyykod`.

### Таблицы

| Таблица | Назначение |
|--------|------------|
| `user` | Пользователи (`id`, `name`, `password`) |
| `currency` | Курсы валют (`id`, `name`, `rate`) |
| `user_currency` | Связь пользователь ↔ валюта (уникальный индекс по паре) |
| `revoked_token` | Отозванные JWT (`jti`, `revoked_at`) |

Миграции лежат в `src/MigrationService/MigrationService.ConsoleApp/Migrations/`.

## API

### Регистрация

```http
POST /api/auth/register
Content-Type: application/json

{
  "name": "ivan",
  "password": "secret"
}
```

Ответ `201`:

```json
{
  "userId": "aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee",
  "userName": "ivan",
  "token": "<jwt>"
}
```

### Вход

```http
POST /api/auth/login
Content-Type: application/json

{
  "name": "ivan",
  "password": "secret"
}
```

Ответ `200` — тот же формат, что и при регистрации.

### Выход

```http
POST /api/auth/logout
Authorization: Bearer <jwt>
```

Ответ `204`.

### Избранные валюты

```http
GET /api/user-currencies
Authorization: Bearer <jwt>
```

```json
{
  "userCurrencies": [
    {
      "currencyId": "R01235",
      "name": "Доллар США",
      "rate": 90.1234567890
    }
  ]
}
```

```http
POST /api/user-currencies/R01235
Authorization: Bearer <jwt>
```

```http
DELETE /api/user-currencies/R01235
Authorization: Bearer <jwt>
```

Через gateway все эти пути доступны на `http://localhost:7730/...`.

Swagger UI: [http://localhost:7730/swagger](http://localhost:7730/swagger)

В Swagger UI для защищённых методов нужно нажать **Authorize** и указать:

```text
Bearer <jwt>
```

или просто токен (схема уже настроена как Bearer).

## Запуск через Docker Compose

Требования: Docker Desktop / Docker Engine с Compose v2.

Из корня репозитория:

```bash
docker compose up -d --build
```

### Порядок запуска

1. `postgresql` — ждёт healthcheck (`pg_isready`)
2. `migration-service` — применяет миграции и завершается
3. `currency-parser-service` — стартует только после успешного завершения миграций
4. `user-service`, `finance-service` — после старта parser service
5. `api-gateway` — после старта API-сервисов

Unit-тесты в Docker **не** запускаются.

### Порты

| Сервис | Порт на хосте |
|--------|----------------|
| API Gateway | `7730` |
| PostgreSQL | `5432` |

UserService и FinanceService внутри сети Docker доступны gateway по именам `user-service` / `finance-service` (порт `8080`), наружу не публикуются.

### Полезные команды

```bash
# Логи
docker compose logs -f

# Логи конкретного сервиса
docker compose logs -f api-gateway
docker compose logs -f currency-parser-service

# Остановка
docker compose down

# Остановка с удалением volume БД
docker compose down -v
```

## Локальный запуск без Docker

Требования:

- .NET 8 SDK
- PostgreSQL (можно поднять только БД: `docker compose up -d postgresql`)

### 1. Миграции

```bash
dotnet run --project src/MigrationService/MigrationService.ConsoleApp
```

### 2. Парсер курсов

```bash
dotnet run --project src/CurrencyParserService/CurrencyParserService.ConsoleApp
```

### 3. API-сервисы и gateway

```bash
dotnet run --project src/UserService/UserService.Api
dotnet run --project src/FinanceService/FinanceService.Api
dotnet run --project src/ApiGateway/ApiGateway.Api
```

Либо открыть `src/VernyykodTestApp.sln` в IDE и запустить несколько проектов сразу.

Порты по умолчанию:

| Сервис | URL |
|--------|-----|
| ApiGateway | http://localhost:7730 |
| UserService | http://localhost:7731 |
| FinanceService | http://localhost:7732 |

## Конфигурация

Параметры берутся из `appsettings.json` и могут быть переопределены переменными окружения (разделитель `__`).

### PostgreSQL

```json
"connections": {
  "postgres": {
    "host": "localhost",
    "port": "5432",
    "username": "postgres",
    "password": "ps",
    "database": "vernyykod"
  }
}
```

В Docker host переопределяется на `postgresql`.

### JWT (UserService / FinanceService)

```json
"jwt": {
  "issuer": "VernyyKod",
  "key": "VernyyKod_key_3dfe6cb4-ea12-456b-9be8-a4978df8ce34",
  "expiresTokenInHours": 2
}
```

Ключ и issuer должны совпадать в обоих сервисах, иначе токен UserService не пройдёт валидацию в FinanceService.

### Парсер валют

```json
"cbr": {
  "http": {
    "baseUrl": "https://www.cbr.ru/",
    "xmldailyurl": "scripts/XML_daily.asp"
  }
},
"currencyParserBackgroundService": {
  "delayInSeconds": 3600
}
```

## Тесты

Unit-тесты (NUnit + NSubstitute + AutoFixture) лежат в `src/Tests/UnitTests` и покрывают request handlers UserService и FinanceService.

```bash
dotnet test src/Tests/UnitTests/UnitTests.csproj
```

или из solution:

```bash
dotnet test src/VernyykodTestApp.sln
```

## Технологии

| Область | Стек |
|---------|------|
| Runtime | .NET 8 |
| API | ASP.NET Core, Swashbuckle |
| Gateway | YARP Reverse Proxy |
| ORM | EF Core + Npgsql |
| Миграции | FluentMigrator |
| Auth | JWT Bearer |
| БД | PostgreSQL 17 |
| Тесты | NUnit, NSubstitute, AutoFixture |
| Контейнеры | Docker, Docker Compose |

## Типичный сценарий использования

1. Поднять стек: `docker compose up -d --build`
2. Открыть Swagger: http://localhost:7730/swagger
3. Зарегистрировать пользователя через `POST /api/auth/register`
4. Скопировать `token` из ответа
5. Authorize в Swagger → вызвать `GET/POST/DELETE /api/user-currencies`
6. Дождаться первого цикла CurrencyParserService (или посмотреть логи), чтобы в БД появились курсы валют
