using System;
using System.Xml.Serialization;
using BinarySerialization;
using DIVALib.Math;

namespace DIVALib.DSCUtils
{
    /// <summary>
    ///     Generic parent class for all Dsc functions
    /// </summary>
    [Serializable]
    public abstract class DscFunc
    {
        /// <summary>
        ///     The number that specifies which function to be used
        /// </summary>
        [XmlIgnore] public int functionId;

        /// <summary>
        ///     The string representation of a DSC function
        /// </summary>
        public override string ToString()
        {
            return string.Format("[DSC Function] 0x{0:X}: {1}\n", functionId, GetType().Name);
        }
    }

    /// <summary>
    ///     Ends the song
    /// </summary>
    public class FEnd : DscFunc
    {
        public uint unk;

        public FEnd()
        {
            functionId = 0x00;
        }
    }

    /// <summary>
    ///     Makes the next function activate after set time
    /// </summary>
    public class FTime : DscFunc
    {
        [FieldOrder(0)] [FieldScale(100)] [SerializeAs(SerializedType.UInt4)] public double timeStamp;

        public FTime()
        {
            functionId = 0x01;
        }
    }

    /// <summary>
    ///     Makes the next function activate after set time
    /// </summary>
    public class F2Time : DscFunc
    {
        [FieldOrder(0)] [FieldEndianness(Endianness.Big)] public uint timeStamp;

        public F2Time()
        {
            functionId = 0x01;
        }
    }

    /// <summary>
    ///     Moves the selected character to a specific position
    /// </summary>
    public class FMikuMove : DscFunc
    {
        [FieldOrder(0)] public uint playerID;

        [FieldOrder(1)] public Vector3 position;

        public FMikuMove()
        {
            functionId = 0x02;
        }
    }

    /// <summary>
    ///     Changes the selected character's orientation
    /// </summary>
    public class FMikuRotate : DscFunc
    {
        /// <summary>
        ///     The character's rotation on the Z axis
        /// </summary>

        [FieldOrder(0)] public uint playerID;

        [FieldOrder(1)] public int orientation;

        public FMikuRotate() => functionId = 0x03;
    }
        
    /// <summary>
    ///     Changes the selected character's display state
    /// </summary>
    public class FMikuDisplay : DscFunc
    {
        [FieldOrder(0)] public uint playerID;

        [FieldOrder(1)] [SerializeAs(SerializedType.UInt4)] public bool state;

        public FMikuDisplay()
        {
            functionId = 0x04;
        }
    }

    /// <summary>
    ///     Changes the selected character's shadow display state
    /// </summary>
    public class FMikuShadow : DscFunc
    {
        [FieldOrder(0)] public uint playerID;

        [FieldOrder(1)] [SerializeAs(SerializedType.UInt4)] public bool state;

        public FMikuShadow()
        {
            functionId = 0x05;
        }
    }

    /// <summary>
    ///     Creates a new target with the specified paramaters
    /// </summary>
    public class FTarget : DscFunc
    {
        public enum EType
        {
            TRIANGLE = 0,
            CIRCLE = 1,
            CROSS = 2,
            SQUARE = 3,
            TRIANGLE_DOUBLE = 4,
            CIRCLE_DOUBLE = 5,
            CROSS_DOUBLE = 6,
            SQUARE_DOUBLE = 7,
            TRIANGLE_HOLD = 8,
            CIRCLE_HOLD = 9,
            CROSS_HOLD = 10,
            SQUARE_HOLD = 11,
            STAR = 12,
            STAR_HOLD = 14,
            CHANCE_STAR = 15
        }

        [FieldOrder(0)] public EType Type;

        [FieldOrder(1)] [FieldScale(100)] [SerializeAs(SerializedType.Int4)] public double HoldLength;

        [FieldOrder(2)] [SerializeAs(SerializedType.Int4)] public int IsHoldEnd;

        [FieldOrder(3)] public Vector2 Position;

        [FieldOrder(4)] [FieldScale(100_000)] [SerializeAs(SerializedType.Int4)] public double EntryAngle;

        [FieldOrder(5)] public int OscillateFrequency;

        [FieldOrder(6)] [FieldScale(100_000)] [SerializeAs(SerializedType.Int4)] public double OscillateAngle; 

        [FieldOrder(7)] public uint OscillateAmplitude;

        [FieldOrder(8)] [SerializeAs(SerializedType.Int4)] public double TimeOut;

        [FieldOrder(9)] public int Pad;
        
        public FTarget()
        {
            functionId = 0x06;
        }
    }

    /// <summary>
    ///     Creates a new target with the specified paramaters
    /// </summary>
    public class F2Target : DscFunc
    {
        public enum EType
        {
            TRIANGLE = 0,
            CIRCLE = 1,
            CROSS = 2,
            SQUARE = 3,
            TRIANGLE_DOUBLE = 4,
            CIRCLE_DOUBLE = 5,
            CROSS_DOUBLE = 6,
            SQUARE_DOUBLE = 7,
            TRIANGLE_HOLD = 8,
            CIRCLE_HOLD = 9,
            CROSS_HOLD = 10,
            SQUARE_HOLD = 11,
            STAR = 12,
            STAR_DOUBLE = 14,
            CHANCE_STAR = 15,
            LINKED_STAR = 22,
            LINKED_STAR_END = 23
        }

        [FieldOrder(0)] public EType type;
        [FieldOrder(1)] public int holdLength;
        [FieldOrder(2)] [SerializeAs(SerializedType.Int4)] public int isHoldEnd;
        [FieldOrder(3)] public int posX;
        [FieldOrder(4)] public int posY;
        [FieldOrder(5)] public int entryAngle;
        [FieldOrder(6)] public int oscillateFrequency;
        [FieldOrder(7)] public int oscillateAngle; 
        [FieldOrder(8)] public uint oscillateAmplitude;
        [FieldOrder(9)] public int timeOut;
        [FieldOrder(10)] public int unk;
        [FieldOrder(11)] public int pad;

        public F2Target()
        {
            functionId = 0x06;
        }
    }

    /// <summary> Plays an animation on the character </summary>
    public class FSetMotion : DscFunc
    {
        /// <summary> Which animation to play </summary>
        [FieldOrder(1)] public uint animID;

        /// <summary> Which player to affect </summary>
        [FieldOrder(0)] public int playerID;

        /// <summary> Animation speed </summary>
        [FieldOrder(3)] public int speed;

        /// <summary> Which frame the animation should start on </summary>
        [FieldOrder(2)] [FieldScale(100)] [SerializeAs(SerializedType.Int4)] public double time;

        public FSetMotion()
        {
            functionId = 0x07;
        }
    }

    /// <summary> Unknown </summary>
    public class FSetPlaydata : DscFunc
    {
        /// <summary> ??? </summary>
        [FieldOrder(1)] public uint mode;

        /// <summary> Which player to affect </summary>
        [FieldOrder(0)] public uint playerID;

        public FSetPlaydata()
        {
            functionId = 0x08;
        }
    }

    /// <summary> Displays an Effect? </summary>
    /// <remark> From old DIVA, Doesn't do anything in new games </remark>
    public class FEffect : DscFunc
    {
        /// <summary> ???? </summary>
        [FieldOrder(0)] public Vector3 unk1;

        /// <summary> ???? </summary>
        [FieldOrder(1)] public Vector3 unk2;

        public FEffect()
        {
            functionId = 0x09;
        }
    }

    /// <summary> ??? </summary>
    /// <remark> From old DIVA, Doesn't do anything in new games </remark>
    public class FFadeinField : DscFunc
    {
        public FFadeinField()
        {
            functionId = 0x0A;
        }
    }

    /// <summary> Hides an Effect? </summary>
    /// <remark> From old DIVA, Doesn't do anything in new games </remark>
    public class FEffectOff : DscFunc
    {
        /// <summary> ???? </summary>
        [FieldOrder(0)] public Vector3 unk1;

        /// <summary> ???? </summary>
        [FieldOrder(1)] public Vector3 unk2;

        public FEffectOff()
        {
            functionId = 0x0B;
        }
    }

    /// <summary>  ?? </summary>
    /// <remark> Something with the cameras. Haven't looked into it </remark>
    public class FSetCamera : DscFunc
    {
        public FSetCamera()
        {
            functionId = 0x0C;
        }
    }

    /// <summary> Works the same as FSetCamera? </summary>
    public class FDataCamera : DscFunc
    {
        public FDataCamera()
        {
            functionId = 0x0D;
        }
    }

    /// <summary> Changes the stage </summary>
    public class FChangeField : DscFunc
    {
        /// <summary> New stage to set. goes by pv_db / pv_field </summary>
        [FieldOrder(0)] public uint fieldID;

        public FChangeField()
        {
            functionId = 0x0E;
        }
    }

    /// <summary> Hides the field </summary>
    /// <remark> Haven't looked into it </remark>
    public class FHideField : DscFunc
    {
        /// <summary> Visibility toggle? </summary>
        [FieldOrder(0)] public uint state;

        public FHideField()
        {
            functionId = 0x0F;
        }
    }

    /// <summary> Moves the field </summary>
    /// <remark> Haven't looked into it </remark>
    public class FMoveField : DscFunc
    {
        public FMoveField()
        {
            functionId = 0x10;
        }
    }

    /// <summary> Acts the same as FFadeInField </summary>
    /// <remark> From old DIVA, Doesn't do anything in new games </remark>
    public class FFadeoutField : DscFunc
    {
        public FFadeoutField()
        {
            functionId = 0x11;
        }
    }

    /// <summary> Plays an animation on the character's eyes </summary>
    /// <remark> Only does blinking? </remark>
    public class FEyeAnim : DscFunc
    {
        /// <summary> Which animation to play? </summary>
        [FieldOrder(1)] public uint animID;

        /// <summary> Which player to affect </summary>
        [FieldOrder(0)] public int playerID;

        /// <summary> ???? </summary>
        [FieldOrder(2)] public uint unk;

        public FEyeAnim()
        {
            functionId = 0x12;
        }
    }

    /// <summary> Plays mouth animations </summary>
    /// <remark> Isn't completely researched </remark>
    public class FMouthAnim : DscFunc
    {
        /// <summary> Which animation to play? </summary>
        [FieldOrder(1)] public uint animID;

        /// <summary> Which player to affect </summary>
        [FieldOrder(0)] public int playerID;

        /// <summary> The animation speed, disabled if -1  </summary>
        [FieldOrder(4)] public int speed;

        /// <summary> When the animation should start </summary>
        [FieldOrder(3)] public int start;

        /// <summary> ??? </summary>
        [FieldOrder(2)] public int unk;

        public FMouthAnim()
        {
            functionId = 0x13;
        }
    }

    /// <summary> Hand motions, but haven't looked much into it </summary>
    public class FHandAnim : DscFunc
    {
        /// <summary> Which animation to play? </summary>
        [FieldOrder(1)] public uint animID;

        /// <summary> Which player to affect </summary>
        [FieldOrder(0)] public int playerID;

        /// <summary> The animation speed, disabled if - </summary>
        [FieldOrder(4)] public int speed;

        /// <summary> When the animation should start </summary>
        [FieldOrder(3)] public int start;

        /// <summary> ??? </summary>
        [FieldOrder(2)] public int unk;

        public FHandAnim()
        {
            functionId = 0x14;
        }
    }

    /// <summary> Makes the character move their eyes, but haven't looked much into it </summary>
    public class FLookAnim : DscFunc
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;

        public FLookAnim()
        {
            functionId = 0x15;
        }
    }

    /// <summary> Expessions for the face, but haven't looked much into it </summary>
    public class FExpression : DscFunc
    {
        /// <summary> Which expression to play </summary>
        [FieldOrder(1)] public uint expID;

        /// <summary> Which player to affect </summary>
        [FieldOrder(0)] public int playerID;

        /// <summary> The animation speed, disabled if - </summary>
        [FieldOrder(3)] public int speed;

        /// <summary> When the animation should start </summary>
        [FieldOrder(2)] public int start;

        public FExpression()
        {
            functionId = 0x16;
        }
    }

    /// <summary> No idea what this is for </summary>
    public class FLookCamera : DscFunc
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;

        public FLookCamera()
        {
            functionId = 0x17;
        }
    }

    /// <summary> Displays a lyric with the specified color </summary>
    public class FLyric : DscFunc
    {
        /// <summary> "Color per byte" </summary>
        [FieldOrder(1)] public uint color;

        /// <summary> "Which lyric to display, Goes by pv_db" </summary>
        [FieldOrder(0)] public uint lyricID;

        public FLyric()
        {
            functionId = 0x18;
        }
    }

    /// <summary> Plays the music </summary>
    public class FMusicPlay : DscFunc
    {
        public FMusicPlay()
        {
            functionId = 0x19;
        }
    }

    /// <summary> Sets the mode. e.g. Chance Time </summary>
    /// <remark> 4 in a row makes it playable in every difficulty </remark>
    public class FModeSelect : DscFunc
    {
        /// <summary> DESC </summary>
        [FieldOrder(1)] public int modeID;

        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint unk;

        public FModeSelect()
        {
            functionId = 0x1A;
        }
    }

    /// <summary>
    ///     This doesn't work anymore. It was used in PSP's edit mode and in FT. However, the speed values still work.
    ///     The motion ID is unfunctional
    /// </summary>
    public class FEditMotion : DscFunc
    {
        /// <summary> Which animation to play </summary>
        [FieldOrder(0)] public uint animID;

        /// <summary> Speed of the animation </summary>
        [FieldOrder(2)] public uint speed;

        /// <summary> When to start the animation </summary>
        [FieldOrder(1)] public uint start;

        public FEditMotion()
        {
            functionId = 0x1B;
        }
    }

    /// <summary>
    ///     First 4 bytes = bpm. This set the actual bpm number rather than time, like CD 00 00 00, which is 205 bpm.
    ///     This doesn't do anything in Sega's dscs because of the bpm time in the notes.
    /// </summary>
    public class FBarTimeSet : DscFunc
    {
        /// <summary> Actual BPM value </summary>
        [FieldOrder(0)] public int bpm;

        /// <summary> The speed of the notes </summary>
        [FieldOrder(1)] public int noteSpeed;

        public FBarTimeSet()
        {
            functionId = 0x1C;
        }
    }

    /// <summary> ???? </summary>
    public class FShadowheight : DscFunc
    {
        /// <summary> ???? </summary>
        [FieldOrder(0)] public uint unk1;

        /// <summary> ???? </summary>
        [FieldOrder(1)] public uint unk2;

        public FShadowheight()
        {
            functionId = 0x1D;
        }
    }

    /// <summary> Haven't looked into this </summary>
    public class FEditFace : DscFunc
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;

        public FEditFace()
        {
            functionId = 0x1E;
        }
    }

    /// <summary> Moves camera. Doesn't reposition around character </summary>
    /// <remark> Values Not Researched </remark>
    public class FMoveCamera : DscFunc
    {
        public FMoveCamera()
        {
            functionId = 0x1F;
        }
    }

    /// <summary> The point where the song ends </summary>
    public class FPvEnd : DscFunc
    {
        /// <summary> Doesn't really exist, made up to please Deserializing </summary>
        [FieldOrder(0)] public uint pad;

        public FPvEnd()
        {
            functionId = 0x20;
        }
    }

    /// <summary> ???? </summary>
    public class FShadowpos : DscFunc
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;

        public FShadowpos()
        {
            functionId = 0x21;
        }
    }

    /// <summary>   </summary>
    /// <remark> From old DIVA, Doesn't do anything in new games </remark>
    public class FEditLyric : DscFunc
    {
        public FEditLyric()
        {
            functionId = 0x22;
        }
    }

    /// <summary>   </summary>
    /// <remark>  </remark>
    public class FEditTarget : DscFunc
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;

        public FEditTarget()
        {
            functionId = 0x23;
        }
    }

    /// <summary> Edit variant of FMouthAnim </summary>
    public class FEditMouth : DscFunc
    {
        /// <summary> The animation speed, disabled if -1  </summary>
        [FieldOrder(2)] public int speed;

        /// <summary> When the animation should start </summary>
        [FieldOrder(1)] public int start;

        /// <summary> ??? </summary>
        [FieldOrder(0)] public int unk;

        public FEditMouth()
        {
            functionId = 0x24;
        }
    }

    /// <summary> Sets the character for edit functions </summary>
    public class FSetCharacter : DscFunc
    {
        /// <summary> Which player to affect </summary>
        [FieldOrder(0)] public int playerID;

        public FSetCharacter()
        {
            functionId = 0x25;
        }
    }

    /// <summary> Edit variant of FMikuMove </summary>
    public class FEditMove : DscFunc
    {
        /// <summary> New position </summary>
        [FieldOrder(0)] public Vector3 position;

        public FEditMove()
        {
            functionId = 0x26;
        }
    }

    /// <summary> Edit variant of FMikuShadow </summary>
    public class FEditShadow : DscFunc
    {
        /// <summary> Visiblity toggle </summary>
        [FieldOrder(0)] public uint state;

        public FEditShadow()
        {
            functionId = 0x27;
        }
    }

    /// <summary> Haven't looked into this </summary>
    public class FEditEyelid : DscFunc
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;

        public FEditEyelid()
        {
            functionId = 0x28;
        }
    }

    /// <summary> Same as above </summary>
    public class FEditEye : DscFunc
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;

        public FEditEye()
        {
            functionId = 0x29;
        }
    }

    /// <summary> First 4 bytes = I guess what hand it goes on for the player. For the player, it goes by SET_CHARA </summary>
    public class FEditItem : DscFunc
    {
        /// <summary> Which item to use, Goes by ???? </summary>
        [FieldOrder(0)] public int itemID;

        public FEditItem()
        {
            functionId = 0x2A;
        }
    }

    /// <summary> EDIT Function: Sets an effect </summary>
    public class FEditEffect : DscFunc
    {
        /// <summary> Which effect to display </summary>
        [FieldOrder(0)] public int fxID;

        /// <summary> How fast should the effect play </summary>
        [FieldOrder(1)] public int speed;

        public FEditEffect()
        {
            functionId = 0x2B;
        }
    }

    /// <summary> Basically MIKU_DISP with 4 less bytes </summary>
    public class FEditDisp : DscFunc
    {
        /// <summary> Visibility toggle </summary>
        [FieldOrder(0)] public uint state;

        public FEditDisp()
        {
            functionId = 0x2C;
        }
    }

    /// <summary> No idea </summary>
    public class FEditHandAnim : DscFunc
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;

        public FEditHandAnim()
        {
            functionId = 0x2D;
        }
    }

    /// <summary> No idea </summary>
    public class FAim : DscFunc
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;

        public FAim()
        {
            functionId = 0x2E;
        }
    }

    /// <summary> I've looked into it, but I forgot what they were </summary>
    public class FHandItem : DscFunc
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;

        public FHandItem()
        {
            functionId = 0x2F;
        }
    }

    /// <summary> First 4 bytes = Blush ID </summary>
    public class FEditBlush : DscFunc
    {
        /// <summary> Which blush to display </summary>
        [FieldOrder(0)] public int blushID;

        public FEditBlush()
        {
            functionId = 0x30;
        }
    }

    /// <summary> Sets the camera's near clip </summary>
    /// <remark> Speculation! </remark>
    public class FNearClip : DscFunc
    {
        /// <summary> The new near clip value </summary>
        [FieldOrder(0)] public uint value;

        public FNearClip()
        {
            functionId = 0x31;
        }
    }

    /// <summary> No idea </summary>
    public class FClothWet : DscFunc
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;

        public FClothWet()
        {
            functionId = 0x32;
        }
    }

    /// <summary> No idea </summary>
    public class FLightRot : DscFunc
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;

        public FLightRot()
        {
            functionId = 0x33;
        }
    }

    /// <summary> First 4 bytes = Fade speed </summary>
    public class FSceneFade : DscFunc
    {
        /// <summary> Fade's Blue Channel </summary>
        [FieldOrder(5)] public uint blue;

        /// <summary> ??? </summary>
        [FieldOrder(1)] public uint fade_in;

        /// <summary> ??? </summary>
        [FieldOrder(2)] public uint fade_out;

        /// <summary> Fade's Green Channel </summary>
        [FieldOrder(4)] public uint green;

        /// <summary> Fade's Red Channel </summary>
        [FieldOrder(3)] public uint red;

        /// <summary> How fast the fade should play </summary>
        [FieldOrder(0)] public uint speed;

        public FSceneFade()
        {
            functionId = 0x34;
        }
    }

    /// <summary> This does load, but always crashes when the function starts. It was used in FT </summary>
    public class FToneTrans : DscFunc
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;

        public FToneTrans()
        {
            functionId = 0x35;
        }
    }

    /// <summary> De/saturates the screen </summary>
    public class FSaturate : DscFunc
    {
        /// <summary> Saturation amount </summary>
        [FieldOrder(0)] public uint amount;

        public FSaturate()
        {
            functionId = 0x36;
        }
    }

    /// <summary> No idea </summary>
    public class FFadeMode : DscFunc
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;

        public FFadeMode()
        {
            functionId = 0x37;
        }
    }

    /// <summary> No idea </summary>
    public class FAutoBlink : DscFunc
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;

        public FAutoBlink()
        {
            functionId = 0x38;
        }
    }

    /// <summary> No idea </summary>
    public class FPartsDisp : DscFunc
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;

        public FPartsDisp()
        {
            functionId = 0x39;
        }
    }

    /// <summary> Sets the BPM for every incoming note </summary>
    /// <remark>  FT Exclusive </remark>
    public class FTargetFlyingTime : DscFunc
    {
        /// <summary> New BPM value </summary>
        [FieldOrder(0)] public uint bpm;

        public FTargetFlyingTime()
        {
            functionId = 0x3A;
        }
    }

    /// <summary> Sets the character's scaling </summary>
    public class FCharacterSize : DscFunc
    {
        /// <summary> Which player to affect </summary>
        [FieldOrder(0)] public int playerID;

        /// <summary> New size </summary>
        [FieldOrder(1)] public uint size;

        public FCharacterSize()
        {
            functionId = 0x3B;
        }
    }

    /// <summary> No idea </summary>
    public class FCharacterHeightAdjust : DscFunc
    {
        /// <summary> Which player to affect </summary>
        [FieldOrder(0)] public int playerID;

        /// <summary> New Height </summary>
        [FieldOrder(1)] public uint height;

        public FCharacterHeightAdjust()
        {
            functionId = 0x3C;
        }
    }

    /// <summary> No idea </summary>
    public class FItemAnim : DscFunc
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;

        /// <summary> DESC </summary>
        [FieldOrder(1)] public uint var1;

        /// <summary> DESC </summary>
        [FieldOrder(2)] public uint var2;

        /// <summary> DESC </summary>
        [FieldOrder(3)] public uint var3;

        public FItemAnim()
        {
            functionId = 0x3D;
        }
    }

    /// <summary> No idea </summary>
    public class FCharacterPosAdjust : DscFunc
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;

        public FCharacterPosAdjust()
        {
            functionId = 0x3E;
        }
    }

    /// <summary> No idea </summary>
    public class FSceneRot : DscFunc
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;

        public FSceneRot()
        {
            functionId = 0x3F;
        }
    }

    /// <summary>   </summary>
    /// <remark> Not Researched </remark>
    public class FEditMotSmoothLen : DscFunc
    {
        /// <summary> ??? </summary>
        [FieldOrder(0)] public uint unk1;

        /// <summary> ??? </summary>
        [FieldOrder(1)] public uint unk2;

        public FEditMotSmoothLen()
        {
            functionId = 0x40;
        }
    }

    /// <summary> Sets the mode for the rest of the dsc </summary>
    public class FPvBranchMode : DscFunc
    {
        /// <summary> Which mode to use </summary>
        [FieldOrder(0)] public uint mode;

        public FPvBranchMode()
        {
            functionId = 0x41;
        }
    }

    /// <summary> I guess starts the camera </summary>
    public class FDataCameraStart : DscFunc
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;

        public FDataCameraStart()
        {
            functionId = 0x42;
        }
    }

    /// <summary> Plays the movie, but doesn't display it </summary>
    public class FMoviePlay : DscFunc
    {
        /// <summary> Play movie? </summary>
        [FieldOrder(0)] [SerializeAs(SerializedType.UInt4)] public bool state;

        public FMoviePlay()
        {
            functionId = 0x43;
        }
    }

    /// <summary> Displays the movie, but doesn't play it </summary>
    public class FMovieDisplay : DscFunc
    {
        /// <summary> Display movie? </summary>
        [FieldOrder(0)] [SerializeAs(SerializedType.UInt4)] public bool state;

        public FMovieDisplay()
        {
            functionId = 0x44;
        }
    }

    /// <summary> No idea </summary>
    public class FWind : DscFunc
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;

        public FWind()
        {
            functionId = 0x45;
        }
    }

    /// <summary> No idea </summary>
    public class FOsageStep : DscFunc
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;

        public FOsageStep()
        {
            functionId = 0x46;
        }
    }

    /// <summary> Moves the hair colliders </summary>
    /// <remark> Speculation! </remark>
    public class FOsageMoveCollider : DscFunc
    {
        /// <summary> New position for the colliders </summary>
        [FieldOrder(0)] public Vector3 position;

        public FOsageMoveCollider()
        {
            functionId = 0x47;
        }
    }

    /// <summary> First 4 bytes = Player ID </summary>
    public class FCharacterColor : DscFunc
    {
        /// <summary> Alpha channel </summary>
        [FieldOrder(4)] public byte a;

        /// <summary> Blue channel </summary>
        [FieldOrder(3)] public byte b;

        /// <summary> Green channel </summary>
        [FieldOrder(2)] public byte g;

        /// <summary> Which player to affect </summary>
        [FieldOrder(0)] public int playerID;

        /// <summary> Red channel </summary>
        [FieldOrder(1)] public byte r;

        public FCharacterColor()
        {
            functionId = 0x48;
        }
    }

    /// <summary> Plays the bonus effect sounds, but I haven't looked into the values </summary>
    public class FSeEffect : DscFunc
    {
        /// <summary> Which sound to play </summary>
        [FieldOrder(0)] public uint sfxID;

        public FSeEffect()
        {
            functionId = 0x49;
        }
    }

    /// <summary> Moves all 3 characters </summary>
    /// <remark> Speculation! </remark>
    public class FEditMoveXYZ : DscFunc
    {
        /// <summary> Player 1's new position </summary>
        [FieldOrder(0)] public Vector3 player1Position;

        /// <summary> Player 2's new position </summary>
        [FieldOrder(1)] public Vector3 player2Position;

        /// <summary> Player 3's new position </summary>
        [FieldOrder(2)] public Vector3 player3Position;

        public FEditMoveXYZ()
        {
            functionId = 0x4A;
        }
    }

    /// <summary> No idea </summary>
    public class FEditEyelidAnim : DscFunc
    {
        /// <summary> ??? </summary>
        [FieldOrder(0)] public uint unk1;

        /// <summary> ??? </summary>
        [FieldOrder(1)] public uint unk2;

        /// <summary> ??? </summary>
        [FieldOrder(2)] public uint unk3;

        public FEditEyelidAnim()
        {
            functionId = 0x4B;
        }
    }

    /// <summary> Sets the instrument the character uses </summary>
    public class FEditInstrumentItem : DscFunc
    {
        /// <summary> ??? </summary>
        [FieldOrder(0)] public int unk;

        /// <summary> ??? </summary>
        [FieldOrder(1)] public uint unk1;

        public FEditInstrumentItem()
        {
            functionId = 0x4C;
        }
    }

    /// <summary> Loops the motion the player uses </summary>
    /// <remark>  Has to have a FSetMotion after it </remark>
    public class FEditMotionLoop : DscFunc
    {
        /// <summary> Which animation to play </summary>
        [FieldOrder(1)] public uint animID;

        /// <summary> Which player to affect </summary>
        [FieldOrder(0)] public int playerID;

        /// <summary> Animation speed </summary>
        [FieldOrder(3)] public int speed;

        /// <summary> Which frame the animation should start on </summary>
        [FieldOrder(2)] [FieldScale(100)] [SerializeAs(SerializedType.Int4)] public double time;

        public FEditMotionLoop()
        {
            functionId = 0x4D;
        }
    }

    /// <summary> Edit variant of FExpression </summary>
    public class FEditExpression : DscFunc
    {
        /// <summary> Which expression to play </summary>
        [FieldOrder(0)] public uint expID;

        /// <summary> The animation speed, disabled if - </summary>
        [FieldOrder(2)] public int speed;

        /// <summary> When the animation should start </summary>
        [FieldOrder(1)] public int start;

        public FEditExpression()
        {
            functionId = 0x4E;
        }
    }

    /// <summary> Basically EYE_ANIM with less values </summary>
    public class FEditEyeAnim : DscFunc
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;

        public FEditEyeAnim()
        {
            functionId = 0x4F;
        }
    }

    /// <summary> Edit variant of FMouthAnim </summary>
    public class FEditMouthAnim : DscFunc
    {
        /// <summary> Which animation to play? </summary>
        [FieldOrder(0)] public uint animID;

        /// <summary> The animation speed, disabled if -1  </summary>
        [FieldOrder(3)] public int speed;

        /// <summary> When the animation should start </summary>
        [FieldOrder(2)] public int start;

        /// <summary> ??? </summary>
        [FieldOrder(1)] public int unk;

        public FEditMouthAnim()
        {
            functionId = 0x50;
        }
    }

    /// <summary> Acts like FMoveCamera but repositions around the player </summary>
    public class FEditCamera : DscFunc
    {
        /// <summary> ??? </summary>
        [FieldOrder(0)] public Vector3 position;

        /// <summary> ??? </summary>
        [FieldOrder(1)] public Vector3 unk1;

        /// <summary> ??? </summary>
        [FieldOrder(2)] public Vector3 unk2;

        /// <summary> ??? </summary>
        [FieldOrder(3)] public Vector3 unk3;

        /// <summary> ??? </summary>
        [FieldOrder(4)] public Vector3 unk4;

        /// <summary> ??? </summary>
        [FieldOrder(5)] public Vector3 unk5;

        /// <summary> ??? </summary>
        [FieldOrder(6)] public Vector3 unk6;

        /// <summary> ??? </summary>
        [FieldOrder(7)] public Vector3 unk7;

        public FEditCamera()
        {
            functionId = 0x51;
        }
    }

    /// <summary> Edit variant of FModeSelect </summary>
    /// <remark> Does nothing </remark>
    public class FEditModeSelect : DscFunc
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;

        public FEditModeSelect()
        {
            functionId = 0x52;
        }
    }
}
