using System;
using System.Collections.Generic;
using System.IO;
using BinarySerialization;
using DIVALib.IO;
using DIVALib.Math;
using System.Linq;

namespace MOT_EDITOR
{
    using System.Windows.Markup;

    public class MotFile : IBinarySerializable
    {
        public const int MaxData = 64;

        public static readonly Dictionary<int, int> FrameValueOffset = new Dictionary<int, int>
        {
            /*
            {5, 1},
            {10, 3},
            {16, 2},
            {21, 2},
            {25, 3},
            {30, 3},
            {33, 14},
            {38, 3},
            {41, 12},
            {47, 2},
            {55, 1},
            {63, 16},
            */
            /*
            {6, 1},
            {11, 3},
            */


        };

        [FieldOrder(0)] public MotHeader Header;

        [FieldOrder(1)] public List<MotHeader> SubHeaders;

        [FieldOrder(2)] public List<MotAnim> Animations;

        public void Initilize()
        {
            Header = new MotHeader
            {
                InfoOffset = 32,
                UnkDataOffset = 36,
                DataOffset = 168
            };

            SubHeaders = new List<MotHeader> { new MotHeader() };

            Animations = new List<MotAnim> { new MotAnim() };
        }

        public MotFile()
        {
            Header = new MotHeader();

            SubHeaders = new List<MotHeader>();

            Animations = new List<MotAnim>();
        }

        public void Serialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
        {
            var serializer = new BinarySerializer();
            serializer.Serialize(stream, Header);
            foreach (var subHeader in SubHeaders)
            {
                serializer.Serialize(stream, subHeader);
            }
            foreach (var animation in Animations)
            {
                serializer.Serialize(stream, animation);
            }
        }

        public void Deserialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext) => throw new NotImplementedException("Use the class method instead");

        public static MotFile Deserialize(Stream stream, bool FMode)
        {
            var serializer = new BinarySerializer();
            var file = new MotFile();
            file.Header = serializer.Deserialize<MotHeader>(stream);

            while (stream.Position < file.Header.InfoOffset)
            {
                file.SubHeaders.Add(serializer.Deserialize<MotHeader>(stream));
            }

            var anim = new MotAnim();
            anim.Deserialize(stream, file.Header, file.SubHeaders[0].InfoOffset, FMode);

            file.Animations.Add(anim);

            for (var i = 0; i < file.SubHeaders.Count; i++)
            {
                if (file.SubHeaders[i].InfoOffset == 0) { continue; }
                anim = new MotAnim();
                var boneEnd = 0;
                try { boneEnd = file.SubHeaders[i + 1].InfoOffset; }
                catch { /* ignored */ }
                anim.Deserialize(stream, file.SubHeaders[i], boneEnd, FMode);
                file.Animations.Add(anim);
            }
            return file;
        }

        public MotData GetMotData(int animIndex, int dataIndex) => Animations[animIndex].Data[dataIndex];

        public List<MotData> GetMotGroup(int animIndex, int dataIndex, int size) =>
            Animations[animIndex].Data.Skip(dataIndex).Take(size).ToList();

        public void SetMotGroup(int animIndex, int dataIndex, int size, Action<MotData> operation) =>
            GetMotGroup(animIndex, dataIndex, size).ForEach(operation);
    }

    public class MotSkeleton
    {
        [FieldOrder(0)] public BoneAnim RootTransform;
        [FieldOrder(1)] public BoneAnim RootRotation;
        [FieldOrder(2)] public BoneAnim IkSpine;
        [FieldOrder(3)] public BoneAnim SpineBase;
        [FieldOrder(4)] public BoneAnim SpineSecondary;
        [FieldOrder(5)] public BoneAnim Neck;
        [FieldOrder(6)] public BoneAnim IkHead;
        [FieldOrder(6)] public BoneAnim Head;
        [FieldOrder(7)] public BoneAnim RightEye;
        [FieldOrder(8)] public BoneAnim LeftShoulder;
        [FieldOrder(9)] public BoneAnim IkLeftElbow;
        [FieldOrder(10)] public BoneAnim LeftArmAdjust;
        [FieldOrder(11)] public BoneAnim RightShoulder;
        [FieldOrder(12)] public BoneAnim IkRightElbow;
        [FieldOrder(13)] public BoneAnim RightArmAdjust;
        [FieldOrder(14)] public BoneAnim IkLeftHand;
        [FieldOrder(15)] public BoneAnim IkRightHand;
        [FieldOrder(16)] public BoneAnim IkLeftLeg;
        [FieldOrder(17)] public BoneAnim IkLeftFoot;
        [FieldOrder(18)] public BoneAnim IkRightLeg;
        [FieldOrder(19)] public BoneAnim IkRightFoot;

        public MotSkeleton(List<MotData> data)
        {
            RootTransform = new BoneAnim(ref data, 0);
            RootRotation = new BoneAnim(ref data, 3);
            IkSpine = new BoneAnim(ref data, 6);
            SpineBase = new BoneAnim(ref data, 9);
            SpineSecondary = new BoneAnim(ref data, 12);
            Neck = new BoneAnim(ref data, 15);
            IkHead = new BoneAnim(ref data, 18);
            Head = new BoneAnim(ref data, 21);
            RightEye = new BoneAnim(ref data, 24, 2);
            LeftShoulder = new BoneAnim(ref data, 26, 2); //26 27
            IkLeftElbow = new BoneAnim(ref data, 28); // 28 29 30
            LeftArmAdjust = new BoneAnim(ref data, 31, 2);
            RightShoulder = new BoneAnim(ref data, 33);
            IkRightElbow = new BoneAnim(ref data, 36);
            RightArmAdjust = new BoneAnim(ref data, 39); // 39 40 41
            IkLeftHand = new BoneAnim(ref data, 42); // 42 43 44
            IkRightHand = new BoneAnim(ref data, 45); // 45 56 47
            IkLeftLeg = new BoneAnim(ref data, 48); // 48 49 50
            IkLeftFoot = new BoneAnim(ref data, 51); // 51 52 53
            IkRightLeg = new BoneAnim(ref data, 54); // 54 55 56
            IkRightFoot = new BoneAnim(ref data, 57); // 57 58 59
        }
    }

    public class MotHeader
    {
        [FieldOrder(0)] public int InfoOffset;

        [FieldOrder(1)] public int UnkDataOffset;

        [FieldOrder(2)] public int DataOffset;

        [FieldOrder(3)] public int BoneOffset;
    }

    public class MotAnim : IBinarySerializable
    {
        [FieldOrder(0)] public MotInfo Info;
        [FieldOrder(1)] public List<uint> Settings;
        [FieldOrder(2)] public List<MotData> Data;
        [Ignore] public MotSkeleton Skeleton;
        [FieldOrder(3)] public MotBones Bones;

        public MotAnim()
        {
            Info = new MotInfo();
            Settings = new List<uint>();
            Data = new List<MotData>();
            Bones = new MotBones();
        }

        public void Deserialize(Stream stream, MotHeader header, int boneEnd, bool bFMode)
        {
            stream.Position = header.InfoOffset;
            var serializer = new BinarySerializer();
            Info = serializer.Deserialize<MotInfo>(stream);
            stream.Position = header.UnkDataOffset;
            while (stream.Position < header.DataOffset)
            {
                Settings.Add(DataStream.ReadUInt32(stream));
            }

            for (var i = 0; i < MotFile.MaxData; ++i)
            {
                var flagCount = MotFile.FrameValueOffset.ContainsKey(i) ? MotFile.FrameValueOffset[i] : 0;

                var motion = MotData.Deserialize(stream, flagCount, bFMode);
                    
                Data.Add(motion);
            }

            Skeleton = new MotSkeleton(Data);

            stream.Position = header.BoneOffset;
            Bones = new MotBones();
            Bones.GetIds(stream, boneEnd);
        }

        public void Serialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
        {
            var serializer = new BinarySerializer();
            serializer.Serialize(stream, Info);
            foreach (var setting in Settings)
            {
                DataStream.WriteUInt32(stream, setting);
            }
            foreach (var motData in Data)
            {
                serializer.Serialize(stream, motData);
            }
            serializer.Serialize(stream, Bones);
        }

        public void Deserialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
        {
            throw new NotImplementedException();
        }
    }

    public class MotInfo
    {
        [FieldOrder(1)] public ushort FrameCount;

        [FieldOrder(0)] public ushort Unk;
    }

    public class BoneAnim
    {
        [FieldCount(3)]
        public List<MotData> AnimData;

        [Ignore]
        public int CurrentFrame;

        public Vector3 FrameValue
        {
            get => new Vector3
                       {
                           AnimData[0].Values[CurrentFrame],
                           AnimData[1].Values[CurrentFrame],
                           AnimData[2].Values[CurrentFrame]
                       };

            set
            {
                AnimData[0].Values[CurrentFrame].Value = (float)value.x;
                AnimData[1].Values[CurrentFrame].Value = (float)value.y;
                AnimData[2].Values[CurrentFrame].Value = (float)value.z;
            }
        }

        public List<Vector3> AnimValues
        {
            get => null;
        }

        public BoneAnim(ref List<MotData> data, int start, int size = 3) => AnimData = data.Skip(start).Take(size).ToList();

        public Vector3 GetAsVector3(int keyframe)
        {
            CurrentFrame = keyframe;
            return FrameValue;
        }

        public void SetEachFrame(int component, double value)
        {
            foreach (var frameValue in AnimData[component].Values)
            {
                frameValue.Value = (float)value;
            }
        }

        public void SetVector3(Vector3 vec)
        {
            for (var i = 0; i < AnimData.Count; i++)
            {
                switch (i)
                {
                    case 0: SetEachFrame(i, vec.x); break;
                    case 1: SetEachFrame(i, vec.y); break;
                    case 2: SetEachFrame(i, vec.z); break;
                }
            }
        }
    }

    public class BoneKeyframe
    {
        public Vector3 Keyframe;

        public BoneKeyframe()
        {
            
        }
    }

    public class MotData : IBinarySerializable
    {
        [FieldOrder(0)] public ushort FrameCount;

        [FieldOrder(1)] public List<ushort> Frames;

        [FieldOrder(2)] public List<FrameValue> Values;

        [FieldOrder(3)] public List<uint> Flags;

        [Ignore] public bool FMode;

        public MotData() { Flags = new List<uint>(); }

        public void Serialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
        {
            DataStream.WriteUInt16(stream, FrameCount);
            if (FrameCount % 2 == 0 && !FMode)
                DataStream.WriteUInt16(stream, 0);
            foreach (var frame in Frames)
                DataStream.WriteUInt16(stream, frame);
            var serializer = new BinarySerializer();
            foreach (var value in Values)
                serializer.Serialize(stream, value);
            foreach (var flag in Flags)
                DataStream.WriteUInt32(stream, flag);
            if (FrameCount % 2 == 0 && FMode)
                DataStream.WriteUInt16(stream, 0);
        }

        public void Deserialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext) => throw new NotImplementedException("Use the non-Binary Serialization Deserialization method");

        public override string ToString()
        {
            if (FMode)
                return $"Motion Value: {FrameCount} frames,  {(FrameCount / 60.0) / 60.0 / 2.0:0.##} mins";
            else
                return $"Motion Value: {FrameCount} frames,  {(FrameCount /60.0) / 60.0:0.##} mins";
        }

        public void GetFlags(Stream stream, int flagCount)
        {
            for (var x = 0; x < flagCount; ++x)
            {
                var flag = DataStream.ReadUInt32(stream);
                Flags.Add(flag);
            }
        }

        public static MotData Deserialize(Stream stream, int flagCount, bool FMode)
        {
            var data = new MotData();
            //data.GetFlags(stream, flagCount);
            data.FrameCount = DataStream.ReadUInt16(stream);
            if (data.FrameCount % 2 == 0 & !FMode)
                DataStream.ReadUInt16(stream);
            data.Frames = new List<ushort>();
            for (var i = 0; i < data.FrameCount; ++i)
                data.Frames.Add(DataStream.ReadUInt16(stream));
            var serializer = new BinarySerializer();
            data.Values = new List<FrameValue>();
            for (var i = 0; i < data.FrameCount; ++i)
                data.Values.Add(serializer.Deserialize<FrameValue>(stream));
            if (data.FrameCount % 2 == 0 & FMode)
                DataStream.ReadUInt16(stream);
            data.GetFlags(stream, flagCount);
            data.FMode = FMode;
            return data;
        }

        public void SetEachFrame(double value) => Values.ForEach(frameValue => frameValue.Value = (float)value);
        public void MultiplyEachFrame(double value) => Values.ForEach(frameValue => frameValue.Value *= (float)value);

    }

    public class FrameValue
    {
        [FieldOrder(1)] public float Unk;

        [FieldOrder(0)] public float Value;
    }

    public class MotBones : IBinarySerializable
    {
        public List<ushort> Ids;

        public MotBones()
        {
            Ids = new List<ushort>();
        }

        public void Serialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
        {
            foreach (var id in Ids)
            {
                DataStream.WriteUInt16(stream, id);
            }
        }

        public void Deserialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
        {
            throw new NotImplementedException("Use GetIds() instead.");
        }

        public void GetIds(Stream stream, int end)
        {
            var endPosition = end != 0 ? end : stream.Length;
            while (stream.Position < endPosition)
            {
                Ids.Add(DataStream.ReadUInt16(stream));
            }
        }
    }
}