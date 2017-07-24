using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using BinOp;

namespace DscFunc
{
    public class DscHeader
    {
        public string magic;
        public long clPos;
        public uint contentLength;
        public uint unk;

        public DscHeader()
        {
            magic = "PVSC";
            clPos = 4;
            contentLength = 12;
            unk = 0;
        }

        public DscHeader(FileStream file)
        {
            BinaryReader br = new BinaryReader(file);
            magic = new string(br.ReadChars(4));
            clPos = file.Position;
            contentLength = br.ReadUInt32();
            file.Seek(24, SeekOrigin.Current);
            unk = br.ReadUInt32();
            file.Seek(28, SeekOrigin.Current);
        }

        public void Write(BinaryWriter bw)
        {
            FileOp.WriteString(bw, magic);
            bw.Write(contentLength);
            FileOp.WriteNullByte(bw, 24);
            bw.Write(unk);
            FileOp.WriteNullByte(bw, 28);
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

        public DscNote(BinaryReader br)
        {
            unk1       = FileOp.ReadUInt32(br, FileOp.Endian.BigEndian);
            timestamp  = FileOp.ReadUInt32(br, FileOp.Endian.BigEndian) / 1000;
            opcode     = FileOp.ReadUInt32(br, FileOp.Endian.BigEndian);
            type       = (NoteType)FileOp.ReadUInt32(br, FileOp.Endian.BigEndian);
            holdLength = FileOp.ReadInt32(br, FileOp.Endian.BigEndian) / 1000;
            isHoldEnd  = FileOp.ReadInt32(br, FileOp.Endian.BigEndian);
            posX       = FileOp.ReadUInt32(br, FileOp.Endian.BigEndian) / 10_000;
            posY       = FileOp.ReadUInt32(br, FileOp.Endian.BigEndian) / 10_000;
            curve1     = FileOp.ReadUInt32(br, FileOp.Endian.BigEndian); 
            curve2     = FileOp.ReadUInt32(br, FileOp.Endian.BigEndian); 
            unk2       = FileOp.ReadUInt32(br, FileOp.Endian.BigEndian); 
            unk3       = FileOp.ReadUInt32(br, FileOp.Endian.BigEndian); 
            timeOut    = FileOp.ReadUInt32(br, FileOp.Endian.BigEndian); 
            unk4       = FileOp.ReadUInt32(br, FileOp.Endian.BigEndian);
            unk5       = FileOp.ReadInt32(br, FileOp.Endian.BigEndian);
        }

        public void Write(BinaryWriter bw)
        {
            FileOp.Write(bw, unk1, FileOp.Endian.BigEndian);
            FileOp.WriteFloatAsUInt(bw, timestamp * 1000, FileOp.Endian.BigEndian);
            FileOp.Write(bw, opcode, FileOp.Endian.BigEndian);
            FileOp.Write(bw, (uint)type, FileOp.Endian.BigEndian);
            FileOp.WriteFloatAsUInt(bw, holdLength * 1000, FileOp.Endian.BigEndian);
            FileOp.Write(bw, isHoldEnd, FileOp.Endian.BigEndian);
            FileOp.WriteFloatAsUInt(bw, posX * 10_000, FileOp.Endian.BigEndian);
            FileOp.WriteFloatAsUInt(bw, posY * 10_000, FileOp.Endian.BigEndian);
            FileOp.WriteFloatAsUInt(bw, curve1, FileOp.Endian.BigEndian);
            FileOp.WriteFloatAsUInt(bw, curve2, FileOp.Endian.BigEndian);
            FileOp.Write(bw, unk2, FileOp.Endian.BigEndian);
            FileOp.Write(bw, unk3, FileOp.Endian.BigEndian);
            FileOp.Write(bw, timeOut, FileOp.Endian.BigEndian);
            FileOp.Write(bw, unk4, FileOp.Endian.BigEndian);
            FileOp.Write(bw, unk5, FileOp.Endian.BigEndian);
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
                    case "opcode": note.timestamp = uint.Parse(child.InnerText); break;
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
        public List<DscNote> notes;

        public DscFile()
        {
            header = new DscHeader();
            notes = new List<DscNote>();
        }

        public DscFile(FileStream file)
        {
            header = new DscHeader(file);
            notes = new List<DscNote>();
            if (header.magic != "PVSC")
            {
                return;
            }
            file.Seek(8, SeekOrigin.Current);
            uint counter = 8;
            BinaryReader br = new BinaryReader(file);
            while (counter <= header.contentLength - 60)
            {
                DscNote newNote = new DscNote(br);
                notes.Add(newNote);
                counter += 60;
            }
        }

        public void SaveToFile(FileStream file)
        {
            BinaryWriter bw = new BinaryWriter(file);
            header.contentLength = (uint)notes.Count * 60 + 12;
            header.Write(bw);
            file.Seek(8, SeekOrigin.Current);
            foreach(DscNote note in notes)
            {
                note.Write(bw);
            }
            bw.Write(0);
            FileOp.WriteString(bw, "EOFC");
            bw.Write(0);
            bw.Write(32);
            bw.Write(268_435_456);
            bw.Write(0); bw.Write(0); bw.Write(0); bw.Write(0);
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