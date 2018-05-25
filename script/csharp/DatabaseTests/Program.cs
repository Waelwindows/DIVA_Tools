using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BinarySerialization;
using DIVALib.Archives;
using DIVALib.Databases;

namespace DatabaseTests
{

    public class TestClass
    {
        [FieldScale(100), SerializeAs(SerializedType.Int4)] public double Test;
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            var serial = new BinarySerializer();
            serial.Endianness = Endianness.Big;
            using (var file = File.Open(@"C:\Users\waelw.WAELS-PC\Desktop\farc\rslt_mik.farc", FileMode.Open))
            {
                var archive = serial.Deserialize<FarcArchiveBin>(file);
                return;
            }
        }

        private static void OnMemberDeserialized(object sender, MemberSerializedEventArgs e)
        {
            Console.CursorLeft = e.Context.Depth * 4;
            var value = e.Value ?? "null";
            Console.WriteLine("D-End: {0} ({1}) @ {2}", e.MemberName, value, e.Offset);
        }   
    }
}
