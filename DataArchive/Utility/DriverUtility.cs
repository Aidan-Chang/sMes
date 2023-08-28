using DataArchive.Driver;

namespace DataArchive.Utility;

public static class DriverUtility {

    private static List<IDriver>? drivers = null;

    public static List<IDriver> Drivers {
        get {
            if (drivers == null) {
                drivers = new List<IDriver>();
                foreach (var driver in GetAvailableDrivers()) {
                    drivers.Add(driver);
                }
            }
            return drivers;
        }
    }

    public static IEnumerable<IDriver> GetAvailableDrivers(Direction[]? directions = null) {
        var type = typeof(IDriver);
        var types = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(
                p => p.IsClass &&
                type.IsAssignableFrom(p))
            .Select(x => (Activator.CreateInstance(x) as IDriver)!);
        if (directions == null) {
            return types;
        }
        else {
            return types.Where(x => directions.Contains(x.Direction));
        }
    }

}
