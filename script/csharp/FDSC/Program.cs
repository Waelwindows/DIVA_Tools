using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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

        static async Task Main(string[] args)
		{
#if DEBUG
            //using (var file = File.Open(@"D:\Emulators\RPSC3\dev_hdd0\game\NPJB00134\USRDIR\rom\script\pv_007_extreme.dsc", FileMode.Open))
            //using (var file = File.Open(@"D:\Emulators\RPSC3\dev_hdd0\disc\BLJM60527\PS3_GAME\USRDIR\rom\script\pv600\pv_600_extreme.dsc", FileMode.Open))
		    var path = @"D:\dsc\pv_611_extreme.dsc";
		    path = @"D:\DIVATools\test_files\dsc\pv_717_normal.dsc";
            using (var file = File.Open(path, FileMode.Open))
		    {
		        var serial = new BinarySerializer();
		        //serial.Endianness = Endianness.Big;
		        serial.MemberDeserialized += OnMemberDeserialized;
                //file.Position = 68;
		        var dsc = await serial.DeserializeAsync<DscFile2>(file);

                
		        var notesWrapper = dsc.Functions.Where(func => func.Function.GetType() == typeof(FTarget)).ToList();
		        var indices = notesWrapper.Select(note => dsc.Functions.FindIndex(elem => elem == note)).ToList();
		        var timesWrapper = indices.Select(i => dsc.Functions[i - 1]).ToList();

		        var times = timesWrapper.Select(wrapper => wrapper.Function).Where(time => time.GetType() == typeof(FTime)).ToList();
		        var notes = notesWrapper.Select(wrapper => wrapper.Function).ToList();

                /*
		        using (var save = new StreamWriter(Path.ChangeExtension(path, ".xml")))
		        {
		            save.WriteLine(@",timestamp,type,holdLength,bIsHoldEnd,posX,posY,entryAngle,oscillationFrequency,oscillationAngle,oscillationAmplitude,timeout");
		            for (var i = 0; i < times.Count; ++i)
		            {
		                var time = (FTime)times[i];
		                var note = (FTarget) notes[i];
		                save.WriteLine($"{i},{(time.TimeStamp > 0 ? time.TimeStamp/1000 : -1)},{note.Type},{note.HoldLength},{note.IsHoldEnd},{note.Position.x/10000},{note.Position.y / 10000}," +
		                               $"{note.EntryAngle},{note.OscillationFrequency},{note.OscillationAngle},{note.OscillationAmplitude},{note.TimeOut/1000}");
		            }
		        }
                */
                return;
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
			    var serial = new BinarySerializer();
			    var dsc = serial.Deserialize<DSC>(file);
				var savePath = Path.ChangeExtension(path, "xml");
				using (var save = File.Create(savePath))
				{
					dsc.File.XmlSerialize(save);
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