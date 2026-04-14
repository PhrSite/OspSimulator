/////////////////////////////////////////////////////////////////////////////////////
//  File:   CertificateSettings.cs                                  6 Feb 24 PHR
/////////////////////////////////////////////////////////////////////////////////////

using SipLib.Logging;
using System.Security.Cryptography.X509Certificates;

namespace PsapSimulator.Settings;

/// <summary>
/// Class for storing the X.509 certificate settings for the application
/// </summary>
public class CertificateSettings
{
    private const string DefaultCertificateFile = @".\OspSimulator.pfx";
    private const string DefaultCertificatePassword = "OspSimulator";

    /// <summary>
    /// If true, then the default X.509 certificate will be used, else the certificate file specified
    /// in the CertificateFilePath property will be used and the CertificatePassword property specifies
    /// the password for the certificate.
    /// </summary>
    public bool UseDefaultCertificate { get; set; } = true;

    /// <summary>
    /// Specifies the path of the X.509 certificate PFX file. Will be used only if UseDefaultCertificate
    /// is false.
    /// </summary>
    public string CertificateFilePath { get; set; } = DefaultCertificateFile;

    /// <summary>
    /// Password for the certificate specified by the CertificateFilePath setting. Used only if
    /// UseDefaultCertificate is false.
    /// </summary>
    public string CertificatePassword { get; set; } = DefaultCertificatePassword;

    /// <summary>
    /// Gets the X.509 certificate.
    /// </summary>
    /// <returns>Returns either the default certificate or the certificate specified by the user's settings.
    /// Returns null if the certificate cannot be found or if an error occurs.</returns>
    public X509Certificate2? GetCertificateFromFile()
    {
        X509Certificate2? certificate = null;
        string filePath;
        string password;

        if (UseDefaultCertificate == true)
        {
            filePath = DefaultCertificateFile;
            password = DefaultCertificatePassword;
        }
        else
        {
            filePath = CertificateFilePath;
            password = CertificatePassword;
        }

        try
        {
            certificate = X509CertificateLoader.LoadPkcs12FromFile(filePath, password);
        }
        catch (Exception ex)
        {
            SipLogger.LogError(ex, $"Failed to read the X.509 certificate from {filePath}");
            certificate = null;
        }

        return certificate;
    }

    /// <summary>
    /// Constructor
    /// </summary>
    public CertificateSettings()
    {
    }
}
