### Entity Framework database migrations

Instructions for adding a new Entity Framework database migration and updating the database.

1. Open a command prompt to this folder (`.\src\Notifications.API\`).

2. Run the following command with an appropriate migration name:

   `dotnet ef migrations add NAME_OF_MIGRATION --msbuildprojectextensionspath ..\..\.artifacts\Notifications.API\obj\`
