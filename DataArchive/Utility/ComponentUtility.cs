using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Resources;

namespace DataArchive.Utility;

public static class ComponentUtility {

    public static void ApplyCaultureUI(Form form) {
        ComponentResourceManager manager = new ComponentResourceManager(form.GetType());
        ApplyCaultureUI(form, manager);
    }

    public static void ApplyCaultureUI(IComponent component, ComponentResourceManager manager) {
        switch (component) {
            case MenuStrip menuStrip:
                manager.ApplyResources(menuStrip, menuStrip.Name, Thread.CurrentThread.CurrentUICulture);
                foreach (ToolStripItem item in menuStrip.Items) {
                    ApplyCaultureUI(item, manager);
                }
                break;
            case StatusStrip statusStrip:
                manager.ApplyResources(statusStrip, statusStrip.Name, Thread.CurrentThread.CurrentUICulture);
                foreach (ToolStripItem item in statusStrip.Items) {
                    ApplyCaultureUI(item, manager);
                }
                break;
            case ToolStrip toolStrip:
                manager.ApplyResources(toolStrip, toolStrip.Name, Thread.CurrentThread.CurrentUICulture);
                foreach (ToolStripItem item in toolStrip.Items) {
                    ApplyCaultureUI(item, manager);
                }
                break;
            case ToolStripMenuItem toolStripMenuItem:
                manager.ApplyResources(toolStripMenuItem, toolStripMenuItem.Name, Thread.CurrentThread.CurrentUICulture);
                foreach (ToolStripItem item in toolStripMenuItem.DropDownItems) {
                    ApplyCaultureUI(item, manager);
                }
                break;
            case ToolStripItem toolStripItem:
                manager.ApplyResources(toolStripItem, toolStripItem.Name, Thread.CurrentThread.CurrentUICulture);
                break;
            case DataGridView dataGridView:
                manager.ApplyResources(dataGridView, dataGridView.Name, Thread.CurrentThread.CurrentUICulture);
                foreach (DataGridViewColumn column in dataGridView.Columns) {
                    ApplyCaultureUI(column, manager);
                }
                break;
            case DataGridViewColumn dataGridViewColumn:
                manager.ApplyResources(dataGridViewColumn, dataGridViewColumn.Name, Thread.CurrentThread.CurrentUICulture);
                break;
            case Form form:
                manager.ApplyResources(form, "$this", Thread.CurrentThread.CurrentUICulture);
                foreach (Control child in form.Controls) {
                    ApplyCaultureUI(child, manager);
                }
                break;
            case Control control:
                manager.ApplyResources(control, control.Name, Thread.CurrentThread.CurrentUICulture);
                foreach (Control child in control.Controls) {
                    ApplyCaultureUI(child, manager);
                }
                break;
        }
    }

    public static IEnumerable<CultureInfo> GetAvailableCultures(Form form) {
        yield return new CultureInfo("en");
        ResourceManager manager = new ResourceManager(form.GetType());
        foreach (CultureInfo culture in CultureInfo.GetCultures(CultureTypes.AllCultures)) {
            if (string.IsNullOrEmpty(culture.Name) == false) {
                ResourceSet? set = manager.GetResourceSet(culture, true, false);
                if (set != null) {
                    yield return culture;
                }
            }
        }
    }

    public static void SaveCulture(string caltureName) {
        CultureInfo culture = CultureInfo.GetCultureInfo(caltureName);
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        config.AppSettings.Settings["Language"].Value = Thread.CurrentThread.CurrentCulture.Name;
        config.Save();
    }

}
