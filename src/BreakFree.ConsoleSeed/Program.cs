using System;
using Microsoft.Data.Sqlite;
using System.IO;

namespace BreakFree.ConsoleSeed
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var baseDir = AppContext.BaseDirectory;

            string schemaPath = Path.Combine(baseDir, "create_breakfree.sql");

            Console.WriteLine("[BreakFree] Console ADO.NET utility");
            Console.WriteLine($"[INFO] DB file: {SqliteHelper.DbPath}");
            Console.WriteLine($"[INFO] Schema:  {schemaPath}");

            SqliteHelper.EnsureDatabase(schemaPath);

            using (var conn = new SqliteConnection(SqliteHelper.ConnectionString))
            {
                conn.Open();
                var checkCmd = conn.CreateCommand();
                checkCmd.CommandText = "SELECT count(*) FROM Users WHERE user_name = 'admin'";
                var exists = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (exists == 0)
                {
                    var insertCmd = conn.CreateCommand();
                    // УВАГА: У реальному проекті паролі треба хешувати! 
                    // Але для лабораторної поки пишемо текстом, як у тебе в Entity.
                    insertCmd.CommandText = "INSERT INTO Users (user_name, email, password) VALUES ('admin', 'admin@test.com', '12345')";
                    insertCmd.ExecuteNonQuery();
                    Console.WriteLine("[SEED] Added test user: admin / 12345");
                }
            

            Seed.SeedWithBogus(SqliteHelper.ConnectionString);

            PrintTable(SqliteHelper.ConnectionString, "Users");
            PrintTable(SqliteHelper.ConnectionString, "Habits");
            PrintTable(SqliteHelper.ConnectionString, "DailyStatuses");
            PrintTable(SqliteHelper.ConnectionString, "Achievements");
            PrintTable(SqliteHelper.ConnectionString, "SOSActions");
            PrintTable(SqliteHelper.ConnectionString, "UserSOSLogs");
            PrintTable(SqliteHelper.ConnectionString, "Quotes");
            PrintTable(SqliteHelper.ConnectionString, "Savings");

            Console.WriteLine("\nDone.");
        }


        public static void PrintTable(string connectionString, string table, int limit = 10)
        {
            using var conn = new SqliteConnection(connectionString);
            conn.Open();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = $"SELECT * FROM {table} LIMIT {limit};";

            using var reader = cmd.ExecuteReader();
            Console.WriteLine($"\n--- {table} ---");

            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var val = reader.IsDBNull(i) ? "" : reader.GetValue(i).ToString();
                    Console.Write($"{reader.GetName(i)}={val}; ");
                }
                Console.WriteLine();
            }
        }
    }
}
