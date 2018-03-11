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
            var tst = new FileStream(@"C:\Users\waelw.WAELS-PC\Desktop\DIVAFILE\pvfield_tstthing.pfl", FileMode.Open);
            var serial = new BinarySerializer();
            var divaFile = new DivaFile(tst);
            using (var save = new FileStream(@"C:\Users\waelw.WAELS-PC\Desktop\DIVAFILE\pvfield_tstthing_divafile.pfl", FileMode.Create))
            {
                serial.Serialize(save, divaFile);
            }
            Console.WriteLine("oo");
        }

        private static void OnMemberDeserialized(object sender, MemberSerializedEventArgs e)
        {
            Console.CursorLeft = e.Context.Depth * 4;
            var value = e.Value ?? "null";
            Console.WriteLine("D-End: {0} ({1}) @ {2}", e.MemberName, value, e.Offset);
        }   
    }
}
