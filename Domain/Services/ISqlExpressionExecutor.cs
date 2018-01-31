using System.Collections.Generic;

namespace Domain.Services
{
    public interface ISqlExpressionExecutor
    {
        int Execute(string sqlConnectionString, string sqlExpression);
        int ExecuteAsDefault(string serverName, string sqlExpression);
        T ExecuteScalar<T>(string sqlConnectionString, string sqlExpression);
        T ExecuteScalarAsDefault<T>(string serverName, string sqlExpression);


        IEnumerable<T1> ExecuteReader<T1>(string connectionString, string sqlExpression);
        IEnumerable<(T1, T2)> ExecuteReader<T1, T2>(string connectionString, string sqlExpression);
        ICollection<(T1, T2, T3)> ExecuteReader<T1, T2, T3>(string connectionString, string sqlExpression);
        IEnumerable<(T1, T2, T3, T4)> ExecuteReader<T1, T2, T3, T4>(string connectionString, string sqlExpression);
        IEnumerable<(T1, T2, T3, T4, T5)> ExecuteReader<T1, T2, T3, T4, T5>(string connectionString, string sqlExpression);
        IEnumerable<(T1, T2, T3, T4, T5, T6)> ExecuteReader<T1, T2, T3, T4, T5, T6>(string connectionString, string sqlExpression);
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7)> ExecuteReader<T1, T2, T3, T4, T5, T6, T7>(string connectionString, string sqlExpression);
        IEnumerable<(T1, T2, T3, T4, T5, T6, T7, T8)> ExecuteReader<T1, T2, T3, T4, T5, T6, T7, T8>(string connectionString, string sqlExpression);
    }
}