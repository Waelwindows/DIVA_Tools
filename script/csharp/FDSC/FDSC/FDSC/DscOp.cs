using System;
using System.IO;
using System.Collections.Generic;
using DIVALib.IO;
using DIVALib.Math;

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
                    case 0x00: funcs.Add(new FEnd(s)); break;
                    case 0x01: funcs.Add(new FTime(s)); break;
                    case 0x02: funcs.Add(new FMikuMove(s)); break;
                    case 0x03: funcs.Add(new FMikuRotate(s)); break;
                    case 0x04: funcs.Add(new FMikuDisplay(s)); break;
                    case 0x05: funcs.Add(new FMikuShadow(s)); break;
                    case 0x06: funcs.Add(new FTarget(s)); break;
                    default: break;
                }
                if (s.Position == s.Length)
                {
                    EOF = true;
                }
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

	/// <summary>  
	///  Generic parent class for all Dsc functions
	/// </summary>  
	public class DSCFunc
    {
		/// <summary>  
		///  The number that specifies which function to use
		/// </summary>  
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

		/// <summary>  
		///  Generic write function for all Dsc functions
		/// </summary>  
		public virtual void Write(Stream s)
        {
            DataStream.WriteUInt32(s, func_id);
        }

        public override string ToString()
        {
            return string.Format(string.Format("[DSC Function] #{0:x}: {1}\n", func_id, this.GetType().Name));
        }
    }

	/// <summary>  
	///  Ends the song
	/// </summary>  
	public class FEnd : DSCFunc
    {
        public uint unk;

        public FEnd() { func_id = 0x00; }
        public FEnd(Stream s) : base(s, 0x00)
        {
            unk = DataStream.ReadUInt32(s);
        }

        public override void Write(Stream s)
        {
            DataStream.WriteUInt32(s, unk);
        }
    }

	/// <summary>  
	///  Makes the next function activate after set time
	/// </summary>  
	public class FTime : DSCFunc 
    {
		/// <summary>  
		/// Time in milliseconds
		/// </summary>  
		public float timestamp;

        public FTime() { func_id = 0x01; }
        public FTime(Stream s) : base(s, 0x01)
        {
            timestamp = DataStream.ReadUInt32(s) / 100.0f;
        }

        public override void Write(Stream s)
        {
            DataStream.WriteUInt32(s, (uint)(timestamp * 100) );
        }
    }

	/// <summary>  
	///  Moves the selected character to a specific position
	/// </summary>  
	public class FMikuMove : DSCFunc
	{
        public uint  playerID;
        public Vector3 position;

		public FMikuMove() { func_id = 0x02; }
		public FMikuMove(Stream s) : base(s, 0x02)
		{
            playerID = DataStream.ReadUInt32(s);
            position = new Vector3(DataStream.ReadInt32(s), DataStream.ReadInt32(s), DataStream.ReadInt32(s));
		}

		public override void Write(Stream s)
		{
			DataStream.WriteUInt32(s, playerID);
            foreach (float component in position)
            {
                DataStream.WriteInt32(s, (int)component);
            }
		}
	}

	/// <summary>  
	///  Changes the selected character's orientation
	/// </summary>  
	public class FMikuRotate : DSCFunc
	{
		public uint playerID;
        /// <summary>  
        /// The character's rotation on the Z axis
        /// </summary>  
        public int orientation;

		public FMikuRotate() { func_id = 0x03; }
		public FMikuRotate(Stream s) : base(s, 0x03)
		{
			playerID = DataStream.ReadUInt32(s);
            orientation = DataStream.ReadInt32(s);
		}

		public override void Write(Stream s)
		{
			DataStream.WriteUInt32(s, playerID);
			DataStream.WriteInt32(s, orientation);
		}
	}

	/// <summary>  
	///  Changes the selected character's display state
	/// </summary>  
	public class FMikuDisplay : DSCFunc
	{
		public uint playerID;
        public bool state;

		public FMikuDisplay() { func_id = 0x04; }
		public FMikuDisplay(Stream s) : base(s, 0x04)
		{
			playerID = DataStream.ReadUInt32(s);
            state = DataStream.ReadUInt32(s) == 1;
		}

		public override void Write(Stream s)
		{
			DataStream.WriteUInt32(s, playerID);
            DataStream.WriteUInt32(s, (uint)((state) ? 1 : 0));
		}
	}

	/// <summary>  
	///  Changes the selected character's shadow display state
	/// </summary>  
	public class FMikuShadow : DSCFunc
	{
		public uint playerID;
        public bool state;

		public FMikuShadow() { func_id = 0x05; }
		public FMikuShadow(Stream s) : base(s, 0x05)
		{
			playerID = DataStream.ReadUInt32(s);
			state = DataStream.ReadUInt32(s) == 1;
		}

		public override void Write(Stream s)
		{
			DataStream.WriteUInt32(s, playerID);
			DataStream.WriteUInt32(s, (uint)((state) ? 1 : 0));
		}
	}

	/// <summary>  
	///  Creates a new target with the specified paramaters
	/// </summary>  
	public class FTarget : DSCFunc
	{
        public enum EType
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
            HOLD_STAR = 14,
            CHANCE_STAR = 15
        };

        const int ud_pos = 100_000;
        const int ud_rot = 100_000;

        public EType type;
        public float holdLength;
        public bool isHoldEnd;
        public Vector2 position;
        public float oscillateAngle;
        public int oscillateFrequency;
        public float entryAngle;
        public uint oscillateAmplitude;
        public uint timeOut;
		

		public FTarget() { func_id = 0x06; }
		public FTarget(Stream s) : base(s, 0x06)
		{
            type = (EType)DataStream.ReadUInt32(s);
            holdLength = DataStream.ReadInt32(s) / 100.0f;
            isHoldEnd = DataStream.ReadInt32(s) != 1;
            position = new Vector2(DataStream.ReadUInt32(s) / ud_pos, DataStream.ReadUInt32(s) / ud_pos);
            oscillateAngle = DataStream.ReadInt32(s) / ud_rot;
            oscillateFrequency = DataStream.ReadInt32(s);
            entryAngle = DataStream.ReadInt32(s) / ud_rot;
            oscillateAmplitude = DataStream.ReadUInt32(s);
            timeOut = DataStream.ReadUInt32(s);
		}

		public override void Write(Stream s)
		{
			DataStream.WriteUInt32(s, (uint)type);
            DataStream.WriteInt32(s, (int)(holdLength * 100));
            DataStream.WriteInt32(s, isHoldEnd ? 0 : -1);
            DataStream.WriteUInt32(s, (uint)(position.x * ud_pos)); DataStream.WriteUInt32(s, (uint)(position.y * ud_pos));
            DataStream.WriteInt32(s, (int)(oscillateAngle * ud_rot));
            DataStream.WriteInt32(s, oscillateFrequency);
            DataStream.WriteInt32(s, (int)(entryAngle * ud_rot));
            DataStream.WriteUInt32(s, oscillateAmplitude);
            DataStream.WriteUInt32(s, timeOut);
		}
	}
}