using DataArchive.Model;
using System.IO.Compression;
using System.Text;
using System.Text.Json;

namespace DataArchive.Utility;

public static class ArchiveFileUtility {

    public static FileAccessResult New(this ArchiveFile archive) {
        var result = archive.Close();
        if (result.State == FileAccessState.Success) {
            archive.Source = null;
            archive.Target = null;
            archive.Mode = Options.Mode.Copy;
            archive.BatchSize = 50;
            archive.RecoveryMode = Options.RecoveryMode.NotSet;
            archive.RebuildIndex = false;
            archive.FileName = null;
            archive.FilePath = null;
            archive.IsDraft = false;
        }
        return result;
    }

    public static FileAccessResult Save(this ArchiveFile archive) {
        if (archive.FilePath == null) {
            return SaveAs(archive);
        }
        else {
            return Save(archive, archive.FilePath);
        }
    }

    public static FileAccessResult SaveAs(this ArchiveFile archive) {
        var dialog = new SaveFileDialog {
            Filter = "Archive Data Setting File|*.adf",
        };
        if (dialog.ShowDialog() == DialogResult.OK) {
            return Save(archive, dialog.FileName);
        }
        return new FileAccessResult {
            State = FileAccessState.Cancel,
        };
    }

    public static FileAccessResult Open(this ArchiveFile archive) {
        var result = archive.Close();
        if (result.State == FileAccessState.Success) {
            var dialog = new OpenFileDialog {
                Filter = "Archive Data Setting File|*.adf",
                Multiselect = false,
            };
            if (dialog.ShowDialog() == DialogResult.OK) {
                result = archive.Load(dialog.FileName);
                if (result.State == FileAccessState.Success) {
                    MessageBox.Show($"{archive.FileName} is loaded", "File Load Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return result;
            }
            return new FileAccessResult {
                State = FileAccessState.Cancel,
            };
        }
        return result;
    }

    public static FileAccessResult Close(this ArchiveFile archive) {
        if (archive.IsDraft && archive.FileName != null) {
            switch (
                MessageBox.Show(
                    $"Do you want to save changes to {archive.FileName}",
                    "Confirm",
                    MessageBoxButtons.YesNo)) {
                case DialogResult.No:
                    return new FileAccessResult { State = FileAccessState.Cancel };
            }
        }
        if (archive.IsDraft) {
            return archive.Save();
        }
        return new FileAccessResult { State = FileAccessState.Success };
    }

    private static void Set(this ArchiveFile model, ArchiveFile copy) {
        model.Source = copy.Source;
        model.Target = copy.Target;
        model.Mode = copy.Mode;
        model.BatchSize = copy.BatchSize;
        model.RecoveryMode = copy.RecoveryMode;
        model.RebuildIndex = copy.RebuildIndex;
        model.FileName = copy.FileName;
        model.FilePath = copy.FilePath;
        model.IsDraft = copy.IsDraft;
    }

    private static FileAccessResult Load(this ArchiveFile archive, string filePath) {
        try {
            using FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using MemoryStream ms = new MemoryStream();
            using GZipStream gz = new GZipStream(fs, CompressionMode.Decompress);
            gz.CopyTo(ms);
            gz.Close();
            var json = Encoding.UTF8.GetString(ms.ToArray());
            ArchiveFile result = JsonSerializer.Deserialize<ArchiveFile>(json)!;
            FileInfo info = new FileInfo(filePath);
            result.FileName = info.Name;
            result.FilePath = info.FullName;
            result.IsDraft = false;
            // set properties from file
            archive.Set(result);
            return new FileAccessResult {
                State = FileAccessState.Success,
            };
        }
        catch (Exception ex) {
            return new FileAccessResult {
                State = FileAccessState.Error,
                Exception = ex,
            };
        }
    }

    private static FileAccessResult Save(ArchiveFile archive, string filePath) {
        try {
            string json = JsonSerializer.Serialize(archive);
            var data = Encoding.UTF8.GetBytes(json);
            using FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            using GZipStream gz = new(fs, CompressionLevel.Optimal);
            gz.Write(data, 0, data.Length);
            gz.Close();
            FileInfo info = new FileInfo(filePath);
            archive.FileName = info.Name;
            archive.FilePath = info.FullName;
            archive.IsDraft = false;
            return new FileAccessResult {
                State = FileAccessState.Success,
            };
        }
        catch (Exception ex) {
            return new FileAccessResult() {
                State = FileAccessState.Error,
                Exception = ex
            };
        }
    }

}

public class FileAccessResult {

    public FileAccessState State { get; set; }

    public Exception? Exception { get; set; } = null;

}

public enum FileAccessState {
    Success,
    Cancel,
    Error,
}
