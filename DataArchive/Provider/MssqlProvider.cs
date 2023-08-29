namespace DataArchive.Provider;

public class MssqlProvider : IProvider {

    public string Name => "Mssql";

    public Direction Direction => Direction.InputOutput;

}
