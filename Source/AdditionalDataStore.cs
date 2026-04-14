/////////////////////////////////////////////////////////////////////////////////////
//  File:   AdditionalDataStore.cs                                  7 Jan 26 PHR
/////////////////////////////////////////////////////////////////////////////////////

using AdditionalData;
using Ng911Lib.Utilities;
using Pidf;
using Veds;
using SipLib.Core;
using SipLib.Logging;
using System.Collections.Concurrent;
using System.IO;

namespace OspSimulator;

/// <summary>
/// This class manages location and additional data for simulating NG9-1-1 calls. To use this class,
/// construct an instance of it and then call the Initialize() method.
/// </summary>
public class AdditionalDataStore
{
    private const string LOCATION_FILE_NAME = "PidfLo.xml";
    private const string COMMENT_FILE_NAME_PATTERN = "Comment";
    private const string DEVICE_INFO_FILE_NAME = "DeviceInfo.xml";
    private const string PROVIDER_INFO_FILE_NAME_PATTERN = "ProviderInfo";
    private const string SERVICE_INFO_FILE_NAME = "ServiceInfo.xml";
    private const string SUBSCRIBER_INFO_FILE_NAME = "SubscriberInfo.xml";
    private const string AUTOMATED_CRASH_NOTIFICATION_FILE_NAME = "AutomatedCrashNotification.xml";

    /// <summary>
    /// The key is the calling party number of the caller.
    /// </summary>
    private ConcurrentDictionary<string, CallAdditionalData> m_CallData = new ConcurrentDictionary<string, CallAdditionalData>();

    /// <summary>
    /// Returns true if there is any additional data available.
    /// </summary>
    public bool AdditionalDataAvailable { get; set; } = false;

    /// <summary>
    /// Constructor
    /// </summary>
    public AdditionalDataStore()
    {
    }

    /// <summary>
    /// Initializes the additional data store. This method must be called before attempting to retrieve
    /// location and additional data for a caller.
    /// Additional data is stored in the local application data directory for the current user. 
    /// For example: C:\Users\John\AppData\Local\OspSimulator\AdditionalData.
    /// If the local application data directory does not exist for the current user then this method creates
    /// it and copies the default data stored in the AdditionalData directory under the application's current working
    /// directory.
    /// </summary>
    public void Initialize()
    {
        string LocalAppDataDirectory = $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}";
        string AdditionalDataDirectory = $"{LocalAppDataDirectory}\\{Program.AppName}\\AdditionalData";
        if (Directory.Exists(AdditionalDataDirectory) == false)
        {
            try
            {
                CopyDirectory("AdditionalData", AdditionalDataDirectory, true);
                AdditionalDataAvailable = true;
            }
            catch (Exception copyEx)
            {
                SipLogger.LogError(copyEx, "Failed to copy the AdditionalData directory");
            }
        }
        else
            AdditionalDataAvailable = true;     // The additional data already exists

        if (AdditionalDataAvailable == false)
            return;

        DirectoryInfo dirInfo = new DirectoryInfo(AdditionalDataDirectory);
        DirectoryInfo[] dirs = dirInfo.GetDirectories();
        if (dirs == null || dirs.Length == 0)
        {
            AdditionalDataAvailable = false;
            return;
        }

        // The directory name is the calling party number and the directory contains XML files
        // for location and additional data.
        foreach (DirectoryInfo dir in dirs)
        {
            // Make sure that the calling party number is URI compatible
            if (dir.Name != SIPEscape.EscapeSpecialCharacters(dir.Name))
            {   // The calling party number contains invalid characters so skip this directory
                SipLogger.LogError($"Invalid AdditionalData subdirectory name: {dir.FullName}");
                continue;
            }

            FileInfo[] fileInfos = dir.GetFiles();
            CallAdditionalData callAdditionalData = new CallAdditionalData(dir.Name);
            foreach (FileInfo fileInfo in fileInfos)
            {
                string strFileText;
                try
                {
                    strFileText = File.ReadAllText(fileInfo.FullName);
                }
                catch (Exception ex)
                {
                    SipLogger.LogError(ex, $"Failed to read additional data text from file: {fileInfo.FullName}");
                    continue;
                }

                if (fileInfo.Name == LOCATION_FILE_NAME)
                {   // Process the file containing PIDF-LO location data
                    Presence? presence = XmlHelper.DeserializeFromString<Presence>(strFileText);
                    if (presence != null)
                        callAdditionalData.Location = presence;
                    else
                        SipLogger.LogError($"Failed to deserialize location file: {fileInfo.FullName}");
                }
                else if (fileInfo.Name.Contains(COMMENT_FILE_NAME_PATTERN) == true)
                {
                    CommentType? commentType = XmlHelper.DeserializeFromString<CommentType>(strFileText);
                    if (commentType != null)
                    {
                        string strCommentName = fileInfo.Name.Replace("." + fileInfo.Extension, "");
                        callAdditionalData.Comments.TryAdd(strCommentName, commentType);
                    }
                    else
                        SipLogger.LogError($"Failed to deserialize comment file: {fileInfo.FullName}");
                }
                else if (fileInfo.Name == DEVICE_INFO_FILE_NAME)
                {
                    DeviceInfoType? device = XmlHelper.DeserializeFromString<DeviceInfoType>(strFileText);
                    if (device != null)
                        callAdditionalData.DeviceInfo = device;
                    else
                        SipLogger.LogError($"Failed to deserialize DeviceInfo file: {fileInfo.FullName}");
                }
                else if (fileInfo.Name.Contains(PROVIDER_INFO_FILE_NAME_PATTERN) == true)
                {
                    ProviderInfoType? provider = XmlHelper.DeserializeFromString<ProviderInfoType>(strFileText);
                    if (provider != null)
                    {
                        string providerName = fileInfo.Name.Replace("." + fileInfo.Extension, "");
                        callAdditionalData.Providers.TryAdd(providerName, provider);
                    }
                    else
                        SipLogger.LogError($"Failed to deserialize ProviderInfo file: {fileInfo.FullName}");
                }
                else if (fileInfo.Name == SUBSCRIBER_INFO_FILE_NAME)
                {
                    SubscriberInfoType subscriber = XmlHelper.DeserializeFromString<SubscriberInfoType>(strFileText);
                    if (subscriber != null)
                        callAdditionalData.SubscriberInfo = subscriber;
                    else
                        SipLogger.LogError($"Failed to deserialize SubscriberInfo file: {fileInfo.FullName}");
                }
                else if (fileInfo.Name == SERVICE_INFO_FILE_NAME)
                {
                    ServiceInfoType? service = XmlHelper.DeserializeFromString<ServiceInfoType>(strFileText);
                    if (service != null)
                        callAdditionalData.ServiceInfo = service;
                    else
                        SipLogger.LogError($"Failed to deserialize ServiceInfo file: {fileInfo.FullName}");
                }
                else if (fileInfo.Name == AUTOMATED_CRASH_NOTIFICATION_FILE_NAME)
                {
                    AutomatedCrashNotificationType? Acn = XmlHelper.DeserializeFromString<AutomatedCrashNotificationType>(strFileText);
                    if (Acn != null)
                        callAdditionalData.AutomatedCrashNotification = Acn;
                    else
                        SipLogger.LogError($"Failed to deserialize AutomatedCrashNotification file: {fileInfo.FullName}");
                }
            } // end foreach

            m_CallData.TryAdd(dir.Name, callAdditionalData);
        } // end foreach

        if (m_CallData.Count == 0)
            AdditionalDataAvailable = false;
    }

    /// <summary>
    /// Gets a list of Calling Party Numbers
    /// </summary>
    /// <returns></returns>
    public List<string> GetCallingPartyNumbers()
    {
        if (AdditionalDataAvailable == false)
            return new List<string>();
        else
            return m_CallData.Keys.ToList();
    }

    /// <summary>
    /// Gets a CallAdditional data object given the caller's calling party number.
    /// </summary>
    /// <param name="callingPartyNumber">The caller's calling party number. This is the user part of
    /// the SIP URI of the From or P-Asserted-Identity header from the INVITE request.</param>
    /// <returns>Returns a CallAdditionalData object if there is location and additional data for the
    /// caller or null if no data is available.</returns>
    public CallAdditionalData? GetCallAdditionalData(string callingPartyNumber)
    {
        CallAdditionalData? callAdditionalData = null;
        bool Success = m_CallData.TryGetValue(callingPartyNumber, out callAdditionalData);
        return callAdditionalData;
    }

    // See: https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-copy-directories
    private void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
    {
        // Get information about the source directory
        DirectoryInfo dir = new DirectoryInfo(sourceDir);

        // Check if the source directory exists
        if (dir.Exists == false)
            throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

        // Cache directories before we start copying
        DirectoryInfo[] dirs = dir.GetDirectories();

        // Create the destination directory
        Directory.CreateDirectory(destinationDir);

        // Get the files in the source directory and copy to the destination directory
        foreach (FileInfo file in dir.GetFiles())
        {
            string targetFilePath = Path.Combine(destinationDir, file.Name);
            file.CopyTo(targetFilePath);
        }

        // If recursive and copying subdirectories, recursively call this method
        if (recursive)
        {
            foreach (DirectoryInfo subDir in dirs)
            {
                string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                CopyDirectory(subDir.FullName, newDestinationDir, true);
            }
        }
    }
}
