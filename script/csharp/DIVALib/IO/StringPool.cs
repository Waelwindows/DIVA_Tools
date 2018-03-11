using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;

using BinarySerialization;

namespace DIVALib.IO
{
    public class StringPool
    {
        private readonly List<StringItem> items = new List<StringItem>();

        private uint stringAlignment = 1;
        private uint groupAlignment = 1;

        public uint StringAlignment
        {
            get => stringAlignment;

            set => stringAlignment = value > 0 ? value : 1;
        }

        public uint GroupAlignment
        {
            get => groupAlignment;

            set => groupAlignment = value > 0 ? value : 1;
        }

        public bool IsBigEndian { get; set; }

        public void Add(Stream destination, string value, int group = -1)
        {
            Add(value, destination.Position, group);
            DataStream.WriteUInt32(destination, 0u);
        }

        public void Add(string value, long position, int group = -1)
        {
            if (items.Exists(item => item.Value == value && item.Group == group))
            {
                var stringItem = items.Find(item => item.Value == value && item.Group == group);
                stringItem.Positions.Add(position);
            }

            else
            {
                var stringItem = new StringItem();
                stringItem.Value = value;
                stringItem.Group = group;
                stringItem.Positions.Add(position);
                items.Add(stringItem);
            }
        }

        public void Write(Stream destination)
        {
            foreach (var group in items.GroupBy(stringItem => stringItem.Group))
            {
                foreach (var item in group.OrderBy(stringItem => stringItem.Positions[0]))
                {
                    long stringPosition = destination.Position;
                    DataStream.WriteCString(destination, item.Value, Encoding.UTF8);
                    DataStream.Pad(destination, stringAlignment);

                    foreach (long position in item.Positions)
                    {
                        DataStream.WriteUInt32EAt(destination, (uint)stringPosition, position, IsBigEndian);
                    }
                }

                DataStream.Pad(destination, groupAlignment);
            }
        }

        public static string Read(Stream source) => Read(source, DataStream.ReadUInt32(source));

        public static string ReadBE(Stream source) => Read(source, DataStream.ReadUInt32BE(source));

        public static string Read(Stream source, long position)
        {
            long previousPosition = source.Position;

            source.Seek(position, SeekOrigin.Begin);
            string value = DataStream.ReadCString(source, Encoding.UTF8);
            source.Seek(previousPosition, SeekOrigin.Begin);
            return value;
        }

        private class StringItem
        {
            public readonly List<long> Positions = new List<long>();

            public string Value;
            public int Group;
        }
    }
}
