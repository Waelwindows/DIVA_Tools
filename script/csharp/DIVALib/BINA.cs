using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinarySerialization;

namespace DIVALib
{
    class BINA
    {
    }

    public class BinaOffsetTable
    {
        [FieldOrder(0), FieldCount(4)] public string Magic = "POF0";
        [FieldOrder(1)] public int Count;
        [FieldOrder(2)] public int Size = 32;
        [FieldOrder(3)] public int Version = 0x1000_0000;
        [FieldOrder(4)] public int UNK = 1;
        [FieldOrder(5), FieldAlignment(16, FieldAlignmentMode.RightOnly)] public int Count2 { get => Count; set { } }
        [FieldOrder(6)] public int Count3 { get => Count; set { } }
        [FieldOrder(8), FieldCount("Count"), SerializeUntil((short)0)] public Stream Offsets;
    }

    public class EndOfFileContainer
    {
        [FieldOrder(0), FieldCount(4)] public string Magic = "EOFC";
        [FieldOrder(1)] public int UNK;
        [FieldOrder(2)] public int Size = 32;
        [FieldOrder(3)] public int Version = 0x1000_0000;
        [FieldOrder(4)] public int UNK2 = 1;
    }
}
