using DataArchive.Model;
using System.Data;

namespace DataArchive.Driver;

public class SqlTextDriver : IDriver {

    public string Name => "SqlText";

    public Direction Direction => Direction.Output;

    public IDbConnection Connection => throw new NotImplementedException();

    public string? GetConnectionString(EndPoint endpoint) {
        throw new NotImplementedException();
    }

    public bool Validate(EndPoint endPoint) {
        return true;
    }

    public IEnumerable<string> GetDatabases() {
        throw new NotImplementedException();
    }

}
