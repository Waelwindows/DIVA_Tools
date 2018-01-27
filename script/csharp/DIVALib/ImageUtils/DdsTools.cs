using System;
using System.Linq;
using System.Collections.Generic;
using BinarySerialization;

namespace DIVALib.ImageUtils
{
    public enum DdsPFType
    {
        RGB,
        RGBA,
        DXT1,
        DXT2,
        DXT3,
        DXT4,
        DXT5,
        ATI2n
    }

    public class DdsPixelFormat
    {
        [Ignore] public DdsPFType Format => (DdsPFType) Enum.Parse(typeof(DdsPFType), CompressionName);

        [FieldOrder(0)]                 public uint   Size = 32;
        [FieldOrder(1)]                 public uint   Flags;
        [FieldOrder(2), FieldLength(4)] public string CompressionName = "    ";
        [FieldOrder(3)]                 public uint   RGBBitCount;
        [FieldOrder(4)]                 public uint   RBitMask;
        [FieldOrder(5)]                 public uint   GBitMask;
        [FieldOrder(6)]                 public uint   BBitMask;
        [FieldOrder(7)]                 public uint   ABitMask;
    
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
                    Flags = DDPF_RGB;
                    RGBBitCount = 24;
                    CompressionName = "RGB ";
                    RBitMask = 0x0000FF;
                    GBitMask = 0x00FF00;
                    BBitMask = 0xFF0000; break;
                case DdsPFType.RGBA:
                    Flags = DDPF_RGB | DDPF_ALPHAPIXELS;
                    RGBBitCount = 32;
                    CompressionName = "RGBA";
                    RBitMask = 0x000000FF;
                    GBitMask = 0x0000FF00;
                    BBitMask = 0x00FF0000;
                    ABitMask = 0xFF000000; break;
                case DdsPFType.DXT1:
                    Flags = DDPF_FOURCC;
                    CompressionName = "DXT1";
                    RGBBitCount = 16;
                    RBitMask = 0x0F;
                    GBitMask = 0xFF;
                    BBitMask = 0xF0; break;
                case DdsPFType.DXT2:
                    Flags = DDPF_FOURCC;
                    CompressionName = "DXT2";
                    RGBBitCount = 24;
                    RBitMask = 0xF0000000;
                    GBitMask = 0x0F000000;
                    BBitMask = 0x00F00000;
                    ABitMask = 0x000F0000; break;
                case DdsPFType.DXT3:
                    Flags = DDPF_FOURCC;
                    CompressionName = "DXT3";
                    RGBBitCount = 24;
                    RBitMask = 0xF0000000;
                    GBitMask = 0x0F000000;
                    BBitMask = 0x00F00000; 
                    ABitMask = 0x000FF000; break;
                case DdsPFType.DXT4:
                    Flags = DDPF_FOURCC;
                    CompressionName = "DXT4";
                    RGBBitCount = 32;
                    RBitMask = 0xFF000000; break;
                case DdsPFType.DXT5:
                    Flags = DDPF_FOURCC;
                    CompressionName = "DXT5";
                    RGBBitCount = 64;
                    RBitMask = 0xFF000000;
                    GBitMask = 0x00FF0000; break;
                case DdsPFType.ATI2n:
                    Flags = DDPF_FOURCC;
                    CompressionName = "ATI2";
                    RGBBitCount = 16;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public bool IsUncompressed()
        {
            return Format == DdsPFType.RGB || Format == DdsPFType.RGBA;
        }
    }

    public class DdsMipMap
    {
        [Ignore] public int Height;
        [Ignore] public int Width;
        [Ignore] public int ByteSize;
        [FieldOrder(0), FieldLength("ByteSize")] public List<byte> Data = new List<byte>();

        public DdsMipMap() { }

        public DdsMipMap(int width, int height, int byteSize)
        {
            Width = width;
            Height = height;
            ByteSize = byteSize;
        }

        public DdsMipMap(int width, int height, int byteSize, IEnumerable<byte> data) : this(width, height, byteSize) => Data = data.ToList();
    }

    public class DdsFile
    {
        [FieldOrder(0), FieldLength(4)]                                                              public string          Magic = "DDS ";
        [FieldOrder(1)]                                                                              public int             HeaderSize = 124;
        [FieldOrder(2)]                                                                              public int             Flag { get => DDSD_CAPS | DDSD_WIDTH | DDSD_HEIGHT | DDSD_PIXELFORMAT |
                                                                                                                                         (MipMapCount > 0 ? DDSD_MIPMAPCOUNT : 0) |
                                                                                                                                         (PixelFormat.IsUncompressed() ? DDSD_PITCH : DDSD_LINEARSIZE) |
                                                                                                                                         (Depth > 0 ? DDSD_DEPTH : 0); set { } }
        [FieldOrder(3)]                                                                              public int             Height;
        [FieldOrder(4)]                                                                              public int             Width;
        [FieldOrder(5)]                                                                              public int             PitchOrLinearSize; // { get => Width * Height * (int)(PixelFormat.RGBBitCount / 8); set { } }
        [FieldOrder(6)]                                                                              public int             Depth = 0;
        [FieldOrder(7)]                                                                              public int             MipMapCount = 0;
        [FieldOrder(8), FieldCount(11)]                                                              public uint[]          Reserved = new uint[11];
        [FieldOrder(9)]                                                                              public DdsPixelFormat  PixelFormat = new DdsPixelFormat();
        [FieldOrder(10)]                                                                             public int             Caps { get => DDSCAPS_TEXTURE | (MipMapCount > 1 ? DDSCAPS_MIPMAP | DDSCAPS_COMPLEX : 0); set { } }
        [FieldOrder(11)]                                                                             public int             Caps2;
        [FieldOrder(12)]                                                                             public int             Caps3;
        [FieldOrder(13)]                                                                             public int             Caps4;
        [FieldOrder(14)]                                                                             public int             Reserved2;
        [FieldOrder(15), FieldLength("PitchOrLinearSize", BindingMode = BindingMode.OneWayToSource)] public List<byte>      Data;
        [FieldOrder(16), FieldCount("MipMapCount", BindingMode = BindingMode.OneWayToSource)]        public List<DdsMipMap> MipMaps = new List<DdsMipMap>();

        public const int DDSD_CAPS        = 0x1;
        public const int DDSD_HEIGHT      = 0x2;
        public const int DDSD_WIDTH       = 0x4;
        public const int DDSD_PITCH       = 0x8;
        public const int DDSD_PIXELFORMAT = 0x1000;
        public const int DDSD_MIPMAPCOUNT = 0x20000;
        public const int DDSD_LINEARSIZE  = 0x80000;
        public const int DDSD_DEPTH       = 0x800000;

        public const int DDSCAPS_COMPLEX = 0x8;
        public const int DDSCAPS_TEXTURE = 0x1000;
        public const int DDSCAPS_MIPMAP  = 0x400000;

        public DdsFile() { }

        public DdsFile(int w, int h)
        {
            Height = h;
            Width = w;
        }

        public DdsFile(DdsPixelFormat format, int w, int h) : this(w, h)
        {
            PixelFormat = format;
            PitchOrLinearSize = w * h * (int)(format.RGBBitCount / 8);
        }


        protected static DdsFile Clone(DdsFile original) => (DdsFile)original.MemberwiseClone();

        public void SetMipMaps()
        {
            MipMaps = new List<DdsMipMap>((int)MipMapCount);
            var mipData = Data.ToList();
            var dataOffset = PitchOrLinearSize;
            int Power(int n, int exp) => Enumerable.Repeat(n, exp).Sum(num => num * num);
            for (var i = 1; i < MipMapCount; ++i)
            {
                var div = (int)System.Math.Pow(2, i);
                var mipWidth = Width / div;
                var mipHeight = Height / div;
                var mipBytesize = (int)(PitchOrLinearSize / System.Math.Pow(4, i));
                var mip = new DdsMipMap(mipWidth, mipHeight, mipBytesize, mipData.GetRange(dataOffset, mipBytesize));
                MipMaps.Add(mip);
                dataOffset += mipBytesize;
            }
            Data = Data.GetRange(0, PitchOrLinearSize);
        }
    }
}
