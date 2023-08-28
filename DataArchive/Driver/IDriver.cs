using DataArchive.Model;
using System.Data;

namespace DataArchive.Driver;

public interface IDriver {

    public string Name { get; }

    public Direction Direction { get; }

    public IDbConnection Connection { get; }

    public string? GetConnectionString(EndPoint endpoint);

    public IEnumerable<string> GetDatabases();

}

public enum Direction {
    InputOutput = 1,
    Input = 2,
    Output = 3,
}