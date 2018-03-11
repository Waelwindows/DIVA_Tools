using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BinarySerialization;

namespace DIVALib.ImageUtils
{
    public static class EnumExtension
    {
        public static T CastTo<T>(this Enum oldEnum) => (T)Enum.Parse(typeof(T).IsEnum ? typeof(T) : throw new InvalidOperationException("Tried to cast enum to invalid type."), oldEnum.ToString());
    }

    public static class DdsExtension
    {
        public static TxpMipmap ToTxp(this DdsMipMap mip, DdsPixelFormat pf, int id = 0)
        {
            if (mip == null) throw new NullReferenceException("Tried to create TXP Mipmap from null DDS mipmap");
            var tex = new TxpMipmap
            {
                Width = mip.Width,
                Height = mip.Height,
                Format = pf.Format.CastTo<TxpMipmap.TexFormat>(),
                ByteSize = mip.ByteSize,
                Id = id,
                Data = mip.Data
            };
            return tex;
        }

        public static TxpMipmap ToTxpMip(this DdsFile dds)
        {
            if (dds == null) throw new NullReferenceException("Tried to create TXP Mipmap from null DDS file");
            var tex = new TxpMipmap
            {
                Width = dds.Width,
                Height = dds.Height,
                Format = dds.PixelFormat.Format.CastTo<TxpMipmap.TexFormat>(),
                ByteSize = dds.PitchOrLinearSize,
                Data = dds.Data
            };
            return tex;
        }

        public static TxpTexture ToTxp(this DdsFile dds)
        {
            var tex = new TxpTexture { MipMapCount = dds.MipMapCount == 0 ? 1 : dds.MipMapCount };
            int mipNum = 1;
            var mips = new List<TxpMipmap>(tex.MipMapCount) { dds.ToTxpMip() };
            if(dds.MipMapCount > 0) dds.MipMaps.ForEach(mip => mips.Add(mip.ToTxp(dds.PixelFormat, mipNum++)));
            mips.ForEach(mip => tex.Mipmaps.Add(mip));
            return tex;
        }
    }

    public static class StringExtension
    {
        public static string Slice(this string str, int start = 0, int end = 0, int step = 1)
        {
            int RemapIndex(int n) => n < 0 ? str.Length + n : n;

            start = RemapIndex(start);
            end = end == 0 ? str.Length : RemapIndex(end);

            var rslt = str.Substring(start, end).Where((c, i) => i % step == 0);
            rslt = step < 0 ? rslt.Reverse() : rslt;
            return new string(rslt.ToArray());
        }
    }

    public class TxpMipmap : TxpBase
    {
        public enum TexFormat : uint
        {
            RGB = 1,
            RGBA = 2,
            DXT1 = 6,
            DXT3 = 7,
            DXT5 = 9,
            ATI2 = 11
        }

        public TxpMipmap() => Magic = 0x54585002; //TXP2
        [FieldOrder(1)]                         public int        Width = 512;
        [FieldOrder(2)]                         public int        Height = 512;
        [FieldOrder(3)]                         public TexFormat  Format = TexFormat.RGB;
        [FieldOrder(4)]                         public int        Id;
        [FieldOrder(5)]                         public int        ByteSize = 512 * 512;
        [FieldOrder(6), FieldCount("ByteSize")] public List<byte> Data;
        [Ignore]                                public int Size => 24 + Data.Count;

        public TxpMipmap(int width, int height, TexFormat format = TexFormat.RGB, int byteSize = 0)// : this()
        {
            Width = width;
            Height = height;
            Format = format;
            ByteSize = byteSize == 0 ? width * height : byteSize;
        }

        public static explicit operator DdsMipMap(TxpMipmap mip)
        {
            var ddsMipMap = new DdsMipMap
            {
                Width = mip.Width,
                Height = mip.Height,
                ByteSize = mip.ByteSize,
                Data = mip.Data
            };
            return ddsMipMap;
        }

        public static explicit operator DdsFile(TxpMipmap mip)
        {
            var ddsMipMap = new DdsFile
            {
                Width = mip.Width,
                Height = mip.Height,
                PitchOrLinearSize = mip.ByteSize,
                PixelFormat = new DdsPixelFormat(mip.Format.CastTo<DdsPFType>()),
                Data = mip.Data
            };
            return ddsMipMap;   
        }

        public override string ToString() => $"Mip #{Id}: {Width}x{Height} {Format} (BS:{ByteSize}, TS:{Size})";
    }

    public class TxpTexture : TxpBase
    {
        public TxpTexture() => Magic = 0x54585004; //TXP4

        [FieldOrder(1)]                            public int             MipMapCount;
        [FieldOrder(2)]                            public int             Version = 0x1010101;
        [FieldOrder(3), FieldCount("MipMapCount")] public List<int>       OffsetTable = new List<int>();
        [FieldOrder(4), FieldCount("MipMapCount")] public List<TxpMipmap> Mipmaps = new List<TxpMipmap>();
        [Ignore]                                   public int Size => 12 + OffsetTable.Count * 4 + Mipmaps.Sum(mip => mip?.Size ?? 0);

        public void SetMipOffsets()
        {
            OffsetTable = new List<int>(MipMapCount);
            var offset = 12 + MipMapCount * 4;
            for (var counter = 0; counter < Mipmaps.Count; ++counter)
            {
                offset += counter != 0 ? Mipmaps[counter - 1].Size : 0;
                OffsetTable.Add(offset);
            }
        }

        public static explicit operator DdsFile(TxpTexture tex)
        {
            var dds = new DdsFile
            {
                Width = tex.Mipmaps[0].Width,
                Height = tex.Mipmaps[0].Height,
                PitchOrLinearSize = tex.Mipmaps[0].ByteSize,
                MipMapCount = tex.MipMapCount,
                PixelFormat = new DdsPixelFormat(tex.Mipmaps[0].Format.CastTo<DdsPFType>()),
                Data = tex.Mipmaps[0].Data
            };
            var mips = tex.Mipmaps.GetRange(1, tex.MipMapCount - 1).Select(mip => (DdsMipMap) mip).ToList();
            dds.MipMaps = mips;
            dds.MipMaps = dds.MipMaps.Where(mip => mip.Data.Any()).ToList();
            dds.MipMaps.Add(new DdsMipMap());
            return dds;
        }

        public override string ToString() => $"TXP4: {MipMapCount} mips ( {Mipmaps[0].ToString().Substring(8)} )";
    }

    public class TxpTextureAtlas : TxpBase
    {
        public TxpTextureAtlas() => Magic = 0x54585003; //TXP3
        [FieldOrder(1)]                             public int              TextureCount;
        [FieldOrder(2)]                             public uint             Version = 0x1010112;
        [FieldOrder(3), FieldCount("TextureCount")] public List<int>        OffsetTable = new List<int>();
        [FieldOrder(4), FieldCount("TextureCount")] public List<TxpTexture> Textures = new List<TxpTexture>();

        public void SetTextureOffsets()
        {
            OffsetTable = new List<int>(TextureCount);
            var offset = 12 + TextureCount * 4;
            for (var counter = 0; counter < Textures.Count; ++counter)
            {
                offset += counter != 0 ? Textures[counter - 1].Size : 0;
                OffsetTable.Add(offset);
            }
        }

        public void SetTextures(Stream file)
        {
            var serializer = new BinarySerializer();
            Textures = new List<TxpTexture>(TextureCount);
            file.Seek(OffsetTable[0], SeekOrigin.Begin);
            OffsetTable.ForEach(offset => Textures.Add(serializer.Deserialize<TxpTexture>(file)));
        }

        public static TxpTextureAtlas FromDds(DdsFile dds)
        {
            var file = new TxpTextureAtlas {Textures = new List<TxpTexture> {dds.ToTxp()}};
            return file;
        }

        public static TxpTextureAtlas FromDds(DdsFile[] ddsFiles)
        {
            var file = new TxpTextureAtlas {Textures = new List<TxpTexture>()};
            foreach (var dds in ddsFiles)
                file.Textures.Add(dds.ToTxp());
            return file;
        }

        public static explicit operator List<DdsFile>(TxpTextureAtlas atlas)
        {
            var ddsTex = new List<DdsFile>(atlas.TextureCount);
            atlas.Textures.ForEach(tex => ddsTex.Add((DdsFile)tex));
            return ddsTex;
        }

        public override string ToString() => $"TXP3: {TextureCount} textures, ({Textures.Count} serialized)";
    }

    public abstract class TxpBase
    {
        public int Magic = 0x54585000; //TXP0
    }
}