using DataArchive.Model;
using System.IO.Compression;
using System.Text;
using System.Text.Json;

namespace DataArchive.Utility;

public static class ModelUtility {

    public static DataModel New(this DataModel model) {
        model.Source = null;
        model.Target = null;
        model.Mode = Options.Mode.Copy;
        model.BatchSize = 50;
        model.RecoveryMode = Options.RecoveryMode.NotSet;
        model.RebuildIndex = false;
        model.FileName = null;
        model.FilePath = null;
        model.IsDraft = false;
        return model;
    }

    public static void Load(this DataModel model, DataModel copy) {
        model.Source = copy.Source;
        model.Target = copy.Target;
        model.Mode = copy.Mode;
        model.BatchSize = copy.BatchSize;
        model.RecoveryMode = copy.RecoveryMode;
        model.RebuildIndex = copy.RebuildIndex;
        model.FileName = copy.FileName;
        model.FilePath = copy.FilePath;
        model.IsDraft = false;
    }

    public static DataModel Open() {
        return new();
    }

    public static ModelAccessResult Save(this DataModel model) {
        if (model.FilePath == null) {
            return SaveAs(model);
        }
        else {
            return Save(model, model.FilePath);
        }
    }

    public static ModelAccessResult SaveAs(this DataModel model) {
        var dialog = new SaveFileDialog {
            Filter = "Archive Data Setting File|*.adf",
        };
        if (dialog.ShowDialog() == DialogResult.OK) {
            Save(model, dialog.FileName);
            return new ModelAccessResult {
                State = ModelAccessState.Success,
                Model = model,
                FilePath = dialog.FileName,
            };
        }
        return new ModelAccessResult {
            State = ModelAccessState.Cancel,
            Model = model,
        };
    }

    public static ModelAccessResult Open(this DataModel model) {
        var dialog = new OpenFileDialog {
            Filter = "Archive Data Setting File|*.adf",
            Multiselect = false,
        };
        if (dialog.ShowDialog() == DialogResult.OK) {
            return Open(dialog.FileName);
        }
        return new ModelAccessResult {
            State = ModelAccessState.Cancel,
            Model = model,
        };
    }

    public static ModelAccessResult Open(string filePath) {
        try {
            using FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using MemoryStream ms = new MemoryStream();
            using GZipStream gz = new GZipStream(fs, CompressionMode.Decompress);
            gz.CopyTo(ms);
            gz.Close();
            var json = Encoding.UTF8.GetString(ms.ToArray());
            DataModel? result = JsonSerializer.Deserialize<DataModel>(json);
            return new ModelAccessResult {
                State = ModelAccessState.Success,
                Model = result,
                FilePath = filePath,
            };
        }
        catch (Exception ex) {
            return new ModelAccessResult {
                State = ModelAccessState.Error,
                Exception = ex,
            };
        }
    }

    public static ModelAccessResult Save(DataModel model, string filePath) {
        try {
            string json = JsonSerializer.Serialize(model);
            var data = Encoding.UTF8.GetBytes(json);
            using FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            using GZipStream gz = new(fs, CompressionLevel.Optimal);
            gz.Write(data, 0, data.Length);
            gz.Close();
            model.IsDraft = false;
            return new ModelAccessResult {
                State = ModelAccessState.Success,
                Model = model,
                FilePath = filePath,
            };
        }
        catch (Exception ex) {
            return new ModelAccessResult() {
                State = ModelAccessState.Error,
                Model = model,
                FilePath = filePath,
                Exception = ex
            };
        }
    }

}

public class ModelAccessResult {

    public ModelAccessState State { get; set; }

    public DataModel? Model { get; set; }

    public string? FilePath { get; set; }

    public Exception? Exception { get; set; } = null;

}

public enum ModelAccessState {
    Success,
    Cancel,
    Error,
}
