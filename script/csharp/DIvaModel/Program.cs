using System;
using System.IO;
using BinarySerialization;

namespace DIvaModel
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var file = File.Open(@"D:\QuickBMS\mik\miki\mikitm001_obj.bin", FileMode.Open))
            {
                var serial = new BinarySerializer();
                var model = serial.Deserialize<BinModelHeader>(file);
                Console.WriteLine("tst");
            }
        }
    }
}
