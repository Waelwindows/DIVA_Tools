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

    public class F2StringArrayHeader
    {
        [FieldOrder(0), FieldLength(4)] public string Magic = "STRA";
        [FieldOrder(1)] public int Bytesize;
        [FieldOrder(2)] public int Size = 64;
        [FieldOrder(3)] public int Version = 0x4000_0000;
        [FieldOrder(4), FieldAlignment(20, FieldAlignmentMode.LeftOnly)
            ,FieldAlignment("Size", FieldAlignmentMode.RightOnly)] public int SubfileBytesize;
    }

    public class F2StringArray
    {
        [FieldOrder(0), FieldEndianness(Endianness.Little)] public F2StringArrayHeader Header;
        [FieldOrder(1)] public int Count;
        [FieldOrder(2), FieldAlignment(16, FieldAlignmentMode.RightOnly)] public int DataOffset;
        [FieldOrder(3), FieldCount("Count")
        ,FieldAlignment(16, FieldAlignmentMode.RightOnly)
        ,FieldAlignment("DataOffset", FieldAlignmentMode.LeftOnly)] public List<F2StringRecord> StringOffsets;
        [FieldOrder(4), SerializeUntil((short)0), FieldEncoding("UTF-8")] public List<string> Strings;
        [FieldOrder(8), FieldEndianness(Endianness.Little)] public BinaOffsetTable OffsetTable;

        public void AssociateRecords() => StringOffsets = StringOffsets.Zip(Strings, (record, str) => new F2StringRecord {ID = record.ID, Offset = record.Offset, String = str}).ToList();
    }

    public class F2StringRecord
    {
        [FieldOrder(0)] public int Offset;
        [FieldOrder(1)] public int ID;  
        [Ignore] public string String;

        public override string ToString() => $"#0x{ID:X}: \"{String}\" at 0x{Offset:X} ";
    }

}
