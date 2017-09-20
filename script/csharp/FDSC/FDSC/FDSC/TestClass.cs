using System;
using BinarySerialization;

namespace FDSC
{
    public class TestClass
    {
        [FieldOrder(0)]
        public uint magic;
        [FieldOrder(1)]
        [FieldScale(100)]
        [SerializeAs(SerializedType.UInt4)]
        public double time;

        public TestClass()
        {
            magic = 0x1234;
            time = 1234.56;
        }
    }
}
