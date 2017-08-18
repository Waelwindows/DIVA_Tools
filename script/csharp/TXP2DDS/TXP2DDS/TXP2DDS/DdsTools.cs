using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using DIVALib.IO;

namespace DdsTools
{
    public enum DdsPFType
    {
        RGB,
        RGBA,
        DXT1,
        DXT3,
        DXT5,
        ATI2n
    }

    public class DdsPixelFormat
    {
        public const uint size = 32;
        public uint       flags;
        public char[]     compressionName = new char[4];
        public uint       RGBBitCount;
        public uint       RBitMask;
        public uint       GBitMask;
        public uint       BBitMask;
        public uint       ABitMask;

        const uint DDPF_ALPHAPIXELS = 0x1;
        const uint DDPF_ALPHA = 0x2;
        const uint DDPF_FOURCC = 0x4;
        const uint DDPF_RGB = 0x40;
        const uint DDPF_YUV = 0x200;
        const uint DDPF_LUMINANCE = 0x20000;

        public DdsPixelFormat() { }

        public DdsPixelFormat(DdsPFType type)
        {
            switch (type)
            {
                case DdsPFType.RGB:
                    flags = DDPF_RGB;
                    compressionName = "RGB ".ToCharArray();
                    RGBBitCount = 24;
                    RBitMask = 0xFF0000;
                    GBitMask = 0x00FF00;
                    BBitMask = 0x0000FF; break;
                case DdsPFType.RGBA:
                    flags = DDPF_RGB | DDPF_ALPHAPIXELS;
                    compressionName = "RGBA".ToCharArray();
                    RGBBitCount = 32;
                    RBitMask = 0xFF000000;
                    GBitMask = 0x00FF0000;
                    BBitMask = 0x0000FF00;
                    ABitMask = 0x000000FF; break;
                case DdsPFType.DXT1:
                    flags = DDPF_FOURCC;
                    compressionName = "DXT1".ToCharArray();
                    RGBBitCount = 12;
                    RBitMask = 0xFF000000;
                    GBitMask = 0x00FF0000;
                    BBitMask = 0x0000FF00; break;
                case DdsPFType.DXT3:
                    flags = DDPF_FOURCC;
                    compressionName = "DXT3".ToCharArray();
                    RGBBitCount = 16;
                    RBitMask = 0xFF000000;
                    GBitMask = 0x00FF0000;
                    BBitMask = 0x0000FF00; break;
                case DdsPFType.DXT5:
                    flags = DDPF_FOURCC;
                    compressionName = "DXT5".ToCharArray();
                    RGBBitCount = 64;
                    RBitMask = 0xFF000000;
                    GBitMask = 0x00FF0000;
                    BBitMask = 0x0000FF00; break;
                case DdsPFType.ATI2n:
                    flags = DDPF_FOURCC;
                    compressionName = "ATI2".ToCharArray();
                    RGBBitCount = 64;
                    break;
            }
        }

        public void Save(Stream s)
        {
            DataStream.WriteUInt32(s, size);
            DataStream.WriteUInt32(s, flags);
            DataStream.WriteChars(s, compressionName);
            DataStream.WriteUInt32(s, RGBBitCount);
            DataStream.WriteUInt32(s, RBitMask);
            DataStream.WriteUInt32(s, GBitMask);
            DataStream.WriteUInt32(s, BBitMask);
            DataStream.WriteUInt32(s, ABitMask);
        }
    }

    public class DdsMipMap
    {
        public uint height;
        public uint width;
        public uint byteSize;
        public byte[] data;

        public DdsMipMap() { }

        public DdsMipMap(uint w, uint h, uint bs)
        {
            width = w; height = h;
            byteSize = bs;
        }

        public DdsMipMap(uint w, uint h, uint bs, List<byte> d)
        {
            width = w; height = h;
            byteSize = bs;
            data = d.ToArray();
        }

        public DdsMipMap(uint w, uint h, uint bs, byte[] d)
        {
            width = w; height = h;
            byteSize = bs;
            data = d;
        }
    }

    public class DdsFile
    {
        public const string magic = "DDS ";
        public const uint headerSize = 124;
        public uint flag;
        public uint height;
        public uint width;
        public uint pitchOrLinearSize;
        public uint depth;
        public uint mipMapCount;
        public DdsPixelFormat pixelFormat;
        public uint caps;
        public uint caps2;
        public uint caps3;
        public uint caps4;
        public byte[] data;
        public DdsMipMap[] mipMaps;

        const uint DDSD_CAPS        = 0x1;
        const uint DDSD_HEIGHT      = 0x2;
        const uint DDSD_WIDTH       = 0x4;
        const uint DDSD_PITCH       = 0x8;
        const uint DDSD_PIXELFORMAT = 0x1000;
        const uint DDSD_MIPMAPCOUNT = 0x20000;
        const uint DDSD_LINEARSIZE  = 0x80000;
        const uint DDSD_DEPTH       = 0x800000;

        public DdsFile() { }

        public DdsFile(uint w, uint h)
        {
            flag = DDSD_CAPS | DDSD_HEIGHT | DDSD_WIDTH | DDSD_PIXELFORMAT | DDSD_MIPMAPCOUNT;
            height = h;
            width = w;
        }

        public DdsFile(DdsPixelFormat format, uint w, uint h)
        {
            height = h;
            width = w;
            pixelFormat = format;
        }

        public DdsFile(Stream s)
        {
            if (DataStream.ReadString(s, 4) != magic) { return; }
            if (DataStream.ReadUInt32(s) != headerSize) { return; }
            flag = DataStream.ReadUInt32(s);
            height = DataStream.ReadUInt32(s);
            width = DataStream.ReadUInt32(s);
            pitchOrLinearSize = DataStream.ReadUInt32(s);
            depth = DataStream.ReadUInt32(s);
            mipMapCount = DataStream.ReadUInt32(s);
            s.Seek(11 * 4, SeekOrigin.Current);

            pixelFormat = new DdsPixelFormat();
            if (DataStream.ReadUInt32(s) != DdsPixelFormat.size) { return; }
            pixelFormat.flags = DataStream.ReadUInt32(s);
            pixelFormat.compressionName = DataStream.ReadChars(s, 4);
            pixelFormat.RGBBitCount = DataStream.ReadUInt32(s);
            pixelFormat.RBitMask = DataStream.ReadUInt32(s);
            pixelFormat.GBitMask = DataStream.ReadUInt32(s);
            pixelFormat.BBitMask = DataStream.ReadUInt32(s);
            pixelFormat.ABitMask = DataStream.ReadUInt32(s);

            caps  = DataStream.ReadUInt32(s);
            caps2 = DataStream.ReadUInt32(s);
            caps3 = DataStream.ReadUInt32(s);
            caps4 = DataStream.ReadUInt32(s);
            DataStream.ReadUInt32(s);
            data = DataStream.ReadBytes(s, (int)pitchOrLinearSize);
            SetMipMaps();
        }

        public void Save(Stream s)
        {
            DataStream.WriteMagic(s, magic);
            DataStream.WriteUInt32(s, headerSize);
            DataStream.WriteUInt32(s, flag);
            DataStream.WriteUInt32(s, height);
            DataStream.WriteUInt32(s, width);
            DataStream.WriteUInt32(s, pitchOrLinearSize);
            DataStream.WriteUInt32(s, depth);
            DataStream.WriteUInt32(s, mipMapCount);
            DataStream.WriteNulls(s, 11 * 4);
            if (pixelFormat == null) { return; }
            pixelFormat.Save(s);
            DataStream.WriteUInt32(s, caps);
            DataStream.WriteUInt32(s, caps2);
            DataStream.WriteUInt32(s, caps3);
            DataStream.WriteUInt32(s, caps4);
            DataStream.WriteNulls(s, 4);
            DataStream.WriteBytes(s, data);
            foreach(DdsMipMap mip in mipMaps)
            {
                if (mip == null) { return; }
                DataStream.WriteBytes(s, mip.data);
            }
        }
       
        protected static DdsFile Clone(DdsFile original)
        {
            return (DdsFile) original.MemberwiseClone();
        }

        public void SetMipMaps()
        {
            mipMaps = new DdsMipMap[mipMapCount];
            uint counter = 1;
            while (counter < mipMapCount)
            {
                int bytesToSkip = 0;
                int dividor = (int)Math.Pow(2, (double)counter);
                bytesToSkip += (int)(pitchOrLinearSize / (dividor * 0.5));
                uint mWidth = (uint)(width / dividor); uint mHeight = (uint)(height / dividor); uint mBS = (uint)(pitchOrLinearSize / dividor);
                mipMaps[counter - 1] = new DdsMipMap(mWidth, mHeight, mBS, data.ToList().Skip(bytesToSkip).ToArray());
                ++counter;
            }
            Array.Resize(ref data, (int)pitchOrLinearSize);
        }

    }
}