#define DEBUG
#undef DEBUG

using System;
using System.IO;
using System.Collections.Generic;
using DIVALib.IO;
using DIVALib.ImageUtils;

namespace TxpFunc
{
    public class TxpMipMap
    {

        public enum TexType : uint
        {
            RGB   = 1,
            RGBA  = 2,
            DXT1  = 6,
            DXT3  = 7,
            DXT5  = 9,
            ATI2n = 11
        }

        public string magic = "TXP";
        public byte flag = 0x2;
        public uint width;
        public uint height;
        public TexType type;
        public uint id;
        public uint byteSize;
        public byte[] data;

        public TxpMipMap() { }

        public TxpMipMap(Stream s)
        {
#if (DEBUG)
            Console.Write("TxpMipMap - Begin at " + s.Position + "\n");
#endif
            if (DataStream.ReadString(s, 3) != magic) { return; }
#if (DEBUG)
            Console.Write("TxpMipMap - Magic correct \n");
#endif
            flag = DataStream.ReadByte(s);
#if (DEBUG)
            Console.Write("TxpMipMap - Flag is " + flag + "\n");
#endif
            if (flag != 2) { Console.Write("TxpMipMap - Flag is not 0x2 \n"); return; }
            width = DataStream.ReadUInt32(s);
            height = DataStream.ReadUInt32(s);
            type = (TexType)DataStream.ReadUInt32(s);
            id = DataStream.ReadUInt32(s);
            byteSize = DataStream.ReadUInt32(s);
            data = DataStream.ReadBytes(s, (int) byteSize);
            //Array.Reverse(data);
        }

        public void Write(Stream s)
        {
            DataStream.WriteMagic(s, magic);
            DataStream.WriteByte(s, flag);
            DataStream.WriteUInt32(s, width);
            DataStream.WriteUInt32(s, height);
            DataStream.WriteUInt32(s, (uint)type);
            DataStream.WriteUInt32(s, id);
            DataStream.WriteUInt32(s, byteSize);
            DataStream.WriteBytes(s, data);
        }

        public static TxpMipMap FromDds(DdsMipMap mip, DdsPixelFormat pf)
        {
            TxpMipMap tex = new TxpMipMap();
            if (mip == null) { return tex; }
            tex.width = mip.width;
            tex.height = mip.height;
            switch (new string(pf.compressionName))
            {
                case "RGB ": tex.type = TexType.RGB; break;
                case "RGBA": tex.type = TexType.RGBA; break;
                case "DXT1": tex.type = TexType.DXT1; break;
                case "DXT3": tex.type = TexType.DXT3; break;
                case "DXT5": tex.type = TexType.DXT5; break;
                case "ATI2": tex.type = TexType.ATI2n; break;
            }
            tex.byteSize = mip.byteSize;
            tex.data = mip.data;
            return tex;
        }

        public DdsFile ToDds()
        {
            DdsFile dds = new DdsFile();
            dds.width = width; dds.height = height;
            dds.pitchOrLinearSize = byteSize;
            dds.flag = 0xA1007;
            dds.mipMapCount = 9;
            dds.pixelFormat = new DdsPixelFormat();
            switch (type)
            {
                case TexType.RGB: dds.pixelFormat = new DdsPixelFormat(DdsPFType.RGB); break;
                case TexType.RGBA: dds.pixelFormat = new DdsPixelFormat(DdsPFType.RGBA); break;
                case TexType.DXT1: dds.pixelFormat = new DdsPixelFormat(DdsPFType.DXT1); break;
                case TexType.DXT3: dds.pixelFormat = new DdsPixelFormat(DdsPFType.DXT3); break;
                case TexType.DXT5: dds.pixelFormat = new DdsPixelFormat(DdsPFType.DXT5); break;
                case TexType.ATI2n: dds.pixelFormat = new DdsPixelFormat(DdsPFType.ATI2n); break;
                default: Console.Write("WTF, Unknown TXP type " + type + "\n"); break;
            }
            dds.caps = 0x401008;
            dds.data = data;
            return dds;
        }
    }

    public class TxpTexture
    {
        public const string magic = "TXP";
        public byte flag = 0x4;
        public uint mipCount = 9;
        public uint version = 0x1010108;
        public TxpMipMap[] mipMaps;

        public TxpTexture() { }

        public TxpTexture(Stream s)
        {
            uint txpStart = (uint)s.Position;
#if (DEBUG)
            Console.Write("TxpTexture - Begin \n");
#endif
            if (DataStream.ReadString(s, 3) != magic) { return; }
#if (DEBUG)
            Console.Write("TxpTexture - Magic correct\n");
#endif
            flag = DataStream.ReadByte(s);
#if (DEBUG)
            Console.Write("TxpTexture - Flag is " + flag + "\n");
#endif
            mipCount = DataStream.ReadUInt32(s);
            mipMaps = new TxpMipMap[mipCount];
            version = DataStream.ReadUInt32(s);
            uint offsetTbl = (uint)s.Position;
            for (int i=0; i<mipCount; i++)
            {
                s.Seek(offsetTbl + (i * 4), SeekOrigin.Begin);
                //Console.Write(offsetTbl + (i * 4) + "\n");
                uint offset = DataStream.ReadUInt32(s);
                s.Seek(txpStart + offset, SeekOrigin.Begin);
                //Console.Write(txpStart + offset + "\n");
                mipMaps[i] = new TxpMipMap(s);
            }
        }

        public void Write(Stream s)
        {
            DataStream.WriteMagic(s, magic);
            DataStream.WriteByte(s, flag);
            DataStream.WriteUInt32(s, mipCount);
            DataStream.WriteUInt32(s, version);
            foreach(TxpMipMap mip in mipMaps)
            {
                mip.Write(s);
            }
        }

        public DdsFile ToDds()
        {
            DdsFile dds = new DdsFile();
            dds.width = mipMaps[0].width; dds.height = mipMaps[0].height;
            dds.pitchOrLinearSize = mipMaps[0].byteSize;
            dds.flag = 0xA1007;
            dds.mipMapCount = mipCount;
            dds.pixelFormat = new DdsPixelFormat();
            switch (mipMaps[0].type)
            {
                case TxpMipMap.TexType.RGB: dds.pixelFormat = new DdsPixelFormat(DdsPFType.RGB); break;
                case TxpMipMap.TexType.RGBA: dds.pixelFormat = new DdsPixelFormat(DdsPFType.RGBA); break;
                case TxpMipMap.TexType.DXT1: dds.pixelFormat = new DdsPixelFormat(DdsPFType.DXT1); break;
                case TxpMipMap.TexType.DXT3: dds.pixelFormat = new DdsPixelFormat(DdsPFType.DXT3); break;
                case TxpMipMap.TexType.DXT5: dds.pixelFormat = new DdsPixelFormat(DdsPFType.DXT5); break;
                case TxpMipMap.TexType.ATI2n: dds.pixelFormat = new DdsPixelFormat(DdsPFType.ATI2n); break;
                default: Console.Write("WTF, Unknown TXP type " + mipMaps[0].type + "\n"); break;
            }
            dds.caps = 0x401008;
            List<byte> bytes = new List<byte>();
            foreach (TxpMipMap mip in mipMaps)
            {
                foreach(byte bitdata in mip.data)
                {
                    bytes.Add(bitdata);
                }
            }
            dds.data = bytes.ToArray();
            dds.SetMipMaps();
            return dds;
        }

        public static TxpTexture FromDds(DdsFile dds)
        {
            TxpTexture tex = new TxpTexture();
            tex.mipCount = dds.mipMapCount;
            uint counter = 0;
            TxpMipMap[] mips = new TxpMipMap[tex.mipCount];
            foreach(DdsMipMap ddsMip in dds.mipMaps)
            {
                Console.Write(ddsMip == null);
                Console.Write("\n");
                TxpMipMap mip = TxpMipMap.FromDds(ddsMip, dds.pixelFormat);
                mips[counter] = mip;
            }
            tex.mipMaps = mips;
            return tex;
        }

    }

    public class TxpFile
    {
        public const string magic = "TXP";
        public byte flag = 0x3;
        public uint texCount = 1;
        public List<TxpTexture> textures;


        public TxpFile() { }
        public TxpFile(Stream s)
        {
#if (DEBUG)
            Console.Write("TexPack - Begin");
#endif
            if (DataStream.ReadString(s, 3) != magic) { return; }
            flag = DataStream.ReadByte(s);
            textures = new List<TxpTexture>();
#if (DEBUG)
            Console.Write("TexPack - Flag is not 0x2\n");
#endif
            texCount = DataStream.ReadUInt32(s);
#if (DEBUG)
            Console.Write("Texcount " + texCount + "\n");
#endif
            s.Seek(4, SeekOrigin.Current);
            uint tblOffset = (uint)s.Position;
            //Console.Write("Original offset is " + tblOffset + "\n");
            for (int i = 0; i < texCount; i++)
            {
                s.Seek(tblOffset + (i * 4), SeekOrigin.Begin);
                //Console.Write("Offset iteration " + (tblOffset + (i * 4)) + "\n");
                uint offset = DataStream.ReadUInt32(s);
                s.Seek(offset, SeekOrigin.Begin);
                //Console.Write("Texture start is at " + offset + "\n");
                textures.Add(new TxpTexture(s));
            }
            //Console.Write("Texcount is " + texCount + " and the added textures were " + textures.Count + " \n");
        }

        public static TxpFile FromDds(DdsFile[] ddsFiles)
        {
            TxpFile texPack = new TxpFile();
            texPack.textures = new List<TxpTexture>();
            foreach(DdsFile dds in ddsFiles)
            {
                if (dds == null) { return null; }
                texPack.textures.Add(TxpTexture.FromDds(dds));
            }
            return texPack;
        }

        public void Save(Stream s)
        {
            DataStream.WriteMagic(s, magic);
            DataStream.WriteByte(s, flag);
            DataStream.WriteUInt32(s, texCount);
            foreach(TxpTexture tex in textures)
            {
                tex.Write(s);
            }
        }
    }
}