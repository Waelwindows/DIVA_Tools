using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using DIVALib.IO;

namespace DIVALib.Archives
{
    public class FarcEntry : EntryBase
    {
        public string FileName { get; set; }
        public long CompressedLength { get; set; }
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
            string signature = DataStream.ReadCString(source, 4);

            if (signature != "FArC" && signature != "FArc" && signature != "FARC")
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
}
