using DataArchive.Provider;

namespace DataArchive.Utility;

public static class ProviderUtility {

    private static List<IProvider>? drivers = null;

    public static List<IProvider> Drivers {
        get {
            if (drivers == null) {
                drivers = new List<IProvider>();
                foreach (var driver in GetAvailableDrivers()) {
                    drivers.Add(driver);
                }
            }
            return drivers;
        }
    }

    public static IEnumerable<IProvider> GetAvailableDrivers(Direction[]? directions = null) {
        var type = typeof(IProvider);
        var types = AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(
                p => p.IsClass &&
                type.IsAssignableFrom(p))
            .Select(x => (Activator.CreateInstance(x) as IProvider)!);
        if (directions == null) {
            return types;
        }
        else {
            return types.Where(x => directions.Contains(x.Direction));
        }
    }

}
