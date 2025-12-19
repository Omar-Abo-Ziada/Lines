# Driver Statistics API - Testing Guide

## Prerequisites

1. Run the migration to update the database:

   - Windows: Double-click `create-migration.bat` OR run `.\create-migration.ps1`
   - Manual: `dotnet ef migrations add AddTipsAndDriverServiceFeeOffers --project Lines.Infrastructure --startup-project Lines.Presentation`

2. Apply the migration:

   ```bash
   dotnet ef database update --project Lines.Infrastructure --startup-project Lines.Presentation
   ```

3. Start the application:
   ```bash
   dotnet run --project Lines.Presentation
   ```

## Testing with Swagger

1. Navigate to `https://localhost:{port}/swagger`
2. Authenticate using the `/api/users/login` endpoint
3. Copy the JWT token and click "Authorize" button
4. Paste token in the format: `Bearer {your-token}`

## API Endpoints

### 1. Daily Statistics

**Endpoint:** `GET /api/driver-statistics/daily/{driverId}`

**Example Request:**

```
GET /api/driver-statistics/daily/11111111-1111-1111-1111-111111111111
Authorization: Bearer {token}
```

**Expected Response:**

```json
{
  "success": true,
  "data": {
    "distanceKm": 16.0,
    "incomeChf": 125.15,
    "netProfitChf": 106.15,
    "numberOfTrips": 6,
    "avgIncomePerTrip": 20.86,
    "appRightsChf": 19.0,
    "availability": "07:24",
    "avgProfitLastTrip": 17.69,
    "tipsChf": 24.54
  },
  "errors": []
}
```

### 2. Weekly Statistics

**Endpoint:** `GET /api/driver-statistics/weekly/{driverId}?from={date}&to={date}`

**Example Request:**

```
GET /api/driver-statistics/weekly/11111111-1111-1111-1111-111111111111?from=2025-01-21&to=2025-01-27
Authorization: Bearer {token}
```

**Query Parameters:**

- `from` (optional): Start date in format YYYY-MM-DD
- `to` (optional): End date in format YYYY-MM-DD

**Expected Response:**

```json
{
  "success": true,
  "data": {
    "distancePerDay": [
      { "day": "Mon", "value": 30.0 },
      { "day": "Tue", "value": 80.0 },
      { "day": "Wed", "value": 60.0 },
      { "day": "Thu", "value": 150.0 },
      { "day": "Fri", "value": 40.0 },
      { "day": "Sat", "value": 200.0 },
      { "day": "Sun", "value": 100.0 }
    ],
    "totalDistanceWeek": 660.0,
    "peakDay": "Sat",
    "minDay": "Mon",
    "incomePerDay": [...],
    "totalIncomeWeek": 2500.00,
    "peakIncomeDay": "Sat",
    "minIncomeDay": "Mon",
    "tripsPerDay": [...],
    "totalTripsWeek": 42,
    "peakTripsDay": "Sat",
    "minTripsDay": "Mon",
    "netProfitPerDay": [...],
    "totalNetProfitWeek": 2100.00,
    "peakNetProfitDay": "Sat",
    "minNetProfitDay": "Mon",
    "workingHoursPerDay": [...],
    "totalWorkingHoursWeek": 48.5,
    "peakWorkingHoursDay": "Sat",
    "minWorkingHoursDay": "Mon"
  },
  "errors": []
}
```

### 3. Driver Offers

**Endpoint:** `GET /api/driver-statistics/offers/{driverId}`

**Example Request:**

```
GET /api/driver-statistics/offers/11111111-1111-1111-1111-111111111111
Authorization: Bearer {token}
```

**Expected Response:**

```json
{
  "success": true,
  "data": [
    {
      "serviceFee": 5.0,
      "validFrom": null,
      "validUntil": "2025-02-21T22:25:00Z"
    },
    {
      "serviceFee": 0.0,
      "validFrom": "2025-02-21T22:25:00Z",
      "validUntil": "2025-02-27T22:25:00Z"
    }
  ],
  "errors": []
}
```

## Testing with Postman

### Setup

1. Create a new collection: "Driver Statistics API"
2. Add environment variables:
   - `baseUrl`: `https://localhost:{port}/api`
   - `token`: (will be set after login)
   - `driverId`: (use a valid driver ID from your database)

### Request Templates

#### Login (to get token)

```
POST {{baseUrl}}/users/login
Content-Type: application/json

{
  "email": "driver@example.com",
  "password": "Password123!"
}
```

Save the token from response and set it in environment.

#### Daily Statistics

```
GET {{baseUrl}}/driver-statistics/daily/{{driverId}}
Authorization: Bearer {{token}}
```

#### Weekly Statistics

```
GET {{baseUrl}}/driver-statistics/weekly/{{driverId}}?from=2025-01-21&to=2025-01-27
Authorization: Bearer {{token}}
```

#### Driver Offers

```
GET {{baseUrl}}/driver-statistics/offers/{{driverId}}
Authorization: Bearer {{token}}
```

## Sample Data for Testing

### Creating Test Trips

You'll need to create test trips in the database with:

- Completed status
- Today's date for daily stats
- This week's dates for weekly stats
- Associated payment and earning records

### Creating Test Service Fee Offers

You can manually insert records into `DriverServiceFeeOffers` table:

```sql
INSERT INTO Driver.DriverServiceFeeOffers (Id, DriverId, ServiceFeePercent, ValidFrom, ValidUntil, CreatedDate, IsDeleted)
VALUES
  (NEWID(), '{your-driver-id}', 5.00, GETUTCDATE(), DATEADD(day, 30, GETUTCDATE()), GETUTCDATE(), 0),
  (NEWID(), '{your-driver-id}', 0.00, DATEADD(day, 30, GETUTCDATE()), DATEADD(day, 60, GETUTCDATE()), GETUTCDATE(), 0);
```

## Validation Testing

### Test Cases

1. **Invalid Driver ID**

   - Should return validation error

2. **Driver with No Trips**

   - Should return zero values for all metrics

3. **Driver with No Offers**

   - Should return empty array

4. **Weekly Stats Without Date Parameters**

   - Should default to current week

5. **Weekly Stats with Invalid Date Range**

   - Should return validation error

6. **Unauthorized Access**
   - Should return 401 Unauthorized

## Common Issues

### Issue: "No trips found"

**Solution:** Ensure you have trips with `TripStatus.Completed` for the test driver.

### Issue: "AppRightsChf is 0"

**Solution:** Ensure trips have associated Payment and Earning records.

### Issue: "Authorization failed"

**Solution:**

- Verify token is valid and not expired
- Ensure "Bearer " prefix is included
- Check user has proper permissions

### Issue: "Migration pending"

**Solution:** Run `dotnet ef database update --project Lines.Infrastructure --startup-project Lines.Presentation`

## Performance Testing

For production readiness, test with:

- Large datasets (1000+ trips)
- Multiple concurrent requests
- Different date ranges
- Edge cases (leap years, month boundaries, etc.)

## Next Steps

1. Create automated integration tests
2. Add monitoring/logging for the endpoints
3. Consider caching for frequently accessed statistics
4. Add pagination for large result sets if needed
5. Consider background jobs for pre-calculating statistics

