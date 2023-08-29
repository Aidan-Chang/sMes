using System.ComponentModel;
using System.Configuration;
using System.Globalization;
using System.Resources;

namespace DataArchive.Utility;

public static class CultureUtility {

    /// <summary>
    /// Apply culture to form
    /// </summary>
    /// <param name="form"></param>
    public static void ApplyCultureUI(this Form form, string? cultureName = null) {
        if(cultureName != null) {
            SaveCulture(cultureName);
        }
        ComponentResourceManager manager = new ComponentResourceManager(form.GetType());
        ApplyCultureUI(form, manager);
    }

    /// <summary>
    /// Apply the resouce calture to ui component recursively
    /// </summary>
    /// <param name="component"></param>
    /// <param name="manager"></param>
    public static void ApplyCultureUI(IComponent component, ComponentResourceManager manager) {
        switch (component) {
            case MenuStrip menuStrip:
                manager.ApplyResources(menuStrip, menuStrip.Name, Thread.CurrentThread.CurrentUICulture);
                foreach (ToolStripItem item in menuStrip.Items) {
                    ApplyCultureUI(item, manager);
                }
                break;
            case StatusStrip statusStrip:
                manager.ApplyResources(statusStrip, statusStrip.Name, Thread.CurrentThread.CurrentUICulture);
                foreach (ToolStripItem item in statusStrip.Items) {
                    ApplyCultureUI(item, manager);
                }
                break;
            case ToolStrip toolStrip:
                manager.ApplyResources(toolStrip, toolStrip.Name, Thread.CurrentThread.CurrentUICulture);
                foreach (ToolStripItem item in toolStrip.Items) {
                    ApplyCultureUI(item, manager);
                }
                break;
            case ToolStripMenuItem toolStripMenuItem:
                manager.ApplyResources(toolStripMenuItem, toolStripMenuItem.Name, Thread.CurrentThread.CurrentUICulture);
                foreach (ToolStripItem item in toolStripMenuItem.DropDownItems) {
                    ApplyCultureUI(item, manager);
                }
                break;
            case ToolStripItem toolStripItem:
                manager.ApplyResources(toolStripItem, toolStripItem.Name, Thread.CurrentThread.CurrentUICulture);
                break;
            case DataGridView dataGridView:
                manager.ApplyResources(dataGridView, dataGridView.Name, Thread.CurrentThread.CurrentUICulture);
                foreach (DataGridViewColumn column in dataGridView.Columns) {
                    ApplyCultureUI(column, manager);
                }
                break;
            case DataGridViewColumn dataGridViewColumn:
                manager.ApplyResources(dataGridViewColumn, dataGridViewColumn.Name, Thread.CurrentThread.CurrentUICulture);
                break;
            case Form form:
                manager.ApplyResources(form, "$this", Thread.CurrentThread.CurrentUICulture);
                foreach (Control child in form.Controls) {
                    ApplyCultureUI(child, manager);
                }
                break;
            case Control control:
                manager.ApplyResources(control, control.Name, Thread.CurrentThread.CurrentUICulture);
                foreach (Control child in control.Controls) {
                    ApplyCultureUI(child, manager);
                }
                break;
        }
    }

    /// <summary>
    /// get available cultures of the form
    /// </summary>
    /// <param name="form"></param>
    /// <returns></returns>
    public static IEnumerable<CultureInfo> GetAvailableCultures(this Form form) {
        yield return new CultureInfo("en");
        ResourceManager manager = new ResourceManager(form.GetType());
        foreach (CultureInfo culture in CultureInfo.GetCultures(CultureTypes.AllCultures)) {
            if (string.IsNullOrEmpty(culture.Name) == false) {
                ResourceSet? set = manager.GetResourceSet(culture, true, false);
                if (set != null && culture.Name != "en") {
                    yield return culture;
                }
            }
        }
    }

    /// <summary>
    /// load culture from app.config
    /// </summary>
    public static void LoadCulture() {
        string cultureName = ConfigurationManager.AppSettings["Culture"] ?? "en";
        CultureInfo culture = CultureInfo.GetCultureInfo(cultureName);
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
    }

    /// <summary>
    /// save culture to user app.config file
    /// </summary>
    /// <param name="caltureName"></param>
    public static void SaveCulture(string cultureName) {
        CultureInfo culture = CultureInfo.GetCultureInfo(cultureName);
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        config.AppSettings.Settings["Culture"].Value = Thread.CurrentThread.CurrentCulture.Name;
        config.Save();
    }

}
