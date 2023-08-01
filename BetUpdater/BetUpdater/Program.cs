using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Web.Script.Serialization;
using System.Threading;

namespace BetUpdater
{
    class Program
    {
        static string remoteVersionURL = "/program-version";
        static Dictionary<string, object> configureData;
        static String m_baseUrl;
        static String m_version;
        static String m_softname;
        static String m_checkerExe = "BetBasketBallChecker.exe";

        static void readConfig()
        {
            StreamReader pReader = new StreamReader("./configure.json");
            string content = pReader.ReadToEnd();
            pReader.Close();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            configureData = ser.Deserialize<Dictionary<string, object>>(content);
            m_baseUrl = configureData["backendURL"].ToString();
            m_version = configureData["version"].ToString();
            m_softname = configureData["softname"].ToString();
            m_checkerExe = configureData["exename"].ToString();
        }

        static void Main(string[] args)
        {
            StartChecker();
            while (true)
            {
                WebClient webClient = new WebClient();
                readConfig();
                Console.WriteLine("Checking for updates...");
                // Format:
                //	<version> <url> <hash>
                string remoteVersionText = webClient.DownloadString(m_baseUrl + remoteVersionURL + "/" + m_softname).Trim();
                string[] remoteVersionParts = remoteVersionText.Split('\n');
                string remoteUrl = remoteVersionParts[1];
                Version localVersion = new Version(m_version);
                Version remoteVersion = new Version(remoteVersionParts[0]);

                if (remoteVersion > localVersion)
                {
                    while (!File.Exists("exit.log"));
                    File.Delete("exit.log");
                    // There is a new version on the server!
                    Console.WriteLine("There is a new version available on the server.");
                    Console.WriteLine("Current Version: {0}, New version: {1}", localVersion, remoteVersion);
                    PerformUpdate(remoteUrl);
                    StartChecker();
                }
                Thread.Sleep(5000);
            }
        }

        static void StartChecker()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(m_checkerExe);
            Process checkerProcess = new Process();
            checkerProcess.StartInfo = startInfo;
            checkerProcess.Start();
            checkerProcess.WaitForExit();
            Console.WriteLine("Checker Exited!");
        }

        static bool PerformUpdate(string remoteUrl)
        {
            Console.WriteLine("Beginning update.");
            string downloadDestination = Path.GetTempFileName();

            Console.Write("Downloading {0} to {1} - ", remoteUrl, downloadDestination);
            WebClient downloadifier = new WebClient();
            downloadifier.DownloadFile(remoteUrl, downloadDestination);
            Console.WriteLine("done.");

            Console.Write("Validating download - ");
            Console.WriteLine("ok.");

            // Since the download doesn't appear to be bad at first sight, let's extract it
            Console.Write("Extracting archive - ");
            string extractTarget = @"./downloadedFiles";
            ZipFile.ExtractToDirectory(downloadDestination, extractTarget);
            // Copy the extracted files and replace everything in the current directory to finish the update
            // C# doesn't easily let us extract & replace at the same time
            foreach (string newPath in Directory.GetFiles(extractTarget, "*.*", SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(extractTarget, "."), true);
            Console.WriteLine("done.");

            // Clean up the temporary files
            Console.Write("Cleaning up - ");
            Directory.Delete(extractTarget, true);
            Console.WriteLine("done.");

            return true;
        }

        /// <summary>
        /// Gets the SHA1 hash from file.
        /// Adapted from https://stackoverflow.com/a/16318156/1460422
        /// </summary>
        /// <param name="fileName">The filename to hash.</param>
        /// <returns>The SHA1 hash from file.</returns>
        static string GetSHA1HashFromFile(string fileName)
        {
            FileStream file = new FileStream(fileName, FileMode.Open);
            SHA1 sha1 = new SHA1CryptoServiceProvider();
            byte[] byteHash = sha1.ComputeHash(file);
            file.Close();

            StringBuilder hashString = new StringBuilder();
            for (int i = 0; i < byteHash.Length; i++)
                hashString.Append(byteHash[i].ToString("x2"));
            return hashString.ToString();
        }
    }
}
