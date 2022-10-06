using System;
using MySqlConnector;

namespace contactgroupAPI
{
    public class Database : IDisposable
    {
        public MySqlConnection Connection { get; }

        public Database(string connectionString)
        {
            //Console.WriteLine("Parameter is "+connectionString);
            //Console.WriteLine("Environment is "+System.Environment.GetEnvironmentVariable("DATABASE_URL"));
            //Connection = new MySqlConnection(System.Environment.GetEnvironmentVariable("DATABASE_URL"));
            try {
                Connection = new MySqlConnection(System.Environment.GetEnvironmentVariable("DATABASE_URL"));
                Console.WriteLine("Database connection established");}
            catch (Exception ex)
            {
                Console.WriteLine("Outer Exception in database connection: " + ex.Message);
            }      
        }

        public void Dispose() => Connection.Dispose();
    }
}