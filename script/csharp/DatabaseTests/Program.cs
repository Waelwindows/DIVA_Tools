using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using BinarySerialization;
using DIVALib.Crypto;
using DIVALib.Databases;
using DIVALib.IO;

namespace DatabaseTests
{

    class Program
    {
        static void Main(string[] args)
        {
            using (var file = File.Open(@"D:\QuickBMS\f2_cam\camdata_binary.bin", FileMode.Open))
            {
                
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
