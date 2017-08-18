using System;
using System.IO;
using System.Xml;
using DIVALib.IO;
using A3daFunc;

namespace A3DA2XML
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            string filePath = "//Users//waelwindows//Documents//farc//effstgpv629//STGPV629S01_EFF_LT_000.a3da";
            string savePath = "//Users//waelwindows//Documents//farc//effstgpv629//STGPV629S01_EFF_LT_000.xml";
            FileStream file = new FileStream(filePath, FileMode.Open);
            FileStream save = new FileStream(savePath, FileMode.Create);
            A3daFile a3da = new A3daFile(file);
            XmlDocument doc = new XmlDocument();
            a3da.ToXml(doc);
            doc.Save(save);
        }
    }
}
