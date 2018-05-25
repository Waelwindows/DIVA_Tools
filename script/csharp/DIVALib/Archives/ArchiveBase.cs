using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

using DIVALib.IO;
using DIVALib.FileBases;
using System.Collections;

namespace DIVALib.Archives
{
    public abstract class EntryBase
    {
        protected long length;

        public virtual long Position { get; set; }

        public virtual long Length
        {
            get => FilePath?.Length ?? length;

            set => length = value;
        }

        public virtual FileInfo FilePath { get; set; }

        public virtual Stream Open(Stream source)
        {
            return new SubStream(source, Position, length);
        }

        public virtual Stream Open()
        {
            return FilePath.OpenRead();
        }
    }

    public abstract class ArchiveBase<T> : FileFormatBase, IList<T>
    {
        protected List<T> entries = new List<T>();

        public virtual int Count
        {
            get
            {
                return entries.Count;
            }
        }

        public virtual bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public virtual T this[int index]
        {
            get
            {
                return entries[index];
            }

            set
            {
                entries[index] = value;
            }
        }

        public virtual int IndexOf(T item)
        {
            return entries.IndexOf(item);
        }

        public virtual void Insert(int index, T item)
        {
            entries.Insert(index, item);
        }

        public virtual void RemoveAt(int index)
        {
            entries.RemoveAt(index);
        }

        public virtual void Add(T item)
        {
            entries.Add(item);
        }

        public virtual void Clear()
        {
            entries.Clear();
        }

        public virtual bool Contains(T item)
        {
            return entries.Contains(item);
        }

        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            entries.CopyTo(array, arrayIndex);
        }

        public virtual bool Remove(T item)
        {
            return entries.Remove(item);
        }

        public virtual IEnumerator<T> GetEnumerator()
        {
            return entries.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return entries.GetEnumerator();
        }
    }

    public abstract class EntryBaseBin
    {
        protected long length;

        public virtual long Position { get; set; }

        public virtual long Length { get => FilePath?.Length ?? length;  set => length = value; }

        public virtual FileInfo FilePath { get; set; }

        public virtual Stream Open(Stream source) => new SubStream(source, Position, length);

        public virtual Stream Open() => FilePath.OpenRead();
    }
}
