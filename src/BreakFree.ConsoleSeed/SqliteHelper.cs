using System;
using System.Data;
using System.IO;
using Microsoft.Data.Sqlite;

namespace BreakFree.ConsoleSeed
{
    public static class SqliteHelper
    {
        // ВИПРАВЛЕННЯ: Використовуємо папку "Мої документи", щоб шлях був спільним для обох програм
        public static string DbPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "breakfree.db");

        public static string ConnectionString => $"Data Source={DbPath};";

        public static void EnsureDatabase(string sqlSchemaPath)
        {
            Console.WriteLine($"[DB] Checking database at: {DbPath}");

            bool dbExists = File.Exists(DbPath);

            if (!dbExists)
            {
                Console.WriteLine("[DB] Database file not found. Creating new file...");
                // Створюємо порожній файл, щоб SQLite міг до нього підключитися
                using (File.Create(DbPath)) { }
            }

            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();

            // Включаємо підтримку зовнішніх ключів (Foreign Keys)
            using (var pragma = conn.CreateCommand())
            {
                pragma.CommandText = "PRAGMA foreign_keys = ON;";
                pragma.ExecuteNonQuery();
            }

            // Якщо в базі немає таблиць (наприклад, Users), запускаємо скрипт створення
            if (NeedSetup(conn))
            {
                Console.WriteLine("[DB] Database is empty. Applying schema...");
                
                if (!File.Exists(sqlSchemaPath))
                {
                    throw new FileNotFoundException($"Schema file not found at: {sqlSchemaPath}. Make sure 'Copy to Output Directory' is set for the SQL file.");
                }

                var schemaSql = File.ReadAllText(sqlSchemaPath);
                using var setup = conn.CreateCommand();
                setup.CommandText = schemaSql;
                setup.ExecuteNonQuery();
                Console.WriteLine("[DB] Schema applied successfully.");
            }
            else
            {
                Console.WriteLine("[DB] Schema already present. Skipping initialization.");
            }
        }

        private static bool NeedSetup(SqliteConnection conn)
        {
            using var cmd = conn.CreateCommand();
            // Перевіряємо наявність таблиці 'Users' як маркера того, що база ініціалізована
            cmd.CommandText = "SELECT count(*) FROM sqlite_master WHERE type='table' AND name='Users';";
            var count = Convert.ToInt32(cmd.ExecuteScalar());
            return count == 0;
        }
    }
}