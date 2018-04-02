using System.Collections.Generic;
using System.Linq;
using BinarySerialization;

namespace DIvaModel
{
    public class BinModelHeader
    {
        [FieldOrder(0)] public int Magic = 0x5062500;
        [FieldOrder(1)] public int SectionCount;
        [FieldOrder(2)] public int BoneCount;
        [FieldOrder(3)] public int SectionTableOffset;
        [FieldOrder(4)] public int BoneTableOffset;
        [FieldOrder(5)] public int MeshNameOffest;
        [FieldOrder(6)] public int MeshNameIdOffset;
        [FieldOrder(7)] public int TextureHashOffset;
        [FieldOrder(8)] public int TextureHashCount;
        [FieldOrder(9), FieldOffset("TextureHashOffset"), FieldCount("TextureHashCount")] public List<int> TextureHashs;
        [Ignore] public List<string> TextureList => TextureHashs.Select(hash => hash.ToString()).ToList();
        [FieldOrder(10), FieldOffset("BoneTableOffset"), FieldCount("SectionCount")] public List<int> BoneTable;
        [FieldOrder(11), FieldOffset("SectionTableOffset"), FieldCount("SectionCount")] public List<int> SectionTable;
    }

    public class BinModel
    {
        [FieldOrder(0)] public BinModelHeader Header;

    }
}
