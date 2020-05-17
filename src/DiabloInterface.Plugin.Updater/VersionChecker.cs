using System;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Zutatensuppe.DiabloInterface.Core.Logging;

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
        private readonly ILogger Logger = LogServiceLocator.Get(MethodBase.GetCurrentMethod().DeclaringType);

        const string ReleasesUrl = "https://github.com/Zutatensuppe/DiabloInterface/releases";
        const string ReleasesLatestUrl = "https://github.com/Zutatensuppe/DiabloInterface/releases/latest";

        internal VersionCheckerResult CheckForUpdate(string lastFoundVersion, bool userTriggered)
        {
            string updateUrl = GetUpdateUrl();

            var r = new VersionCheckerResult();
            if (updateUrl != null)
            {
                if (updateUrl != lastFoundVersion || userTriggered)
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

        string GetUpdateUrl()
        {
            Match verMatch = Regex.Match(Application.ProductVersion, @"^(\d+)\.(\d+)\.(\d+)(?:\.PR\.(\d+))?$");
            if (!verMatch.Success)
                return null;

            string location;

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                       | SecurityProtocolType.Tls11
                       | SecurityProtocolType.Tls12
                       | SecurityProtocolType.Ssl3;

                HttpWebRequest r = (HttpWebRequest)WebRequest.Create(ReleasesLatestUrl);
                r.Method = WebRequestMethods.Http.Head;
                r.AllowAutoRedirect = false;

                using (var response = r.GetResponse() as HttpWebResponse)
                {
                    location = response.GetResponseHeader("Location");
                }
            }
            catch (WebException e)
            {
                Logger.Error("Failed to retrieve latest release from Url", e);
                return null;
            }

            Match tagMatch = Regex.Match(location, @"/releases/tag/v(\d+)\.(\d+)\.(\d+)$");
            if (!tagMatch.Success)
                return null;

            // version compare.
            int major = Convert.ToInt32(verMatch.Groups[1].Value);
            int majorNew = Convert.ToInt32(tagMatch.Groups[1].Value);
            if (majorNew > major)
                return location;

            if (majorNew < major)
                return null;

            int minor = Convert.ToInt32(verMatch.Groups[2].Value);
            int minorNew = Convert.ToInt32(tagMatch.Groups[2].Value);
            if (minorNew > minor)
                return location;

            if (minorNew < minor)
                return null;

            int patch = Convert.ToInt32(verMatch.Groups[3].Value);
            int patchNew = Convert.ToInt32(tagMatch.Groups[3].Value);
            if (patchNew > patch)
                return location;

            if (patchNew < patch)
                return null;

            try
            {
                int pre = Convert.ToInt32(verMatch.Groups[4].Value);
                if (pre > 0)
                    return location;
            }
            catch
            {
            }

            return null;
        }
    }
}
