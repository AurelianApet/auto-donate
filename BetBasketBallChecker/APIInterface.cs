using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;

namespace BetBasketBallChecker
{
    class APIInterface
    {
        String m_baseUrl = "http://localhost:8181";
        String m_softname = "shutterfly";
        String m_restartFile = "restart.txt";
        public String  m_version = "0.0";

        public String m_errorResponse = "";
        protected Dictionary<string, object> configureData;
        public APIInterface()
        {
            StreamReader pReader = new StreamReader("./configure.json");
            string content = pReader.ReadToEnd();
            pReader.Close();
            JavaScriptSerializer ser = new JavaScriptSerializer();
            configureData = ser.Deserialize<Dictionary<string, object>>(content);
            m_baseUrl = configureData["backendURL"].ToString();
            m_version = configureData["version"].ToString();
            m_softname = configureData["softname"].ToString();
        }
        public String getVersion()
        {
            return m_version;
        }
        public bool checkUpdate()
        {
            WebClient webClient = new WebClient();
            string remoteVersionText = webClient.DownloadString(m_baseUrl + "/program-version/" + m_softname).Trim();
            string[] remoteVersionParts = remoteVersionText.Split('\n');
            string remoteUrl = remoteVersionParts[1];

            Version localVersion = new Version(m_version);
            Version remoteVersion = new Version(remoteVersionParts[0]);

            String remoteCount = remoteVersionParts[2];
            if (!File.Exists(m_restartFile))
            {
                var myFile = File.Create(m_restartFile);
                myFile.Close();
                File.WriteAllText(m_restartFile, remoteCount);
            }
            String localCount = File.ReadAllText(m_restartFile);
            
            File.WriteAllText(m_restartFile, remoteCount);

            if (remoteVersion > localVersion || localCount != remoteCount)
            {
                return true;
            }
            return false;
        }

        public List<Dictionary<string, string>> getDataList(int checker)
        {
            List<Dictionary<string, string>> ret = new List<Dictionary<string, string>>();
            var request = createNewRequest("/getDataList_ASDFQWER1234/" + checker.ToString());
            request.Method = "GET";
            using (WebResponse getResponse = request.GetResponse())
            {
                using (StreamReader sr = new StreamReader(getResponse.GetResponseStream()))
                {
                    var content = sr.ReadToEnd();
                    JArray datalist = JArray.Parse(content);
                    foreach (JObject o in datalist.Children<JObject>())
                    {
                        Dictionary<string, string> temp = new Dictionary<string, string>();
                        foreach (JProperty p in o.Properties())
                        {
                            string name = p.Name;
                            string value = (string)p.Value;
                            temp.Add(name, value);
                        }
                        ret.Add(temp);
                    }
                }
            }
            return ret;
        }

        public List<Dictionary<string, string>> getDataList2()
        {
            List<Dictionary<string, string>> ret = new List<Dictionary<string, string>>();
            var request = createNewRequest("/getDataList2_ASDFQWER1234");
            request.Method = "GET";
            using (WebResponse getResponse = request.GetResponse())
            {
                using (StreamReader sr = new StreamReader(getResponse.GetResponseStream()))
                {
                    var content = sr.ReadToEnd();
                    JArray datalist = JArray.Parse(content);
                    foreach (JObject o in datalist.Children<JObject>())
                    {
                        Dictionary<string, string> temp = new Dictionary<string, string>();
                        foreach (JProperty p in o.Properties())
                        {
                            string name = p.Name;
                            string value = (string)p.Value;
                            temp.Add(name, value);
                        }
                        ret.Add(temp);
                    }
                }
            }
            return ret;
        }

        public bool postCheckResult(List<Dictionary<string, string>> result)
        {
            bool ret = false;
            var request = createNewRequest("/postCheckResult_ASDFQWER1234");
            request.ContentType = "application/json;charset=UTF-8";
            request.Method = "POST";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                string json = encodeJSON(result);
                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();
            }
            using (WebResponse getResponse = request.GetResponse())
            {
                using (StreamReader sr = new StreamReader(getResponse.GetResponseStream()))
                {
                    var content = sr.ReadToEnd();
                    if(content == "ok")
                    {
                        ret = true;
                    }else
                    {
                        m_errorResponse = content;
                        ret = false;
                    }
                }
            }
            request.GetResponse().Dispose();
            return ret;
        }

        protected String encodeJSON(List<Dictionary<string, string>> result)
        {
            String ret = "";
            foreach (Dictionary<string, string> item in result)
            {
                ret += '{' + String.Format(" \"number\":\"{0}\", \"status\" :{1}, \"family\" :{2}, \"country\" :\"{3}\", \"cvv\" :\"{4}\" ", item["number"], item["status"], item["family"], item["country"], item["cvv"]) + "},";
            }
            ret = ret.Substring(0, ret.Length - 1);
            return "[" + ret + "]"; 
        }

        protected Dictionary<string, object>  parseResponse(String content)
        {
            JavaScriptSerializer ser = new JavaScriptSerializer();
            Dictionary<string, object> dict = ser.Deserialize<Dictionary<string, object>>(content);
            return dict;
        }
        protected HttpWebRequest createNewRequest(String url)
        {
            try
            {
                var request = WebRequest.Create(m_baseUrl+ url) as HttpWebRequest;
                return request;
            }
            catch (Exception e)
            {
            }
            return null;
        }
    }
}
