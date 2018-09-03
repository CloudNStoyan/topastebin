using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ToPasteBin
{
    public class PastebinAPI
    {
        private string Key { get; }
        private string UserId { get; set; }

        public PastebinAPI(string key,string username,string password)
        {
            this.Key = key;
            this.UserId = this.GetUserId(username, password);
        }

        public PastebinAPI()
        {
            string json = File.ReadAllText("../../apisettings.json");
            var config = JsonConvert.DeserializeObject<Config>(json);
            this.Key = config.Key;
            this.UserId = this.GetUserId(config.Username, config.Password);
        }

        public Paste[] GetPastes()
        {
            var dataDic = new Dictionary<string, string>()
            {
                {"api_dev_key", this.Key},
                {"api_user_key", this.UserId},
                {"api_result_limit", "100"},
                {"api_option", "list"}
            };
            string xmlDocText = "<root>" + SendRequest("https://pastebin.com/api/api_post.php", dataDic) + "</root>";

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlDocText);

            var pastes = new List<Paste>();
            for (int i = 0; i < xmlDocument.ChildNodes[0].ChildNodes.Count; i++)
            {
                string xmlNodeText = xmlDocument.ChildNodes[0].ChildNodes[i].OuterXml;
                string jsonText = Utility.XmlToJson(xmlNodeText);
                var json = JObject.Parse(jsonText)["paste"];
                var paste = JsonConvert.DeserializeObject<Paste>(json.ToString());

                pastes.Add(paste);
            }


            return pastes.ToArray();
        }

        public string GetRawPaste(Paste paste)
        {
            string id = "/" + paste.Key;
            string response = SendRequest("https://pastebin.com/raw" + id, new Dictionary<string, string>());
            return response;
        }

        public string GetRawPaste(string pasteUrl)
        {
            var url = new Uri(pasteUrl);
            string id = url.AbsolutePath;
            string response = SendRequest("https://pastebin.com/raw" + id, new Dictionary<string, string>());
            return response;
        }

        public string CreatePaste(string title, string content)
        {
            var dataDic = new Dictionary<string, string>()
            {
                {"api_dev_key", this.Key},
                {"api_user_key", this.UserId},
                {"api_option", "paste"},
                {"api_paste_code", content},
                {"api_paste_name", title},
                {"api_paste_private", "1"}
            };

            return SendRequest("https://pastebin.com/api/api_post.php", dataDic);
        }



        public string GetPasteContent(Paste paste)
        {
            var dataDic = new Dictionary<string, string>()
            {
                {"api_dev_key", this.Key},
                {"api_user_key", this.UserId},
                {"api_paste_key", paste.Key},
                {"api_option", "show_paste"}
            };

            string response = SendRequest("https://pastebin.com/api/api_raw.php", dataDic);

            Console.WriteLine(response);

            return "";
        }

        private string GetUserId(string username,string password)
        {
            
            var dataDic = new Dictionary<string,string>()
            {
                {"api_dev_key", this.Key},
                {"api_user_name", username},
                {"api_user_password", password}
            };
            return SendRequest("https://pastebin.com/api/api_login.php", dataDic);
        }

        private static string SendRequest(string url,Dictionary<string,string> dataDic)
        {
            string webAddr = url;

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.Method = "POST";

            string postData = string.Empty;
            foreach (var data in dataDic)
            {
                postData += data.Key + "=" + data.Value + "&";
            }


            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(postData);
                streamWriter.Flush();
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var responseText = streamReader.ReadToEnd();
                return responseText;
                //Now you have your response.
                //or false depending on information in the response     
            }
        }
    }
}
