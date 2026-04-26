/////////////////////////////////////////////////////////////////////////////////////
//  File:   HelpUtils.cs                                            23 Apr 26 PHR
/////////////////////////////////////////////////////////////////////////////////////

using System.Diagnostics;

namespace OspSimulator;

internal class HelpUtils
{
    private const string HelpUriBase = "https://phrsite.github.io/OspSimulator/docs/";

    public static void ShowHelp(string strHelp)
    {
        ProcessStartInfo psi = new ProcessStartInfo(HelpUriBase + strHelp)
        {
            UseShellExecute = true
        };

        Process.Start(psi);
    }
}
