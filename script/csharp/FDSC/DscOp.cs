using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using BinarySerialization;
using System.Collections.Generic;
using DIVALib.IO;
using DIVALib.Math;

namespace DscOp
{
    public class DSC : IBinarySerializable
    {
        public DscFile file;

		public void Deserialize(Stream s, Endianness endianness, BinarySerializationContext context)
		{
            var magic = DataStream.ReadUInt32(s);
            s.Position -= 4;
            var serial = new BinarySerializer();
            switch(magic)
            {
                case 0x12020220: file = (FDscFile)serial.Deserialize(s, typeof(FDscFile)); break;
                case 0x43535650: serial.Endianness = Endianness.Big; 
                    file = (F2DSCFile)serial.Deserialize(s, typeof(F2DSCFile)); break;
                default: throw new Exception("invalid dsc file");
            }
		}

		public void Serialize(Stream s, Endianness endianness, BinarySerializationContext context)
		{
            var serial = new BinarySerializer();
            serial.Serialize(s, file);
		}

        public static DSC Deserialize(Stream s)
        {
			var serial = new BinarySerializer();
			return (DSC)serial.Deserialize(s, typeof(DSC));
        }

    }

    public abstract class DscFile
    {

        public abstract void BinSerialize(Stream s, bool close = true);
        public abstract dynamic BinDeserialize(Stream s, bool close = true);

        public abstract void XmlSerialize(Stream s, bool close = true);
        public abstract dynamic XmlDeserialize(Stream s, bool close = true);

    }

	[XmlRoot("dsc")]
	[XmlInclude(typeof(DSCFunc))]
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
	public class FDscFile : DscFile, IBinarySerializable
	{
		[XmlAttribute("version")]
		[FieldOrder(0)]
		public uint magic = 302121504;
		[XmlArray("functions")]
		[FieldOrder(1)]
		public List<DSCFunc> functions = new List<DSCFunc>();

		public void Deserialize(Stream s, Endianness endianness, BinarySerializationContext context)
		{
			magic = DataStream.ReadUInt32(s);
			bool EOF = false;
			Console.Write("File size is {0}\n", s.Length);
			while (!EOF)
			{
				Console.Write("Current position {0}\n", s.Position);
				if (s.Position >= s.Length)
				{
					EOF = true; break;
				}
                var endian = (endianness == Endianness.Little) ? DataStream.Endian.LittleEndian : DataStream.Endian.BigEndian;
				uint readFuncId = DataStream.ReadUInt32(s, endian);
				s.Position -= 4;
				BinarySerializer serial = new BinarySerializer();
                serial.Endianness = endianness;
				switch (readFuncId)
				{
					case 0x00: functions.Add((FEnd)serial.Deserialize(s, typeof(FEnd))); break;
					case 0x01: functions.Add((FTime)serial.Deserialize(s, typeof(FTime))); break;
					case 0x02: functions.Add((FMikuMove)serial.Deserialize(s, typeof(FMikuMove))); break;
					case 0x03: functions.Add((FMikuRotate)serial.Deserialize(s, typeof(FMikuRotate))); break;
					case 0x04: functions.Add((FMikuDisplay)serial.Deserialize(s, typeof(FMikuDisplay))); break;
					case 0x05: functions.Add((FMikuShadow)serial.Deserialize(s, typeof(FMikuShadow))); break;
					case 0x06: functions.Add((FTarget)serial.Deserialize(s, typeof(FTarget))); break;
					case 0x07: functions.Add((FSetMotion)serial.Deserialize(s, typeof(FSetMotion))); break;
					case 0x08: functions.Add((FSetPlaydata)serial.Deserialize(s, typeof(FSetPlaydata))); break;
					case 0x09: functions.Add((FEffect)serial.Deserialize(s, typeof(FEffect))); break;
					case 0x0A: functions.Add((FFadeinField)serial.Deserialize(s, typeof(FFadeinField))); break;
					case 0x0B: functions.Add((FEffectOff)serial.Deserialize(s, typeof(FEffectOff))); break;
					case 0x0C: functions.Add((FSetCamera)serial.Deserialize(s, typeof(FSetCamera))); break;
					case 0x0D: functions.Add((FDataCamera)serial.Deserialize(s, typeof(FDataCamera))); break;
					case 0x0E: functions.Add((FChangeField)serial.Deserialize(s, typeof(FChangeField))); break;
					case 0x0F: functions.Add((FHideField)serial.Deserialize(s, typeof(FHideField))); break;
					case 0x10: functions.Add((FMoveField)serial.Deserialize(s, typeof(FMoveField))); break;
					case 0x11: functions.Add((FFadeoutField)serial.Deserialize(s, typeof(FFadeoutField))); break;
					case 0x12: functions.Add((FEyeAnim)serial.Deserialize(s, typeof(FEyeAnim))); break;
					case 0x13: functions.Add((FMouthAnim)serial.Deserialize(s, typeof(FMouthAnim))); break;
					case 0x14: functions.Add((FHandAnim)serial.Deserialize(s, typeof(FHandAnim))); break;
					case 0x15: functions.Add((FLookAnim)serial.Deserialize(s, typeof(FLookAnim))); break;
					case 0x16: functions.Add((FExpression)serial.Deserialize(s, typeof(FExpression))); break;
					case 0x17: functions.Add((FLookCamera)serial.Deserialize(s, typeof(FLookCamera))); break;
					case 0x18: functions.Add((FLyric)serial.Deserialize(s, typeof(FLyric))); break;
					case 0x19: functions.Add((FMusicPlay)serial.Deserialize(s, typeof(FMusicPlay))); break;
					case 0x1A: functions.Add((FModeSelect)serial.Deserialize(s, typeof(FModeSelect))); break;
					case 0x1B: functions.Add((FEditMotion)serial.Deserialize(s, typeof(FEditMotion))); break;
					case 0x1C: functions.Add((FBarTimeSet)serial.Deserialize(s, typeof(FBarTimeSet))); break;
					case 0x1D: functions.Add((FShadowheight)serial.Deserialize(s, typeof(FShadowheight))); break;
					case 0x1E: functions.Add((FEditFace)serial.Deserialize(s, typeof(FEditFace))); break;
					case 0x1F: functions.Add((FMoveCamera)serial.Deserialize(s, typeof(FMoveCamera))); break;
					case 0x20: functions.Add((FPvEnd)serial.Deserialize(s, typeof(FPvEnd))); break;
					case 0x21: functions.Add((FShadowpos)serial.Deserialize(s, typeof(FShadowpos))); break;
					case 0x22: functions.Add((FEditLyric)serial.Deserialize(s, typeof(FEditLyric))); break;
					case 0x23: functions.Add((FEditTarget)serial.Deserialize(s, typeof(FEditTarget))); break;
					case 0x24: functions.Add((FEditMouth)serial.Deserialize(s, typeof(FEditMouth))); break;
					case 0x25: functions.Add((FSetCharacter)serial.Deserialize(s, typeof(FSetCharacter))); break;
					case 0x26: functions.Add((FEditMove)serial.Deserialize(s, typeof(FEditMove))); break;
					case 0x27: functions.Add((FEditShadow)serial.Deserialize(s, typeof(FEditShadow))); break;
					case 0x28: functions.Add((FEditEyelid)serial.Deserialize(s, typeof(FEditEyelid))); break;
					case 0x29: functions.Add((FEditEye)serial.Deserialize(s, typeof(FEditEye))); break;
					case 0x2A: functions.Add((FEditItem)serial.Deserialize(s, typeof(FEditItem))); break;
					case 0x2B: functions.Add((FEditEffect)serial.Deserialize(s, typeof(FEditEffect))); break;
					case 0x2C: functions.Add((FEditDisp)serial.Deserialize(s, typeof(FEditDisp))); break;
					case 0x2D: functions.Add((FEditHandAnim)serial.Deserialize(s, typeof(FEditHandAnim))); break;
					case 0x2E: functions.Add((FAim)serial.Deserialize(s, typeof(FAim))); break;
					case 0x2F: functions.Add((FHandItem)serial.Deserialize(s, typeof(FHandItem))); break;
					case 0x30: functions.Add((FEditBlush)serial.Deserialize(s, typeof(FEditBlush))); break;
					case 0x31: functions.Add((FNearClip)serial.Deserialize(s, typeof(FNearClip))); break;
					case 0x32: functions.Add((FClothWet)serial.Deserialize(s, typeof(FClothWet))); break;
					case 0x33: functions.Add((FLightRot)serial.Deserialize(s, typeof(FLightRot))); break;
					case 0x34: functions.Add((FSceneFade)serial.Deserialize(s, typeof(FSceneFade))); break;
					case 0x35: functions.Add((FToneTrans)serial.Deserialize(s, typeof(FToneTrans))); break;
					case 0x36: functions.Add((FSaturate)serial.Deserialize(s, typeof(FSaturate))); break;
					case 0x37: functions.Add((FFadeMode)serial.Deserialize(s, typeof(FFadeMode))); break;
					case 0x38: functions.Add((FAutoBlink)serial.Deserialize(s, typeof(FAutoBlink))); break;
					case 0x39: functions.Add((FPartsDisp)serial.Deserialize(s, typeof(FPartsDisp))); break;
					case 0x3A: functions.Add((FTargetFlyingTime)serial.Deserialize(s, typeof(FTargetFlyingTime))); break;
					case 0x3B: functions.Add((FCharacterSize)serial.Deserialize(s, typeof(FCharacterSize))); break;
					case 0x3C: functions.Add((FCharacterHeightAdjust)serial.Deserialize(s, typeof(FCharacterHeightAdjust))); break;
					case 0x3D: functions.Add((FItemAnim)serial.Deserialize(s, typeof(FItemAnim))); break;
					case 0x3E: functions.Add((FCharacterPosAdjust)serial.Deserialize(s, typeof(FCharacterPosAdjust))); break;
					case 0x3F: functions.Add((FSceneRot)serial.Deserialize(s, typeof(FSceneRot))); break;
					case 0x40: functions.Add((FEditMotSmoothLen)serial.Deserialize(s, typeof(FEditMotSmoothLen))); break;
					case 0x41: functions.Add((FPvBranchMode)serial.Deserialize(s, typeof(FPvBranchMode))); break;
					case 0x42: functions.Add((FDataCameraStart)serial.Deserialize(s, typeof(FDataCameraStart))); break;
					case 0x43: functions.Add((FMoviePlay)serial.Deserialize(s, typeof(FMoviePlay))); break;
					case 0x44: functions.Add((FMovieDisplay)serial.Deserialize(s, typeof(FMovieDisplay))); break;
					case 0x45: functions.Add((FWind)serial.Deserialize(s, typeof(FWind))); break;
					case 0x46: functions.Add((FOsageStep)serial.Deserialize(s, typeof(FOsageStep))); break;
					case 0x47: functions.Add((FOsageMoveCollider)serial.Deserialize(s, typeof(FOsageMoveCollider))); break;
					case 0x48: functions.Add((FCharacterColor)serial.Deserialize(s, typeof(FCharacterColor))); break;
					case 0x49: functions.Add((FSeEffect)serial.Deserialize(s, typeof(FSeEffect))); break;
					case 0x4A: functions.Add((FEditMoveXYZ)serial.Deserialize(s, typeof(FEditMoveXYZ))); break;
					case 0x4B: functions.Add((FEditEyelidAnim)serial.Deserialize(s, typeof(FEditEyelidAnim))); break;
					case 0x4C: functions.Add((FEditInstrumentItem)serial.Deserialize(s, typeof(FEditInstrumentItem))); break;
					case 0x4D: functions.Add((FEditMotionLoop)serial.Deserialize(s, typeof(FEditMotionLoop))); break;
					case 0x4E: functions.Add((FEditExpression)serial.Deserialize(s, typeof(FEditExpression))); break;
					case 0x4F: functions.Add((FEditEyeAnim)serial.Deserialize(s, typeof(FEditEyeAnim))); break;
					case 0x50: functions.Add((FEditMouthAnim)serial.Deserialize(s, typeof(FEditMouthAnim))); break;
					case 0x51: functions.Add((FEditCamera)serial.Deserialize(s, typeof(FEditCamera))); break;
					case 0x52: functions.Add((FEditModeSelect)serial.Deserialize(s, typeof(FEditModeSelect))); break;
					default:
						Console.Write("Unknown opcode: 0x{0:X}\n", readFuncId);
						Console.Write("Last func: {0}\n", functions[functions.Count - 1]);
                        foreach (DSCFunc func in functions)
                        {
                            Console.Write("{0}\n", func);
                        }
						EOF = true; return;
				}
			}
		}

		public void Serialize(Stream s, Endianness endianness, BinarySerializationContext context)
		{
			DataStream.WriteUInt32(s, magic);
			BinarySerializer serial = new BinarySerializer();
			foreach (DSCFunc function in functions)
			{
				serial.Serialize(s, function);
			}
		}

		public override void BinSerialize(Stream s, bool close = true)
		{
			BinarySerializer serializer = new BinarySerializer();
			serializer.Serialize(s, this);
			if (!close) { s.Close(); }
		}

		public override dynamic BinDeserialize(Stream s, bool close = true)
		{
			BinarySerializer serializer = new BinarySerializer();
			FDscFile dsc = (FDscFile)serializer.Deserialize(s, typeof(FDscFile));
			if (!close) { s.Close(); }
			return dsc;
		}

		public override void  XmlSerialize(Stream s, bool close = true)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(FDscFile));
			serializer.Serialize(s, this);
			if (!close) { s.Close(); }
		}

		public override dynamic XmlDeserialize(Stream s, bool close = true)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(FDscFile));
			FDscFile dsc = serializer.Deserialize(s) as FDscFile;
			if (!close) { s.Close(); }
			return dsc;
		}
	}

	[XmlRoot("dsc")]
	[XmlInclude(typeof(DSCFunc))]
	[XmlInclude(typeof(FEnd))]
	[XmlInclude(typeof(FTime))]
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
    //F2nd dsc body
    public class F2DSCFile : DscFile, IBinarySerializable
    {
        [FieldOrder(0), FieldEndianness(Endianness.Little)]
        public F2DSCHeader header;
        [FieldOrder(1), FieldEndianness(Endianness.Big)]
        public F2DSCunk unk;
        [FieldOrder(2), FieldEndianness(Endianness.Big)]
        public List<DSCFunc> functions = new List<DSCFunc>();
        [FieldOrder(3), FieldEndianness(Endianness.Big), XmlIgnore]
        public F2DSCEnd end;

		public void Deserialize(Stream s, Endianness endianness, BinarySerializationContext context)
		{
			BinarySerializer serial = new BinarySerializer();
			header = (F2DSCHeader)serial.Deserialize(s, typeof(F2DSCHeader));
            unk = (F2DSCunk)serial.Deserialize(s, typeof(F2DSCunk));
			bool EOF = false;
			Console.Write("File size is {0}\n", s.Length);
			while (!EOF)
			{
				Console.Write("Current position {0}\n", s.Position);
                if (s.Position >= header.subfileSize - 60)
				{
					EOF = true; break;
				}
				var endian = (endianness == Endianness.Little) ? DataStream.Endian.LittleEndian : DataStream.Endian.BigEndian;
				uint readFuncId = DataStream.ReadUInt32(s, endian);
				s.Position -= 4;
				BinarySerializer deserial = new BinarySerializer();
				deserial.Endianness = endianness;
				switch (readFuncId)
				{
					//case 0x00: functions.Add((FEnd)deserial.Deserialize(s, typeof(FEnd))); break;
                    case 0x00: functions.Add((FEnd)deserial.Deserialize(s, typeof(FEnd))); break;
					case 0x01: functions.Add((FTime)deserial.Deserialize(s, typeof(FTime))); break;
					case 0x02: functions.Add((FMikuMove)deserial.Deserialize(s, typeof(FMikuMove))); break;
					case 0x03: functions.Add((FMikuRotate)deserial.Deserialize(s, typeof(FMikuRotate))); break;
					case 0x04: functions.Add((FMikuDisplay)deserial.Deserialize(s, typeof(FMikuDisplay))); break;
					case 0x05: functions.Add((FMikuShadow)deserial.Deserialize(s, typeof(FMikuShadow))); break;
                    case 0x06: functions.Add((F2Target)deserial.Deserialize(s, typeof(F2Target))); break;
					case 0x07: functions.Add((FSetMotion)deserial.Deserialize(s, typeof(FSetMotion))); break;
					case 0x08: functions.Add((FSetPlaydata)deserial.Deserialize(s, typeof(FSetPlaydata))); break;
					case 0x09: functions.Add((FEffect)deserial.Deserialize(s, typeof(FEffect))); break;
					case 0x0A: functions.Add((FFadeinField)deserial.Deserialize(s, typeof(FFadeinField))); break;
					case 0x0B: functions.Add((FEffectOff)deserial.Deserialize(s, typeof(FEffectOff))); break;
					case 0x0C: functions.Add((FSetCamera)deserial.Deserialize(s, typeof(FSetCamera))); break;
					case 0x0D: functions.Add((FDataCamera)deserial.Deserialize(s, typeof(FDataCamera))); break;
					case 0x0E: functions.Add((FChangeField)deserial.Deserialize(s, typeof(FChangeField))); break;
					case 0x0F: functions.Add((FHideField)deserial.Deserialize(s, typeof(FHideField))); break;
					case 0x10: functions.Add((FMoveField)deserial.Deserialize(s, typeof(FMoveField))); break;
					case 0x11: functions.Add((FFadeoutField)deserial.Deserialize(s, typeof(FFadeoutField))); break;
					case 0x12: functions.Add((FEyeAnim)deserial.Deserialize(s, typeof(FEyeAnim))); break;
					case 0x13: functions.Add((FMouthAnim)deserial.Deserialize(s, typeof(FMouthAnim))); break;
					case 0x14: functions.Add((FHandAnim)deserial.Deserialize(s, typeof(FHandAnim))); break;
					case 0x15: functions.Add((FLookAnim)deserial.Deserialize(s, typeof(FLookAnim))); break;
					case 0x16: functions.Add((FExpression)deserial.Deserialize(s, typeof(FExpression))); break;
					case 0x17: functions.Add((FLookCamera)deserial.Deserialize(s, typeof(FLookCamera))); break;
					case 0x18: functions.Add((FLyric)deserial.Deserialize(s, typeof(FLyric))); break;
					case 0x19: functions.Add((FMusicPlay)deserial.Deserialize(s, typeof(FMusicPlay))); break;
					case 0x1A: functions.Add((FModeSelect)deserial.Deserialize(s, typeof(FModeSelect))); break;
					case 0x1B: functions.Add((FEditMotion)deserial.Deserialize(s, typeof(FEditMotion))); break;
					case 0x1C: functions.Add((FBarTimeSet)deserial.Deserialize(s, typeof(FBarTimeSet))); break;
					case 0x1D: functions.Add((FShadowheight)deserial.Deserialize(s, typeof(FShadowheight))); break;
					case 0x1E: functions.Add((FEditFace)deserial.Deserialize(s, typeof(FEditFace))); break;
					case 0x1F: functions.Add((FMoveCamera)deserial.Deserialize(s, typeof(FMoveCamera))); break;
					case 0x20: functions.Add((FPvEnd)deserial.Deserialize(s, typeof(FPvEnd))); break;
					case 0x21: functions.Add((FShadowpos)deserial.Deserialize(s, typeof(FShadowpos))); break;
					case 0x22: functions.Add((FEditLyric)deserial.Deserialize(s, typeof(FEditLyric))); break;
					case 0x23: functions.Add((FEditTarget)deserial.Deserialize(s, typeof(FEditTarget))); break;
					case 0x24: functions.Add((FEditMouth)deserial.Deserialize(s, typeof(FEditMouth))); break;
					case 0x25: functions.Add((FSetCharacter)deserial.Deserialize(s, typeof(FSetCharacter))); break;
					case 0x26: functions.Add((FEditMove)deserial.Deserialize(s, typeof(FEditMove))); break;
					case 0x27: functions.Add((FEditShadow)deserial.Deserialize(s, typeof(FEditShadow))); break;
					case 0x28: functions.Add((FEditEyelid)deserial.Deserialize(s, typeof(FEditEyelid))); break;
					case 0x29: functions.Add((FEditEye)deserial.Deserialize(s, typeof(FEditEye))); break;
					case 0x2A: functions.Add((FEditItem)deserial.Deserialize(s, typeof(FEditItem))); break;
					case 0x2B: functions.Add((FEditEffect)deserial.Deserialize(s, typeof(FEditEffect))); break;
					case 0x2C: functions.Add((FEditDisp)deserial.Deserialize(s, typeof(FEditDisp))); break;
					case 0x2D: functions.Add((FEditHandAnim)deserial.Deserialize(s, typeof(FEditHandAnim))); break;
					case 0x2E: functions.Add((FAim)deserial.Deserialize(s, typeof(FAim))); break;
					case 0x2F: functions.Add((FHandItem)deserial.Deserialize(s, typeof(FHandItem))); break;
					case 0x30: functions.Add((FEditBlush)deserial.Deserialize(s, typeof(FEditBlush))); break;
					case 0x31: functions.Add((FNearClip)deserial.Deserialize(s, typeof(FNearClip))); break;
					case 0x32: functions.Add((FClothWet)deserial.Deserialize(s, typeof(FClothWet))); break;
					case 0x33: functions.Add((FLightRot)deserial.Deserialize(s, typeof(FLightRot))); break;
					case 0x34: functions.Add((FSceneFade)deserial.Deserialize(s, typeof(FSceneFade))); break;
					case 0x35: functions.Add((FToneTrans)deserial.Deserialize(s, typeof(FToneTrans))); break;
					case 0x36: functions.Add((FSaturate)deserial.Deserialize(s, typeof(FSaturate))); break;
					case 0x37: functions.Add((FFadeMode)deserial.Deserialize(s, typeof(FFadeMode))); break;
					case 0x38: functions.Add((FAutoBlink)deserial.Deserialize(s, typeof(FAutoBlink))); break;
					case 0x39: functions.Add((FPartsDisp)deserial.Deserialize(s, typeof(FPartsDisp))); break;
					case 0x3A: functions.Add((FTargetFlyingTime)deserial.Deserialize(s, typeof(FTargetFlyingTime))); break;
					case 0x3B: functions.Add((FCharacterSize)deserial.Deserialize(s, typeof(FCharacterSize))); break;
					case 0x3C: functions.Add((FCharacterHeightAdjust)deserial.Deserialize(s, typeof(FCharacterHeightAdjust))); break;
					case 0x3D: functions.Add((FItemAnim)deserial.Deserialize(s, typeof(FItemAnim))); break;
					case 0x3E: functions.Add((FCharacterPosAdjust)deserial.Deserialize(s, typeof(FCharacterPosAdjust))); break;
					case 0x3F: functions.Add((FSceneRot)deserial.Deserialize(s, typeof(FSceneRot))); break;
					case 0x40: functions.Add((FEditMotSmoothLen)deserial.Deserialize(s, typeof(FEditMotSmoothLen))); break;
					case 0x41: functions.Add((FPvBranchMode)deserial.Deserialize(s, typeof(FPvBranchMode))); break;
					case 0x42: functions.Add((FDataCameraStart)deserial.Deserialize(s, typeof(FDataCameraStart))); break;
					case 0x43: functions.Add((FMoviePlay)deserial.Deserialize(s, typeof(FMoviePlay))); break;
					case 0x44: functions.Add((FMovieDisplay)deserial.Deserialize(s, typeof(FMovieDisplay))); break;
					case 0x45: functions.Add((FWind)deserial.Deserialize(s, typeof(FWind))); break;
					case 0x46: functions.Add((FOsageStep)deserial.Deserialize(s, typeof(FOsageStep))); break;
					case 0x47: functions.Add((FOsageMoveCollider)deserial.Deserialize(s, typeof(FOsageMoveCollider))); break;
					case 0x48: functions.Add((FCharacterColor)deserial.Deserialize(s, typeof(FCharacterColor))); break;
					case 0x49: functions.Add((FSeEffect)deserial.Deserialize(s, typeof(FSeEffect))); break;
					case 0x4A: functions.Add((FEditMoveXYZ)deserial.Deserialize(s, typeof(FEditMoveXYZ))); break;
					case 0x4B: functions.Add((FEditEyelidAnim)deserial.Deserialize(s, typeof(FEditEyelidAnim))); break;
					case 0x4C: functions.Add((FEditInstrumentItem)deserial.Deserialize(s, typeof(FEditInstrumentItem))); break;
					case 0x4D: functions.Add((FEditMotionLoop)deserial.Deserialize(s, typeof(FEditMotionLoop))); break;
					case 0x4E: functions.Add((FEditExpression)deserial.Deserialize(s, typeof(FEditExpression))); break;
					case 0x4F: functions.Add((FEditEyeAnim)deserial.Deserialize(s, typeof(FEditEyeAnim))); break;
					case 0x50: functions.Add((FEditMouthAnim)deserial.Deserialize(s, typeof(FEditMouthAnim))); break;
					case 0x51: functions.Add((FEditCamera)deserial.Deserialize(s, typeof(FEditCamera))); break;
					case 0x52: functions.Add((FEditModeSelect)deserial.Deserialize(s, typeof(FEditModeSelect))); break;
					default:
                        Console.Write("Unknown opcode: 0x{0:X} | {0}\n", readFuncId);
						Console.Write("Last func: {0}\n", functions[functions.Count - 1]);
						EOF = true; return;
				}
			}
		}

		public void Serialize(Stream s, Endianness endianness, BinarySerializationContext context)
		{
			BinarySerializer serial = new BinarySerializer();
            serial.Serialize(s, header);
            serial.Serialize(s, unk);
			foreach (DSCFunc function in functions)
			{
				serial.Serialize(s, function);
			}
            while (s.Position < header.byteSize)
            {
                DataStream.WriteInt32(s, 0);
            }
		}

		public override void BinSerialize(Stream s, bool close = true)
		{
			BinarySerializer serializer = new BinarySerializer();
			serializer.Serialize(s, this);
			if (!close) { s.Close(); }
		}

		public override dynamic BinDeserialize(Stream s, bool close = true)
		{
			var serializer = new BinarySerializer();
			var dsc = (F2DSCFile)serializer.Deserialize(s, typeof(F2DSCFile));
			if (!close) { s.Close(); }
			return dsc;
		}

		public override void XmlSerialize(Stream s, bool close = true)
		{
			var serializer = new XmlSerializer(typeof(F2DSCFile));
			serializer.Serialize(s, this);
			if (!close) { s.Close(); }
		}

		public override dynamic XmlDeserialize(Stream s, bool close = true)
		{
			var serializer = new XmlSerializer(typeof(FDscFile));
			var dsc = serializer.Deserialize(s) as F2DSCFile;
			if (!close) { s.Close(); }
			return dsc;
		}

    }

    public class F2DSCHeader
    {
		[XmlAttribute("version")]
		[FieldOrder(0)]
		public uint magic = 1347834691; //PVSC
		[FieldOrder(1)]
		public uint byteSize;
		[FieldOrder(2)]
		public uint headerSize = 64;
		[FieldOrder(3)]
		public uint version = 0x18000000;
		[XmlIgnore, FieldOrder(4)]
		public uint ebr;
		[FieldOrder(5)]
        public uint subfileSize;
		[XmlIgnore, FieldOrder(6)]
		public double ebr1;
		[FieldOrder(7)]
		public uint unk2;
		[XmlIgnore, FieldOrder(8)]
		public uint unk3;
		[XmlIgnore, FieldOrder(9)]
		public double ebr2;
		[FieldOrder(10)]
		public uint unk4;
		[XmlIgnore, FieldOrder(11)]
		public uint unk5;
		[XmlIgnore, FieldOrder(12)]
		public double ebr3;
		[XmlIgnore, FieldOrder(13)]
		public double ebr4;
    }

    public class F2DSCunk
    {
        [FieldOrder(0)]
        uint unk1;
        [FieldOrder(1)]
        uint unk2;
    }

	public class F2DSCEnd
	{
        [FieldOrder(0)]
        public string id = "EOFC";
        [FieldOrder(1)]
        public int unk1;
		[FieldOrder(2)]
		public int unk2 = 0x20000000;
		[FieldOrder(3)]
		public int size = 16;
        [FieldOrder(4)]
        public double ebr1;
		[FieldOrder(5)]
		public int ebr2;
		[FieldOrder(6)]
		public byte ebr3;
	}

	/// <summary>  
	///  Generic parent class for all Dsc functions
	/// </summary>  
	[Serializable]
	public abstract class DSCFunc
	{
		/// <summary>  
		///  The number that specifies which function to be used
		/// </summary>  
		[XmlIgnore]
		public int functionId;

		/// <summary>  
		///  The string representation of a DSC function
		/// </summary>  
		public override string ToString()
		{
			return string.Format(string.Format("[DSC Function] 0x{0:X}: {1}\n", functionId, GetType().Name));
		}
	}

	/// <summary>  
	///  Ends the song
	/// </summary>  
	public class FEnd : DSCFunc
	{
		public FEnd() { functionId = 0x00; }
		public uint unk;
	}

	/// <summary>  
	///  Makes the next function activate after set time
	/// </summary>  
	public class FTime : DSCFunc
	{
		public FTime() { functionId = 0x01; }
		[FieldOrder(0)]
		[FieldScale(100)]
		[SerializeAs(SerializedType.UInt4)]
		public double timeStamp;
	}

	/// <summary>  
	///  Moves the selected character to a specific position
	/// </summary>  
	public class FMikuMove : DSCFunc
	{
		public FMikuMove() { functionId = 0x02; }
		[FieldOrder(0)]
		public uint playerID;
		[FieldOrder(1)]
		public Vector3 position;
	}

	/// <summary>  
	///  Changes the selected character's orientation
	/// </summary>  
	public class FMikuRotate : DSCFunc
	{
		public FMikuRotate() { functionId = 0x03; }
		[FieldOrder(0)]
		public uint playerID;
		/// <summary>  
		/// The character's rotation on the Z axis
		/// </summary> 
		[FieldOrder(1)]
		public int orientation;
	}

	/// <summary>  
	///  Changes the selected character's display state
	/// </summary>  
	public class FMikuDisplay : DSCFunc
	{
		public FMikuDisplay() { functionId = 0x04; }
		[FieldOrder(0)]
		public uint playerID;
		[FieldOrder(1), SerializeAs(SerializedType.UInt4)]
		public bool state;
	}

	/// <summary>  
	///  Changes the selected character's shadow display state
	/// </summary>  
	public class FMikuShadow : DSCFunc
	{
		public FMikuShadow() { functionId = 0x05; }
		[FieldOrder(0)]
		public uint playerID;
		[FieldOrder(1), SerializeAs(SerializedType.UInt4)]
		public bool state;
	}

	/// <summary>  
	///  Creates a new target with the specified paramaters
	/// </summary>  
	public class FTarget : DSCFunc
	{
		public enum EType
		{
			TRIANGLE = 0,
			CIRCLE = 1,
			CROSS = 2,
			SQUARE = 3,
			TRIANGLE_DOUBLE = 4,
			CIRCLE_DOUBLE = 5,
			CROSS_DOUBLE = 6,
			SQUARE_DOUBLE = 7,
			TRIANGLE_HOLD = 8,
			CIRCLE_HOLD = 9,
			CROSS_HOLD = 10,
			SQUARE_HOLD = 11,
			STAR = 12,
			STAR_HOLD = 14,
			CHANCE_STAR = 15
		};

		public FTarget() { functionId = 0x06; }

		[FieldOrder(0)] public EType type;
		[FieldOrder(1), FieldScale(100), SerializeAs(SerializedType.Int4)]
		public double holdLength;
		[FieldOrder(2), SerializeAs(SerializedType.Int4)]
		public int isHoldEnd;
		[FieldOrder(3)] public Vector2 position;
		[FieldOrder(4), FieldScale(100_000), SerializeAs(SerializedType.Int4)]
		public double oscillateAngle;
		[FieldOrder(5)] public int oscillateFrequency;
		[FieldOrder(6), FieldScale(100_000), SerializeAs(SerializedType.Int4)]
		public double entryAngle;
		[FieldOrder(7)] public uint oscillateAmplitude;
		[FieldOrder(8), SerializeAs(SerializedType.Int4)]
		public double timeOut;
		[FieldOrder(9)] public int pad;
	}

	/// <summary>  
	///  Creates a new target with the specified paramaters
	/// </summary>  
	public class F2Target : DSCFunc
	{
		public enum EType
		{
			TRIANGLE = 0,
			CIRCLE = 1,
			CROSS = 2,
			SQUARE = 3,
			TRIANGLE_DOUBLE = 4,
			CIRCLE_DOUBLE = 5,
			CROSS_DOUBLE = 6,
			SQUARE_DOUBLE = 7,
			TRIANGLE_HOLD = 8,
			CIRCLE_HOLD = 9,
			CROSS_HOLD = 10,
			SQUARE_HOLD = 11,
			STAR = 12,
			STAR_DOUBLE = 14,
			CHANCE_STAR = 15,
			LINKED_STAR = 22,
			LINKED_STAR_END = 23
		};

        public F2Target() { functionId = 0x06; }

		[FieldOrder(0)] public EType type;
		[FieldOrder(1), FieldScale(100), SerializeAs(SerializedType.Int4)]
		public double holdLength;
		[FieldOrder(2), SerializeAs(SerializedType.Int4)]
		public int isHoldEnd;
		[FieldOrder(3)] public Vector2 position;
		[FieldOrder(4), FieldScale(100_000), SerializeAs(SerializedType.Int4)]
		public double oscillateAngle;
		[FieldOrder(5)] public int oscillateFrequency;
		[FieldOrder(6), FieldScale(100_000), SerializeAs(SerializedType.Int4)]
		public double entryAngle;
		[FieldOrder(7)] public uint oscillateAmplitude;
		[FieldOrder(8), SerializeAs(SerializedType.Int4)]
		public double timeOut;
		[FieldOrder(9)] public int unk;
        [FieldOrder(10)] public int pad;
	}

	/// <summary> Plays an animation on the character </summary>
	public class FSetMotion : DSCFunc
	{
		public FSetMotion() { functionId = 0x07; }
		/// <summary> Which player to affect </summary>
		[FieldOrder(0)]
		public int playerID;
		/// <summary> Which animation to play </summary>
		[FieldOrder(1)]
		public uint animID;
		/// <summary> Which frame the animation should start on </summary>
		[FieldOrder(2), FieldScale(100), SerializeAs(SerializedType.Int4)]
		public double time;
		/// <summary> Animation speed </summary>
		[FieldOrder(3)]
		public int speed;
	}

	/// <summary> Unknown </summary>
	public class FSetPlaydata : DSCFunc
	{
		public FSetPlaydata() { functionId = 0x08; }
		/// <summary> Which player to affect </summary>
		[FieldOrder(0)]
		public uint playerID;
		/// <summary> ??? </summary>
		[FieldOrder(1)]
		public uint mode;
	}

	/// <summary> Displays an Effect? </summary>
	/// <remark> From old DIVA, Doesn't do anything in new games </remark>
	public class FEffect : DSCFunc
	{
		public FEffect() { functionId = 0x09; }
		/// <summary> ???? </summary>
		[FieldOrder(0)]
		public Vector3 unk1;
		/// <summary> ???? </summary>
		[FieldOrder(1)]
		public Vector3 unk2;
	}

	/// <summary> ??? </summary>
	/// <remark> From old DIVA, Doesn't do anything in new games </remark>
	public class FFadeinField : DSCFunc
	{
		public FFadeinField() { functionId = 0x0A; }

	}

	/// <summary> Hides an Effect? </summary>
	/// <remark> From old DIVA, Doesn't do anything in new games </remark>
	public class FEffectOff : DSCFunc
	{
		public FEffectOff() { functionId = 0x0B; }
		/// <summary> ???? </summary>
		[FieldOrder(0)]
		public Vector3 unk1;
		/// <summary> ???? </summary>
		[FieldOrder(1)]
		public Vector3 unk2;
	}

	/// <summary>  ?? </summary>
	/// <remark> Something with the cameras. Haven't looked into it </remark>
	public class FSetCamera : DSCFunc
	{
		public FSetCamera() { functionId = 0x0C; }

	}

	/// <summary> Works the same as FSetCamera? </summary>
	public class FDataCamera : DSCFunc
	{
		public FDataCamera() { functionId = 0x0D; }

	}

	/// <summary> Changes the stage </summary>
	public class FChangeField : DSCFunc
	{
		public FChangeField() { functionId = 0x0E; }
		/// <summary> New stage to set. goes by pv_db / pv_field </summary>
		[FieldOrder(0)]
		public uint fieldID;
	}

	/// <summary> Hides the field </summary>
	/// <remark> Haven't looked into it </remark>
	public class FHideField : DSCFunc
	{
		public FHideField() { functionId = 0x0F; }
		/// <summary> Visibility toggle? </summary>
		[FieldOrder(0)]
		public uint state;
	}

	/// <summary> Moves the field </summary>
	/// <remark> Haven't looked into it </remark>
	public class FMoveField : DSCFunc
	{
		public FMoveField() { functionId = 0x10; }

	}

	/// <summary> Acts the same as FFadeInField </summary>
	/// <remark> From old DIVA, Doesn't do anything in new games </remark>
	public class FFadeoutField : DSCFunc
	{
		public FFadeoutField() { functionId = 0x11; }

	}

	/// <summary> Plays an animation on the character's eyes </summary>
	/// <remark> Only does blinking? </remark>
	public class FEyeAnim : DSCFunc
	{
		public FEyeAnim() { functionId = 0x12; }
		/// <summary> Which player to affect </summary>
		[FieldOrder(0)]
		public int playerID;
		/// <summary> Which animation to play? </summary>
		[FieldOrder(1)]
		public uint animID;
		/// <summary> ???? </summary>
		[FieldOrder(2)]
		public uint unk;
	}

	/// <summary> Plays mouth animations </summary>
	/// <remark> Isn't completely researched </remark>
	public class FMouthAnim : DSCFunc
	{
		public FMouthAnim() { functionId = 0x13; }
		/// <summary> Which player to affect </summary>
		[FieldOrder(0)]
		public int playerID;
		/// <summary> Which animation to play? </summary>
		[FieldOrder(1)]
		public uint animID;
		/// <summary> ??? </summary>
		[FieldOrder(2)]
		public int unk;
		/// <summary> When the animation should start </summary>
		[FieldOrder(3)]
		public int start;
		/// <summary> The animation speed, disabled if -1  </summary>
		[FieldOrder(4)]
		public int speed;
	}

	/// <summary> Hand motions, but haven't looked much into it </summary>
	public class FHandAnim : DSCFunc
	{
		public FHandAnim() { functionId = 0x14; }
		/// <summary> Which player to affect </summary>
		[FieldOrder(0)]
		public int playerID;
		/// <summary> Which animation to play? </summary>
		[FieldOrder(1)]
		public uint animID;
		/// <summary> ??? </summary>
		[FieldOrder(2)]
		public int unk;
		/// <summary> When the animation should start </summary>
		[FieldOrder(3)]
		public int start;
		/// <summary> The animation speed, disabled if - </summary>
		[FieldOrder(4)]
		public int speed;
	}

	/// <summary> Makes the character move their eyes, but haven't looked much into it </summary>
	public class FLookAnim : DSCFunc
	{
		public FLookAnim() { functionId = 0x15; }
		/// <summary> DESC </summary>
		[FieldOrder(0)]
		public uint var;
	}

	/// <summary> Expessions for the face, but haven't looked much into it </summary>
	public class FExpression : DSCFunc
	{
		public FExpression() { functionId = 0x16; }
		/// <summary> Which player to affect </summary>
		[FieldOrder(0)]
		public int playerID;
		/// <summary> Which expression to play </summary>
		[FieldOrder(1)]
		public uint expID;
		/// <summary> When the animation should start </summary>
		[FieldOrder(2)]
		public int start;
		/// <summary> The animation speed, disabled if - </summary>
		[FieldOrder(3)]
		public int speed;
	}

	/// <summary> No idea what this is for </summary>
	public class FLookCamera : DSCFunc
	{
		public FLookCamera() { functionId = 0x17; }
		/// <summary> DESC </summary>
		[FieldOrder(0)]
		public uint var;
	}

	/// <summary> Displays a lyric with the specified color </summary>
	public class FLyric : DSCFunc
	{
		public FLyric() { functionId = 0x18; }
		/// <summary> "Which lyric to display, Goes by pv_db" </summary>
		[FieldOrder(0)]
		public uint lyricID;
		/// <summary> "Color per byte" </summary>
		[FieldOrder(1)]
		public uint color;
	}

	/// <summary> Plays the music </summary>
	public class FMusicPlay : DSCFunc
	{
		public FMusicPlay() { functionId = 0x19; }

	}

	/// <summary> Sets the mode. e.g. Chance Time </summary>
	/// <remark> 4 in a row makes it playable in every difficulty </remark>
	public class FModeSelect : DSCFunc
	{
		public FModeSelect() { functionId = 0x1A; }
		/// <summary> DESC </summary>
		[FieldOrder(0)]
		public uint unk;
		/// <summary> DESC </summary>
		[FieldOrder(1)]
		public int modeID;
	}

	/// <summary> This doesn't work anymore. It was used in PSP's edit mode and in FT. However, the speed values still work. The motion ID is unfunctional </summary>
	public class FEditMotion : DSCFunc
	{
		public FEditMotion() { functionId = 0x1B; }
		/// <summary> Which animation to play </summary>
		[FieldOrder(0)]
		public uint animID;
		/// <summary> When to start the animation </summary>
		[FieldOrder(1)]
		public uint start;
		/// <summary> Speed of the animation </summary>
		[FieldOrder(2)]
		public uint speed;
	}

	/// <summary> First 4 bytes = bpm. This set the actual bpm number rather than time, like CD 00 00 00, which is 205 bpm. This doesn't do anything in Sega's dscs because of the bpm time in the notes. </summary>
	public class FBarTimeSet : DSCFunc
	{
		public FBarTimeSet() { functionId = 0x1C; }
		/// <summary> Actual BPM value </summary>
		[FieldOrder(0)]
		public int bpm;
		/// <summary> The speed of the notes </summary>
		[FieldOrder(1)]
		public int noteSpeed;
	}

	/// <summary> ???? </summary>
	public class FShadowheight : DSCFunc
	{
		public FShadowheight() { functionId = 0x1D; }
		/// <summary> ???? </summary>
		[FieldOrder(0)]
		public uint unk1;
		/// <summary> ???? </summary>
		[FieldOrder(1)]
		public uint unk2;
	}

	/// <summary> Haven't looked into this </summary>
	public class FEditFace : DSCFunc
	{
		public FEditFace() { functionId = 0x1E; }
		/// <summary> DESC </summary>
		[FieldOrder(0)]
		public uint var;
	}

	/// <summary> Moves camera. Doesn't reposition around character </summary>
	/// <remark> Values Not Researched </remark>
	public class FMoveCamera : DSCFunc
	{
		public FMoveCamera() { functionId = 0x1F; }

	}

	/// <summary> The point where the song ends </summary>
	public class FPvEnd : DSCFunc
	{
		public FPvEnd() { functionId = 0x20; }
		/// <summary> Doesn't really exist, made up to please Deserializing </summary>
		[FieldOrder(0)]
		public uint pad;
	}

	/// <summary> ???? </summary>
	public class FShadowpos : DSCFunc
	{
		public FShadowpos() { functionId = 0x21; }
		/// <summary> DESC </summary>
		[FieldOrder(0)]
		public uint var;
	}

	/// <summary>   </summary>
	/// <remark> From old DIVA, Doesn't do anything in new games </remark>
	public class FEditLyric : DSCFunc
	{
		public FEditLyric() { functionId = 0x22; }

	}

	/// <summary>   </summary>
	/// <remark>  </remark>
	public class FEditTarget : DSCFunc
	{
		public FEditTarget() { functionId = 0x23; }
		/// <summary> DESC </summary>
		[FieldOrder(0)]
		public uint var;
	}

	/// <summary> Edit variant of FMouthAnim </summary>
	public class FEditMouth : DSCFunc
	{
		public FEditMouth() { functionId = 0x24; }
		/// <summary> ??? </summary>
		[FieldOrder(0)]
		public int unk;
		/// <summary> When the animation should start </summary>
		[FieldOrder(1)]
		public int start;
		/// <summary> The animation speed, disabled if -1  </summary>
		[FieldOrder(2)]
		public int speed;
	}

	/// <summary> Sets the character for edit functions </summary>
	public class FSetCharacter : DSCFunc
	{
		public FSetCharacter() { functionId = 0x25; }
		/// <summary> Which player to affect </summary>
		[FieldOrder(0)]
		public int playerID;
	}

	/// <summary> Edit variant of FMikuMove </summary>
	public class FEditMove : DSCFunc
	{
		public FEditMove() { functionId = 0x26; }
		/// <summary> New position </summary>
		[FieldOrder(0)]
		public Vector3 position;
	}

	/// <summary> Edit variant of FMikuShadow </summary>
	public class FEditShadow : DSCFunc
	{
		public FEditShadow() { functionId = 0x27; }
		/// <summary> Visiblity toggle </summary>
		[FieldOrder(0)]
		public uint state;
	}

	/// <summary> Haven't looked into this </summary>
	public class FEditEyelid : DSCFunc
	{
		public FEditEyelid() { functionId = 0x28; }
		/// <summary> DESC </summary>
		[FieldOrder(0)]
		public uint var;
	}

	/// <summary> Same as above </summary>
	public class FEditEye : DSCFunc
	{
		public FEditEye() { functionId = 0x29; }
		/// <summary> DESC </summary>
		[FieldOrder(0)]
		public uint var;
	}

	/// <summary> First 4 bytes = I guess what hand it goes on for the player. For the player, it goes by SET_CHARA </summary>
	public class FEditItem : DSCFunc
	{
		public FEditItem() { functionId = 0x2A; }
		/// <summary> Which item to use, Goes by ???? </summary>
		[FieldOrder(0)]
		public int itemID;
	}

	/// <summary> EDIT Function: Sets an effect </summary>
	public class FEditEffect : DSCFunc
	{
		public FEditEffect() { functionId = 0x2B; }
		/// <summary> Which effect to display </summary>
		[FieldOrder(0)]
		public int fxID;
		/// <summary> How fast should the effect play </summary>
		[FieldOrder(1)]
		public int speed;
	}

	/// <summary> Basically MIKU_DISP with 4 less bytes </summary>
	public class FEditDisp : DSCFunc
	{
		public FEditDisp() { functionId = 0x2C; }
		/// <summary> Visibility toggle </summary>
		[FieldOrder(0)]
		public uint state;
	}

	/// <summary> No idea </summary>
	public class FEditHandAnim : DSCFunc
	{
		public FEditHandAnim() { functionId = 0x2D; }
		/// <summary> DESC </summary>
		[FieldOrder(0)]
		public uint var;
	}

	/// <summary> No idea </summary>
	public class FAim : DSCFunc
	{
		public FAim() { functionId = 0x2E; }
		/// <summary> DESC </summary>
		[FieldOrder(0)]
		public uint var;
	}

	/// <summary> I've looked into it, but I forgot what they were </summary>
	public class FHandItem : DSCFunc
	{
		public FHandItem() { functionId = 0x2F; }
		/// <summary> DESC </summary>
		[FieldOrder(0)]
		public uint var;
	}

	/// <summary> First 4 bytes = Blush ID </summary>
	public class FEditBlush : DSCFunc
	{
		public FEditBlush() { functionId = 0x30; }
		/// <summary> Which blush to display </summary>
		[FieldOrder(0)]
		public int blushID;
	}

	/// <summary> Sets the camera's near clip </summary>
	/// <remark> Speculation! </remark>
	public class FNearClip : DSCFunc
	{
		public FNearClip() { functionId = 0x31; }
		/// <summary> The new near clip value </summary>
		[FieldOrder(0)]
		public uint value;
	}

	/// <summary> No idea </summary>
	public class FClothWet : DSCFunc
	{
		public FClothWet() { functionId = 0x32; }
		/// <summary> DESC </summary>
		[FieldOrder(0)]
		public uint var;
	}

	/// <summary> No idea </summary>
	public class FLightRot : DSCFunc
	{
		public FLightRot() { functionId = 0x33; }
		/// <summary> DESC </summary>
		[FieldOrder(0)]
		public uint var;
	}

	/// <summary> First 4 bytes = Fade speed </summary>
	public class FSceneFade : DSCFunc
	{
		public FSceneFade() { functionId = 0x34; }
		/// <summary> How fast the fade should play </summary>
		[FieldOrder(0)]
		public uint speed;
		/// <summary> ??? </summary>
		[FieldOrder(1)]
		public uint fade_in;
		/// <summary> ??? </summary>
		[FieldOrder(2)]
		public uint fade_out;
		/// <summary> Fade's Red Channel </summary>
		[FieldOrder(3)]
		public uint red;
		/// <summary> Fade's Green Channel </summary>
		[FieldOrder(4)]
		public uint green;
		/// <summary> Fade's Blue Channel </summary>
		[FieldOrder(5)]
		public uint blue;
	}

	/// <summary> This does load, but always crashes when the function starts. It was used in FT </summary>
	public class FToneTrans : DSCFunc
	{
		public FToneTrans() { functionId = 0x35; }
		/// <summary> DESC </summary>
		[FieldOrder(0)]
		public uint var;
	}

	/// <summary> De/saturates the screen </summary>
	public class FSaturate : DSCFunc
	{
		public FSaturate() { functionId = 0x36; }
		/// <summary> Saturation amount </summary>
		[FieldOrder(0)]
		public uint amount;
	}

	/// <summary> No idea </summary>
	public class FFadeMode : DSCFunc
	{
		public FFadeMode() { functionId = 0x37; }
		/// <summary> DESC </summary>
		[FieldOrder(0)]
		public uint var;
	}

	/// <summary> No idea </summary>
	public class FAutoBlink : DSCFunc
	{
		public FAutoBlink() { functionId = 0x38; }
		/// <summary> DESC </summary>
		[FieldOrder(0)]
		public uint var;
	}

	/// <summary> No idea </summary>
	public class FPartsDisp : DSCFunc
	{
		public FPartsDisp() { functionId = 0x39; }
		/// <summary> DESC </summary>
		[FieldOrder(0)]
		public uint var;
	}

	/// <summary> Sets the BPM for every incoming note </summary>
	/// <remark>  FT Exclusive </remark>
	public class FTargetFlyingTime : DSCFunc
	{
		public FTargetFlyingTime() { functionId = 0x3A; }
		/// <summary> New BPM value </summary>
		[FieldOrder(0)]
		public uint bpm;
	}

	/// <summary> Sets the character's scaling </summary>
	public class FCharacterSize : DSCFunc
	{
		public FCharacterSize() { functionId = 0x3B; }
		/// <summary> Which player to affect </summary>
		[FieldOrder(0)]
		public int playerID;
		/// <summary> New size </summary>
		[FieldOrder(1)]
		public uint size;
	}

	/// <summary> No idea </summary>
	public class FCharacterHeightAdjust : DSCFunc
	{
		public FCharacterHeightAdjust() { functionId = 0x3C; }
		/// <summary> Which player to affect </summary>
		[FieldOrder(0)]
		public int playerID;
		/// <summary> New height </summary>
		[FieldOrder(1)]
		public uint height;
	}

	/// <summary> No idea </summary>
	public class FItemAnim : DSCFunc
	{
		public FItemAnim() { functionId = 0x3D; }
		/// <summary> DESC </summary>
		[FieldOrder(0)]
		public uint var;
	}

	/// <summary> No idea </summary>
	public class FCharacterPosAdjust : DSCFunc
	{
		public FCharacterPosAdjust() { functionId = 0x3E; }
		/// <summary> DESC </summary>
		[FieldOrder(0)]
		public uint var;
	}

	/// <summary> No idea </summary>
	public class FSceneRot : DSCFunc
	{
		public FSceneRot() { functionId = 0x3F; }
		/// <summary> DESC </summary>
		[FieldOrder(0)]
		public uint var;
	}

	/// <summary>   </summary>
	/// <remark> Not Researched </remark>
	public class FEditMotSmoothLen : DSCFunc
	{
		public FEditMotSmoothLen() { functionId = 0x40; }
		/// <summary> ??? </summary>
		[FieldOrder(0)]
		public uint unk1;
		/// <summary> ??? </summary>
		[FieldOrder(1)]
		public uint unk2;
	}

	/// <summary> Sets the mode for the rest of the dsc </summary>
	public class FPvBranchMode : DSCFunc
	{
		public FPvBranchMode() { functionId = 0x41; }
		/// <summary> Which mode to use </summary>
		[FieldOrder(0)]
		public uint mode;
	}

	/// <summary> I guess starts the camera </summary>
	public class FDataCameraStart : DSCFunc
	{
		public FDataCameraStart() { functionId = 0x42; }
		/// <summary> DESC </summary>
		[FieldOrder(0)]
		public uint var;
	}

	/// <summary> Plays the movie, but doesn't display it </summary>
	public class FMoviePlay : DSCFunc
	{
		public FMoviePlay() { functionId = 0x43; }
		/// <summary> Play movie? </summary>
		[FieldOrder(0), SerializeAs(SerializedType.UInt4)]
		public bool state;
	}

	/// <summary> Displays the movie, but doesn't play it </summary>
	public class FMovieDisplay : DSCFunc
	{
		public FMovieDisplay() { functionId = 0x44; }
		/// <summary> Display movie? </summary>
		[FieldOrder(0), SerializeAs(SerializedType.UInt4)]
		public bool state;
	}

	/// <summary> No idea </summary>
	public class FWind : DSCFunc
	{
		public FWind() { functionId = 0x45; }
		/// <summary> DESC </summary>
		[FieldOrder(0)]
		public uint var;
	}

	/// <summary> No idea </summary>
	public class FOsageStep : DSCFunc
	{
		public FOsageStep() { functionId = 0x46; }
		/// <summary> DESC </summary>
		[FieldOrder(0)]
		public uint var;
	}

	/// <summary> Moves the hair colliders </summary>
	/// <remark> Speculation! </remark>
	public class FOsageMoveCollider : DSCFunc
	{
		public FOsageMoveCollider() { functionId = 0x47; }
		/// <summary> New position for the colliders </summary>
		[FieldOrder(0)]
		public Vector3 position;
	}

	/// <summary> First 4 bytes = Player ID </summary>
	public class FCharacterColor : DSCFunc
	{
		public FCharacterColor() { functionId = 0x48; }
		/// <summary> Which player to affect </summary>
		[FieldOrder(0)]
		public int playerID;
		/// <summary> Red channel </summary>
		[FieldOrder(1)]
		public byte r;
		/// <summary> Green channel </summary>
		[FieldOrder(2)]
		public byte g;
		/// <summary> Blue channel </summary>
		[FieldOrder(3)]
		public byte b;
		/// <summary> Alpha channel </summary>
		[FieldOrder(4)]
		public byte a;
	}

	/// <summary> Plays the bonus effect sounds, but I haven't looked into the values </summary>
	public class FSeEffect : DSCFunc
	{
		public FSeEffect() { functionId = 0x49; }
		/// <summary> Which sound to play </summary>
		[FieldOrder(0)]
		public uint sfxID;
	}

	/// <summary> Moves all 3 characters </summary>
	/// <remark> Speculation! </remark>
	public class FEditMoveXYZ : DSCFunc
	{
		public FEditMoveXYZ() { functionId = 0x4A; }
		/// <summary> Player 1's new position </summary>
		[FieldOrder(0)]
		public Vector3 player1Position;
		/// <summary> Player 2's new position </summary>
		[FieldOrder(1)]
		public Vector3 player2Position;
		/// <summary> Player 3's new position </summary>
		[FieldOrder(2)]
		public Vector3 player3Position;
	}

	/// <summary> No idea </summary>
	public class FEditEyelidAnim : DSCFunc
	{
		public FEditEyelidAnim() { functionId = 0x4B; }
		/// <summary> ??? </summary>
		[FieldOrder(0)]
		public uint unk1;
		/// <summary> ??? </summary>
		[FieldOrder(1)]
		public uint unk2;
		/// <summary> ??? </summary>
		[FieldOrder(2)]
		public uint unk3;
	}

	/// <summary> Sets the instrument the character uses </summary>
	public class FEditInstrumentItem : DSCFunc
	{
		public FEditInstrumentItem() { functionId = 0x4C; }
		/// <summary> ??? </summary>
		[FieldOrder(0)]
		public int unk;
		/// <summary> ??? </summary>
		[FieldOrder(1)]
		public uint unk1;
	}

	/// <summary> Loops the motion the player uses </summary>
	/// <remark>  Has to have a FSetMotion after it </remark>
	public class FEditMotionLoop : DSCFunc
	{
		public FEditMotionLoop() { functionId = 0x4D; }
		/// <summary> Which player to affect </summary>
		[FieldOrder(0)]
		public int playerID;
		/// <summary> Which animation to play </summary>
		[FieldOrder(1)]
		public uint animID;
		/// <summary> Which frame the animation should start on </summary>
		[FieldOrder(2), FieldScale(100), SerializeAs(SerializedType.Int4)]
		public double time;
		/// <summary> Animation speed </summary>
		[FieldOrder(3)]
		public int speed;
	}

	/// <summary> Edit variant of FExpression </summary>
	public class FEditExpression : DSCFunc
	{
		public FEditExpression() { functionId = 0x4E; }
		/// <summary> Which expression to play </summary>
		[FieldOrder(0)]
		public uint expID;
		/// <summary> When the animation should start </summary>
		[FieldOrder(1)]
		public int start;
		/// <summary> The animation speed, disabled if - </summary>
		[FieldOrder(2)]
		public int speed;
	}

	/// <summary> Basically EYE_ANIM with less values </summary>
	public class FEditEyeAnim : DSCFunc
	{
		public FEditEyeAnim() { functionId = 0x4F; }
		/// <summary> DESC </summary>
		[FieldOrder(0)]
		public uint var;
	}

	/// <summary> Edit variant of FMouthAnim </summary>
	public class FEditMouthAnim : DSCFunc
	{
		public FEditMouthAnim() { functionId = 0x50; }
		/// <summary> Which animation to play? </summary>
		[FieldOrder(0)]
		public uint animID;
		/// <summary> ??? </summary>
		[FieldOrder(1)]
		public int unk;
		/// <summary> When the animation should start </summary>
		[FieldOrder(2)]
		public int start;
		/// <summary> The animation speed, disabled if -1  </summary>
		[FieldOrder(3)]
		public int speed;
	}

	/// <summary> Acts like FMoveCamera but repositions around the player </summary>
	public class FEditCamera : DSCFunc
	{
		public FEditCamera() { functionId = 0x51; }
		/// <summary> ??? </summary>
		[FieldOrder(0)]
		public Vector3 position;
		/// <summary> ??? </summary>
		[FieldOrder(1)]
		public Vector3 unk1;
		/// <summary> ??? </summary>
		[FieldOrder(2)]
		public Vector3 unk2;
		/// <summary> ??? </summary>
		[FieldOrder(3)]
		public Vector3 unk3;
		/// <summary> ??? </summary>
		[FieldOrder(4)]
		public Vector3 unk4;
		/// <summary> ??? </summary>
		[FieldOrder(5)]
		public Vector3 unk5;
		/// <summary> ??? </summary>
		[FieldOrder(6)]
		public Vector3 unk6;
		/// <summary> ??? </summary>
		[FieldOrder(7)]
		public Vector3 unk7;
	}

	/// <summary> Edit variant of FModeSelect </summary>
	/// <remark> Does nothing </remark>
	public class FEditModeSelect : DSCFunc
	{
		public FEditModeSelect() { functionId = 0x52; }
		/// <summary> DESC </summary>
		[FieldOrder(0)]
		public uint var;
	}

}