using System;
using System.IO;
using DscOp;
using BinarySerialization;

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
			switch (ext)
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
            var dsc = DSC.Deserialize(file);
			Console.Clear();
			Console.Write("DSC Deserialization Successful\n");
            Console.Write("DSC Type {0}\n", dsc.file.GetType());
			string spath = path.Substring(0, path.Length - 3) + "xml";
			FileStream save = new FileStream(spath, FileMode.Create);
			Console.Write("Beginning XML Serialization\n");
            dsc.file.XmlSerialize(save);
			Console.Write("XML Serialization Successful\n");
			return;
		}

		public static void XmlConvert(string path)
		{
			FileStream file = new FileStream(path, FileMode.Open);
			Console.Write("Beginning XML Deserialization\n");
            //var dsc = .XmlDeserialize(file);
            var dsc = new DSC();
			Console.Clear();
			Console.Write("XML Deserialization Successful\n");
			string spath = path.Substring(0, path.Length - 3) + "dsc";
			FileStream save = new FileStream(spath, FileMode.Create);
			Console.Write("Beginning DSC Serialization\n");
            dsc.file.BinSerialize(save, true);
			Console.Write("DSC Serialization Successful\n");
			return;
		}
	}
}