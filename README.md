-----------------------------------Migrations---------------------------------
dotnet ef migrations add InitialMigrations --project SavingTracker.Data --startup-project SavingTracker.Api
dotnet ef database update --project ResourceTracker.Data --startup-project SavingTracker.Api
dotnet ef migrations remove