/////////////////////////////////////////////////////////////////////////////////////
//  File:   Program.cs                                              7 Dec 25 PHR
/////////////////////////////////////////////////////////////////////////////////////

using Serilog;
using Serilog.Core;
using Serilog.Extensions.Logging;
using SipLib.Logging;
using System.IO;

namespace OspSimulator;

internal static class Program
{
    public const string AppName = "OspSimulator";
    public static string LoggingDirectory = @"\var\log\PsapSimulator";
    private const string LoggingFileName = $"{AppName}.log";
    private static LoggingLevelSwitch m_LevelSwitch = new LoggingLevelSwitch();

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // Setup application logging using Serilog
        LoggingDirectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\\{AppName}\\Logs";
        if (Directory.Exists(LoggingDirectory) == false)
            Directory.CreateDirectory(LoggingDirectory);
        string LoggingPath = Path.Combine(LoggingDirectory, LoggingFileName);
        Logger log = new LoggerConfiguration()
            .MinimumLevel.ControlledBy(m_LevelSwitch)
            .WriteTo.File(LoggingPath, fileSizeLimitBytes: 1000000, retainedFileCountLimit: 5,
            outputTemplate: "{Timestamp:yyyy-MM-ddTHH:mm:ss.ffffffzzz} [{Level}] {Message}{NewLine}{Exception}")
            .CreateLogger();
        SerilogLoggerFactory factory = new SerilogLoggerFactory(log);
        SipLogger.Log = factory.CreateLogger(AppName);

        SipLogger.LogInformation($"Starting {AppName} now");

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        try
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
        catch (Exception ex)
        {
            SipLogger.LogCritical(ex, $"Critical exception in {AppName}");
        }

        SipLogger.LogInformation($"Exiting {AppName} now");
    }

}
