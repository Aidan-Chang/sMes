using Dapper;
using DataArchive.Model;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DataArchive.Driver;

public class MssqlDriver : IDriver {

    public string Name => "Mssql";

    public Direction Direction => Direction.InputOutput;

    private IDbConnection? connection = null;
    public IDbConnection Connection {
        get {
            if (connection == null) {
                connection = new SqlConnection();
            }
            return connection;
        }
    }

    public string? GetConnectionString(EndPoint endpoint) {
        if (string.IsNullOrEmpty(endpoint.Host) == false && string.IsNullOrEmpty(endpoint.UserName) == false && string.IsNullOrEmpty(endpoint.Password) == false) {
            return $"Server={endpoint.Host};User Id={endpoint.UserName};Password={endpoint.Password};{(string.IsNullOrEmpty(endpoint.DatabaseName) ? string.Empty : $"Database={endpoint.DatabaseName};")}";
        }
        return null;
    }

    public bool Validate(EndPoint endPoint) {
        string? connectionString = GetConnectionString(endPoint);
        if (string.IsNullOrEmpty(connectionString) == false) {
            Connection.ConnectionString = connectionString;
            try {
                Connection.Open();
                Connection.Close();
                return true;
            }
            catch (Exception ex) {
            }
        }
        return false;
    }

    public IEnumerable<string> GetDatabases() {
        return Connection.Query<string>(@"
            SELECT [Name]
            FROM sysdatabases
        ");
    }

}
