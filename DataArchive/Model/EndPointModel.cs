using System.ComponentModel;

namespace DataArchive.Model;

public class EndPoint : INotifyPropertyChanged {

    private string? driverName = string.Empty;
    public string? DriverName {
        get { return driverName; }
        set { driverName = value; }
    }

    public string Host { get; set; } = string.Empty;

    public bool Secure { get; set; } = false;

    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string DatabaseName { get; set; } = string.Empty;

    public event PropertyChangedEventHandler? PropertyChanged;

    public EndPoint Clone() {
        return MemberwiseClone() as EndPoint ?? new();
    }

}