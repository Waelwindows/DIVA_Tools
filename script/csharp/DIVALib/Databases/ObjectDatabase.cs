using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using BinarySerialization;
using DIVALib.IO;
using DIVALib.FileBases;

namespace DIVALib.Databases
{
    public class MeshEntry
    {
        [FieldOrder(0)] public ushort Id { get; set; }
        [FieldOrder(1)] public string Name { get; set; }
    }

    public class ObjectEntry
    {
        public string Name { get; set; }
        public uint Id { get; set; }
        public string FileName { get; set; }
        public string TextureFileName { get; set; }
        public string FarcName { get; set; }
        public List<MeshEntry> Meshes { get; set; }

        public ObjectEntry()
        {
            Meshes = new List<MeshEntry>();
        }
    }

    // obj_db.bin
    public class ObjectDatabase : FileFormatBase
    {
        private readonly List<ObjectEntry> entries = new List<ObjectEntry>();
        
        public uint Unknown { get; set; }

        public List<ObjectEntry> Entries
        {
            get
            {
                return entries;
            }
        }

        public override void Read(Stream source)
        {
            uint objectCount = DataStream.ReadUInt32(source);
            Unknown = DataStream.ReadUInt32(source);
            uint objectsPosition = DataStream.ReadUInt32(source);
            uint meshCount = DataStream.ReadUInt32(source);
            uint meshesPosition = DataStream.ReadUInt32(source);

            source.Seek(objectsPosition, SeekOrigin.Begin);

            Dictionary<uint, ObjectEntry> entryDictionary = new Dictionary<uint, ObjectEntry>();
            for (uint i = 0; i < objectCount; i++)
            {
                var entry = new ObjectEntry
                {
                    Name = StringPool.Read(source),
                    Id = DataStream.ReadUInt32(source),
                    FileName = StringPool.Read(source),
                    TextureFileName = StringPool.Read(source),
                    FarcName = StringPool.Read(source),
                };

                entries.Add(entry);
                entryDictionary.Add(entry.Id, entry);

                source.Seek(16, SeekOrigin.Current);
            }

            source.Seek(meshesPosition, SeekOrigin.Begin);
            for (uint i = 0; i < meshCount; i++)
            {
                var mesh = new MeshEntry();
                mesh.Id = DataStream.ReadUInt16(source);
                ushort parent = DataStream.ReadUInt16(source);
                mesh.Name = StringPool.Read(source);
                entryDictionary[parent].Meshes.Add(mesh);
            }
        }

        public override void Write(Stream destination)
        {
            destination.Seek(32, SeekOrigin.Begin);

            Dictionary<string, long> strings = new Dictionary<string, long>();

            void AddString(string value)
            {
                if (!strings.ContainsKey(value))
                {
                    strings.Add(value, destination.Position);
                    DataStream.WriteCString(destination, value);
                }
            }

            foreach (var entry in entries)
            {
                AddString(entry.Name);
                AddString(entry.FileName);
                AddString(entry.TextureFileName);
                AddString(entry.FarcName);
            }

            DataStream.Pad(destination, 16);

            long objectsPosition = destination.Position;
            foreach (var entry in entries)
            {
                DataStream.WriteUInt32(destination, (uint)strings[entry.Name]);
                DataStream.WriteUInt32(destination, entry.Id);
                DataStream.WriteUInt32(destination, (uint)strings[entry.FileName]);
                DataStream.WriteUInt32(destination, (uint)strings[entry.TextureFileName]);
                DataStream.WriteUInt32(destination, (uint)strings[entry.FarcName]);
                destination.Seek(16, SeekOrigin.Current);
            }

            DataStream.Pad(destination, 16);

            strings.Clear();

            foreach (var entry in entries)
            {
                foreach (var mesh in entry.Meshes)
                {
                    AddString(mesh.Name);
                }
            }

            DataStream.Pad(destination, 16);

            uint meshCount = 0;
            long meshesPosition = destination.Position;
            foreach (var entry in entries)
            {
                foreach (var mesh in entry.Meshes)
                {
                    meshCount++;
                    DataStream.WriteUInt16(destination, mesh.Id);
                    DataStream.WriteUInt16(destination, (ushort)entry.Id);
                    DataStream.WriteUInt32(destination, (uint)strings[mesh.Name]);
                }
            }

            DataStream.Pad(destination, 16);

            destination.Seek(0, SeekOrigin.Begin);
            DataStream.WriteUInt32(destination, (uint)entries.Count);
            DataStream.WriteUInt32(destination, Unknown);
            DataStream.WriteUInt32(destination, (uint)objectsPosition);
            DataStream.WriteUInt32(destination, meshCount);
            DataStream.WriteUInt32(destination, (uint)meshesPosition);
        }
    }
}
