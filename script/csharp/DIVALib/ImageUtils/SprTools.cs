using System;
using System.Collections.Generic;
using BinarySerialization;

namespace DIVALib.ImageUtils
{
    public class SprOffsetTable
    {
        [FieldOrder(0)] public int Magic = 0x53_50_52_43;
        [FieldOrder(1)] public int EofOffset = 240;
        [FieldOrder(2)] public int Size = 32;
        [FieldOrder(3)] public int Version = 0x18_00_00_00;
        [FieldOrder(5)] public int Reserved;
        [FieldOrder(4)] public int CompressionNameOffest;
    }

    public class SpriteScale
    {
        [FieldOrder(0)] public int UNK;
        [FieldOrder(1)] public int Scale;
    }

    public class SpriteProperties
    {
        [FieldOrder(0)] public uint TextureIndex;
        [FieldOrder(1)] public float unk1;
        [FieldOrder(2)] public float unk2;
        [FieldOrder(3)] public float unk3;
        [FieldOrder(4)] public float unk4;
        [FieldOrder(5)] public float unk5;
        [FieldOrder(6)] public float RectangleX;
        [FieldOrder(7)] public float RectangleY;
        [FieldOrder(8)] public float RectangleWidth;
        [FieldOrder(9)] public float RectangleHeight;
    }

    public class SpritePOF
    {
        [FieldOrder(0)] public uint Magic = 0x50_4F_46_30;
        [FieldOrder(1)] public uint unk1;
        [FieldOrder(2)] public uint Size = 32;
        [FieldOrder(3)] public uint Version = 0x10_00_00_00;
        [FieldOrder(4)] public uint unk2;
        [FieldOrder(5)] public uint unk3;
    }

    public class SprHeader
    {
        [FieldOrder(0)] public int Id;
        [FieldOrder(1)] public int TextureOffset;
        [FieldOrder(2)] public int TextureCount;
        [FieldOrder(3)] public int SpriteCount;
        [FieldOrder(4)] public int SpritePropertiesOffset;
        [FieldOrder(5)] public int TextureNamesOffset;
        [FieldOrder(6)] public int SpriteNamesOffest;
        [FieldOrder(7)] public int SpriteScalesOffset;

        [FieldOrder(8), FieldCount("SpriteCount"), FieldAlignment(16, FieldAlignmentMode.RightOnly)] public List<SpriteScale> SpriteScales;
        [FieldOrder(9), FieldCount("SpriteCount"), FieldAlignment(16, FieldAlignmentMode.RightOnly)] public List<SpriteProperties> SpriteProperties;
        [FieldOrder(10), FieldCount("SpriteCount"), FieldAlignment(16, FieldAlignmentMode.RightOnly)] public List<int> SpriteNameOffsets;
        [FieldOrder(11), FieldCount("TextureCount"), FieldAlignment(16, FieldAlignmentMode.RightOnly)] public List<int> TextureNameOffsets;
        [FieldOrder(12), FieldCount("SpriteCount")] public List<string> SpriteNames;
        [FieldOrder(13), FieldCount("TextureCount"), FieldAlignment(16, FieldAlignmentMode.RightOnly)] public List<string> TextureNames;
        [FieldOrder(14), FieldEndianness(Endianness.Little)] public SpritePOF POF;
        [FieldOrder(15), FieldEndianness(Endianness.Little), SerializeUntil((byte)69)] public List<byte> UNK;
        [FieldOrder(17), FieldEndianness(Endianness.Little), FieldAlignment(16, FieldAlignmentMode.RightOnly)] public EndOfFileContainer EOFC;
    }

    public class TXPContainer
    {
        [FieldOrder(0)] public int Magic = 0x54_58_50_43;
        [FieldOrder(1)] public int Bytesize1;
        [FieldOrder(2)] public int Size = 32;
        [FieldOrder(3)] public int Version = 0x18_00_00_00;
        [FieldOrder(4)] public int UNK;
        [FieldOrder(5)] public int Bytesize2;

    }

    public class SpriteFile
    {
        [FieldOrder(0), FieldAlignment(16, FieldAlignmentMode.RightOnly)] public SprOffsetTable OffsetTable;

        [FieldOrder(1), FieldEndianness(Endianness.Big)] public SprHeader Header;
        //[FieldOrder(1), FieldEndianness(Endianness.Big), FieldCount(584)] public List<int> FauxHeader;

        [FieldOrder(2), FieldAlignment(16, FieldAlignmentMode.RightOnly)] public TXPContainer Container;

        [FieldOrder(7), FieldEndianness(Endianness.Big), FieldAlignment(16, FieldAlignmentMode.RightOnly)] public TxpTextureAtlas Atlas;

        [FieldOrder(8), FieldAlignment(16, FieldAlignmentMode.RightOnly)] public EndOfFileContainer EndOfFile;


    }
}
