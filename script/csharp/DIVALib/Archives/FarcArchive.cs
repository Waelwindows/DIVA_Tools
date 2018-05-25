using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BinarySerialization;
using System.Linq;

using DIVALib.IO;
using System.Collections;
using System.Security.Cryptography;
using System.IO.Compression;

namespace DIVALib.Archives
{
    public class FarcEntry : EntryBase
    {
        [FieldOrder(0)] public string FileName { get; set; }
        [FieldOrder(1)] public long CompressedLength { get; set; }

        public static explicit operator FarcEntryBin(FarcEntry entry) => new FarcEntryBin
        { FileName = entry.FileName, FilePath = entry.FilePath,
          CompressedLength = entry.CompressedLength,
          Length = (int)entry.length, Position = (int)entry.Position};
    }

    public class FarcArchive : ArchiveBase<FarcEntry>
    {
        public const string FarcEncryptionKey = "project_diva.bin"; 
        public static readonly byte[] FarcEncryptionKeyBytes = Encoding.ASCII.GetBytes(FarcEncryptionKey);

        public bool IsCompressed { get; set; }
        public bool IsEncrypted { get; set; }
        public uint Alignment { get; set; }

        public override void Read(Stream source)
        {
            string signature = DataStream.ReadMagic(source, 4);
            
            if (!string.Equals(signature, "farc", StringComparison.OrdinalIgnoreCase))
            {
                throw new InvalidDataException("'FARC' signature could not be found. (expected 'FArC', 'FArc' or 'FARC')");
            }

            IsCompressed = signature == "FArC" || signature == "FARC";
            IsEncrypted = signature == "FARC";

            uint headerLength = DataStream.ReadUInt32BE(source) + 0x8;

            if (IsEncrypted)
            {
                source.Seek(8, SeekOrigin.Current);
            }

            Alignment = DataStream.ReadUInt32BE(source);

            if (IsEncrypted)
            {
                source.Seek(8, SeekOrigin.Current);
            }

            while (source.Position < headerLength)
            {
                var entry = new FarcEntry();
                entry.FileName = DataStream.ReadCString(source, Encoding.UTF8);
                entry.Position = DataStream.ReadUInt32BE(source);

                if (IsCompressed)
                {
                    entry.CompressedLength = DataStream.ReadUInt32BE(source);
                }

                entry.Length = DataStream.ReadUInt32BE(source);

                entries.Add(entry);
            }
        }

        public override void Write(Stream destination)
        {
            var dataPool = new DataPool();
            dataPool.IsBigEndian = true;
            dataPool.Alignment = Alignment;
            dataPool.AlignmentNullByte = 0x78;

            DataStream.WriteCString(destination, IsCompressed ? "FArC" : "FArc", 4, Encoding.UTF8);
            DataStream.WriteUInt32BE(destination, 0u);
            DataStream.WriteUInt32BE(destination, Alignment);

            foreach (var entry in entries)
            {
                DataStream.WriteCString(destination, entry.FileName, Encoding.UTF8);
                dataPool.Add(destination, entry.FilePath, IsCompressed, false);

                if (IsCompressed)
                {
                    DataStream.WriteUInt32BE(destination, (uint)entry.Length);
                }
            }

            long headerEndPosition = destination.Position;
            DataStream.Pad(destination, Alignment, 0x78);

            dataPool.Write(destination);

            destination.Seek(4, SeekOrigin.Begin);
            DataStream.WriteUInt32BE(destination, (uint)(headerEndPosition - 8));
        }
    }

    public class FarcArchiveBin
    {
        public const char AlignmentChar = 'x';

        [FieldOrder(0), FieldLength(4)]                  public string Magic = "FArc";
        [FieldOrder(1), FieldEndianness(Endianness.Big)] public int    EntryListSize { get => 4 + Entries.Sum(entry => entry.Size); set { } }
        [FieldOrder(2), FieldEndianness(Endianness.Big)] public int    Alignment = 64;
        [Ignore]                                         public bool   IsCompressed { get => Magic == "FArC" || IsEncrypted;
                                                                                      set => Magic = value && !IsEncrypted ? "FArC" : Magic; }
        [Ignore]                                         public bool   IsEncrypted { get => Magic == "FARC";
                                                                                     set => Magic = value ? "FARC" : Magic; }
        [FieldOrder(3), FieldEndianness(Endianness.Big),
         FieldLength("EntryListSize", 
            ConverterType = typeof(FarcEntryConverter))] public List<FarcEntryBin> Entries = new List<FarcEntryBin>();
        [FieldOrder(4)]                                  public Stream             Data = new MemoryStream();

        [Ignore] public int Size => EntryListSize + 8;

        public void Unpack(string destinationPath)
        {
            Directory.CreateDirectory(destinationPath);

            //using (Stream Data = File.OpenRead(sourcePath))
            foreach (var entry in Entries)
            {
                using (Stream entrySource = new SubStream(Data, entry.Position - Size, entry.Length))
                using (Stream destination = File.Create(Path.Combine(destinationPath, entry.FileName)))
                {
                    if (IsEncrypted)
                    {
                        using (AesManaged aes = new AesManaged
                        {
                            KeySize = 128,
                            Key = FarcArchive.FarcEncryptionKeyBytes,
                            BlockSize = 128,
                            Mode = CipherMode.ECB,
                            Padding = PaddingMode.Zeros,
                            IV = new byte[16],
                        })
                        using (CryptoStream cryptoStream = new CryptoStream(
                            entrySource,
                            aes.CreateDecryptor(),
                            CryptoStreamMode.Read))
                        {
                            if (IsCompressed && entry.Length != entry.CompressedLength)
                            {
                                using (GZipStream gzipStream = new GZipStream(cryptoStream, CompressionMode.Decompress))
                                {
                                    gzipStream.CopyTo(destination);
                                }
                            }

                            else { cryptoStream.CopyTo(destination); }
                        }
                    }

                    else if (IsCompressed && entry.Length != entry.CompressedLength)
                    {
                        using (GZipStream gzipStream = new GZipStream(entrySource, CompressionMode.Decompress))
                        {
                            gzipStream.CopyTo(destination);
                        }
                    }

                    else { entrySource.CopyTo(destination); }
                }
            }
        }

        public void Flush()
        {
            Data = new MemoryStream();
            //DataStream.WriteNulls(Data, Size + 8 - 248 - ((int)Data.Length % Alignment));
            var temp = new MemoryStream();
            DataStream.WriteNulls(temp, Size);
            var firstAlignment = Alignment - (int)temp.Length % Alignment;
            var tempSize = temp.Length;
            Console.WriteLine(firstAlignment);
            DataStream.WriteChars(temp, Enumerable.Repeat(AlignmentChar, firstAlignment).ToArray());

            var dataPool = new DataPool
            {
                IsBigEndian = true,
                Alignment = (uint)Alignment,
                AlignmentNullByte = (byte)AlignmentChar
            };
            //Entries.ForEach(entry => dataPool.Add(Data, entry.FilePath, IsCompressed, false));
            var empty = new MemoryStream();
            var size = new MemoryStream();
            DataStream.WriteNulls(size, Size + firstAlignment);
            foreach (var entry in Entries)
            {
                dataPool.Add(empty, entry.FilePath, IsCompressed, false);
                entry.Position = (int)size.Length;
                size = new MemoryStream();
                DataStream.WriteNulls(size, temp.Length - 8);
                dataPool.Write(size);
            }
            //Entries.ForEach(entry => entry.Length = (int)entry.FilePath.Length);
            dataPool.Write(temp);
            temp.Position = Size;
            temp.CopyTo(Data);
            Data.Position = 0;
        }

        public void Add(FarcEntryBin entry) => Entries.Add(entry);

        [Ignore] public FarcEntryBin this[int i] => Entries[i];
        public IEnumerator<FarcEntryBin> GetEnumerator() => Entries.GetEnumerator();
        public override string ToString() => $"Farc: {Entries.Count} entries{(IsCompressed ? " Compressed" : "")} {(IsEncrypted ? " Encrypted" : "")}";
    }

    public class FarcEntryBin
    {
        private string _name;
        private int _length;
        
        [FieldOrder(0), FieldEncoding("UTF-8")]          public string FileName { get => FilePath?.Name ?? _name; set => _name = value; }
        [FieldOrder(1), FieldEndianness(Endianness.Big)] public int    Position;
        [FieldOrder(2),
         FieldEndianness(Endianness.Big),
         SerializeWhen("IsCompressed", true, 
         RelativeSourceMode = RelativeSourceMode.FindAncestor,
         AncestorType = typeof(FarcArchiveBin))]         public long   CompressedLength;
        [FieldOrder(3), FieldEndianness(Endianness.Big)] public int    Length { get => (int?)FilePath?.Length ?? _length; set => _length = value; }

        [Ignore] public FileInfo FilePath;
        [Ignore] public int Size => 8 + 1 + (CompressedLength == 0 ? 0 : 8) + FileName.Length;
        [Ignore] public bool IsCompressed => CompressedLength != Length && CompressedLength != 0;

        public FarcEntryBin() { }
        public FarcEntryBin(string path) => FilePath = new FileInfo(path);
        public FarcEntryBin(string name, int length) { FileName = name; Length = length; }

        public override string ToString() => $"Name:'{FileName}' at 0x{Position:X} S:{Length}{(IsCompressed ? $" CS:{CompressedLength}" : "")}";
    }

    public class FarcEntryConverter : IValueConverter
    {
        public object Convert(object value, object parameter, BinarySerializationContext context) => (int)value - 4;
        public object ConvertBack(object value, object parameter, BinarySerializationContext context) => (long)value + 4;
    }
}
