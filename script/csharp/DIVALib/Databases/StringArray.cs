using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using DIVALib.IO;
using DIVALib.FileBases;
using BinarySerialization;

namespace DIVALib.Databases
{
    // str_array.bin / string_array.bin
    public class StringArray : FileFormatBase
    {
        private readonly List<string> strings = new List<string>();

        public List<string> Strings => strings;

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

    public class OffsetString
    {
        [FieldOrder(0)] public int Offset;
        [FieldOrder(1), FieldOffset("Offset")] public string String;

        public override string ToString() => $"\"{String}\" at 0x{Offset:X}";

        public void Serialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
        {
            throw new NotImplementedException();
        }

        public void Deserialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
        {
            Offset = DataStream.ReadInt32(stream, endianness);
            var curPos = stream.Position;
            //stream.Position += -8;
            stream.Seek(0, SeekOrigin.Begin);
            Console.WriteLine(DataStream.ReadUInt32(stream));
            stream.Position = Offset;
            var encodingStr = serializationContext.MemberInfo.CustomAttributes.Where(attr => attr.AttributeType.Name == "FieldEncodingAttribute")?.First().ConstructorArguments[0].Value ?? "ASCII";
            String = DataStream.ReadCString(stream, Encoding.GetEncoding((string)encodingStr));
            stream.Position = curPos;
        }
    }

    public class StringArrayBinary
    {
        [FieldOrder(0), SerializeUntil(0), FieldEncoding("UTF-8")] public List<OffsetString> StringArray;
    }
}
