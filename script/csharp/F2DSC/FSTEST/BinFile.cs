using System;
using System.IO;
namespace BinOp
{
    public class BinFile
    {
        public FileStream file;
        public BinaryReader br;
        public BinaryWriter bw;

        public BinFile() { }

        public BinFile(string path)
        {
            file = File.OpenRead(path);
            br = new BinaryReader(file);
            bw = new BinaryWriter(file);
        }

        public static BinFile Create(string path)
        {
            BinFile bf = new BinFile();
            bf.file = File.Create(path);
            bf.br = new BinaryReader(bf.file);
            bf.bw = new BinaryWriter(bf.file);
            return bf;
        }

    }

    public static class FileOp
    {

        public enum Endian : uint
        {
            LittleEndian,
            BigEndian
        };

        public static void Write(BinaryWriter bw, uint value, Endian endianness=Endian.LittleEndian)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (endianness == Endian.BigEndian)
            {
                Array.Reverse(bytes);
            }
            bw.Write(bytes);
        }

        public static void Write(BinaryWriter bw, int value, Endian endianness = Endian.LittleEndian)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (endianness == Endian.BigEndian)
            {
                Array.Reverse(bytes);
            }
            bw.Write(bytes);
        }

        public static void Write(BinaryWriter bw, float value, Endian endianness = Endian.LittleEndian)
        {
            byte[] bytes = BitConverter.GetBytes(value);
            if (endianness == Endian.BigEndian)
            {
                Array.Reverse(bytes);
            }
            bw.Write(bytes);
        }

        public static uint ReadUInt32(BinaryReader br, Endian endianness = Endian.LittleEndian)
        {
            byte[] bytes = BitConverter.GetBytes(br.ReadUInt32());
            if (endianness == Endian.BigEndian)
            {
                Array.Reverse(bytes);
            }
            return BitConverter.ToUInt32(bytes, 0);
        }

        public static int ReadInt32(BinaryReader br, Endian endianness = Endian.LittleEndian)
        {
            byte[] bytes = BitConverter.GetBytes(br.ReadInt32());
            if (endianness == Endian.BigEndian)
            {
                Array.Reverse(bytes);
            }
            return BitConverter.ToInt32(bytes, 0);
        }

        public static void WriteNullByte(BinaryWriter bw, uint count = 1)
        {
            byte nullByte = 0;
            for (int i = 0; i < count; ++i)
            {
                bw.Write(nullByte);
            }
        }

        public static void WriteString(BinaryWriter bw, string str)
        {
            bw.Write(str.ToCharArray());
        }

        public static void WriteNullStr(BinaryWriter bw, string str)
        {
            WriteString(bw, str);
            WriteNullByte(bw);
        }

        public static void WriteFloatAsUInt(BinaryWriter bw, float value, Endian endianness = Endian.LittleEndian)
        {
            Write(bw, (uint)value, endianness);
        }
    }
}