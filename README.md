# CurrencyPRI

CurrencyPRI 專案使用 ASP.NET Core 8.0 建置.呼叫 Coindesk API 取得相關資訊並提供新的 API 來查詢貨幣資訊, API 功能包含以下功能 : 

## Features

## Technologies Used

* Package
    ```cs
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer
    dotnet add package Microsoft.EntityFrameworkCore.Design
    dotnet add package Newtonsoft.Json
    dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection

    ```

## Getting Started

### Prerequisites
- .NET Core SDK 8.0
- SQL Server 2022 Express LocalDB
- Docker
- Docker Compose

### Installation

1. Clone the repository:
    ```bash
    git clone https://github.com/chloeychsu/CurrencyPRI.git
    cd CurrencyPRI
    ```
2. Run Docker Compose
    ```bash
    docker-compose up -d
    ```
3. Migrations and update database
    ```bash
    cd CurrencyApi
    dotnet ef migrations add "InitialCreate" -o Data/Migrations
    dotnet watch
    ```
### API Endpoints

- `GET /api/currencies` - Get all currencies.
- `GET /api/currencies/{id}` - Get a specific currency by ID.
- `POST /api/currencies` - Add a new currency.
- `PUT /api/currencies/{id}` - Update a currency.
- `DELETE /api/currencies/{id}` - Delete a currency.
- `GET /api/coindesk` - Get transformed data from Coindesk API.


### Unit Tests

Run the unit tests using the following command:
```bash
dotnet test
```