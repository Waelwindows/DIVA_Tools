using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using DIVALib.IO;
using DIVALib.FileBases;

namespace DIVALib.Databases
{
    public class TextureEntry
    {
        public uint Id { get; set; }
        public string Name { get; set; }
    }

    // tex_db.bin
    public class TextureDatabase : FileFormatBase
    {
        private readonly List<TextureEntry> entries = new List<TextureEntry>();

        public List<TextureEntry> Entries
        {
            get
            {
                return entries;
            }
        }

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

            Dictionary<string, long> strings = new Dictionary<string, long>();
            foreach (string value in entries.Select(entry => entry.Name))
            {
                if (!strings.ContainsKey(value))
                {
                    strings.Add(value, destination.Position);
                    DataStream.WriteCString(destination, value);
                }
            }

            long texturesPosition = destination.Position;
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
}
