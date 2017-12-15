using System;
using System.Globalization;
using System.IO;
using BinarySerialization;
using DIVALib.IO;

namespace A3DA2XML
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            const string path = @"D:\QuickBMS\dt_cam\CAMPV001_PARTS02.a3da";
            using (var file = new FileStream(path, FileMode.Open))
            {
                var serializer = new BinarySerializer();
                //var a3DaFile = serializer.Deserialize<A3DaFile>(file);
                //var a3DaHeader = serializer.Deserialize<A3DaHeader>(file);
                var a3DaHeader = new A3DaHeader();
                a3DaHeader.Deserialize(file);
                Console.WriteLine(a3DaHeader);
                

                Console.ReadLine();
            }
        }
    }
}
