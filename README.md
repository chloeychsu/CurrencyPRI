# CurrencyPRI

CurrencyPRI 專案使用 ASP.NET Core 8.0 建置.呼叫 Coindesk API 取得相關資訊並提供新的 API 來查詢貨幣資訊.

## 實作功能
- 從 Coindesk API 獲取幣別資料並轉換為新 API。
- 幣別資料庫的 CRUD 操作。
- 多語系支援。
- 紀錄所有 API 請求和回應軌跡。
- Error handling 處理 API response。
- 集成 Swagger-UI 進行 API 文件生成。
- Repository design pattern 實作。
- 在 Docker 中運行。
- UnitTest 單元測試。


## Prerequisites
- .NET Core SDK 8.0
- SQL Server 2022 Express LocalDB
- Docker
- Docker Compose

## 執行
您可以按照以下說明在本機執行此 API：

1. 打開終端機並下載 Git Repo 原始碼至您的本機
    ```bash
    git clone https://github.com/chloeychsu/CurrencyPRI.git
    ```
2. 進入 CurrencyPRI 專案資料夾
    ```bash
    cd CurrencyPRI
    ```
3. 建置 Docker 容器,安裝請先確保您的本機端已安裝 Docker 和 Docker Compose
    ```bash
    docker compose build
    docker compose up -d
    ```
4. Access API
    - Swagger-UI: `http://localhost:5212/swagger/index.html`


## API Endpoints

### Currency
- `GET /api/currency` - Get all currencies.
- `GET /api/currency/{code}` - Get a specific currency by Code.
- `POST /api/currency` - Add a new currency.
- `PUT /api/currency/{code}` - Update a currency.
- `DELETE /api/currency/{code}` - Delete a currency.

### Server
- `GET /api/server/GetCoindeskData` - Get transformed data from Coindesk API.
- `GET /api/server/GetAllActionTrail` - Get all request and response trail data.

## SQL Server Database
使用 Docker 容器服務建立 SQL Server 2022 資料庫伺服器, EntityFrameworkCore Code First 建置資料庫和資料表.

```bash
dotnet ef migrations add "InitialCreate" -o Data/Migrations
dotnet ef database update
```


## Unit Tests

Run the unit tests using the following command:
```bash
dotnet test
```


