using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using BinarySerialization;
using DIVALib.IO;
using DIVALib.FileBases;

namespace DIVALib.Databases
{
    public class TextureEntry
    {
        [FieldOrder(0)] public uint Id { get; set; }
        [FieldOrder(2)] public string Name { get; set; }

        [Ignore] public int Size => 8 + Name.Length;
    }

    // tex_db.bin
    public class TextureDatabase : FileFormatBase
    {
        private readonly List<TextureEntry> entries = new List<TextureEntry>();

        public List<TextureEntry> Entries => entries;

        public override void Read(Stream source)
        {
            uint textureCount = DataStream.ReadUInt32(source);
            uint texturesPosition = DataStream.ReadUInt32(source);

            source.Seek(texturesPosition, SeekOrigin.Begin);
            for (uint i = 0; i < textureCount; i++)
            {
                var entry = new TextureEntry();
                entry.Id = DataStream.ReadUInt32(source);
                entry.Name = StringPool.Read(source);
                entries.Add(entry);
            }
        }

        public override void Write(Stream destination)
        {
            destination.Seek(16, SeekOrigin.Begin);

            var strings = new Dictionary<string, long>();
            foreach (var value in entries.Select(entry => entry.Name))
            {
                if (strings.ContainsKey(value)) continue;
                strings.Add(value, destination.Position);
                DataStream.WriteCString(destination, value);
            }

            var texturesPosition = destination.Position;
            foreach (var entry in entries)
            {
                DataStream.WriteUInt32(destination, entry.Id);
                DataStream.WriteUInt32(destination, (uint)strings[entry.Name]);
            }

            destination.Seek(0, SeekOrigin.Begin);
            DataStream.WriteUInt32(destination, (uint)entries.Count);
            DataStream.WriteUInt32(destination, (uint)texturesPosition);
        }
    }

    public class TextureDatabaseSerial
    {
        [FieldOrder(0)]                                                          public int                      TextureCount;
        [FieldOrder(1)]                                                          public int                      TextureOffest;
        [FieldOrder(2)]                                                          public Int64                    Reserved;
        [FieldOrder(3), SerializeUntil(0)]                                       public List<string>             TextureNames;
        [FieldOrder(4), FieldCount("TextureCount"),FieldOffset("TextureOffest")] public List<TextureEntrySerial> TextureEntries;

        [Ignore] public List<TextureNameEntrySerial> NameEntries { get => TextureEntries.Zip(TextureNames, (e, n) => new TextureNameEntrySerial(e, n)).ToList(); set => ApplyChanges(value); }

        public void SetOffsets()
        {
            var offset = 16;
            for (var i = 0; i < TextureEntries.Count(); ++i)
            {
                offset += i != 0 ? TextureNames[i-1].Length+1 : 0;
                TextureEntries[i].Offset = offset;
            }
        }

        public void ApplyChanges(List<TextureNameEntrySerial> nameEntry = null)
        {
            nameEntry = nameEntry ?? NameEntries;
            TextureNames = nameEntry.Select(entry => entry.Name).ToList();
            TextureEntries = nameEntry.Select(entry => entry.ToEntrySerial()).ToList();
            
            TextureOffest = TextureNames.Sum(name => name.Length + 1) + 15;
            TextureOffest += 4 - (TextureOffest % 4);
        }
    }

    public class TextureEntrySerial
    {
        [FieldOrder(0)] public int Id;
        [FieldOrder(1)] public int Offset;

        [Ignore] public virtual int Size => 8;

        public TextureEntrySerial(int id, int offset)
        {
            Id = id;
            Offset = offset;
        }
    }

    public class TextureNameEntrySerial : TextureEntrySerial
    {
        public string Name;

        [Ignore] public override int Size => 8 + Name.Count() + 1;

        public TextureNameEntrySerial(TextureEntrySerial entry, string name) : base(entry.Id, entry.Offset) => Name = name;

        public override string ToString() => $"Texture: \"{Name}\" ID {Id} at 0x{Offset:X}";

        public TextureEntrySerial ToEntrySerial() => new TextureEntrySerial(Id, Offset);
    }
}
