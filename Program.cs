using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json.Linq;
using System.Windows.Forms;

namespace ToPasteBin
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            var api = new PastebinAPI();

            if (args.Length > 0)
            {
                string path = args[0];

                string title = path.Split('\\')[path.Split('\\').Length - 1];
                string content = File.ReadAllText(path);
                string link = api.CreatePaste(title, content);
                Clipboard.SetText(link);
                File.WriteAllText(path.Replace(title,"") + "\\link.txt", link);
            }
        }
    }
}