namespace DataArchive.Driver;

public abstract class DriverBase {

    public abstract string Name { get; }

    public override int GetHashCode() {
        return Name.GetHashCode();
    }

}
