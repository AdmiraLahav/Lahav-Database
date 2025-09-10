using System;
using System.Data.OleDb;

class Program
{
    static void Main()
    {
        Console.Write("Enter path to your .accdb file: ");
        string filePath = Console.ReadLine();

        string connectionString = $@"
            Provider=Microsoft.ACE.OLEDB.12.0;
            Data Source={filePath};
            Persist Security Info=False;";

        using (var connection = new OleDbConnection(connectionString))
        {
            try
            {
                connection.Open();
                Console.WriteLine("Connected successfully!");

                Console.Write("Enter table name: ");
                string tableName = Console.ReadLine();

                Console.Write("Enter comma-separated field names (or * for all): ");
                string fields = Console.ReadLine();

                string query = $"SELECT {fields} FROM {tableName}";
                var command = new OleDbCommand(query, connection);

                using (var reader = command.ExecuteReader())
                {
                    int fieldCount = reader.FieldCount;

                    // Print header row
                    for (int i = 0; i < fieldCount; i++)
                        Console.Write(reader.GetName(i) + "\t");
                    Console.WriteLine();

                    // Print rows
                    while (reader.Read())
                    {
                        for (int i = 0; i < fieldCount; i++)
                            Console.Write(reader[i].ToString() + "\t");
                        Console.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}