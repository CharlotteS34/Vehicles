# vehicles

# Running locally
To get this up and running locally, 
1. Update the database connection string in app.config to your sql server connection string
2. In Package Manager Console in Visual Studio (Tools >> Nuget Package Manager >> Package Manager Console), run the command update-database -Project "Vehicles.Repository" -StartupProject "Vehicles.Api" - this will generate the Vehicles database on your SQL server
3. Hit run