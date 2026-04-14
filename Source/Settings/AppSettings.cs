/////////////////////////////////////////////////////////////////////////////////////
//  File:   AppSettings.cs                                          20 Jan 26 PHR
/////////////////////////////////////////////////////////////////////////////////////

using Ng911Lib.Utilities;
using PsapSimulator.Settings;
using SipLib.Logging;
using System.IO;
using System.Text.Json.Serialization;

namespace OspSimulator.Settings;

/// <summary>
/// All settings for the application
/// </summary>
public class AppSettings
{
    /// <summary>
    /// Gets or sets the network configuration settings for the OspSimulator application.
    /// </summary>
    public NetworkSettings NetworkSettings { get; set; } = new NetworkSettings();

    /// <summary>
    /// Gets or sets the MediaSettings for the OspSimulator application.
    /// </summary>
    public MediaSettings MediaSettings { get; set; } = new MediaSettings();

    /// <summary>
    /// Gets or sets the X.509 certificate settings for the OspSimulator application.
    /// </summary>
    public CertificateSettings CertificateSettings { get; set; } = new CertificateSettings();

    /// <summary>
    /// Gets or sets the settings that determine how location information is delivered with an
    /// outgoing call.
    /// </summary>
    public LocationSettings LocationSettings { get; set; } = new LocationSettings();

    /// <summary>
    /// Gets or sets the settings that determine how additional data (see RFC 7852) is delivered with
    /// an outgoing call.
    /// </summary>
    public AdditionalDataSettings AddtionalDataSettings { get; set; } = new AdditionalDataSettings();

    /// <summary>
    /// Gets or sets the settings used for the most recent outgoing call from the OspSimulator application.
    /// </summary>
    public CallSettings LastCallSettings { get; set; } = new CallSettings();

    /// <summary>
    /// Gets or sets the settings to use for the audio and video devices.
    /// </summary>
    public DeviceSettings DeviceSettings { get; set; } = new DeviceSettings();

    /// <summary>
    /// Constructor
    /// </summary>
    [JsonConstructor]
    public AppSettings()
    {
    }

    private const string SettingsFileName = $"{Program.AppName}.json";

    /// <summary>
    /// Gets the saved configuration settings if they exist or the default settings if they do not.
    /// </summary>
    /// <returns>Returns the configuration settings</returns>
    public static AppSettings GetAppSettings()
    {
        AppSettings? appSettings = new AppSettings();
        string MyDocmentsDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string SettingsDirectory = @$"{MyDocmentsDir}\{Program.AppName}";
        string SettingsFilePath = Path.Combine(SettingsDirectory, SettingsFileName);
        if (File.Exists(SettingsFilePath) == true)
        {
            string strSettings = File.ReadAllText(SettingsFilePath);
            appSettings = JsonHelper.DeserializeFromString<AppSettings>(strSettings);
            if (appSettings == null)
            {   // An error occurred, use the default settings
                appSettings = new AppSettings();
                SipLogger.LogError("Error deserializing the settings file, using the default settings");
            }
        }
        else
        {   // Use the default settings
            SipLogger.LogInformation("The settings file does not exist, using the default settings");
        }

        return appSettings;
    }

    /// <summary>
    /// Saves the settings to a file.
    /// </summary>
    /// <param name="appSettings">Settings to save.</param>
    public static void SaveAppSettings(AppSettings appSettings)
    {
        string MyDocmentsDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        string SettingsDirectory = @$"{MyDocmentsDir}\{Program.AppName}";
        string SettingsFilePath = Path.Combine(SettingsDirectory, SettingsFileName);
        try
        {
            if (Directory.Exists(SettingsDirectory) == false)
            {
                DirectoryInfo dirInfo = Directory.CreateDirectory(SettingsDirectory);
                if (dirInfo.Exists == false)
                {
                    SipLogger.LogError($"Unable to create the settings directory: {SettingsDirectory}. " +
                        "Settings not saved.");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            SipLogger.LogError(ex, $"Unable to create the settings directory: {SettingsDirectory}. " +
                "Settings not saved.");
            return;
        }

        string strSettings = JsonHelper.SerializeToString(appSettings);
        if (string.IsNullOrEmpty(strSettings) == true)
        {
            SipLogger.LogError("Unable to serialize the settings. Settings not saved");
            return;
        }

        try
        {
            File.WriteAllText(SettingsFilePath, strSettings);
        }
        catch (Exception Ex)
        {
            SipLogger.LogError(Ex, $"Unable to write the settings file: {SettingsFileName}");
        }
    }
}
