# BankApp

> A console-based banking application built in C#/.NET as a group project at Chas Academy.

---

## Table of Contents

- [About the Project](#about-the-project)
- [Features](#features)
- [Getting Started](#getting-started)
- [UML Diagram](#uml-diagram)
- [Project Structure](#project-structure)
- [Classes Overview](#classes-overview)
- [How It Works](#how-it-works)
- [Our Workflow](#our-workflow)
- [Team](#team)

---

## About the Project

This project is a console-based banking application that allows users to create accounts, perform transactions and loans, and manageing through a admin interface.
The goal of the project is to practice object-oriented programming (OOP) principles, collaboration using Git and GitHub, and to create a functional banking application with a user-friendly interface.

**Built with:**
- C# 14 / .NET 10 (net10.0)
- Visual Studio Community 2026 
- Git & GitHub

---

## Features

- User login and account creation
- Separate roles for **customers** and **admins**
- Checking and savings accounts with balances
- Multiple currencies and exchange rates
- Transfers between accounts and between users
- Queued transfers processed every 15 minutes
- Transaction log / history
- Loans with interest and monthly-payment calculation
- Colored console menus and ASCII logo

<!--and add anything missing. -->

---

## Getting Started

### Prerequisites

- [.NET SDK 10.0.403](https://dotnet.microsoft.com/download)

### Run the app

```bash
git clone https://github.com/danieelvergara-cmyk/Bankapp.git
cd Bankapp
dotnet run --project BankApp
```

### Test accounts

| Role     | Username | Password |
|----------|----------|----------|
| Admin    | `[admin]`| `[admin]`|


---

## UML Diagram


<!-- Or link to it if it lives somewhere else: -->
<!-- [View the UML diagram](https://link-to-diagram) -->

---

## Project Structure

```
Bankapp/
├── BankApp.sln
└── BankApp/
    ├── BankApp.csproj
    ├── Program.cs              # Entry point – starts the app, no business logic
    ├── Account.cs
    ├── AccountMenu.cs
    ├── AccountService.cs
    ├── Admin.cs
    ├── AdminMenu.cs
    ├── Currency.cs
    ├── ExchangeRate.cs
    ├── Loan.cs
    ├── LoanMenu.cs
    ├── LoanRules.cs
    ├── LoanService.cs
    ├── LogInService.cs
    ├── NotificationService.cs
    ├── SavingsAccount.cs
    ├── Scheduler.cs
    ├── StatisticsService.cs
    ├── Transaction.cs
    ├── TransactionMenu.cs
    ├── TransactionService.cs
    ├── User.cs
    ├── UserInterface.cs
    └── UserService.cs
```

---

## Classes Overview

A rough explanation of the main classes and objects in the source code.

### Models

| Class | Responsibility |
|-------|----------------|
| `User` | `[...]` |
| `Admin` | `[...]` |
| `Account` | `[...]` |
| `SavingsAccount` | `[...]` |
| `Transaction` | `[...]` |
| `Loan` | `[...]` |
| `ExchangeRate` | `[...]` |
| `Currency` (enum) | `[...]` |

### Services

| Class | Responsibility |
|-------|----------------|
| `AccountService` | `[...]` |
| `LoginService` | `[...]` |
| `UserService` | `[...]` |
| `TransactionService` | `[...]` |
| `LoanService` | `[...]` |
| `NotificationService` | `[...]` |
| `StatisticsService` | `[...]` |
| `Scheduler` | `[...]` |

### Rules

| Class | Responsibility |
|-------|----------------|
| `LoanRules` | `[...]` |

### UI

| Class | Responsibility |
|-------|----------------|
| `UserInterface` | `[...]` |
| `AccountMenu` | `[...]` |
| `AdminMenu` | `[...]` |
| `LoanMenu` | `[...]` |
| `TransactionMenu` | `[...]` |

### Relationships

- `Admin` inherits from `User`, and `SavingsAccount` inherits from `Account`.
- A `User` has a list of `Account` and a list of `Loan`.
- `UserService` holds all users, and `LoginService` uses it to authenticate them.
- `LoanService` uses `LoanRules` and `ExchangeRate` to evaluate loan applications.
- `TransactionService` handles transfers between accounts, and the `Scheduler` uses it to process the queue.
- `Program` creates the services and passes them to the menus. The menus talk to the services, and the services work with the models.

---

## How It Works

1. The user is prompted to log in and is authenticated via the `LoginService`.
2. Depending on the role:
 - A customer can view accounts, make transfers, open a savings account or apply for a loan.
 - An admin can create new user accounts for customers, view statistics and manage rules and exchange rates.
3. New accounts and savings accounts are created via the `AccountService`. When the user deposits to a savings account, they are shown the expected interest after a year before the deposit is made.
4. A loan application is evaluated based on the rules in `LoanRules` and either approved or denied.
5. Transactions are created and managed via the `TransactionService`, which handles transfers and transaction history.
6. All transfers are placed in a queue and executed all at once every 15 minutes by the `Scheduler` service.
7. All transactions are saved in the transaction log.

---

## Our Workflow

- **Planning:** `[Trello, user stories, backlog, excalidraw]`
- **Branching:** One feature branch per feature, merged into `dev` via pull requests.
- **Code review:** `[Least one other team member reviews each PR]`
- **Communication:** `[Initally weekly meetings in Discord hub, (Last week daily)]`
- **Conventions:** English for code and documentation, business logic kept out of `Main`, `.vs/`, `bin/` and `obj/` ignored via `.gitignore`.

---

## Team

| Name | GitHub | Main responsibilities |
|------|--------|-----------------------|
| William Ekman | [@TheOakMan](https://github.com/theoakman) | Loans (`Loan`, `LoanRules`, `LoanService`), Logo `UserInterface` Menus (`AdminMenu`, `LoanMenu`), Exchange Rate `ExchangeRate` |
| `[Name]` | `[@handle]` | `[...]` |
| `[Name]` | `[@handle]` | `[...]` |
| `[Name]` | `[@handle]` | `[...]` |

---
