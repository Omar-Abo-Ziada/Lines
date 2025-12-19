# Rewards Wallet System - Implementation Summary

## Overview

Successfully implemented a comprehensive "My Rewards" (Offers) feature for the Lines driver application, allowing drivers to activate limited-time promotional offers by paying with their wallet balance.

---

## 🎯 What Was Implemented

### 1. Domain Layer (Lines.Domain)

#### New Entities Created:

- **`DriverOfferActivation.cs`** - Tracks which drivers have activated which offers
- **`Wallet.cs`** - Driver wallet with balance and transaction history
- **`WalletTransaction.cs`** - Individual wallet transactions (credits/debits)

#### New Enums:

- **`TransactionType.cs`** - Credit or Debit
- **`WalletTransactionCategory.cs`** - OfferPurchase, TripEarning, Refund, TopUp, Withdrawal, Adjustment

#### Updated Entities:

- **`DriverServiceFeeOffer.cs`** - Extended with Title, Description, Price, DurationDays, IsGloballyActive
- **`Driver.cs`** - Added navigation properties for Wallet and OfferActivations

---

### 2. Infrastructure Layer (Lines.Infrastructure)

#### EF Core Configurations:

- **`DriverServiceFeeOfferConfig.cs`** - Updated with new properties and seed data (3 sample offers)
- **`DriverOfferActivationConfig.cs`** - NEW configuration
- **`WalletConfig.cs`** - NEW configuration with one-to-one relationship
- **`WalletTransactionConfig.cs`** - NEW configuration
- **`ApplicationDBContext.cs`** - Added DbSets for new entities

#### Background Service:

- **`OfferExpiryBackgroundService.cs`** - Runs hourly to deactivate expired offers automatically
- Registered in **`InfrastructureServiceRegistration.cs`**

---

### 3. Application Layer (Lines.Application)

#### RewardOffers Feature:

```
Lines.Application/Features/RewardOffers/
├── DTOs/
│   ├── AvailableOfferDto.cs
│   ├── ActiveOfferDto.cs
│   └── ActivateOfferDto.cs
├── GetAvailableOffers/
│   ├── Queries/GetAvailableOffersQuery.cs
│   └── Orchestrators/GetAvailableOffersOrchestrator.cs
├── GetActiveOffer/
│   ├── Queries/GetActiveOfferQuery.cs
│   └── Orchestrators/GetActiveOfferOrchestrator.cs
└── ActivateOffer/
    ├── Commands/ActivateOfferCommand.cs (with full business logic)
    └── Orchestrators/ActivateOfferOrchestrator.cs
```

#### Wallets Feature:

```
Lines.Application/Features/Wallets/
├── DTOs/
│   ├── WalletBalanceDto.cs
│   ├── TopUpWalletDto.cs
│   └── WalletTransactionDto.cs
├── GetWalletBalance/
│   ├── Queries/GetWalletBalanceQuery.cs
│   └── Orchestrators/GetWalletBalanceOrchestrator.cs
├── TopUpWallet/
│   ├── Commands/TopUpWalletCommand.cs
│   └── Orchestrators/TopUpWalletOrchestrator.cs
└── GetWalletTransactions/
    ├── Queries/GetWalletTransactionsQuery.cs
    └── Orchestrators/GetWalletTransactionsOrchestrator.cs
```

---

### 4. Presentation Layer (Lines.Presentation)

#### Reward Offers Endpoints:

```
GET /api/reward-offers
    → Get all available offers (no auth required)
    Response: List of offers with prices and durations

GET /api/reward-offers/active
    → Get active offer for logged-in driver (requires auth)
    Response: Active offer details or null

POST /api/reward-offers/activate/{offerId}
    → Activate an offer (requires auth)
    Response: Activation details with payment reference
```

#### Wallet Endpoints:

```
GET /api/wallet
    → Get wallet balance and recent transactions (requires auth)
    Response: Balance, last updated, recent 10 transactions

POST /api/wallet/topup
    → Add funds to wallet (requires auth)
    Request: { "amount": 50.00 }
    Response: New balance, transaction reference

GET /api/wallet/transactions?page=1&pageSize=20
    → Get paginated transaction history (requires auth)
    Response: Paginated list of transactions
```

#### FluentValidation:

- All request DTOs have validators
- Validates amounts, pagination params, offer IDs, etc.

---

## 🔐 Business Rules Implemented

1. ✅ **Single Active Offer**: Drivers can only have one active offer at a time
2. ✅ **Wallet Balance Check**: Activation fails if insufficient balance
3. ✅ **Offer Availability**: Only active offers within valid date range can be activated
4. ✅ **Automatic Expiry**: Background service deactivates expired offers every hour
5. ✅ **Transaction Tracking**: All wallet operations create transaction records
6. ✅ **Auto-Create Wallet**: Wallet is automatically created for drivers on first use

---

## 🧪 Seed Data

Three sample offers are seeded:

- **Service Fee Cap: 7%** - 3 days, 5.00 CHF
- **Service Fee Cap: 5%** - 7 days, 10.00 CHF
- **Service Fee Cap: 3%** - 14 days, 20.00 CHF

---

## 🚀 Next Steps

### 1. Create Database Migration

Run the following PowerShell command in the project root:

```powershell
dotnet ef migrations add AddRewardWalletSystem --project Lines.Infrastructure --startup-project Lines.Presentation
```

### 2. Apply Migration

```powershell
dotnet ef database update --project Lines.Infrastructure --startup-project Lines.Presentation
```

### 3. Test the Endpoints

#### Step 1: Top up wallet

```bash
POST /api/wallet/topup
Authorization: Bearer {driver_jwt_token}
Content-Type: application/json

{
  "amount": 50.00
}
```

#### Step 2: Get available offers

```bash
GET /api/reward-offers
```

#### Step 3: Activate an offer

```bash
POST /api/reward-offers/activate/{offerId}
Authorization: Bearer {driver_jwt_token}
```

#### Step 4: Check active offer

```bash
GET /api/reward-offers/active
Authorization: Bearer {driver_jwt_token}
```

#### Step 5: View transactions

```bash
GET /api/wallet/transactions?page=1&pageSize=10
Authorization: Bearer {driver_jwt_token}
```

---

## 📊 HTTP Status Codes

| Code | Meaning      | When                                                              |
| ---- | ------------ | ----------------------------------------------------------------- |
| 200  | OK           | Successful GET/POST requests                                      |
| 400  | Bad Request  | Validation errors, insufficient balance, already has active offer |
| 401  | Unauthorized | Missing or invalid JWT token                                      |
| 404  | Not Found    | Offer not found, wallet not found                                 |

---

## 🔄 Background Service

**`OfferExpiryBackgroundService`** runs every hour:

- Queries all active `DriverOfferActivations` where `ExpirationDate <= DateTime.UtcNow`
- Sets `IsActive = false` for expired offers
- Logs expiry actions to console

---

## 🎨 Architecture Highlights

✅ **Clean Architecture** - Domain → Application → Infrastructure → Presentation
✅ **CQRS Pattern** - Separate Commands and Queries
✅ **Repository Pattern** - Generic repository for data access
✅ **Mediator Pattern** - MediatR for request handling
✅ **AutoMapper/Mapster** - DTO mapping
✅ **Dependency Injection** - All services registered in DI container
✅ **FluentValidation** - Input validation
✅ **Async/Await** - All operations are async with cancellation tokens
✅ **Business Logic in Domain** - Entities have business methods
✅ **Error Handling** - Result pattern with typed errors

---

## 📝 Example Response Formats

### Available Offers Response:

```json
{
  "data": [
    {
      "id": "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa",
      "title": "Service Fee Cap: 7%",
      "description": "Don't miss out — activate now and maximize every trip!",
      "price": 5.0,
      "durationDays": 3,
      "serviceFeePercent": 7.0,
      "validFrom": "2025-10-27T00:00:00Z",
      "validUntil": "2025-11-27T00:00:00Z"
    }
  ],
  "isSuccess": true,
  "statusCode": 200
}
```

### Activate Offer Response:

```json
{
  "data": {
    "activationId": "...",
    "offerId": "...",
    "offerTitle": "Service Fee Cap: 7%",
    "activationDate": "2025-10-28T12:00:00Z",
    "expirationDate": "2025-10-31T12:00:00Z",
    "amountPaid": 5.0,
    "newWalletBalance": 45.0,
    "paymentReference": "OFFER-...-A1B2C3D4"
  },
  "isSuccess": true,
  "statusCode": 200
}
```

### Wallet Balance Response:

```json
{
  "data": {
    "walletId": "...",
    "balance": 45.0,
    "lastUpdated": "2025-10-28T12:00:00Z",
    "recentTransactions": [
      {
        "id": "...",
        "amount": 5.0,
        "type": "Debit",
        "category": "OfferPurchase",
        "description": "Purchase of offer: Service Fee Cap: 7%",
        "createdDate": "2025-10-28T12:00:00Z"
      },
      {
        "id": "...",
        "amount": 50.0,
        "type": "Credit",
        "category": "TopUp",
        "description": "Wallet top-up",
        "createdDate": "2025-10-28T11:00:00Z"
      }
    ]
  },
  "isSuccess": true,
  "statusCode": 200
}
```

---

## 🛠️ Technologies Used

- **.NET 8** - Framework
- **EF Core 8** - ORM
- **MediatR** - CQRS/Mediator pattern
- **Mapster** - Object mapping
- **FluentValidation** - Input validation
- **ASP.NET Core Identity** - Authentication
- **JWT Bearer** - Authorization
- **SQL Server** - Database
- **Serilog** - Logging

---

## ✅ Implementation Complete

All features have been implemented according to the plan:

- ✅ Domain entities and enums
- ✅ EF Core configurations
- ✅ Application layer features (Commands, Queries, Orchestrators)
- ✅ Presentation layer endpoints with validation
- ✅ Background service for automatic expiry
- ✅ Comprehensive error handling
- ✅ Business rules enforcement
- ✅ Seed data for testing

**Status**: Ready for migration and testing! 🎉
