using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Domain.Services
{
    public class SqlExpressionExecutor : ISqlExpressionExecutor
    {
        public void Execute(string sqlConnectionString, string sqlExpression)
        {
            void PrintStatus() => Console.WriteLine($"\n{sqlConnectionString}\n{sqlExpression}");
            void PrintResult(int result) => Console.WriteLine($"Rows affected: {result}.");

            using (SqlConnection connection = new SqlConnection(sqlConnectionString))
            {
                PrintStatus();

                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                int result = command.ExecuteNonQuery();

                PrintResult(result);
            }
        }

        public void ExecuteAsDefault(string serverName, string sqlExpression)
        {
            string connectionString = $"Data Source={serverName};Initial Catalog=master;Integrated Security=True";

            Execute(connectionString, sqlExpression);
        }

        public TResult ExecuteScalar<TResult>(string sqlConnectionString, string sqlExpression)
        {
            void PrintStatus() => Console.WriteLine($"\n{sqlConnectionString}\n{sqlExpression}");

            TResult result;

            using (SqlConnection connection = new SqlConnection(sqlConnectionString))
            {
                PrintStatus();

                connection.Open();
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                result = (TResult) command.ExecuteScalar();
            }

            return result;
        }

        public TResult ExecuteScalarAsDefault<TResult>(string serverName, string sqlExpression)
        {
            string connectionString = $"Data Source={serverName};Initial Catalog=master;Integrated Security=True";

            return ExecuteScalar<TResult>(connectionString, sqlExpression);
        }

        public IEnumerable<T1> ExecuteReader<T1>(string connectionString, string sqlExpression)
            => ExecuteReader(connectionString, sqlExpression)
                .Select(rowValuesArray =>
                    (T1) rowValuesArray[0])
                .ToList();

        public IEnumerable<(T1, T2)> ExecuteReader<T1, T2>(string connectionString, string sqlExpression)
            => ExecuteReader(connectionString, sqlExpression)
                .Select(rowValuesArray =>
                    ((T1) rowValuesArray[0],
                    (T2) rowValuesArray[1]))
                .ToList();

        public ICollection<(T1, T2, T3)> ExecuteReader<T1, T2, T3>(string connectionString, string sqlExpression)
            => ExecuteReader(connectionString, sqlExpression)
                .Select(rowValuesArray =>
                    ((T1) rowValuesArray[0],
                    (T2) rowValuesArray[1],
                    (T3) rowValuesArray[2]))
                .ToList();

        public IEnumerable<(T1, T2, T3, T4)> ExecuteReader<T1, T2, T3, T4>(string connectionString, string sqlExpression)
            => ExecuteReader(connectionString, sqlExpression)
                .Select(rowValuesArray =>
                    ((T1) rowValuesArray[0],
                    (T2) rowValuesArray[1],
                    (T3) rowValuesArray[2],
                    (T4) rowValuesArray[3]))
                .ToList();

        public IEnumerable<(T1, T2, T3, T4, T5)> ExecuteReader<T1, T2, T3, T4, T5>(string connectionString, string sqlExpression)
            => ExecuteReader(connectionString, sqlExpression)
                .Select(rowValuesArray =>
                    ((T1) rowValuesArray[0],
                    (T2) rowValuesArray[1],
                    (T3) rowValuesArray[2],
                    (T4) rowValuesArray[3],
                    (T5) rowValuesArray[4]))
                .ToList();

        public IEnumerable<(T1, T2, T3, T4, T5, T6)> ExecuteReader<T1, T2, T3, T4, T5, T6>(string connectionString, string sqlExpression)
            => ExecuteReader(connectionString, sqlExpression)
                .Select(rowValuesArray =>
                    ((T1) rowValuesArray[0],
                    (T2) rowValuesArray[1],
                    (T3) rowValuesArray[2],
                    (T4) rowValuesArray[3],
                    (T5) rowValuesArray[4],
                    (T6) rowValuesArray[5]))
                .ToList();

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ExecuteReader<T1, T2, T3, T4, T5, T6, T7>(string connectionString, string sqlExpression)
            => ExecuteReader(connectionString, sqlExpression)
                .Select(rowValuesArray =>
                    ((T1) rowValuesArray[0],
                    (T2) rowValuesArray[1],
                    (T3) rowValuesArray[2],
                    (T4) rowValuesArray[3],
                    (T5) rowValuesArray[4],
                    (T6) rowValuesArray[5],
                    (T7) rowValuesArray[6]))
                .ToList();

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ExecuteReader<T1, T2, T3, T4, T5, T6, T7, T8>(string connectionString, string sqlExpression)
            => ExecuteReader(connectionString, sqlExpression)
                .Select(rowValuesArray =>
                    ((T1) rowValuesArray[0],
                    (T2) rowValuesArray[1],
                    (T3) rowValuesArray[2],
                    (T4) rowValuesArray[3],
                    (T5) rowValuesArray[4],
                    (T6) rowValuesArray[5],
                    (T7) rowValuesArray[6],
                    (T8) rowValuesArray[7]))
                .ToList();

        private static IEnumerable<object[]> ExecuteReader(string connectionString, string sqlExpression)
        {
            ICollection<object[]> resultCollection = new List<object[]>();

            using (SqlConnection connection = new SqlConnection(connectionString: connectionString))
            {
                connection.Open();

                SqlDataReader reader = new SqlCommand(cmdText: sqlExpression, connection: connection).ExecuteReader();

                if (!reader.HasRows) return resultCollection;

                while (reader.Read())
                {
                    object[] objectArray = new object[reader.FieldCount];

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        objectArray[i] = reader.GetValue(i);
                    }

                    resultCollection.Add(objectArray);
                }
            }

            return resultCollection;
        }
    }
}