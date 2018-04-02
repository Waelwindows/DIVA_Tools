using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinarySerialization;

namespace DIVALib.Databases
{
    class BoneDatabase
    {
        [FieldOrder(0)] public int Magic;
        [FieldOrder(1)] public int SkeletonCount;
        [FieldOrder(2)] public int SkeletonOffset1;
        [FieldOrder(3)] public int SkeletonOffset2;
        [FieldOrder(4), FieldOffset("SkeletonOffset1"), FieldCount("SkeletonCount")] public List<int> SkeletonInfo1;
        [FieldOrder(5), FieldOffset("SkeletonOffset2"), FieldCount("SkeletonCount")] public List<int> SkeletonInfo2;
        //[FieldOrder(6), FieldCount("SkeletonCount")] public List<int> SkeletonDictionary;
    }
}
