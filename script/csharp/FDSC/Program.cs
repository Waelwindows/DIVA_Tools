using System;
using System.IO;
using BinarySerialization;
using DIVALib.DSCUtils;

namespace FDSC
{

	class Program
	{
	    private static void OnMemberDeserialized(object sender, MemberSerializedEventArgs e)
	    {
	        Console.CursorLeft = e.Context.Depth * 4;
	        var value = e.Value ?? "null";
	        Console.WriteLine("D-End: {0} ({1}) @ {2}", e.MemberName, value, e.Offset);
	    }

        static void Main(string[] args)
		{
#if DEBUG
		    using (var file = new FileStream(@"D:\DIVATools\test_files\dsc\pv_609_extreme.dsc", FileMode.Open))
		    {
		        var serial = new BinarySerializer();
		        //var dsc = serial.Deserialize<DSC>(file);
		        //serial.MemberDeserialized += OnMemberDeserialized;
                //file.Position += 4;
		        var fst = serial.Deserialize<DscFile1>(file);
                Console.WriteLine("tst");
		    }
		    return;
#endif
            if (args.Length < 1)
            {
                Console.WriteLine("FDSC Tool");
                Console.WriteLine("=========");
                Console.WriteLine("Converter for all Project Diva DSC(Diva Script Container) files.\n");
                Console.WriteLine("Currently Supports:");
                Console.WriteLine("	- All DT games");
                Console.WriteLine("	- F");
                Console.WriteLine("	- F2nd\n");
                Console.WriteLine("Usage:");
                Console.WriteLine("	Drag'n'Drop the file onto the application\n");
                Console.WriteLine("	or\n");
                Console.WriteLine("	Run from command line:");
                Console.WriteLine("	");
                Console.WriteLine("	FDSC [source] ");
                Console.WriteLine("	Source is a URI to your dsc file");
                Console.ReadLine();
            }

            if (!File.Exists(args[0]))
				throw new IOException("file doesn't exist.");

			if (File.GetAttributes(args[0]).HasFlag(FileAttributes.Directory))
				throw new NotImplementedException("FDSC doesn't support batch conversion currently.");

			switch (Path.GetExtension(args[0]))
			{
				case ".dsc": DscConvert(args[0]); break;
				case ".xml": XmlConvert(args[0]); break;
			}

			Console.Write("Conversion Complete!\n");
			Console.ReadLine();
		}

		public static void DscConvert(string path)
		{
			using (var file = new FileStream(path, FileMode.Open))
			{
				Console.Write("Beginning DSC Deserialization\n");
			    var serial = new BinarySerializer();
			    var dsc = serial.Deserialize<DSC>(file);
				Console.Clear();
				Console.WriteLine($"DSC has {dsc.File.Functions.Count} functions.");
				Console.Write("DSC Deserialization Successful\n");
				Console.Write($"DSC Type {dsc.File.GetType()}\n");
				var savePath = Path.ChangeExtension(path, "xml");
				using (var save = new FileStream(savePath, FileMode.Create))
				{
					Console.Write("Beginning XML Serialization\n");
					dsc.File.XmlSerialize(save);
					Console.Write("XML Serialization Successful\n");
				}
			}
		}

		public static void XmlConvert(string path)
		{
			using (var file = new FileStream(path, FileMode.Open))
			{
				Console.Write("Beginning XML Deserialization\n");
				var dsc = new DSC();
				dsc.XmlDeserialize(file);
				Console.Clear();
				Console.WriteLine($"DSC has {dsc.File.Functions.Count} functions.");
				Console.Write("XML Deserialization Successful\n");
				var savePath = Path.ChangeExtension(path, "dsc");
				using (var save = new FileStream(savePath, FileMode.Create))
				{
					Console.Write("Beginning DSC Serialization\n");
					dsc.File.BinSerialize(save);
					Console.Write("DSC Serialization Successful\n");
				}
			}
		}
	}
}