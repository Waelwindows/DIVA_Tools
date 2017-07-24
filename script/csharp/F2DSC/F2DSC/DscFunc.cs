using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using DIVALib.IO;

namespace DscFunc
{
    public class DscHeader
    {
        public string magic;
        public uint size;
        public uint version;
        public long clPos;
        public uint contentLength;
        public uint unk1;
        public uint unk2;

        public DscHeader()
        {
            magic = "PVSC";
            size = 64;
            version = 402_653_184;
            clPos = 4;
            contentLength = 12;
            unk1 = 0;
            unk2 = 0;
        }

        public DscHeader(Stream s)
        {
            magic = DataStream.ReadCString(s, 4);
            contentLength = DataStream.ReadUInt32(s);
            size = DataStream.ReadUInt32(s);
            version = DataStream.ReadUInt32(s);
            s.Seek(16, SeekOrigin.Current);
            unk1 = DataStream.ReadUInt32(s);
            s.Seek(12, SeekOrigin.Current);
            unk2 = DataStream.ReadUInt32(s);
            s.Seek(12, SeekOrigin.Current);
        }

        public void Write(Stream s)
        {
            DataStream.WriteMagic(s, magic);
            DataStream.WriteUInt32(s, contentLength);
            DataStream.WriteUInt32(s, size);
            DataStream.WriteUInt32(s, version);
            DataStream.WriteUInt32(s, 0);
            DataStream.WriteUInt32(s, contentLength);
            DataStream.WriteNulls(s, 8);
            DataStream.WriteUInt32(s, unk1);
            DataStream.WriteNulls(s, 12);
            DataStream.WriteUInt32(s, unk2);
            DataStream.WriteNulls(s, 12);
        }
    }

    public class DscNote
    {
        //uint offset;
        public uint unk1;
        public float timestamp;
        public uint opcode;
        public NoteType type;
        public float holdLength;
        public int isHoldEnd;
        public float posX;
        public float posY;
        public float curve1;
        public float curve2;
        public uint unk2;
        public uint unk3;
        public float timeOut;
        public uint unk4;
        public int unk5;

        public enum NoteType : uint
        {
            TRIANGLE = 0,
            CIRCLE = 1,
            CROSS = 2,
            SQUARE = 3,
            ARROW_TRIANGLE = 4,
            ARROW_CIRCLE = 5,
            ARROW_CROSS = 6,
            ARROW_SQUARE = 7,
            HOLD_TRIANGLE = 8,
            HOLD_CIRCLE = 9,
            HOLD_CROSS = 10,
            HOLD_SQUARE = 11,
            STAR = 12,
            DOUBLE_STAR = 14,
            CHANCE_STAR = 15,
            LINKED_STAR = 22,
            LINKED_STAR_END = 23
        };

        public DscNote()
        {

        }

        public DscNote(Stream s)
        {
            unk1       = DataStream.ReadUInt32(s, DataStream.Endian.BigEndian);
            timestamp  = DataStream.ReadUInt32(s, DataStream.Endian.BigEndian) / 1000.0f;
            opcode     = DataStream.ReadUInt32(s, DataStream.Endian.BigEndian);
            type       = (NoteType)DataStream.ReadUInt32(s, DataStream.Endian.BigEndian);
            holdLength = DataStream.ReadInt32(s, DataStream.Endian.BigEndian) / 1000.0f;
            isHoldEnd  = DataStream.ReadInt32(s, DataStream.Endian.BigEndian);
            posX       = DataStream.ReadUInt32(s, DataStream.Endian.BigEndian) / 10_000.0f;
            posY       = DataStream.ReadUInt32(s, DataStream.Endian.BigEndian) / 10_000.0f;
            curve1     = DataStream.ReadUInt32(s, DataStream.Endian.BigEndian); 
            curve2     = DataStream.ReadUInt32(s, DataStream.Endian.BigEndian); 
            unk2       = DataStream.ReadUInt32(s, DataStream.Endian.BigEndian); 
            unk3       = DataStream.ReadUInt32(s, DataStream.Endian.BigEndian); 
            timeOut    = DataStream.ReadUInt32(s, DataStream.Endian.BigEndian); 
            unk4       = DataStream.ReadUInt32(s, DataStream.Endian.BigEndian);
            unk5       = DataStream.ReadInt32(s, DataStream.Endian.BigEndian);
        }
        
        public void Write(Stream s)
        {
            DataStream.WriteUInt32(s, unk1, DataStream.Endian.BigEndian);
            DataStream.WriteUInt32(s, (uint)(timestamp*1000.0f), DataStream.Endian.BigEndian);
            DataStream.WriteUInt32(s, opcode, DataStream.Endian.BigEndian);
            DataStream.WriteUInt32(s, (uint)type, DataStream.Endian.BigEndian);
            DataStream.WriteInt32(s, (int)(holdLength*1000.0f), DataStream.Endian.BigEndian);
            DataStream.WriteInt32(s, isHoldEnd, DataStream.Endian.BigEndian);
            DataStream.WriteUInt32(s, (uint)(posX * 10_000.0f), DataStream.Endian.BigEndian);
            DataStream.WriteUInt32(s, (uint)(posY * 10_000.0f), DataStream.Endian.BigEndian);
            DataStream.WriteInt32(s, (int)curve1, DataStream.Endian.BigEndian);
            DataStream.WriteInt32(s, (int)curve2, DataStream.Endian.BigEndian);
            DataStream.WriteUInt32(s, unk2, DataStream.Endian.BigEndian);
            DataStream.WriteUInt32(s, unk3, DataStream.Endian.BigEndian);
            DataStream.WriteUInt32(s, (uint)timeOut, DataStream.Endian.BigEndian);
            DataStream.WriteUInt32(s, unk4, DataStream.Endian.BigEndian);
            DataStream.WriteInt32(s, unk5, DataStream.Endian.BigEndian);
        }

        public static DscNote FromXmlNode(XmlNode node)
        {
            if (node.Name != "note")
            {
                return new DscNote();
            }
            DscNote note = new DscNote();
            foreach (XmlNode child in node.ChildNodes)
            {
                switch (child.Name)
                {
                    case "unk1": note.unk1 = uint.Parse(child.InnerText); break;
                    case "timestamp": note.timestamp = float.Parse(child.InnerText); break;
                    case "opcode": note.opcode = uint.Parse(child.InnerText); break;
                    case "type": note.type = (NoteType)NoteType.Parse(typeof(NoteType), child.InnerText); break;
                    case "holdLength": note.holdLength = float.Parse(child.InnerText); break;
                    case "isHoldEnd": note.isHoldEnd = int.Parse(child.InnerText); break;
                    case "posX": note.posX = float.Parse(child.InnerText); break;
                    case "posY": note.posX = float.Parse(child.InnerText); break;
                    case "curve1": note.curve1 = float.Parse(child.InnerText); break;
                    case "curve2": note.curve1 = float.Parse(child.InnerText); break;
                    case "unk2": note.unk2 = uint.Parse(child.InnerText); break;
                    case "unk3": note.unk3 = uint.Parse(child.InnerText); break;
                    case "timeOut": note.timeOut = float.Parse(child.InnerText); break;
                    case "unk4": note.unk4 = uint.Parse(child.InnerText); break;
                    case "unk5": note.unk5 = int.Parse(child.InnerText); break;
                    default: return new DscNote();
                }
            }
            return note;
        }

        public void AddNodeToXml(XmlDocument doc)
        {
            XmlNode noteNode = doc.CreateElement("note");

            XmlNode unk1Node       = doc.CreateElement("unk1"); unk1Node.InnerText = unk1.ToString();
            XmlNode timestampNode  = doc.CreateElement("timestamp"); timestampNode.InnerText = Convert.ToString(timestamp);
            XmlNode opcodeNode     = doc.CreateElement("opcode"); opcodeNode.InnerText = opcode.ToString();
            XmlNode typeNode       = doc.CreateElement("type"); typeNode.InnerText = type.ToString();
            XmlNode holdLengthNode = doc.CreateElement("holdLength"); holdLengthNode.InnerText = Convert.ToString(holdLength);
            XmlNode isHoldEndNode  = doc.CreateElement("isHoldEnd"); isHoldEndNode.InnerText = isHoldEnd.ToString();
            XmlNode posXNode       = doc.CreateElement("posX"); posXNode.InnerText = Convert.ToString(posX);
            XmlNode posYNode       = doc.CreateElement("posY"); posYNode.InnerText = Convert.ToString(posY);
            XmlNode curve1Node     = doc.CreateElement("curve1"); curve1Node.InnerText = curve1.ToString();
            XmlNode curve2Node     = doc.CreateElement("curve2"); curve2Node.InnerText = curve2.ToString();
            XmlNode unk2Node       = doc.CreateElement("unk2"); unk2Node.InnerText = unk2.ToString();
            XmlNode unk3Node       = doc.CreateElement("unk3"); unk3Node.InnerText = unk3.ToString();
            XmlNode timeOutNode    = doc.CreateElement("timeOut"); timeOutNode.InnerText = Convert.ToString(timeOut);
            XmlNode unk4Node       = doc.CreateElement("unk4"); unk4Node.InnerText = unk4.ToString();
            XmlNode unk5Node       = doc.CreateElement("unk5"); unk5Node.InnerText = unk5.ToString();
            
            noteNode.AppendChild(unk1Node);
            noteNode.AppendChild(timestampNode);
            noteNode.AppendChild(opcodeNode);
            noteNode.AppendChild(typeNode);
            noteNode.AppendChild(holdLengthNode);
            noteNode.AppendChild(isHoldEndNode);
            noteNode.AppendChild(posXNode);
            noteNode.AppendChild(posYNode);
            noteNode.AppendChild(curve1Node);
            noteNode.AppendChild(curve2Node);
            noteNode.AppendChild(unk2Node);
            noteNode.AppendChild(unk3Node);
            noteNode.AppendChild(timeOutNode);
            noteNode.AppendChild(unk4Node);
            noteNode.AppendChild(unk5Node);

            doc.DocumentElement.AppendChild(noteNode);
        }
    }

    public class DscFile
    {

        public DscHeader header;
        public bool[] flags = new bool[8];
        public List<DscNote> notes;

        public DscFile()
        {
            header = new DscHeader();
            notes = new List<DscNote>();
        }

        public DscFile(Stream s)
        {
            header = new DscHeader(s);
            notes = new List<DscNote>();
            if (header.magic != "PVSC")
            {
                return;
            }
            //s.Seek(8, SeekOrigin.Current);
            for (int i=0; i<8; i++)
            {
                flags[i] = DataStream.ReadBoolean(s);
            }
            uint counter = 8;
            while (counter <= header.contentLength - 60)
            {
                DscNote newNote = new DscNote(s);
                notes.Add(newNote);
                counter += 60;
            }
        }

        public void SaveToFile(Stream s)
        {
            header.contentLength = (uint)notes.Count * 60 + 12;
            header.Write(s);
            DataStream.WriteUInt32(s, 0); DataStream.WriteUInt32(s, 0); 
            foreach (DscNote note in notes)
            {
                note.Write(s);
            }
            DataStream.WriteUInt32(s, 0);
            DataStream.WriteMagic(s, "EOFC");
            DataStream.WriteUInt32(s, 0);
            DataStream.WriteUInt32(s, 32, DataStream.Endian.BigEndian);
            DataStream.WriteUInt32(s, 268_435_456, DataStream.Endian.BigEndian);
            DataStream.WriteUInt32(s, 0); DataStream.WriteUInt32(s, 0); DataStream.WriteUInt32(s, 0); DataStream.WriteUInt32(s, 0);
        }

        public void CreateNotesFromXml(XmlDocument doc)
        {
            foreach(XmlNode node in doc.DocumentElement)
            {
                notes.Add(DscNote.FromXmlNode(node));
            }
        }

        public void OutputToXml(XmlDocument doc)
        {
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            XmlNode root = doc.CreateElement("f2nd_dsc");
            doc.AppendChild(dec);
            doc.AppendChild(root);
            XmlComment commentNode = doc.CreateComment("");
            foreach (DscNote note in notes)
            {
                note.AddNodeToXml(doc);
            }
        }
    }
}