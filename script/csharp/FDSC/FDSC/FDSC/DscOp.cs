using System;
using System.IO;
using System.Collections.Generic;
using DIVALib.IO;

namespace DscOp
{
    public class DscFile
    {
        byte[] magic = { 0x20, 02, 02, 0x12 };
        List<DSCFunc> funcs = new List<DSCFunc>();
        public DscFile()
        {

        }

        public DscFile(Stream s)
        {
            if (DataStream.ReadBytes(s, 4) != magic)
            {
                return;
            }
            bool EOF = false;
            while (!EOF)
            {
                uint readFuncId = DataStream.ReadUInt32(s);
                EOF = true;
            }
        }

        public void Write(Stream s)
        {
            DataStream.WriteBytes(s, magic);
            foreach(DSCFunc func in funcs)
            {

            }
        }
    }

    public class DSCFunc
    {
        public uint func_id;

        public DSCFunc() { }

        public DSCFunc(Stream s, uint id)
        {
            func_id = id;
            if (DataStream.ReadUInt32(s) != func_id)
            {
                return;
            }
        }

        public virtual void Write(Stream s)
        {
            DataStream.WriteUInt32(s, func_id);
        }
    }

    public class fEnd : DSCFunc
    {
        public uint unk;

        public fEnd() : base() { }
        public fEnd(Stream s) : base(s, 0x00)
        {
            unk = DataStream.ReadUInt32(s);
        }

        public override void Write(Stream s)
        {
            DataStream.WriteUInt32(s, (uint)(unk));
        }
    }

    public class fTime : DSCFunc
    {
        public  float timestamp;

        public fTime() : base() { }
        public fTime(Stream s) : base(s, 0x01)
        {
            timestamp = DataStream.ReadUInt32(s) / 100.0f;
        }

        public override void Write(Stream s)
        {
            DataStream.WriteUInt32(s, (uint)(timestamp * 100) );
        }
    }
}