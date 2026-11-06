# BankApp

> A console-based banking application built in C#/.NET as a group project at Chas Academy.

<!-- One or two sentences on what the app does and who it is for. -->

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

<!-- Describe the purpose of the app and the goals of the project, e.g. practising OOP, collaboration with Git, etc. -->

**Built with:**
- C# / .NET `[version]`
- Visual Studio `[version]`
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

<!-- Remove what you didn't finish and add anything missing. -->

---

## Getting Started

### Prerequisites

- [.NET SDK `[version]`](https://dotnet.microsoft.com/download)

### Run the app

```bash
git clone https://github.com/[org-or-user]/[repo-name].git
cd [repo-name]
dotnet run --project [ProjectName]
```

### Test accounts

| Role     | Username | Password |
|----------|----------|----------|
| Admin    | `[admin]` | `[password]` |
| Customer | `[user]`  | `[password]` |

---

## UML Diagram

![UML diagram](docs/uml.png)

<!-- Or link to it if it lives somewhere else: -->
<!-- [View the UML diagram](https://link-to-diagram) -->

---

## Project Structure

```
[ProjectName]/
├── Program.cs              # Entry point – starts the app, no business logic
├── Models/
│   ├── User.cs
│   ├── Customer.cs
│   ├── Admin.cs
│   ├── Account.cs
│   ├── SavingsAccount.cs
│   ├── Transaction.cs
│   └── Loan.cs
├── Services/
│   ├── BankService.cs
│   ├── TransferService.cs
│   ├── CurrencyService.cs
│   └── LoanService.cs
├── Rules/
│   └── LoanRules.cs
└── UI/
    └── UserInterface.cs
```

<!-- Update the tree to match your actual folders and file names. -->

---

## Classes Overview

A rough explanation of the main classes and objects in the source code.

### Models

| Class | Responsibility |
|-------|----------------|
| `User` | Base class for everyone who can log in. Holds username, password and role. |
| `Customer` | A bank customer. Owns one or more accounts and loans. |
| `Admin` | Can create users, update exchange rates, `[...]`. |
| `Account` | A bank account with an account number, balance and currency. |
| `SavingsAccount` | Inherits from `Account`. Adds interest on the balance. |
| `Transaction` | One transfer/deposit/withdrawal: amount, sender, receiver, timestamp. |
| `Loan` | A loan taken by a customer: amount, interest rate, monthly payment, remaining balance. |

### Services

| Class | Responsibility |
|-------|----------------|
| `BankService` | `[...]` |
| `TransferService` | Handles transfers and the queue that is processed every 15 minutes. |
| `CurrencyService` | Holds exchange rates and converts between currencies. |
| `LoanService` | Validates loan requests, calculates interest and monthly payments, and creates `Loan` objects. |

### Rules

| Class | Responsibility |
|-------|----------------|
| `LoanRules` | Constants and rules for loans – e.g. 5% base interest rate and a maximum loan of 5× the customer's balance. |

### UI

| Class | Responsibility |
|-------|----------------|
| `UserInterface` | Draws the menus, ASCII logo and colors, and reads user input. Calls the services – contains no business logic. |

### Relationships

<!-- Short description of how the classes connect, e.g.: -->
- `Customer` and `Admin` inherit from `User`.
- A `Customer` has a list of `Account` and a list of `Loan`.
- `SavingsAccount` inherits from `Account`.
- `UserInterface` talks to the services; the services work with the models.

---

## How It Works

<!-- Describe a typical flow through the app, e.g.: -->
1. The user logs in or creates an account.
2. Depending on role, a customer or admin menu is shown.
3. A customer can view accounts, make transfers, open a savings account or apply for a loan.
4. Transfers are placed in a queue and executed every 15 minutes.
5. All transactions are saved in the transaction log.

---

## Our Workflow

- **Planning:** `[e.g. Trello / GitHub Projects / Kanban board, user stories, backlog]`
- **Branching:** One feature branch per feature, merged into `main` via pull requests.
- **Code review:** `[e.g. at least one other team member reviews each PR]`
- **Communication:** `[e.g. daily stand-ups, Discord/Slack]`
- **Conventions:** English for code and documentation, business logic kept out of `Main`, `.vs/`, `bin/` and `obj/` ignored via `.gitignore`.

---

## Team

| Name | GitHub | Main responsibilities |
|------|--------|-----------------------|
| William Ekman | [@theoakman](https://github.com/theoakman) | Loans (`Loan`, `LoanRules`, `LoanService`), `UserInterface` |
| `[Name]` | `[@handle]` | `[...]` |
| `[Name]` | `[@handle]` | `[...]` |
| `[Name]` | `[@handle]` | `[...]` |

---

## Future Improvements

- Profile updates
- Transaction statistics
- Notifications for large transactions
- Credit-based interest rates for loans
