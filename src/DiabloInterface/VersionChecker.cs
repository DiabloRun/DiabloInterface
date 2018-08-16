using Zutatensuppe.DiabloInterface.Core.Logging;
using System;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Zutatensuppe.DiabloInterface
{
    internal class VersionChecker
    {
        const string ReleasesUrl = "https://github.com/Zutatensuppe/DiabloInterface/releases";
        const string ReleasesLatestUrl = "https://github.com/Zutatensuppe/DiabloInterface/releases/latest";

        public static void ManuallyCheckForUpdate()
        {
            CheckForUpdate(true);
        }

        public static void AutomaticallyCheckForUpdate()
        {
            CheckForUpdate(false);
        }

        private static void CheckForUpdate(bool userTriggered)
        {
            string updateUrl = GetUpdateUrl();

            if (updateUrl != null)
            {
                string lastFoundVersion = Properties.Settings.Default.LastFoundVersion;
                if (updateUrl != lastFoundVersion || userTriggered)
                {
                    Properties.Settings.Default.LastFoundVersion = updateUrl;
                    Properties.Settings.Default.Save();

                    if (MessageBox.Show(@"A new version of DiabloInterface is available. Go to download page now?", @"New version available",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(updateUrl);
                    }
                }

            }
            else if (userTriggered)
            {
                if (MessageBox.Show(@"No new version is available, but there might be a pre-release. Go to releases overview now?", @"No new version available",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(ReleasesUrl);
                }
            }
        }

        static string GetUpdateUrl()
        {
            Match verMatch = Regex.Match(Application.ProductVersion, @"^(\d+)\.(\d+)\.(\d+)(?:\.PR\.(\d+))?$");
            if (!verMatch.Success)
            {
                return null;
            }

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
                LogServiceLocator.Get(typeof(VersionChecker)).Error("Failed to retrieve latest release from Url", e);
                return null;
            }

            Match tagMatch = Regex.Match(location, @"/releases/tag/v(\d+)\.(\d+)\.(\d+)$");
            if (!tagMatch.Success)
            {
                return null;
            }

            // version compare.

            int major = Convert.ToInt32(verMatch.Groups[1].Value);
            int majorNew = Convert.ToInt32(tagMatch.Groups[1].Value);
            if (majorNew > major)
            {
                return location;
            }

            if (majorNew < major)
            {
                return null;
            }

            int minor = Convert.ToInt32(verMatch.Groups[2].Value);
            int minorNew = Convert.ToInt32(tagMatch.Groups[2].Value);
            if (minorNew > minor)
            {
                return location;
            }

            if (minorNew < minor)
            {
                return null;
            }

            int patch = Convert.ToInt32(verMatch.Groups[3].Value);
            int patchNew = Convert.ToInt32(tagMatch.Groups[3].Value);
            if (patchNew > patch)
            {
                return location;
            }

            if (patchNew < patch)
            {
                return null;
            }

            try
            {
                int pre = Convert.ToInt32(verMatch.Groups[4].Value);
                if (pre > 0)
                {
                    return location;
                }
            }
            catch
            {
            }

            return null;
        }
    }
}
