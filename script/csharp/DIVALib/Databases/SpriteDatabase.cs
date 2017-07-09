using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using DIVALib.IO;
using DIVALib.FileBases;

namespace DIVALib.Databases
{
    public class SpriteFileEntry
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public uint Index { get; set; }
    }

    public class SpriteEntry
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public ushort Index { get; set; }
        public ushort GroupId { get; set; }
    }

    // spr_db.bin
    public class SpriteDatabase : FileFormatBase
    {
        private readonly List<SpriteFileEntry> fileEntries = new List<SpriteFileEntry>();
        private readonly List<SpriteEntry> entries = new List<SpriteEntry>();

        public List<SpriteFileEntry> FileEntries
        {
            get
            {
                return fileEntries;
            }
        }

        public List<SpriteEntry> Entries
        {
            get
            {
                return entries;
            }
        }

        public override void Read(Stream source)
        {
            uint spriteFileEntriesCount = DataStream.ReadUInt32(source);
            uint spriteFileEntriesPosition = DataStream.ReadUInt32(source);
            uint spriteEntriesCount = DataStream.ReadUInt32(source);
            uint spriteEntriesPosition = DataStream.ReadUInt32(source);

            source.Seek(spriteFileEntriesPosition, SeekOrigin.Begin);
            for (int i = 0; i < spriteFileEntriesCount; i++)
            {
                var entry = new SpriteFileEntry();
                entry.Id = DataStream.ReadUInt32(source);
                entry.Name = StringPool.Read(source);
                entry.FileName = StringPool.Read(source);
                entry.Index = DataStream.ReadUInt32(source);
                fileEntries.Add(entry);
            }

            source.Seek(spriteEntriesPosition, SeekOrigin.Begin);
            for (int i = 0; i < spriteEntriesCount; i++)
            {
                var entry = new SpriteEntry();
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

            long spriteFileEntriesPosition = destination.Position;
            foreach (var entry in fileEntries)
            {
                DataStream.WriteUInt32(destination, entry.Id);
                stringPool1.Add(destination, entry.Name);
                stringPool1.Add(destination, entry.FileName);
                DataStream.WriteUInt32(destination, entry.Index);
            }

            DataStream.Pad(destination, 0x10);

            var stringPool2 = new StringPool();
            stringPool2.GroupAlignment = 0x4;

            long spriteEntriesPosition = destination.Position;
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
            DataStream.WriteUInt32(destination, (uint)spriteFileEntriesPosition);
            DataStream.WriteUInt32(destination, (uint)entries.Count);
            DataStream.WriteUInt32(destination, (uint)spriteEntriesPosition);
        }
    }
}
