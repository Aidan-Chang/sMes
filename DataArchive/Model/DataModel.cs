using DataArchive.Driver;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DataArchive.Model;

public class DataModel: INotifyPropertyChanged {

    public EndPoint? Source { get; set; }

    public EndPoint? Target { get; set; }

    private Options.Mode mode = Options.Mode.Copy;
    public Options.Mode Mode {
        get => mode;
        set {
            if (mode != value) {
                mode = value;
                NotifyPropertyChanged();
            }
        }
    }

    private int batchSize = 50;
    public int BatchSize {
        get => batchSize;
        set {
            if (batchSize != value) {
                batchSize = value;
                NotifyPropertyChanged();
            }
        }
    }

    private Options.RecoveryMode recoveryMode = Options.RecoveryMode.NotSet;
    public Options.RecoveryMode RecoveryMode {
        get => recoveryMode;
        set {
            if (recoveryMode != value) {
                recoveryMode = value;
                NotifyPropertyChanged();
            }
        }
    }

    private bool rebuildIndex = false;
    public bool RebuildIndex {
        get => rebuildIndex;
        set {
            if (rebuildIndex != value) {
                rebuildIndex = value;
                NotifyPropertyChanged();
            }
        }
    }

    private string? fileName;
    public string? FileName {
        get => fileName;
        set {
            if (fileName != value) {
                fileName = value;
                NotifyPropertyChanged();
            }
        }
    }

    private string? filePath;
    public string? FilePath {
        get => filePath;
        set {
            if (filePath != value) {
                filePath = value;
                NotifyPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] string name = "")
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    public bool IsDraft { get; set; } = false;
}

public class EndPoint : INotifyPropertyChanged {

    public IDriver? Driver { get; set; } = null;

    public string Host { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string DatabaseName { get; set; } = string.Empty;

    public event PropertyChangedEventHandler? PropertyChanged;

    public EndPoint Clone() {
        return MemberwiseClone() as EndPoint ?? new();
    }

    public bool Validated {
        get {
            if (Driver != null) {
                string? connectionString = Driver.GetConnectionString(this);
                if (string.IsNullOrEmpty(connectionString) == false) {
                    Driver.Connection.ConnectionString = connectionString;
                }
            }
            return false;
        }
    }

}

public static class Options {

    public static int[] BatchSizes =  { 50, 100, 500, 1000 };

    public enum RecoveryMode {
        Simple,
        Full,
        NotSet,
    }

    public enum Mode {
        Copy,
        Move,
    }

}