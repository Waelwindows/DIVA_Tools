using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using DIVALib.IO;
using DIVALib.FileBases;

namespace DIVALib.Databases
{
    // str_array.bin / string_array.bin
    public class StringArray : FileFormatBase
    {
        private readonly List<string> strings = new List<string>();

        public List<string> Strings
        {
            get
            {
                return strings;
            }
        }

        public override void Read(Stream source)
        {
            uint position;

            while ((position = DataStream.ReadUInt32(source)) != 0)
            {
                strings.Add(StringPool.Read(source, position));
            }
        }

        public override void Write(Stream destination)
        {
            var stringPool = new StringPool();
            stringPool.StringAlignment = 0x2;

            foreach (string value in strings)
            {
                stringPool.Add(destination, value);
            }

            DataStream.Pad(destination, 0x10);
            stringPool.Write(destination);
        }
    }
}
