using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using BinarySerialization;
using DIVALib.IO;

namespace DIVALib.DSCUtils
{
    public class DSC
    {
        [FieldOrder(0)] public int Magic;
        [FieldOrder(1), FieldOffset(0)]
        [Subtype("Magic", 0x1202_0220, typeof(DscFile))]
        [Subtype("Magic", 0x4353_5650, typeof(F2DscFile))]
        public DscContainer File;

        public void XmlDeserialize(Stream s)
        {
            var doc = new XmlDocument();
            doc.Load(s);
            var root = doc.DocumentElement;
            if (root != null)
            {
                Console.WriteLine(root.Attributes);
                var verStr = "0";
                foreach (XmlNode node in doc.ChildNodes)
                {
                    if (node.Name != "dsc") continue;
                    verStr = node.Attributes["version"].Value;
                    break;
                }
                if (verStr == "0")
                    Console.WriteLine("Note: Couldn't find the xml file's ver");
                    verStr = "302121504";
                Console.WriteLine(verStr);
                var ver = int.Parse(verStr);
                s.Position = 0;
                XmlSerializer serializer;
                switch (ver)
                {
                    case 0x12020220:
                        serializer = new XmlSerializer(typeof(DscFile));
                        File = serializer.Deserialize(s) as DscFile;
                        break;
                    case 0x43535650:
                        serializer = new XmlSerializer(typeof(F2DscFile));
                        File = serializer.Deserialize(s) as F2DscFile;
                        break;
                    default: throw new XmlException("document doesn't contain valid version");
                }
            }
        }
    }

    public abstract class DscContainer
    {
        public List<DscFunctionWrapper> Functions = new List<DscFunctionWrapper>();

        public abstract void BinSerialize(Stream s);
        public abstract dynamic BinDeserialize(Stream s);

        public abstract void XmlSerialize(Stream s);
        public abstract dynamic XmlDeserialize(Stream s);
    }

    public class DscFile1
    {
        [XmlAttribute("version")] [FieldOrder(0)] public uint Magic = 302121504;
        [FieldOrder(1), SerializeUntil((Int64)32)] public List<DscFunctionWrapper> Functions = new List<DscFunctionWrapper>();

    }

    [XmlRoot("dsc")]
    //[XmlInclude(typeof(DscFunc))]
    [XmlInclude(typeof(FEnd))]
    [XmlInclude(typeof(FTime))]
    [XmlInclude(typeof(FMikuMove))]
    [XmlInclude(typeof(FMikuRotate))]
    [XmlInclude(typeof(FMikuDisplay))]
    [XmlInclude(typeof(FMikuShadow))]
    [XmlInclude(typeof(FTarget))]
    [XmlInclude(typeof(FSetMotion))]
    [XmlInclude(typeof(FSetPlaydata))]
    [XmlInclude(typeof(FEffect))]
    [XmlInclude(typeof(FFadeinField))]
    [XmlInclude(typeof(FEffectOff))]
    [XmlInclude(typeof(FSetCamera))]
    [XmlInclude(typeof(FDataCamera))]
    [XmlInclude(typeof(FChangeField))]
    [XmlInclude(typeof(FHideField))]
    [XmlInclude(typeof(FMoveField))]
    [XmlInclude(typeof(FFadeoutField))]
    [XmlInclude(typeof(FEyeAnim))]
    [XmlInclude(typeof(FMouthAnim))]
    [XmlInclude(typeof(FHandAnim))]
    [XmlInclude(typeof(FLookAnim))]
    [XmlInclude(typeof(FExpression))]
    [XmlInclude(typeof(FLookCamera))]
    [XmlInclude(typeof(FLyric))]
    [XmlInclude(typeof(FMusicPlay))]
    [XmlInclude(typeof(FModeSelect))]
    [XmlInclude(typeof(FEditMotion))]
    [XmlInclude(typeof(FBarTimeSet))]
    [XmlInclude(typeof(FShadowheight))]
    [XmlInclude(typeof(FEditFace))]
    [XmlInclude(typeof(FMoveCamera))]
    [XmlInclude(typeof(FPvEnd))]
    [XmlInclude(typeof(FShadowpos))]
    [XmlInclude(typeof(FEditLyric))]
    [XmlInclude(typeof(FEditTarget))]
    [XmlInclude(typeof(FEditMouth))]
    [XmlInclude(typeof(FSetCharacter))]
    [XmlInclude(typeof(FEditMove))]
    [XmlInclude(typeof(FEditShadow))]
    [XmlInclude(typeof(FEditEyelid))]
    [XmlInclude(typeof(FEditEye))]
    [XmlInclude(typeof(FEditItem))]
    [XmlInclude(typeof(FEditEffect))]
    [XmlInclude(typeof(FEditDisp))]
    [XmlInclude(typeof(FEditHandAnim))]
    [XmlInclude(typeof(FAim))]
    [XmlInclude(typeof(FHandItem))]
    [XmlInclude(typeof(FEditBlush))]
    [XmlInclude(typeof(FNearClip))]
    [XmlInclude(typeof(FClothWet))]
    [XmlInclude(typeof(FLightRot))]
    [XmlInclude(typeof(FSceneFade))]
    [XmlInclude(typeof(FToneTrans))]
    [XmlInclude(typeof(FSaturate))]
    [XmlInclude(typeof(FFadeMode))]
    [XmlInclude(typeof(FAutoBlink))]
    [XmlInclude(typeof(FPartsDisp))]
    [XmlInclude(typeof(FTargetFlyingTime))]
    [XmlInclude(typeof(FCharacterSize))]
    [XmlInclude(typeof(FCharacterHeightAdjust))]
    [XmlInclude(typeof(FItemAnim))]
    [XmlInclude(typeof(FCharacterPosAdjust))]
    [XmlInclude(typeof(FSceneRot))]
    [XmlInclude(typeof(FEditMotSmoothLen))]
    [XmlInclude(typeof(FPvBranchMode))]
    [XmlInclude(typeof(FDataCameraStart))]
    [XmlInclude(typeof(FMoviePlay))]
    [XmlInclude(typeof(FMovieDisplay))]
    [XmlInclude(typeof(FWind))]
    [XmlInclude(typeof(FOsageStep))]
    [XmlInclude(typeof(FOsageMoveCollider))]
    [XmlInclude(typeof(FCharacterColor))]
    [XmlInclude(typeof(FSeEffect))]
    [XmlInclude(typeof(FEditMoveXYZ))]
    [XmlInclude(typeof(FEditEyelidAnim))]
    [XmlInclude(typeof(FEditInstrumentItem))]
    [XmlInclude(typeof(FEditMotionLoop))]
    [XmlInclude(typeof(FEditExpression))]
    [XmlInclude(typeof(FEditEyeAnim))]
    [XmlInclude(typeof(FEditMouthAnim))]
    [XmlInclude(typeof(FEditCamera))]
    [XmlInclude(typeof(FEditModeSelect))]
    [Serializable]
    public class DscFile : DscContainer
    {
        [XmlAttribute("version")] [FieldOrder(0)] public uint Magic = 302121504;

        public override void BinSerialize(Stream s)
        {
            var serializer = new BinarySerializer();
            serializer.Serialize(s, this);
        }

        public override dynamic BinDeserialize(Stream s)
        {
            var serializer = new BinarySerializer();
            var dsc = serializer.Deserialize(s, typeof(DscFile)) as DscFile;
            return dsc;
        }

        public override void XmlSerialize(Stream s)
        {
            var serializer = new XmlSerializer(typeof(DscFile));
            serializer.Serialize(s, this);
        }

        public override dynamic XmlDeserialize(Stream s)
        {
            var serializer = new XmlSerializer(typeof(DscFile));
            var dsc = serializer.Deserialize(s) as DscFile;
            return dsc;
        }
    }

    [XmlRoot("dsc")]
    [XmlInclude(typeof(DscFunctionWrapper))]
    [XmlInclude(typeof(FEnd))]
    [XmlInclude(typeof(F2Time))]
    [XmlInclude(typeof(FMikuMove))]
    [XmlInclude(typeof(FMikuRotate))]
    [XmlInclude(typeof(FMikuDisplay))]
    [XmlInclude(typeof(FMikuShadow))]
    [XmlInclude(typeof(F2Target))]
    [XmlInclude(typeof(FSetMotion))]
    [XmlInclude(typeof(FSetPlaydata))]
    [XmlInclude(typeof(FEffect))]
    [XmlInclude(typeof(FFadeinField))]
    [XmlInclude(typeof(FEffectOff))]
    [XmlInclude(typeof(FSetCamera))]
    [XmlInclude(typeof(FDataCamera))]
    [XmlInclude(typeof(FChangeField))]
    [XmlInclude(typeof(FHideField))]
    [XmlInclude(typeof(FMoveField))]
    [XmlInclude(typeof(FFadeoutField))]
    [XmlInclude(typeof(FEyeAnim))]
    [XmlInclude(typeof(FMouthAnim))]
    [XmlInclude(typeof(FHandAnim))]
    [XmlInclude(typeof(FLookAnim))]
    [XmlInclude(typeof(FExpression))]
    [XmlInclude(typeof(FLookCamera))]
    [XmlInclude(typeof(FLyric))]
    [XmlInclude(typeof(FMusicPlay))]
    [XmlInclude(typeof(FModeSelect))]
    [XmlInclude(typeof(FEditMotion))]
    [XmlInclude(typeof(FBarTimeSet))]
    [XmlInclude(typeof(FShadowheight))]
    [XmlInclude(typeof(FEditFace))]
    [XmlInclude(typeof(FMoveCamera))]
    [XmlInclude(typeof(FPvEnd))]
    [XmlInclude(typeof(FShadowpos))]
    [XmlInclude(typeof(FEditLyric))]
    [XmlInclude(typeof(FEditTarget))]
    [XmlInclude(typeof(FEditMouth))]
    [XmlInclude(typeof(FSetCharacter))]
    [XmlInclude(typeof(FEditMove))]
    [XmlInclude(typeof(FEditShadow))]
    [XmlInclude(typeof(FEditEyelid))]
    [XmlInclude(typeof(FEditEye))]
    [XmlInclude(typeof(FEditItem))]
    [XmlInclude(typeof(FEditEffect))]
    [XmlInclude(typeof(FEditDisp))]
    [XmlInclude(typeof(FEditHandAnim))]
    [XmlInclude(typeof(FAim))]
    [XmlInclude(typeof(FHandItem))]
    [XmlInclude(typeof(FEditBlush))]
    [XmlInclude(typeof(FNearClip))]
    [XmlInclude(typeof(FClothWet))]
    [XmlInclude(typeof(FLightRot))]
    [XmlInclude(typeof(FSceneFade))]
    [XmlInclude(typeof(FToneTrans))]
    [XmlInclude(typeof(FSaturate))]
    [XmlInclude(typeof(FFadeMode))]
    [XmlInclude(typeof(FAutoBlink))]
    [XmlInclude(typeof(FPartsDisp))]
    [XmlInclude(typeof(FTargetFlyingTime))]
    [XmlInclude(typeof(FCharacterSize))]
    [XmlInclude(typeof(FCharacterHeightAdjust))]
    [XmlInclude(typeof(FItemAnim))]
    [XmlInclude(typeof(FCharacterPosAdjust))]
    [XmlInclude(typeof(FSceneRot))]
    [XmlInclude(typeof(FEditMotSmoothLen))]
    [XmlInclude(typeof(FPvBranchMode))]
    [XmlInclude(typeof(FDataCameraStart))]
    [XmlInclude(typeof(FMoviePlay))]
    [XmlInclude(typeof(FMovieDisplay))]
    [XmlInclude(typeof(FWind))]
    [XmlInclude(typeof(FOsageStep))]
    [XmlInclude(typeof(FOsageMoveCollider))]
    [XmlInclude(typeof(FCharacterColor))]
    [XmlInclude(typeof(FSeEffect))]
    [XmlInclude(typeof(FEditMoveXYZ))]
    [XmlInclude(typeof(FEditEyelidAnim))]
    [XmlInclude(typeof(FEditInstrumentItem))]
    [XmlInclude(typeof(FEditMotionLoop))]
    [XmlInclude(typeof(FEditExpression))]
    [XmlInclude(typeof(FEditEyeAnim))]
    [XmlInclude(typeof(FEditMouthAnim))]
    [XmlInclude(typeof(FEditCamera))]
    [XmlInclude(typeof(FEditModeSelect))]
    [Serializable]
    //F2nd dsc body
    public class F2DscFile : DscContainer
    {
        [XmlAttribute("version")] [Ignore] public uint Magic = 0x43535650;

        [FieldOrder(0)] [FieldEndianness(Endianness.Little)] public F2DscHeader Header;

        [FieldOrder(1)] [FieldEndianness(Endianness.Big)] public F2Dscunk Unk;

        [FieldOrder(3)] [FieldEndianness(Endianness.Big)] [XmlIgnore] public F2DscEnd End;
        /*
        public void Deserialize(Stream s, Endianness endianness, BinarySerializationContext context)
        {
            var serial = new BinarySerializer();
            Header = (F2DscHeader) serial.Deserialize(s, typeof(F2DscHeader));
            Unk = (F2Dscunk) serial.Deserialize(s, typeof(F2Dscunk));
            var EOF = false;
            Console.Write("File size is {0}\n", s.Length);
            while (!EOF)
            {
                Console.Write("Current position {0}\n", s.Position);
                if (s.Position >= Header.subfileSize - 60)
                {
                    EOF = true;
                    break;
                }
                var endian = endianness == Endianness.Little
                    ? DataStream.Endian.LittleEndian
                    : DataStream.Endian.BigEndian;
                var readFuncId = DataStream.ReadUInt32(s, endian);
                s.Position -= 4;
                var deserial = new BinarySerializer();
                deserial.Endianness = endianness;
                switch (readFuncId)
                {
                    //case 0x00: functions.Add((FEnd)deserial.Deserialize(s, typeof(FEnd))); break;
                    case 0x00:
                        Functions.Add((FEnd) deserial.Deserialize(s, typeof(FEnd)));
                        break;
                    case 0x01:
                        Functions.Add((F2Time) deserial.Deserialize(s, typeof(F2Time)));
                        break;
                    case 0x02:
                        Functions.Add((FMikuMove) deserial.Deserialize(s, typeof(FMikuMove)));
                        break;
                    case 0x03:
                        Functions.Add((FMikuRotate) deserial.Deserialize(s, typeof(FMikuRotate)));
                        break;
                    case 0x04:
                        Functions.Add((FMikuDisplay) deserial.Deserialize(s, typeof(FMikuDisplay)));
                        break;
                    case 0x05:
                        Functions.Add((FMikuShadow) deserial.Deserialize(s, typeof(FMikuShadow)));
                        break;
                    case 0x06:
                        Functions.Add((F2Target) deserial.Deserialize(s, typeof(F2Target)));
                        break;
                    case 0x07:
                        Functions.Add((FSetMotion) deserial.Deserialize(s, typeof(FSetMotion)));
                        break;
                    case 0x08:
                        Functions.Add((FSetPlaydata) deserial.Deserialize(s, typeof(FSetPlaydata)));
                        break;
                    case 0x09:
                        Functions.Add((FEffect) deserial.Deserialize(s, typeof(FEffect)));
                        break;
                    case 0x0A:
                        Functions.Add((FFadeinField) deserial.Deserialize(s, typeof(FFadeinField)));
                        break;
                    case 0x0B:
                        Functions.Add((FEffectOff) deserial.Deserialize(s, typeof(FEffectOff)));
                        break;
                    case 0x0C:
                        Functions.Add((FSetCamera) deserial.Deserialize(s, typeof(FSetCamera)));
                        break;
                    case 0x0D:
                        Functions.Add((FDataCamera) deserial.Deserialize(s, typeof(FDataCamera)));
                        break;
                    case 0x0E:
                        Functions.Add((FChangeField) deserial.Deserialize(s, typeof(FChangeField)));
                        break;
                    case 0x0F:
                        Functions.Add((FHideField) deserial.Deserialize(s, typeof(FHideField)));
                        break;
                    case 0x10:
                        Functions.Add((FMoveField) deserial.Deserialize(s, typeof(FMoveField)));
                        break;
                    case 0x11:
                        Functions.Add((FFadeoutField) deserial.Deserialize(s, typeof(FFadeoutField)));
                        break;
                    case 0x12:
                        Functions.Add((FEyeAnim) deserial.Deserialize(s, typeof(FEyeAnim)));
                        break;
                    case 0x13:
                        Functions.Add((FMouthAnim) deserial.Deserialize(s, typeof(FMouthAnim)));
                        break;
                    case 0x14:
                        Functions.Add((FHandAnim) deserial.Deserialize(s, typeof(FHandAnim)));
                        break;
                    case 0x15:
                        Functions.Add((FLookAnim) deserial.Deserialize(s, typeof(FLookAnim)));
                        break;
                    case 0x16:
                        Functions.Add((FExpression) deserial.Deserialize(s, typeof(FExpression)));
                        break;
                    case 0x17:
                        Functions.Add((FLookCamera) deserial.Deserialize(s, typeof(FLookCamera)));
                        break;
                    case 0x18:
                        Functions.Add((FLyric) deserial.Deserialize(s, typeof(FLyric)));
                        break;
                    case 0x19:
                        Functions.Add((FMusicPlay) deserial.Deserialize(s, typeof(FMusicPlay)));
                        break;
                    case 0x1A:
                        Functions.Add((FModeSelect) deserial.Deserialize(s, typeof(FModeSelect)));
                        break;
                    case 0x1B:
                        Functions.Add((FEditMotion) deserial.Deserialize(s, typeof(FEditMotion)));
                        break;
                    case 0x1C:
                        Functions.Add((FBarTimeSet) deserial.Deserialize(s, typeof(FBarTimeSet)));
                        break;
                    case 0x1D:
                        Functions.Add((FShadowheight) deserial.Deserialize(s, typeof(FShadowheight)));
                        break;
                    case 0x1E:
                        Functions.Add((FEditFace) deserial.Deserialize(s, typeof(FEditFace)));
                        break;
                    case 0x1F:
                        Functions.Add((FMoveCamera) deserial.Deserialize(s, typeof(FMoveCamera)));
                        break;
                    case 0x20:
                        Functions.Add((FPvEnd) deserial.Deserialize(s, typeof(FPvEnd)));
                        break;
                    case 0x21:
                        Functions.Add((FShadowpos) deserial.Deserialize(s, typeof(FShadowpos)));
                        break;
                    case 0x22:
                        Functions.Add((FEditLyric) deserial.Deserialize(s, typeof(FEditLyric)));
                        break;
                    case 0x23:
                        Functions.Add((FEditTarget) deserial.Deserialize(s, typeof(FEditTarget)));
                        break;
                    case 0x24:
                        Functions.Add((FEditMouth) deserial.Deserialize(s, typeof(FEditMouth)));
                        break;
                    case 0x25:
                        Functions.Add((FSetCharacter) deserial.Deserialize(s, typeof(FSetCharacter)));
                        break;
                    case 0x26:
                        Functions.Add((FEditMove) deserial.Deserialize(s, typeof(FEditMove)));
                        break;
                    case 0x27:
                        Functions.Add((FEditShadow) deserial.Deserialize(s, typeof(FEditShadow)));
                        break;
                    case 0x28:
                        Functions.Add((FEditEyelid) deserial.Deserialize(s, typeof(FEditEyelid)));
                        break;
                    case 0x29:
                        Functions.Add((FEditEye) deserial.Deserialize(s, typeof(FEditEye)));
                        break;
                    case 0x2A:
                        Functions.Add((FEditItem) deserial.Deserialize(s, typeof(FEditItem)));
                        break;
                    case 0x2B:
                        Functions.Add((FEditEffect) deserial.Deserialize(s, typeof(FEditEffect)));
                        break;
                    case 0x2C:
                        Functions.Add((FEditDisp) deserial.Deserialize(s, typeof(FEditDisp)));
                        break;
                    case 0x2D:
                        Functions.Add((FEditHandAnim) deserial.Deserialize(s, typeof(FEditHandAnim)));
                        break;
                    case 0x2E:
                        Functions.Add((FAim) deserial.Deserialize(s, typeof(FAim)));
                        break;
                    case 0x2F:
                        Functions.Add((FHandItem) deserial.Deserialize(s, typeof(FHandItem)));
                        break;
                    case 0x30:
                        Functions.Add((FEditBlush) deserial.Deserialize(s, typeof(FEditBlush)));
                        break;
                    case 0x31:
                        Functions.Add((FNearClip) deserial.Deserialize(s, typeof(FNearClip)));
                        break;
                    case 0x32:
                        Functions.Add((FClothWet) deserial.Deserialize(s, typeof(FClothWet)));
                        break;
                    case 0x33:
                        Functions.Add((FLightRot) deserial.Deserialize(s, typeof(FLightRot)));
                        break;
                    case 0x34:
                        Functions.Add((FSceneFade) deserial.Deserialize(s, typeof(FSceneFade)));
                        break;
                    case 0x35:
                        Functions.Add((FToneTrans) deserial.Deserialize(s, typeof(FToneTrans)));
                        break;
                    case 0x36:
                        Functions.Add((FSaturate) deserial.Deserialize(s, typeof(FSaturate)));
                        break;
                    case 0x37:
                        Functions.Add((FFadeMode) deserial.Deserialize(s, typeof(FFadeMode)));
                        break;
                    case 0x38:
                        Functions.Add((FAutoBlink) deserial.Deserialize(s, typeof(FAutoBlink)));
                        break;
                    case 0x39:
                        Functions.Add((FPartsDisp) deserial.Deserialize(s, typeof(FPartsDisp)));
                        break;
                    case 0x3A:
                        Functions.Add((FTargetFlyingTime) deserial.Deserialize(s, typeof(FTargetFlyingTime)));
                        break;
                    case 0x3B:
                        Functions.Add((FCharacterSize) deserial.Deserialize(s, typeof(FCharacterSize)));
                        break;
                    case 0x3C:
                        Functions.Add((FCharacterHeightAdjust) deserial.Deserialize(s, typeof(FCharacterHeightAdjust)));
                        break;
                    case 0x3D:
                        Functions.Add((FItemAnim) deserial.Deserialize(s, typeof(FItemAnim)));
                        break;
                    case 0x3E:
                        Functions.Add((FCharacterPosAdjust) deserial.Deserialize(s, typeof(FCharacterPosAdjust)));
                        break;
                    case 0x3F:
                        Functions.Add((FSceneRot) deserial.Deserialize(s, typeof(FSceneRot)));
                        break;
                    case 0x40:
                        Functions.Add((FEditMotSmoothLen) deserial.Deserialize(s, typeof(FEditMotSmoothLen)));
                        break;
                    case 0x41:
                        Functions.Add((FPvBranchMode) deserial.Deserialize(s, typeof(FPvBranchMode)));
                        break;
                    case 0x42:
                        Functions.Add((FDataCameraStart) deserial.Deserialize(s, typeof(FDataCameraStart)));
                        break;
                    case 0x43:
                        Functions.Add((FMoviePlay) deserial.Deserialize(s, typeof(FMoviePlay)));
                        break;
                    case 0x44:
                        Functions.Add((FMovieDisplay) deserial.Deserialize(s, typeof(FMovieDisplay)));
                        break;
                    case 0x45:
                        Functions.Add((FWind) deserial.Deserialize(s, typeof(FWind)));
                        break;
                    case 0x46:
                        Functions.Add((FOsageStep) deserial.Deserialize(s, typeof(FOsageStep)));
                        break;
                    case 0x47:
                        Functions.Add((FOsageMoveCollider) deserial.Deserialize(s, typeof(FOsageMoveCollider)));
                        break;
                    case 0x48:
                        Functions.Add((FCharacterColor) deserial.Deserialize(s, typeof(FCharacterColor)));
                        break;
                    case 0x49:
                        Functions.Add((FSeEffect) deserial.Deserialize(s, typeof(FSeEffect)));
                        break;
                    case 0x4A:
                        Functions.Add((FEditMoveXYZ) deserial.Deserialize(s, typeof(FEditMoveXYZ)));
                        break;
                    case 0x4B:
                        Functions.Add((FEditEyelidAnim) deserial.Deserialize(s, typeof(FEditEyelidAnim)));
                        break;
                    case 0x4C:
                        Functions.Add((FEditInstrumentItem) deserial.Deserialize(s, typeof(FEditInstrumentItem)));
                        break;
                    case 0x4D:
                        Functions.Add((FEditMotionLoop) deserial.Deserialize(s, typeof(FEditMotionLoop)));
                        break;
                    case 0x4E:
                        Functions.Add((FEditExpression) deserial.Deserialize(s, typeof(FEditExpression)));
                        break;
                    case 0x4F:
                        Functions.Add((FEditEyeAnim) deserial.Deserialize(s, typeof(FEditEyeAnim)));
                        break;
                    case 0x50:
                        Functions.Add((FEditMouthAnim) deserial.Deserialize(s, typeof(FEditMouthAnim)));
                        break;
                    case 0x51:
                        Functions.Add((FEditCamera) deserial.Deserialize(s, typeof(FEditCamera)));
                        break;
                    case 0x52:
                        Functions.Add((FEditModeSelect) deserial.Deserialize(s, typeof(FEditModeSelect)));
                        break;
                    default:
                        Console.Write("Unknown opcode: 0x{0:X} | {0}\n", readFuncId);
                        Console.Write("Last func: {0}\n", Functions[Functions.Count - 1]);
                        EOF = true;
                        return;
                }
            }
        }

        public void Serialize(Stream s, Endianness endianness, BinarySerializationContext context)
        {
            var serial = new BinarySerializer();
            serial.Serialize(s, Header);
            serial.Serialize(s, Unk);
            foreach (var function in Functions)
                serial.Serialize(s, function);
            while (s.Position < Header.byteSize)
                DataStream.WriteInt32(s, 0);
        }
        */
        public override void BinSerialize(Stream s)
        {
            var serializer = new BinarySerializer();
            serializer.Serialize(s, this);
        }

        public override dynamic BinDeserialize(Stream s)
        {
            var serializer = new BinarySerializer();
            var dsc = (F2DscFile) serializer.Deserialize(s, typeof(F2DscFile));
            return dsc;
        }

        public override void XmlSerialize(Stream s)
        {
            var serializer = new XmlSerializer(typeof(F2DscFile));
            serializer.Serialize(s, this);
        }

        public override dynamic XmlDeserialize(Stream s)
        {
            var serializer = new XmlSerializer(typeof(F2DscFile));
            var dsc = (F2DscFile) serializer.Deserialize(s);
            return dsc;
        }
    }

    public class F2DscHeader
    {
        [FieldOrder(1)] public uint byteSize;

        [XmlIgnore] [FieldOrder(4)] public uint ebr;

        [XmlIgnore] [FieldOrder(6)] public double ebr1;

        [XmlIgnore] [FieldOrder(9)] public double ebr2;

        [XmlIgnore] [FieldOrder(12)] public double ebr3;

        [XmlIgnore] [FieldOrder(13)] public double ebr4;

        [FieldOrder(2)] public uint headerSize = 64;

        [XmlAttribute("version")] [FieldOrder(0)] public uint magic = 1347834691; //PVSC

        [FieldOrder(5)] public uint subfileSize;

        [FieldOrder(7)] public uint unk2;

        [XmlIgnore] [FieldOrder(8)] public uint unk3;

        [FieldOrder(10)] public uint unk4;

        [XmlIgnore] [FieldOrder(11)] public uint unk5;

        [FieldOrder(3)] public uint version = 0x18000000;
    }

    public class F2Dscunk
    {
        [FieldOrder(0)] private uint unk1;

        [FieldOrder(1)] private uint unk2;
    }

    public class F2DscEnd
    {
        [FieldOrder(4)] public double ebr1;

        [FieldOrder(5)] public int ebr2;

        [FieldOrder(6)] public byte ebr3;

        [FieldOrder(0)] public string Id = "EOFC";

        [FieldOrder(3)] public int size = 16;

        [FieldOrder(1)] public int unk1;

        [FieldOrder(2)] public int unk2 = 0x20000000;
    }

    public class SerializationTest : IBinarySerializable
    {
        [FieldOrder(0)]  public uint test1;
        [FieldOrder(1)] public string test2;
        [FieldOrder(2)] public bool test3;

        public void Serialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
        {
            DataStream.WriteUInt32(stream, test1);
            DataStream.WriteCString(stream, test2);
            DataStream.WriteBoolean(stream, test3);
        }

        public void Deserialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
        {
            test1 = DataStream.ReadUInt32(stream);
            test2 = DataStream.ReadCString(stream);
            test3 = DataStream.ReadBoolean(stream);
        }
    }

}