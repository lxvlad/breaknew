using System;
using System.Data;
using System.IO;
using Microsoft.Data.Sqlite;

namespace BreakFree.ConsoleSeed
{
    public static class SqliteHelper
    {
        public static string DbPath => Path.Combine(AppContext.BaseDirectory, "breakfree.db");
        public static string ConnectionString => $"Data Source={DbPath};";

        public static void EnsureDatabase(string sqlSchemaPath)
        {
            var firstCreate = !File.Exists(DbPath);
            if (firstCreate)
            {
                using var _ = File.Create(DbPath);
            }

            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using (var pragma = conn.CreateCommand())
            {
                pragma.CommandText = "PRAGMA foreign_keys = ON;";
                pragma.ExecuteNonQuery();
            }

            if (NeedSetup(conn))
            {
                var schemaSql = File.ReadAllText(sqlSchemaPath);
                using var setup = conn.CreateCommand();
                setup.CommandText = schemaSql;
                setup.ExecuteNonQuery();
                Console.WriteLine("[DB] Schema applied.");
            }
            else
            {
                Console.WriteLine("[DB] Schema already present.");
            }
        }

        private static bool NeedSetup(SqliteConnection conn)
        {
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT count(*) FROM sqlite_master WHERE type='table' AND name='Users';";
            var count = Convert.ToInt32(cmd.ExecuteScalar());
            return count == 0;
        }
    }
}
