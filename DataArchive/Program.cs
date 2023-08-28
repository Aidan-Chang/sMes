using DataArchive.Driver;
using DataArchive.Utility;
using System.Configuration;
using System.Globalization;

namespace DataArchive;

internal static class Program {

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main() {

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.

        // set language
        string languageName = ConfigurationManager.AppSettings["Language"] ?? "en";
        CultureInfo culture = CultureInfo.GetCultureInfo(languageName);
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;

        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());
    }

}