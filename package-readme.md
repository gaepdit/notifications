# NuGet Package maintenance

Some NuGet packages have been added directly to work around vulnerable dependencies in other packages.

- `SQLitePCLRaw.lib.e_sqlite3` 2.1.12 has added to `Notifications.API` to avoid a vulnerable version in
  `Microsoft.EntityFrameworkCore.Sqlite`.