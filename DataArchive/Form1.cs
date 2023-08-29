using DataArchive.Model;
using DataArchive.Utility;

namespace DataArchive;

public partial class Form1 : Form {

    private ArchiveFile archive = new();

    public Form1() {
        InitializeComponent();

        // menu mode
        foreach (var mode in Enum.GetValues(typeof(Options.Mode)).Cast<Options.Mode>()) {
            ToolStripMenuItem item = new ToolStripMenuItem {
                Name = $"{mode.ToString().ToLower()}modeToolStripMenuItem",
                Tag = mode.ToString(),
                Checked = mode == archive.Mode,
            };
            item.Click += (sender, target)
                => archive.Mode = Enum.TryParse((sender as ToolStripMenuItem)?.Tag?.ToString() ?? "Copy", out Options.Mode m) ? m : Options.Mode.Copy;
            modeToolStripMenuItem.DropDownItems.Add(item);
        }

        // menu batch size
        foreach (var batchSize in Options.BatchSizes) {
            ToolStripMenuItem item = new ToolStripMenuItem {
                Text = batchSize.ToString(),
                Tag = batchSize.ToString(),
                Checked = batchSize == archive.BatchSize,
            };
            item.Click += (sender, target)
                => archive.BatchSize = int.TryParse((sender as ToolStripMenuItem)?.Tag?.ToString() ?? "50", out int size) ? size : 50;
            batchSizeToolStripMenuItem.DropDownItems.Add(item);
        }

        // menu recovery mode
        foreach (var mode in Enum.GetValues(typeof(Options.RecoveryMode)).Cast<Options.RecoveryMode>()) {
            ToolStripMenuItem item = new ToolStripMenuItem {
                Name = $"{mode.ToString().ToLower()}recoveryModeToolStripMenuItem",
                Tag = mode.ToString(),
                Checked = mode == archive.RecoveryMode,
            };
            item.Click += (sender, target)
                => archive.RecoveryMode = Enum.TryParse((sender as ToolStripMenuItem)?.Tag?.ToString() ?? "NotSet", out Options.RecoveryMode m) ? m : Options.RecoveryMode.NotSet;
            recoveryModeToolStripMenuItem.DropDownItems.Add(item);
        }

        // menu language
        foreach (var culture in CultureUtility.GetAvailableCultures(this)) {
            ToolStripMenuItem item = new ToolStripMenuItem {
                Text = culture.DisplayName,
                Tag = culture.Name,
                Checked = Thread.CurrentThread.CurrentUICulture.Name == culture.Name,
            };
            item.Click += (sender, target) => {
                var caltureName = (sender as ToolStripMenuItem)?.Tag?.ToString() ?? "en";
                this.ApplyCultureUI(caltureName);
                foreach (var dropdownItem in languageToolStripMenuItem.DropDownItems) {
                    if (dropdownItem is ToolStripMenuItem) {
                        ((ToolStripMenuItem)dropdownItem).Checked = (((ToolStripMenuItem)dropdownItem).Tag?.ToString() ?? "en") == caltureName;
                    }
                }
            };
            languageToolStripMenuItem.DropDownItems.Add(item);
        }

        // update calture of ui
        this.ApplyCultureUI();

        // on values changed
        archive.PropertyChanged += (sender, args) => {
            switch (args.PropertyName) {
                case "Mode":
                    foreach (ToolStripMenuItem dropdownItem in modeToolStripMenuItem.DropDownItems) {
                        dropdownItem.Checked = dropdownItem.Tag.ToString() == archive.Mode.ToString();
                    }
                    break;
                case "BatchSize":
                    foreach (ToolStripMenuItem dropdownItem in batchSizeToolStripMenuItem.DropDownItems) {
                        dropdownItem.Checked = dropdownItem.Tag.ToString() == archive.BatchSize.ToString();
                    }
                    break;
                case "RecoveryMode":
                    foreach (ToolStripMenuItem dropdownItem in recoveryModeToolStripMenuItem.DropDownItems) {
                        dropdownItem.Checked = dropdownItem.Tag.ToString() == archive.RecoveryMode.ToString();
                    }
                    break;
                case "RebuildIndex":
                    rebuildIndexesToolStripMenuItem.Checked = archive.RebuildIndex;
                    break;
                case "FileName":
                case "FilePath":
                    break;
            }
        };
    }

    private void newToolStripMenuItem_Click(object sender, EventArgs e) {
        var result = archive.New();
    }

    private void openToolStripMenuItem_Click(object sender, EventArgs e) {
        var result = archive.Open();
    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
        var result = archive.Save();
        switch (result.State) {
            case FileAccessState.Success:
                MessageBox.Show("Save Success", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                break;
            case FileAccessState.Cancel:
                break;
            case FileAccessState.Error:
                MessageBox.Show(result.Exception?.Message ?? "", "Get unexcetpt error");
                break;
        }
    }

    private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
        var result = archive.SaveAs();
        switch (result.State) {
            case FileAccessState.Success:
                MessageBox.Show("Save Success", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                break;
            case FileAccessState.Cancel:
                break;
            case FileAccessState.Error:
                MessageBox.Show(result.Exception?.Message ?? "", "Get unexcetpt error");
                break;
        }
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
        Application.Exit();
    }

    private void sourceToolStripMenuItem_Click(object sender, EventArgs e) {
        Form2 form = new Form2(archive.Source);
        var result = form.ShowDialog();
        if (result == DialogResult.OK) {
            archive.Source = form.Respository;
        }
    }

    private void targetToolStripMenuItem_Click(object sender, EventArgs e) {
        Form3 form = new Form3(archive.Target);
        var result = form.ShowDialog();
        if (result == DialogResult.OK) {
            archive.Target = form.Respository;
        }
    }

    private void rebuildIndexesToolStripMenuItem_Click(object sender, EventArgs e) {
        archive.RebuildIndex = true;
    }

}