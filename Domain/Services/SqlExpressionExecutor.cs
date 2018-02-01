using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Domain.Entities.Attribute.Integer;
using Attribute = Domain.Entities.Attribute.Attribute;
using String = Domain.Entities.Attribute.String;

namespace Domain.Services
{
    public class SqlExpressionExecutor : ISqlExpressionExecutor
    {
        public int Execute(string sqlConnectionString, string sqlExpression)
        {
            if (sqlConnectionString is null)
                throw new ArgumentNullException(nameof(sqlConnectionString));
            if (sqlExpression is null)
                throw new ArgumentNullException(nameof(sqlExpression));

            int result;

            using (SqlConnection connection = new SqlConnection(connectionString: sqlConnectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(cmdText: sqlExpression, connection: connection);

                result = command.ExecuteNonQuery();
            }

            return result;
        }

        public int ExecuteAsDefault(string serverName, string sqlExpression)
        {
            if (serverName is null)
                throw new ArgumentNullException(nameof(serverName));
            if (sqlExpression is null)
                throw new ArgumentNullException(nameof(sqlExpression));

            string connectionString = $"Data Source={serverName};Initial Catalog=master;Integrated Security=True";

            int result = Execute(sqlConnectionString: connectionString, sqlExpression: sqlExpression);

            return result;
        }

        public TResult ExecuteScalar<TResult>(string sqlConnectionString, string sqlExpression)
        {
            if (sqlConnectionString is null)
                throw new ArgumentNullException(nameof(sqlConnectionString));
            if (sqlExpression is null)
                throw new ArgumentNullException(nameof(sqlExpression));

            TResult result;

            using (SqlConnection connection = new SqlConnection(connectionString: sqlConnectionString))
            {
                SqlCommand command = new SqlCommand(cmdText: sqlExpression, connection: connection);

                connection.Open();

                result = (TResult) command.ExecuteScalar();
            }

            return result;
        }

        public TResult ExecuteScalarAsDefault<TResult>(string serverName, string sqlExpression)
        {
            if (serverName is null)
                throw new ArgumentNullException(nameof(serverName));
            if (sqlExpression is null)
                throw new ArgumentNullException(nameof(sqlExpression));

            string connectionString = $"Data Source={serverName};Initial Catalog=master;Integrated Security=True";

            return ExecuteScalar<TResult>(sqlConnectionString: connectionString, sqlExpression: sqlExpression);
        }

        public IEnumerable<T1> ExecuteReader<T1>(string connectionString, string sqlExpression)
        {
            return ExecuteReader(connectionString: connectionString, sqlExpression: sqlExpression)
                   .Select(rowValuesArray =>
                               (T1) rowValuesArray[0])
                   .ToList();
        }

        public IEnumerable<(T1, T2)> ExecuteReader<T1, T2>(string connectionString, string sqlExpression)
        {
            return ExecuteReader(connectionString: connectionString, sqlExpression: sqlExpression)
                   .Select(rowValuesArray =>
                               ((T1) rowValuesArray[0],
                               (T2) rowValuesArray[1]))
                   .ToList();
        }

        public ICollection<(T1, T2, T3)> ExecuteReader<T1, T2, T3>(string connectionString, string sqlExpression)
        {
            return ExecuteReader(connectionString: connectionString, sqlExpression: sqlExpression)
                   .Select(rowValuesArray =>
                               ((T1) rowValuesArray[0],
                               (T2) rowValuesArray[1],
                               (T3) rowValuesArray[2]))
                   .ToList();
        }

        public IEnumerable<(T1, T2, T3, T4)> ExecuteReader<T1, T2, T3, T4>(string connectionString, string sqlExpression)
        {
            return ExecuteReader(connectionString: connectionString, sqlExpression: sqlExpression)
                   .Select(rowValuesArray =>
                               ((T1) rowValuesArray[0],
                               (T2) rowValuesArray[1],
                               (T3) rowValuesArray[2],
                               (T4) rowValuesArray[3]))
                   .ToList();
        }

        public IEnumerable<(T1, T2, T3, T4, T5)> ExecuteReader<T1, T2, T3, T4, T5>(string connectionString, string sqlExpression)
        {
            return ExecuteReader(connectionString: connectionString, sqlExpression: sqlExpression)
                   .Select(rowValuesArray =>
                               ((T1) rowValuesArray[0],
                               (T2) rowValuesArray[1],
                               (T3) rowValuesArray[2],
                               (T4) rowValuesArray[3],
                               (T5) rowValuesArray[4]))
                   .ToList();
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6)> ExecuteReader<T1, T2, T3, T4, T5, T6>(string connectionString, string sqlExpression)
        {
            return ExecuteReader(connectionString: connectionString, sqlExpression: sqlExpression)
                   .Select(rowValuesArray =>
                               ((T1) rowValuesArray[0],
                               (T2) rowValuesArray[1],
                               (T3) rowValuesArray[2],
                               (T4) rowValuesArray[3],
                               (T5) rowValuesArray[4],
                               (T6) rowValuesArray[5]))
                   .ToList();
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ExecuteReader<T1, T2, T3, T4, T5, T6, T7>(string connectionString, string sqlExpression)
        {
            return ExecuteReader(connectionString: connectionString, sqlExpression: sqlExpression)
                   .Select(rowValuesArray =>
                               ((T1) rowValuesArray[0],
                               (T2) rowValuesArray[1],
                               (T3) rowValuesArray[2],
                               (T4) rowValuesArray[3],
                               (T5) rowValuesArray[4],
                               (T6) rowValuesArray[5],
                               (T7) rowValuesArray[6]))
                   .ToList();
        }

        public IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ExecuteReader<T1, T2, T3, T4, T5, T6, T7, T8>(string connectionString, string sqlExpression)
        {
            return ExecuteReader(connectionString: connectionString, sqlExpression: sqlExpression)
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
        }

        private static IEnumerable<object[]> ExecuteReader(string connectionString, string sqlExpression)
        {
            ICollection<object[]> resultCollection = new List<object[]>();

            using (SqlConnection connection = new SqlConnection(connectionString: connectionString))
            {
                SqlCommand command = new SqlCommand(cmdText: sqlExpression, connection: connection);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                    while (reader.Read())
                    {
                        object[] objectArray = new object[reader.FieldCount];

                        for (int i = 0; i < reader.FieldCount; i++) objectArray[i] = reader.GetValue(i: i);

                        resultCollection.Add(item: objectArray);
                    }

                reader.Close();
            }

            return resultCollection;
        }

        public IEnumerable<IDictionary<Attribute, object>> ExecuteDictionaryReader(
            string connectionString,
            string sqlExpression,
            IEnumerable<Attribute> attributes)
        {
            if (connectionString is null)
                throw new ArgumentNullException(nameof(connectionString));
            if (sqlExpression is null)
                throw new ArgumentNullException(nameof(sqlExpression));
            if (attributes is null)
                throw new ArgumentNullException(nameof(attributes));

            IList<IDictionary<Attribute, object>> resultCollection = new List<IDictionary<Attribute, object>>();

            using (SqlConnection connection = new SqlConnection(connectionString: connectionString))
            {
                SqlCommand command = new SqlCommand(cmdText: sqlExpression, connection: connection);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                    while (reader.Read())
                    {
                        Dictionary<Attribute, object> dictionary = new Dictionary<Attribute, object>();

                        foreach (Attribute attribute in attributes)
                        {
                            switch (attribute)
                            {
                                case String s:
                                    dictionary[key: s] = reader.GetString(reader.GetOrdinal(name: s.Name));
                                    break;
                                case IntegerNumber i:
                                    dictionary[key: i] = reader.GetInt32(reader.GetOrdinal(name: i.Name));
                                    break;
                                default:
                                    throw new ArgumentException("Unexpected attribute type.");
                            }
                        }

                        resultCollection.Add(item: dictionary);
                    }

                reader.Close();
            }

            return resultCollection;
        }
    }
}