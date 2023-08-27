using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DataArchive.Model;

public class DataModel: INotifyPropertyChanged {

    public Source? Source { get; set; }

    public Target? Target { get; set; }

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

    public void Reset() {
        Mode = Options.Mode.Copy;
        BatchSize = 50;
        RecoveryMode = Options.RecoveryMode.NotSet;
        RebuildIndex = false;
        FileName = null;
        FilePath = null;
    }

}

public class Source: INotifyPropertyChanged {

    public Options.SourceDriver Driver { get; set; } = Options.SourceDriver.Mssql;

    public string Host { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Database { get; set; } = string.Empty;

    public event PropertyChangedEventHandler? PropertyChanged;

    public Source Clone() {
        return MemberwiseClone() as Source ?? new();
    }

}

public class Target : INotifyPropertyChanged {

    public Options.TargetDriver Driver { get; set; } = Options.TargetDriver.Mssql;

    public string Host { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Database { get; set; } = string.Empty;

    public event PropertyChangedEventHandler? PropertyChanged;

    public Target Clone() {
        return MemberwiseClone() as Target ?? new();
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

    public enum SourceDriver {
        Mssql,
        Oracle
    }

    public enum TargetDriver {
        Mssql,
        Oracle,
        SqlText,
    }

}