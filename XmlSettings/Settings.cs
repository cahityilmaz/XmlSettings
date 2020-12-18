using System;
using System.IO;
using System.Text;
using System.Xml;

namespace XmlSettings {
    public class Settings {

        private static readonly string fileName = "settings.xml";
        private static readonly string filePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}\{System.Windows.Forms.Application.ProductName}\";
        private static readonly string settingFile = filePath + fileName;

        public static string Printer1 { get; set; }
        public static string Printer2 { get; set; }
        public static string DesignName { get; set; }
        public static bool Preview { get; set; }

        public static void GetSettings() {
            if (File.Exists(settingFile)) {
                var xDoc = new XmlDocument();
                xDoc.Load(settingFile);
                var xRootElement = xDoc.DocumentElement;
                var xNodeList = xRootElement.ChildNodes;
                var propArray = typeof(Settings).GetProperties();
                for (int i = 0; i < xNodeList.Count; i++) {
                    var xNode = xNodeList[i];
                    foreach (var prop in propArray) {
                        if (prop.Name.Equals(xNode.Name)) {
                            prop.SetValue(typeof(Settings), Convert.ChangeType(xNode.InnerText, prop.PropertyType), null);
                        }
                    }
                }
            }
            else {
                System.Diagnostics.Debug.WriteLine("Settings file not found!");
            }
        }

        private static string getOldSettingValue(string elementName) {
            var value = string.Empty;
            var xDoc = new XmlDocument();
            xDoc.Load(settingFile);
            var xRootElement = xDoc.DocumentElement;            
            var xElement = xRootElement[elementName];
            if (xElement != null) {
                value = xElement.InnerText;
            }
            return value;
        }

        private static bool IsNodeExists(string elementName) {
            var xDoc = new XmlDocument();
            xDoc.Load(settingFile);
            var xRootElement = xDoc.DocumentElement;
            var xElement = xRootElement[elementName];
            return xElement != null;
        }

        public static void SaveSettings() {
            var xWSettings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                ConformanceLevel = ConformanceLevel.Auto,
                Indent = true
            };
            var propArray = typeof(Settings).GetProperties();
            if (!Directory.Exists(filePath)) {
                Directory.CreateDirectory(filePath);
            }
            if (File.Exists(settingFile)) {
                var xDoc = new XmlDocument();
                xDoc.Load(settingFile);
                var xRootElement = xDoc.DocumentElement;
                foreach (var item in propArray) {
                    var name = item.Name;
                    if (IsNodeExists(name)) {
                        xRootElement[name].InnerText = Convert.ToString(item.GetValue(typeof(Settings), null));
                    }
                    else {
                        var xNode = xDoc.CreateNode(XmlNodeType.Element, item.Name, "");
                        xNode.InnerText = Convert.ToString(item.GetValue(typeof(Settings), null));
                        xRootElement.AppendChild(xNode);
                    }
                }
                xDoc.Save(settingFile);
            }
            else {
                using (var xWriter = XmlWriter.Create(settingFile, xWSettings)) {
                    xWriter.WriteStartDocument();
                    xWriter.WriteStartElement("Settings");
                    foreach (var item in propArray) {
                        xWriter.WriteElementString(item.Name, Convert.ToString(item.GetValue(typeof(Settings), null)));
                    }
                    xWriter.WriteEndElement();
                    xWriter.WriteEndDocument();
                }
            }
            System.Diagnostics.Debug.WriteLine("Settings saved!");
        }
    }
}
