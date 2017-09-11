using System;
using System.IO;
using System.Collections.Generic;
using DIVALib.IO;

namespace DscOp
{
    public class DscFile
    {
        const uint magic = 302121504;
        public List<DSCFunc> funcs = new List<DSCFunc>();
        public DscFile()
        {

        }

        public DscFile(Stream s)
        {
            if (DataStream.ReadInt32(s) != magic)
            {
                return;
            }
            bool EOF = false;
            while (!EOF)
            {
                uint readFuncId = DataStream.ReadUInt32(s);
                s.Position -= 4;
                switch(readFuncId)
                {
                    case 0x00: funcs.Add(new fEnd(s)); break;
                    case 0x01: funcs.Add(new fTime(s)); break;
                    default: break;
                }
                EOF = true;
            }
        }

        public void Write(Stream s)
        {
            DataStream.WriteUInt32(s, magic, DataStream.Endian.LittleEndian);
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

        public fEnd() { func_id = 0x00; }
        public fEnd(Stream s) : base(s, 0x00)
        {
            unk = DataStream.ReadUInt32(s);
        }

        public override void Write(Stream s)
        {
            DataStream.WriteUInt32(s, unk);
        }
    }

    public class fTime : DSCFunc
    {
        public  float timestamp;

        public fTime() { func_id = 0x01; }
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