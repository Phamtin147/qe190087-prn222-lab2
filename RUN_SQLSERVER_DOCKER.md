# Lab 02 - SQL Server Docker run guide

Lab 02 is the Razor Pages + SignalR version of the Product Management app. The app uses EF Core SQL Server provider and creates/seeds the database on startup.

## 1. Start SQL Server

The project is configured to connect to SQL Server on host port `14330`:

```bash
docker start sqlserver-lab
```

If the container does not exist yet, create it:

```bash
docker run -e "ACCEPT_EULA=Y" \
  -e "MSSQL_SA_PASSWORD=YourStrong@Passw0rd" \
  -p 14330:1433 \
  --name sqlserver-lab \
  -d mcr.microsoft.com/mssql/server:2022-latest
```

SQL Server requires a strong password when the container is created. After it starts, this lab uses:

- User: `sa`
- Password: `123`

If needed, change the password inside SQL Server:

```sql
ALTER LOGIN sa WITH PASSWORD = '123', CHECK_POLICY = OFF;
```

## 2. DBeaver connection

Create a new `MS SQL Server` connection:

- Host: `127.0.0.1`
- Port: `14330`
- Database: `master` for first test, or `MyStoreDB_Lab02` after the app has started once
- Username: `sa`
- Password: `123`

Driver properties:

- `encrypt=false`
- `trustServerCertificate=true`

The Docker mapping is `14330 -> 1433`, so use `14330` in DBeaver, not `1433`.

## 3. Run Lab 02

From this `Lab02_ProductManagementRazorPages` folder:

```bash
dotnet run --project ProductManagementRazorPages/ProductManagementRazorPages.csproj
```

On first startup, the app creates database `MyStoreDB_Lab02` with these tables:

- `AccountMember`
- `Categories`
- `Products`

Seed accounts:

- `admin / 123`
- `staff / 123`

## 4. Test flow

Open the app URL shown by `dotnet run`, then:

1. Login with `admin / 123`.
2. Open Products.
3. Create a product.
4. Edit the product.
5. Delete the product.

The Products page includes SignalR refresh support through `/signalRServer`.
