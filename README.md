# Finance Tracker

A full-stack personal finance management app. Track income and expenses, organize them by category, and visualize your financial health with a real-time dashboard.

![.NET 8](https://img.shields.io/badge/.NET-8-512BD4?logo=dotnet&logoColor=white)
![Nuxt 4](https://img.shields.io/badge/Nuxt-4-00DC82?logo=nuxtdotjs&logoColor=white)
![Vue 3](https://img.shields.io/badge/Vue-3-4FC08D?logo=vuedotjs&logoColor=white)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?logo=microsoftsqlserver&logoColor=white)
![TypeScript](https://img.shields.io/badge/TypeScript-3178C6?logo=typescript&logoColor=white)

## Features

**Authentication**
- Email/password registration and login
- JWT access tokens (short-lived, kept in memory on the client) + rotating refresh tokens stored in an httpOnly cookie
- Backend-for-frontend pattern: the Nuxt server handles the refresh cookie so it's never exposed to client-side JavaScript

**Transactions**
- Create, edit, and delete income/expense entries
- Filter by date range, category, and type, with pagination
- Export any filtered range to CSV

**Categories**
- 8 pre-seeded default categories (Food, Transport, Health, Housing, Education, Leisure, Salary, Other)
- Full CRUD for user-created custom categories

**Dashboard**
- Current balance, monthly income, and monthly expense summary cards
- Spending-by-category breakdown (pie chart)
- Balance evolution over the last 6 months (line chart)

## Tech Stack

| Layer | Technology |
|---|---|
| Frontend | Nuxt 4, Vue 3, Pinia, Tailwind CSS v4, shadcn-vue, Chart.js |
| Backend | .NET 8 Web API, Entity Framework Core, FluentValidation, Serilog, Swashbuckle |
| Database | SQL Server |
| Auth | JWT bearer tokens + httpOnly refresh cookie (BFF pattern) |
| Testing | xUnit, Moq |

## Architecture

```
Browser  <--HTTP-->  Nuxt 4 (SSR + BFF)  <--HTTP-->  .NET 8 Web API  <--EF Core-->  SQL Server
```

- The **access token** lives only in memory (Pinia store) on the client — never persisted to `localStorage` or a cookie readable by JavaScript.
- The **refresh token** is stored in an httpOnly, `SameSite=Lax` cookie, set and rotated by Nuxt server routes (`server/api/auth/*`) that proxy to the .NET API. This keeps it inaccessible to XSS while still allowing session restoration on page reload.
- All other API calls (transactions, categories, dashboard) go directly from the browser to the .NET API over CORS, authenticated with the in-memory access token.

## Project Structure

```
finance-tracker/
├── FinanceTracker.Api/          # .NET 8 Web API
│   ├── Controllers/
│   ├── Services/
│   ├── Repositories/
│   ├── Models/
│   │   ├── Entities/
│   │   └── DTOs/
│   ├── Middleware/
│   ├── Data/                    # AppDbContext + Migrations
│   └── Helpers/
├── FinanceTracker.Api.Tests/    # xUnit + Moq unit tests
└── finance-tracker-web/         # Nuxt 4 frontend
    ├── app/
    │   ├── pages/
    │   ├── components/
    │   ├── composables/
    │   ├── stores/               # Pinia
    │   ├── services/             # API fetch wrapper
    │   └── middleware/           # Route auth guard
    └── server/
        └── api/auth/             # BFF routes (cookie handling)
```

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js 18+](https://nodejs.org/)
- SQL Server (LocalDB is enough for local development)

### 1. Clone the repository

```bash
git clone https://github.com/EnzoPegorari/finance-tracker.git
cd finance-tracker
```

### 2. Backend setup

```bash
cd FinanceTracker.Api
dotnet restore

# Install the EF Core CLI tool if you don't already have it
dotnet tool install --global dotnet-ef

# Create the database and apply migrations (uses LocalDB by default)
dotnet ef database update

dotnet run
```

The API starts at `http://localhost:5299`. Swagger UI is available at `http://localhost:5299/swagger`.

By default it connects to `(localdb)\mssqllocaldb`. To use a different SQL Server instance, edit `ConnectionStrings:DefaultConnection` in `FinanceTracker.Api/appsettings.json`.

> **Note:** the `Jwt:Secret` in `appsettings.json` is a placeholder. Replace it with a real secret (via [user secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) or an environment variable) before deploying anywhere outside your machine.

### 3. Frontend setup

In a second terminal:

```bash
cd finance-tracker-web
npm install
npm run dev
```

The app runs at `http://localhost:3000` by default. Open it in your browser and register a new account to get started.

If you run the frontend on a different port, add it to `Cors:AllowedOrigins` in `FinanceTracker.Api/appsettings.json` so the browser is allowed to call the API.

### 4. Run backend tests

```bash
cd FinanceTracker.Api.Tests
dotnet test
```

## API Reference

Base URL: `/api/v1`

| Method | Route | Description |
|---|---|---|
| POST | `/auth/register` | Register a new user |
| POST | `/auth/login` | Log in, returns access + refresh token |
| POST | `/auth/refresh` | Exchange a refresh token for a new access token |
| POST | `/auth/logout` | Revoke a refresh token |
| GET | `/transactions` | List transactions (filters: `from`, `to`, `categoryId`, `type`, `page`, `pageSize`) |
| POST | `/transactions` | Create a transaction |
| PUT | `/transactions/{id}` | Update a transaction |
| DELETE | `/transactions/{id}` | Delete a transaction |
| GET | `/transactions/export` | Export filtered transactions as CSV |
| GET | `/categories` | List the user's categories + global defaults |
| POST | `/categories` | Create a custom category |
| PUT | `/categories/{id}` | Update a custom category |
| DELETE | `/categories/{id}` | Delete a custom category |
| GET | `/dashboard/summary` | Current balance and monthly income/expense |
| GET | `/dashboard/by-category` | Expense breakdown by category (`month`, `year`) |
| GET | `/dashboard/balance-history` | Balance evolution over the last N months |

Full interactive documentation is available via Swagger once the API is running.

## Roadmap

- [ ] Deploy frontend to Vercel and backend + database to Railway
- [ ] Recurring transactions
- [ ] Budget goals per category
