using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;

using DIVALib.FileBases;

namespace DIVALib.IO
{
    public class DataPool
    {
        private readonly List<DataItem> items = new List<DataItem>();
        private uint alignment = 1;

        public uint Alignment
        {
            get
            {
                return alignment;
            }

            set
            {
                alignment = value > 0 ? value : 0;
            }
        }

        public byte AlignmentNullByte { get; set; }

        public bool IsBigEndian { get; set; }

        public void Add(object source, long position, bool isCompressed = false, bool ignoreLength = false)
        {
            items.Add(new DataItem
            {
                Source = source,
                Position = position,
                IsCompressed = isCompressed,
                IgnoreLength = ignoreLength,
            });
        }

        public void Add(Stream destination, object source, bool isCompressed = false, bool ignoreLength = false)
        {
            Add(source, destination.Position, isCompressed, ignoreLength);

            DataStream.WriteUInt32(destination, 0u);

            if (!ignoreLength)
            {
                DataStream.WriteUInt32(destination, 0u);
            }
        }

        public void Write(Stream destination)
        {
            foreach (var item in items)
            {
                long itemPosition = destination.Position;

                if (item.Source is FileFormatBase fileFormatBase)
                {
                    fileFormatBase.Write(destination);
                }

                else
                {
                    Stream source =
                        item.Source is Stream stream ? stream :
                        item.Source is byte[] byteArray ? new MemoryStream(byteArray) :
                        item.Source is FileInfo fileInfo ? (Stream)fileInfo.OpenRead() :
                        throw new ArgumentException(nameof(item.Source));

                    if (item.IsCompressed)
                    {
                        using (GZipStream gzipStream = new GZipStream(destination, CompressionMode.Compress, true))
                        {
                            source.CopyTo(gzipStream);
                        }
                    }

                    else
                    {
                        source.CopyTo(destination);
                    }

                    if (!(item.Source is Stream))
                    {
                        source.Close();
                    }
                }

                long previousPosition = destination.Position;

                destination.Seek(item.Position, SeekOrigin.Begin);
                DataStream.WriteUInt32E(destination, (uint)itemPosition, IsBigEndian);

                if (!item.IgnoreLength)
                {
                    DataStream.WriteUInt32E(destination, (uint)(previousPosition - itemPosition), IsBigEndian);
                }

                destination.Seek(previousPosition, SeekOrigin.Begin);
                DataStream.Pad(destination, Alignment, AlignmentNullByte);
            }
        }

        private class DataItem
        {
            public object Source;
            public long Position;
            public bool IsCompressed;
            public bool IgnoreLength;
        }
    }
}
