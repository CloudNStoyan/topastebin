using System.Xml;
using Newtonsoft.Json;

namespace ToPasteBin
{
    public static class Utility
    {
        public static string XmlToJson(string xml)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);
            return JsonConvert.SerializeXmlNode(xmlDocument);
        }
    }
}
