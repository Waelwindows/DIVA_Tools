using System;
using System.IO;
using DscOp;

namespace FDSC
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                throw new ArgumentException("Needs directory");
            }
            string ext = args[0].Substring(args[0].Length - 3, 3);
            switch(ext)
            {
                case "dsc": DscConvert(args[0]); break;
                case "xml": XmlConvert(args[0]); break;
            }
        }

        public static void DscConvert(string path)
        {
            FileStream file = new FileStream(path, FileMode.Open);
            DscFile dsc = new DscFile(file);
            file.Close();
            string spath = path.Substring(0, path.Length - 3) + "xml";
            FileStream save = new FileStream(spath, FileMode.Create);
            dsc.Serialize(save);
            return;
        }

        public static void XmlConvert(string path)
        {
			FileStream file = new FileStream(path, FileMode.Open);
            DscFile dsc = DscFile.Deserialize(file);
			string spath = path.Substring(0, path.Length - 3) + "dsc";
			FileStream save = new FileStream(spath, FileMode.Create);
			dsc.Save(save);
			save.Close();
			return;
        }
    }
}
