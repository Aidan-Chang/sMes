using DataArchive.Model;
using DataArchive.Utility;
using System.ComponentModel;

namespace DataArchive;

public partial class Form1 : Form {

    public Form1() {
        InitializeComponent();

        // menu mode
        foreach (var mode in Enum.GetValues(typeof(Options.Mode)).Cast<Options.Mode>()) {
            ToolStripMenuItem item = new ToolStripMenuItem {
                Name = $"{mode.ToString().ToLower()}modeToolStripMenuItem",
                Text = mode.ToString(),
                Tag = mode.ToString(),
                Checked = mode == Program.file.Mode,
            };
            item.Click += (sender, target)
                => Program.file.Mode = Enum.TryParse((sender as ToolStripMenuItem)?.Tag?.ToString() ?? "Copy", out Options.Mode m) ? m : Options.Mode.Copy;
            modeToolStripMenuItem.DropDownItems.Add(item);
        }

        // menu batch size
        foreach (var batchSize in Options.BatchSizes) {
            ToolStripMenuItem item = new ToolStripMenuItem {
                Text = batchSize.ToString(),
                Tag = batchSize.ToString(),
                Checked = batchSize == Program.file.BatchSize,
            };
            item.Click += (sender, target)
                => Program.file.BatchSize = int.TryParse((sender as ToolStripMenuItem)?.Tag?.ToString() ?? "50", out int size) ? size : 50;
            batchSizeToolStripMenuItem.DropDownItems.Add(item);
        }

        // menu recovery mode
        foreach (var mode in Enum.GetValues(typeof(Options.RecoveryMode)).Cast<Options.RecoveryMode>()) {
            ToolStripMenuItem item = new ToolStripMenuItem {
                Name = $"{mode.ToString().ToLower()}recoveryModeToolStripMenuItem",
                Text = mode.ToString(),
                Tag = mode.ToString(),
                Checked = mode == Program.file.RecoveryMode,
            };
            item.Click += (sender, target)
                => Program.file.RecoveryMode = Enum.TryParse((sender as ToolStripMenuItem)?.Tag?.ToString() ?? "NotSet", out Options.RecoveryMode m) ? m : Options.RecoveryMode.NotSet;
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
        Program.file.PropertyChanged += (sender, args) => {
            switch (args.PropertyName) {
                case "Mode":
                    foreach (ToolStripMenuItem dropdownItem in modeToolStripMenuItem.DropDownItems) {
                        dropdownItem.Checked = dropdownItem.Tag.ToString() == Program.file.Mode.ToString();
                    }
                    break;
                case "BatchSize":
                    foreach (ToolStripMenuItem dropdownItem in batchSizeToolStripMenuItem.DropDownItems) {
                        dropdownItem.Checked = dropdownItem.Tag.ToString() == Program.file.BatchSize.ToString();
                    }
                    break;
                case "RecoveryMode":
                    foreach (ToolStripMenuItem dropdownItem in recoveryModeToolStripMenuItem.DropDownItems) {
                        dropdownItem.Checked = dropdownItem.Tag.ToString() == Program.file.RecoveryMode.ToString();
                    }
                    break;
                case "RebuildIndex":
                    rebuildIndexesToolStripMenuItem.Checked = Program.file.RebuildIndex;
                    break;
                case "FileName":
                    break;
                case "FilePath":
                    break;
            }
        };
    }

    private void newToolStripMenuItem_Click(object sender, EventArgs e) {
        var result = Program.file.New();
    }

    private void openToolStripMenuItem_Click(object sender, EventArgs e) {
        var result = Program.file.Open();
    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
        var result = Program.file.Save();
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
        var result = Program.file.SaveAs();
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
        Form2 form = new Form2(Program.file.Source);
        var result = form.ShowDialog();
        if (result == DialogResult.OK) {
            Program.file.Source = form.Respository;
        }
    }

    private void targetToolStripMenuItem_Click(object sender, EventArgs e) {
        Form3 form = new Form3(Program.file.Target);
        var result = form.ShowDialog();
        if (result == DialogResult.OK) {
            Program.file.Target = form.Respository;
        }
    }

    private void rebuildIndexesToolStripMenuItem_Click(object sender, EventArgs e) {
        Program.file.RebuildIndex = true;
    }

}