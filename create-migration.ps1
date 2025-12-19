# PowerShell script to create EF Core migration for Driver Statistics

Write-Host "Creating EF Core Migration for Driver Statistics..." -ForegroundColor Green

dotnet ef migrations add AddTipsAndDriverServiceFeeOffers --project Lines.Infrastructure --startup-project Lines.Presentation

if ($LASTEXITCODE -eq 0) {
    Write-Host "`nMigration created successfully!" -ForegroundColor Green
    Write-Host "`nTo apply the migration, run:" -ForegroundColor Yellow
    Write-Host "dotnet ef database update --project Lines.Infrastructure --startup-project Lines.Presentation" -ForegroundColor Cyan
} else {
    Write-Host "`nMigration creation failed!" -ForegroundColor Red
}

Read-Host "`nPress Enter to continue"


