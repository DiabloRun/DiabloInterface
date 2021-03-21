using System;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Zutatensuppe.DiabloInterface.Lib;

[assembly: InternalsVisibleTo("DiabloInterface.Plugin.Updater.Test")]
namespace Zutatensuppe.DiabloInterface.Plugin.Updater
{
    internal class VersionCheckerResult
    {
        internal string updateUrl;
        internal string question;
        internal string title;
        internal string target;
    }

    internal class VersionChecker
    {
        static readonly ILogger Logger = Logging.CreateLogger(MethodBase.GetCurrentMethod().DeclaringType);

        const string ReleasesUrl = "https://github.com/DiabloRun/DiabloInterface/releases";
        const string ReleasesLatestUrl = ReleasesUrl + "/latest";

        internal VersionCheckerResult CheckForUpdate(
            string currentVersion,
            string lastFoundVersionUrl,
            bool userTriggered
        ) {
            string updateUrl = GetUpdateUrl(currentVersion);

            var r = new VersionCheckerResult();
            if (updateUrl != null)
            {
                if (updateUrl != lastFoundVersionUrl || userTriggered)
                {
                    r.updateUrl = updateUrl;
                    r.question = @"A new version of DiabloInterface is available. Go to download page now?";
                    r.title = @"New version available";
                    r.target = updateUrl;
                }
            } else if (userTriggered)
            {
                r.question = @"No new version is available, but there might be a pre-release. Go to releases overview now?";
                r.title = @"No new version available";
                r.target = ReleasesUrl;
            }
            return r;
        }

        internal static Version TryParseVersionString(string str, string regexPrefix = "")
        {
            if (str == null)
                return null;

            string regex = @"(.*)";
            Match m = Regex.Match(str, "^" + regexPrefix + regex + "$");

            if (!m.Success)
                return null;

            string v = m.Groups[m.Groups.Count - 1].Value;
            try { return Version.Parse(v); }
            catch { return null; }
        }

        string GetUpdateUrl(string currentVersion)
        {
            Version curr = TryParseVersionString(currentVersion);
            if (curr == null)
                return null;

            string latestVersionUrl = DetermineFinalLocation(ReleasesLatestUrl);
            if (latestVersionUrl == null)
                return null;

            Version next = TryParseVersionString(latestVersionUrl, @".*/releases/tag/v");
            if (next == null || next <= curr)
                return null;

            return latestVersionUrl;
        }

        string DetermineFinalLocation(string url)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = (
                    SecurityProtocolType.Tls
                    | SecurityProtocolType.Tls11
                    | SecurityProtocolType.Tls12
                    | SecurityProtocolType.Ssl3
                );

                HttpWebRequest r = (HttpWebRequest)WebRequest.Create(url);
                r.Method = WebRequestMethods.Http.Head;
                r.AllowAutoRedirect = false;

                using (var response = r.GetResponse() as HttpWebResponse)
                {
                    return response.GetResponseHeader("Location");
                }
            }
            catch (WebException e)
            {
                Logger.Error("Failed to retrieve final location from Url", e);
                return null;
            }
        }
    }
}
