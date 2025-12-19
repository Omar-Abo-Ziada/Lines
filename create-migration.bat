@echo off
echo Creating EF Core Migration for Driver Statistics...
dotnet ef migrations add AddTipsAndDriverServiceFeeOffers --project Lines.Infrastructure --startup-project Lines.Presentation
echo.
echo Migration created successfully!
echo.
echo To apply the migration, run:
echo dotnet ef database update --project Lines.Infrastructure --startup-project Lines.Presentation
pause


