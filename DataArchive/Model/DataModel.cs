using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

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

    [IgnoreDataMember]
    private string? fileName;
    [IgnoreDataMember]
    public string? FileName {
        get => fileName;
        set {
            if (fileName != value) {
                fileName = value;
                NotifyPropertyChanged();
            }
        }
    }

    [IgnoreDataMember]
    private string? filePath;
    [IgnoreDataMember]
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

    [IgnoreDataMember]
    public bool IsDraft = false;
}