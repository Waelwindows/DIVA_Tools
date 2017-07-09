using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using DIVALib.IO;
using DIVALib.FileBases;

namespace DIVALib.Databases
{
    public class AetFileEntry
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public uint Index { get; set; }
        public uint SpriteId { get; set; }
    }

    public class AetEntry
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public ushort Index { get; set; }
        public ushort GroupId { get; set; }
    }

    // aet_db.bin
    public class AetDatabase : FileFormatBase
    {
        private readonly List<AetFileEntry> fileEntries = new List<AetFileEntry>();
        private readonly List<AetEntry> entries = new List<AetEntry>();

        public List<AetFileEntry> FileEntries
        {
            get
            {
                return fileEntries;
            }
        }

        public List<AetEntry> Entries
        {
            get
            {
                return entries;
            }
        }

        public override void Read(Stream source)
        {
            uint aetFileEntriesCount = DataStream.ReadUInt32(source);
            uint aetFileEntriesPosition = DataStream.ReadUInt32(source);
            uint aetEntriesCount = DataStream.ReadUInt32(source);
            uint aetEntriesPosition = DataStream.ReadUInt32(source);

            source.Seek(aetFileEntriesPosition, SeekOrigin.Begin);
            for (int i = 0; i < aetFileEntriesCount; i++)
            {
                var entry = new AetFileEntry();
                entry.Id = DataStream.ReadUInt32(source);
                entry.Name = StringPool.Read(source);
                entry.FileName = StringPool.Read(source);
                entry.Index = DataStream.ReadUInt32(source);
                entry.SpriteId = DataStream.ReadUInt32(source);
                fileEntries.Add(entry);
            }

            source.Seek(aetEntriesPosition, SeekOrigin.Begin);
            for (int i = 0; i < aetEntriesCount; i++)
            {
                var entry = new AetEntry();
                entry.Id = DataStream.ReadUInt32(source);
                entry.Name = StringPool.Read(source);
                entry.Index = DataStream.ReadUInt16(source);
                entry.GroupId = DataStream.ReadUInt16(source);
                entries.Add(entry);
            }
        }

        public override void Write(Stream destination)
        {
            destination.Seek(16, SeekOrigin.Begin);

            var stringPool1 = new StringPool();
            stringPool1.GroupAlignment = 0x10;

            long aetFileEntriesPosition = destination.Position;
            foreach (var entry in fileEntries)
            {
                DataStream.WriteUInt32(destination, entry.Id);
                stringPool1.Add(destination, entry.Name);
                stringPool1.Add(destination, entry.FileName);
                DataStream.WriteUInt32(destination, entry.Index);
                DataStream.WriteUInt32(destination, entry.SpriteId);
            }

            DataStream.Pad(destination, 0x10);

            var stringPool2 = new StringPool();
            stringPool2.GroupAlignment = 0x4;

            long aetEntriesPosition = destination.Position;
            foreach (var entry in entries)
            {
                DataStream.WriteUInt32(destination, entry.Id);
                stringPool2.Add(destination, entry.Name, entry.GroupId);
                DataStream.WriteUInt16(destination, entry.Index);
                DataStream.WriteUInt16(destination, entry.GroupId);
            }

            stringPool1.Write(destination);
            stringPool2.Write(destination);

            destination.Seek(0, SeekOrigin.Begin);
            DataStream.WriteUInt32(destination, (uint)fileEntries.Count);
            DataStream.WriteUInt32(destination, (uint)aetFileEntriesPosition);
            DataStream.WriteUInt32(destination, (uint)entries.Count);
            DataStream.WriteUInt32(destination, (uint)aetEntriesPosition);
        }
    }
}
