using DataArchive.Model;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace DataArchive.Driver;

public class OracleDriver : IDriver {

    public string Name => "Oracle";

    public Direction Direction => Direction.InputOutput;

    private IDbConnection? connection = null;
    public IDbConnection Connection {
        get {
            if (connection == null) {
                connection = new OracleConnection();
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
        throw new NotImplementedException();
    }

    public IEnumerable<string> GetDatabases() {
        throw new NotImplementedException();
    }

}
