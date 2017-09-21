using System;
using System.IO;
using BinarySerialization;
using DscOp;

namespace FDSC
{
    class Program
    {
        static void Main(string[] args)
        {
            /* */
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
            Console.Write("Conversion Complete!\n");
        }

        public static void DscConvert(string path)
        {
            FileStream file = new FileStream(path, FileMode.Open);
            Console.Write("Beginning DSC Deserialization\n");
            DscFile dsc = DscFile.BinDeserialize(file, true);
			Console.Clear();
			Console.Write("DSC Deserialization Successful\n");
            string spath = path.Substring(0, path.Length - 3) + "xml";
            FileStream save = new FileStream(spath, FileMode.Create);
            Console.Write("Beginning XML Serialization\n");
            dsc.XmlSerialize(save, true);
            Console.Write("XML Serialization Successful\n");
            return;
        }

        public static void XmlConvert(string path)
        {
			FileStream file = new FileStream(path, FileMode.Open);
            Console.Write("Beginning XML Deserialization\n");
            DscFile dsc = DscFile.XmlDeserialize(file, true);
            Console.Clear();
            Console.Write("XML Deserialization Successful\n");
			string spath = path.Substring(0, path.Length - 3) + "dsc";
			FileStream save = new FileStream(spath, FileMode.Create);
            Console.Write("Beginning DSC Serialization\n");
			dsc.BinSerialize(save, true);
            Console.Write("DSC Serialization Successful\n");
			return;
        }
    }
}
