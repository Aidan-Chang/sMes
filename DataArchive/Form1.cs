using DataArchive.Model;

namespace DataArchive;

public partial class Form1 : Form {

    private DataModel model = new();
    private bool modelChanged = false;

    public Form1() {
        InitializeComponent();

        // menu mode
        foreach (var mode in Enum.GetValues(typeof(Options.Mode)).Cast<Options.Mode>()) {
            ToolStripMenuItem item = new ToolStripMenuItem {
                Name = $"{mode.ToString().ToLower()}modeToolStripMenuItem",
                Tag = mode.ToString(),
                Checked = mode == model.Mode,
            };
            item.Click += (sender, target)
                => model.Mode = Enum.TryParse((sender as ToolStripMenuItem)?.Tag?.ToString() ?? "Copy", out Options.Mode m) ? m : Options.Mode.Copy;
            modeToolStripMenuItem.DropDownItems.Add(item);
        }

        // menu batch size
        foreach (var batchSize in Options.BatchSizes) {
            ToolStripMenuItem item = new ToolStripMenuItem {
                Text = batchSize.ToString(),
                Tag = batchSize.ToString(),
                Checked = batchSize == model.BatchSize,
            };
            item.Click += (sender, target)
                => model.BatchSize = int.TryParse((sender as ToolStripMenuItem)?.Tag?.ToString() ?? "50", out int size) ? size : 50;
            batchSizeToolStripMenuItem.DropDownItems.Add(item);
        }

        // menu recovery mode
        foreach (var mode in Enum.GetValues(typeof(Options.RecoveryMode)).Cast<Options.RecoveryMode>()) {
            ToolStripMenuItem item = new ToolStripMenuItem {
                Name = $"{mode.ToString().ToLower()}recoveryModeToolStripMenuItem",
                Tag = mode.ToString(),
                Checked = mode == model.RecoveryMode,
            };
            item.Click += (sender, target)
                => model.RecoveryMode = Enum.TryParse((sender as ToolStripMenuItem)?.Tag?.ToString() ?? "NotSet", out Options.RecoveryMode m) ? m : Options.RecoveryMode.NotSet;
            recoveryModeToolStripMenuItem.DropDownItems.Add(item);
        }

        // menu language
        foreach (var culture in Program.GetCultures(this)) {
            ToolStripMenuItem item = new ToolStripMenuItem {
                Text = culture.DisplayName,
                Tag = culture.Name,
                Checked = Thread.CurrentThread.CurrentUICulture.Name == culture.Name,
            };
            item.Click += (sender, target) => {
                var caltureName = (sender as ToolStripMenuItem)?.Tag?.ToString() ?? "en";
                Program.SaveCulture(caltureName);
                Program.ApplyCaultureUI(this);
                foreach (var dropdownItem in languageToolStripMenuItem.DropDownItems) {
                    if (dropdownItem is ToolStripMenuItem) {
                        ((ToolStripMenuItem)dropdownItem).Checked = (((ToolStripMenuItem)dropdownItem).Tag?.ToString() ?? "en") == caltureName;
                    }
                }
            };
            languageToolStripMenuItem.DropDownItems.Add(item);
        }

        // update calture of ui
        Program.ApplyCaultureUI(this);

        // on values changed
        model.PropertyChanged += (sender, args) => {
            modelChanged = true;
            switch (args.PropertyName) {
                case "Mode":
                    foreach (ToolStripMenuItem dropdownItem in modeToolStripMenuItem.DropDownItems) {
                        dropdownItem.Checked = dropdownItem.Tag.ToString() == model.Mode.ToString();
                    }
                    break;
                case "BatchSize":
                    foreach (ToolStripMenuItem dropdownItem in batchSizeToolStripMenuItem.DropDownItems) {
                        dropdownItem.Checked = dropdownItem.Tag.ToString() == model.BatchSize.ToString();
                    }
                    break;
                case "RecoveryMode":
                    foreach (ToolStripMenuItem dropdownItem in recoveryModeToolStripMenuItem.DropDownItems) {
                        dropdownItem.Checked = dropdownItem.Tag.ToString() == model.RecoveryMode.ToString();
                    }
                    break;
                case "RebuildIndex":
                    rebuildIndexesToolStripMenuItem.Checked = model.RebuildIndex;
                    break;
                case "FileName":
                case "FilePath":
                    modelChanged = false;
                    break;
            }
        };
    }

    private void newToolStripMenuItem_Click(object sender, EventArgs e) {
        if (modelChanged) {
            if (model.FileName == null) {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.Filter = "Archive Setting File|*.das";
                dialog.Title = "Save Data Archive Setting File";
                switch (dialog.ShowDialog()) {
                    case DialogResult.OK:
                        model.Reset();
                        break;
                    case DialogResult.Cancel:
                        model.Reset();
                        break;
                }
            }
            else {
                var result = MessageBox.Show(
                    $"Do you want to save changes to {model.FileName}?",
                    "Confirm ",
                    MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes) {
                    model.Reset();
                }
            }
        }
        else {
            model.Reset();
        }
    }

    private void openToolStripMenuItem_Click(object sender, EventArgs e) {

    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
    }

    private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
        Application.Exit();
    }

    private void sourceToolStripMenuItem_Click(object sender, EventArgs e) {
        Form2 form = new Form2(model.Source);
        var result = form.ShowDialog();
        if (result == DialogResult.Yes) {
            model.Source = form.Source;
        }
    }

    private void targetToolStripMenuItem_Click(object sender, EventArgs e) {
        Form3 form = new Form3(model.Target);
        var result = form.ShowDialog();
        if (result == DialogResult.Yes) {
            model.Target = form.Target;
        }
    }

    private void rebuildIndexesToolStripMenuItem_Click(object sender, EventArgs e) {
        model.RebuildIndex = true;
    }

}