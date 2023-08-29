using System.ComponentModel;

namespace DataArchive.Model;

public class Respository : INotifyPropertyChanged {

    public string? Provider { get; set; } = null;

    public string Host { get; set; } = string.Empty;

    public bool Trust { get; set; } = false;

    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string DatabaseName { get; set; } = string.Empty;

    public event PropertyChangedEventHandler? PropertyChanged;

    public Respository Clone() {
        return MemberwiseClone() as Respository ?? new();
    }

}