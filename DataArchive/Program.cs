using DataArchive.Model;
using DataArchive.Utility;

namespace DataArchive;

internal static class Program {

    public static ArchiveFile file = new();

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main() {

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.

        // load saved culture
        CultureUtility.LoadCulture();

        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());
    }

}