# Contributing: MySQL local setup

This document explains how to set up a local MySQL server and configure the project to use it for development.

## Prerequisites
- Windows 10/11 or a supported OS
- .NET SDK (matching the solution)
- PowerShell (or any terminal)

## 1. Install MySQL Server (Community)
1. Download MySQL Community Server from https://dev.mysql.com/downloads/mysql/ and run the installer.
2. Choose a Developer Default installation and follow prompts.
3. Note the root password you set during installation.
4. Ensure the MySQL service is running (Services app or `Get-Service -Name MySQL*`).

Optional: install MySQL Workbench for GUI management.

## 2. Create database and developer user
Open PowerShell and run the CLI (replace `root` and `YOUR_ROOT_PASSWORD`):

```powershell
mysql -u root -p
# enter root password when prompted
CREATE DATABASE rishtanatahdb CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;
CREATE USER 'devuser'@'localhost' IDENTIFIED BY 'dev_password';
GRANT ALL PRIVILEGES ON rishtanatahdb.* TO 'devuser'@'localhost';
FLUSH PRIVILEGES;
EXIT;
```

Use different username/password for your environment and keep secrets out of source control.

## 3. Configure the project connection string
- Open `Presentation/appsettings.json` and set the `ConnectionStrings:DefaultConnection` value.
- Example connection string for MySQL (adjust user, password, host, port, and database):

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Port=3306;Database=rishtanatahdb;User=devuser;Password=dev_password;"
}
```

Prefer storing secrets in user secrets or environment variables for development:

```powershell
# set environment variable for current PowerShell session
$env:ConnectionStrings__DefaultConnection = "Server=localhost;Port=3306;Database=rishtanatahdb;User=devuser;Password=dev_password;"
```

## 4. Add EF Core MySQL provider (if not present)
The project commonly uses the Pomelo MySQL provider. From the repository root run:

```powershell
dotnet add Presentation package Pomelo.EntityFrameworkCore.MySql
dotnet add Presentation package Microsoft.EntityFrameworkCore.Design
```

## 5. Tools and migrations
1. (Optional) Install the EF Core CLI tool if you need global `dotnet ef`:

```powershell
dotnet tool install --global dotnet-ef
```

2. Create and apply migrations (adjust project/startup-project if your API project is not `Presentation`):

```powershell
dotnet ef migrations add InitialCreate --project Presentation --startup-project Presentation
dotnet ef database update --project Presentation --startup-project Presentation
```

If migrations fail, verify the provider is registered in `Program.cs` / `Startup.cs`:

```
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseMySql(configuration.GetConnectionString("DefaultConnection"), ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection"))));
```

## 6. Common troubleshooting
- "Can't connect": confirm MySQL service is running and port 3306 is open.
- "Access denied": verify user, host (`'devuser'@'localhost'`) and privileges.
- If `ServerVersion.AutoDetect` fails, specify version explicitly, e.g. `new MySqlServerVersion(new Version(8,0,33))`.
- Use `mysql -h 127.0.0.1 -P 3306 -u devuser -p` to rule out socket vs TCP issues.

## 7. Security notes
- Do not commit passwords into source control. Use user secrets, environment variables, or a secrets manager.
- Restrict DB user privileges for development/CI to only what is necessary.

