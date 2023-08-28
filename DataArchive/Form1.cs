using DataArchive.Model;
using DataArchive.Utility;

namespace DataArchive;

public partial class Form1 : Form {

    private DataModel model = new();

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
        foreach (var culture in ComponentUtility.GetAvailableCultures(this)) {
            ToolStripMenuItem item = new ToolStripMenuItem {
                Text = culture.DisplayName,
                Tag = culture.Name,
                Checked = Thread.CurrentThread.CurrentUICulture.Name == culture.Name,
            };
            item.Click += (sender, target) => {
                var caltureName = (sender as ToolStripMenuItem)?.Tag?.ToString() ?? "en";
                ComponentUtility.SaveCulture(caltureName);
                ComponentUtility.ApplyCaultureUI(this);
                foreach (var dropdownItem in languageToolStripMenuItem.DropDownItems) {
                    if (dropdownItem is ToolStripMenuItem) {
                        ((ToolStripMenuItem)dropdownItem).Checked = (((ToolStripMenuItem)dropdownItem).Tag?.ToString() ?? "en") == caltureName;
                    }
                }
            };
            languageToolStripMenuItem.DropDownItems.Add(item);
        }

        // update calture of ui
        ComponentUtility.ApplyCaultureUI(this);

        // on values changed
        model.PropertyChanged += (sender, args) => {
            model.IsDraft = true;
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
                    break;
            }
        };
    }

    private void newToolStripMenuItem_Click(object sender, EventArgs e) {
        if (model.IsDraft) {
            if (model.FileName == null) {
                var saveResult = model.Save();
                switch (saveResult.State) {
                    case ModelAccessState.Success:
                        model.New();
                        break;
                    case ModelAccessState.Cancel:
                        break;
                    case ModelAccessState.Error:
                        MessageBox.Show(saveResult.Exception?.Message ?? "", "Get unexcetpt error");
                        break;
                }
            }
            else {
                var result = MessageBox.Show(
                    $"Do you want to save changes to {model.FileName}?",
                    "Confirm ",
                    MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes) {
                    var saveResult = model.Save();
                    switch (saveResult.State) {
                        case ModelAccessState.Success:
                            model.New();
                            break;
                        case ModelAccessState.Cancel:
                            break;
                        case ModelAccessState.Error:
                            MessageBox.Show(saveResult.Exception?.Message ?? "", "Get unexcetpt error");
                            break;
                    }
                }
                else {
                    model.New();
                }
            }
        }
        else {
            model.New();
        }
    }

    private void openToolStripMenuItem_Click(object sender, EventArgs e) {
        if (model.IsDraft) {
            if (model.FileName == null) {
                var message = MessageBox.Show(
                    $"Do you want to save changes?",
                    "Confirm ",
                    MessageBoxButtons.YesNo);
                if (message == DialogResult.Yes) {
                    var saveResult = model.Save();
                    switch (saveResult.State) {
                        case ModelAccessState.Success:
                            var openResult = model.Open();
                            switch (openResult.State) {
                                case ModelAccessState.Success:
                                    model.Load(openResult.Model!);
                                    break;
                                case ModelAccessState.Error:
                                    MessageBox.Show(openResult.Exception?.Message ?? "", "Get unexcetpt error");
                                    break;
                            }
                            break;
                        case ModelAccessState.Cancel:
                            break;
                        case ModelAccessState.Error:
                            MessageBox.Show(saveResult.Exception?.Message ?? "", "Get unexcetpt error");
                            break;
                    }
                }
                else {
                    var openResult = model.Open();
                    switch (openResult.State) {
                        case ModelAccessState.Success:
                            model.Load(openResult.Model!);
                            break;
                        case ModelAccessState.Error:
                            MessageBox.Show(openResult.Exception?.Message ?? "", "Get unexcetpt error");
                            break;
                    }
                }
            }
            else {
                var message = MessageBox.Show(
                    $"Do you want to save changes to {model.FileName}?",
                    "Confirm ",
                    MessageBoxButtons.YesNo);
                if (message == DialogResult.Yes) {
                    var saveResult = model.Save();
                    switch (saveResult.State) {
                        case ModelAccessState.Success:
                            var openResult = model.Open();
                            switch (openResult.State) {
                                case ModelAccessState.Success:
                                    model.Load(openResult.Model!);
                                    break;
                                case ModelAccessState.Error:
                                    MessageBox.Show(openResult.Exception?.Message ?? "", "Get unexcetpt error");
                                    break;
                            }
                            break;
                        case ModelAccessState.Cancel:
                            break;
                        case ModelAccessState.Error:
                            MessageBox.Show(saveResult.Exception?.Message ?? "", "Get unexcetpt error");
                            break;
                    }
                }
                else {
                    var openResult = model.Open();
                    switch (openResult.State) {
                        case ModelAccessState.Success:
                            model.Load(openResult.Model!);
                            break;
                        case ModelAccessState.Error:
                            MessageBox.Show(openResult.Exception?.Message ?? "", "Get unexcetpt error");
                            break;
                    }
                }
            }
        }
        else {
            var openResult = model.Open();
            switch (openResult.State) {
                case ModelAccessState.Success:
                    model.Load(openResult.Model!);
                    break;
                case ModelAccessState.Error:
                    MessageBox.Show(openResult.Exception?.Message ?? "", "Get unexcetpt error");
                    break;
            }
        }
    }

    private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
        var saveResult = model.Save();
        switch (saveResult.State) {
            case ModelAccessState.Success:
                break;
            case ModelAccessState.Cancel:
                break;
            case ModelAccessState.Error:
                MessageBox.Show(saveResult.Exception?.Message ?? "", "Get unexcetpt error");
                break;
        }
    }

    private void saveAsToolStripMenuItem_Click(object sender, EventArgs e) {
        var saveResult = model.SaveAs();
        switch (saveResult.State) {
            case ModelAccessState.Success:
                break;
            case ModelAccessState.Cancel:
                break;
            case ModelAccessState.Error:
                MessageBox.Show(saveResult.Exception?.Message ?? "", "Get unexcetpt error");
                break;
        }
    }

    private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
        Application.Exit();
    }

    private void sourceToolStripMenuItem_Click(object sender, EventArgs e) {
        Form2 form = new Form2(model.Source);
        var result = form.ShowDialog();
        if (result == DialogResult.OK) {
            model.Source = form.EndPoint;
        }
    }

    private void targetToolStripMenuItem_Click(object sender, EventArgs e) {
        Form3 form = new Form3(model.Target);
        var result = form.ShowDialog();
        if (result == DialogResult.OK) {
            model.Target = form.EndPoint;
        }
    }

    private void rebuildIndexesToolStripMenuItem_Click(object sender, EventArgs e) {
        model.RebuildIndex = true;
    }

}