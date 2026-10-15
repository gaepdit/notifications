# NuGet Package maintenance

Some NuGet packages have been added directly to work around vulnerable dependencies in other packages.

- `SQLitePCLRaw.lib.e_sqlite3` was added and pinned to version 2.x to avoid a vulnerable version in
  `Microsoft.EntityFrameworkCore.Sqlite`.

- `System.Security.Cryptography.Pkcs` was added to avoid a vulnerable version referened in `Microsoft.Data.SqlClient`.
