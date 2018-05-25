using System;
//using System.Numerics;
using System.Xml.Serialization;
using BinarySerialization;
using DIVALib.Math;
using Vector2 = System.Numerics.Vector2;

namespace DIVALib.DSCUtils
{
    public class DscFunctionWrapper
    {
        [FieldOrder(0)] public int FunctionId;
        [FieldOrder(1)]
        [Subtype("FunctionId", 0x00, typeof(FEnd))]
        [Subtype("FunctionId", 0x01, typeof(FTime))]
        [Subtype("FunctionId", 0x02, typeof(FMikuMove))]
        [Subtype("FunctionId", 0x03, typeof(FMikuRotate))]
        [Subtype("FunctionId", 0x04, typeof(FMikuDisplay))]
        [Subtype("FunctionId", 0x05, typeof(FMikuShadow))]
        [Subtype("FunctionId", 0x06, typeof(FTarget))]
        //[Subtype("FunctionId", 0x06, typeof(F2Target))]
        [Subtype("FunctionId", 0x07, typeof(FSetMotion))]
        [Subtype("FunctionId", 0x08, typeof(FSetPlaydata))]
        [Subtype("FunctionId", 0x09, typeof(FEffect))]
        [Subtype("FunctionId", 0x0A, typeof(FFadeinField))]
        [Subtype("FunctionId", 0x0B, typeof(FEffectOff))]
        [Subtype("FunctionId", 0x0C, typeof(FSetCamera))]
        [Subtype("FunctionId", 0x0D, typeof(FDataCamera))]
        [Subtype("FunctionId", 0x0E, typeof(FChangeField))]
        [Subtype("FunctionId", 0x0F, typeof(FHideField))]
        [Subtype("FunctionId", 0x10, typeof(FMoveField))]
        [Subtype("FunctionId", 0x11, typeof(FFadeoutField))]
        [Subtype("FunctionId", 0x12, typeof(FEyeAnim))]
        [Subtype("FunctionId", 0x13, typeof(FMouthAnim))]
        [Subtype("FunctionId", 0x14, typeof(FHandAnim))]
        [Subtype("FunctionId", 0x15, typeof(FLookAnim))]
        [Subtype("FunctionId", 0x16, typeof(FExpression))]
        [Subtype("FunctionId", 0x17, typeof(FLookCamera))]
        [Subtype("FunctionId", 0x18, typeof(FLyric))]
        [Subtype("FunctionId", 0x19, typeof(FMusicPlay))]
        [Subtype("FunctionId", 0x1A, typeof(FModeSelect))]
        [Subtype("FunctionId", 0x1B, typeof(FEditMotion))]
        [Subtype("FunctionId", 0x1C, typeof(FBarTimeSet))]
        [Subtype("FunctionId", 0x1D, typeof(FShadowheight))]
        [Subtype("FunctionId", 0x1E, typeof(FEditFace))]
        [Subtype("FunctionId", 0x1F, typeof(FMoveCamera))]
        [Subtype("FunctionId", 0x20, typeof(FPvEnd))]
        [Subtype("FunctionId", 0x21, typeof(FShadowpos))]
        [Subtype("FunctionId", 0x22, typeof(FEditLyric))]
        [Subtype("FunctionId", 0x23, typeof(FEditTarget))]
        [Subtype("FunctionId", 0x24, typeof(FEditMouth))]
        [Subtype("FunctionId", 0x25, typeof(FSetCharacter))]
        [Subtype("FunctionId", 0x26, typeof(FEditMove))]
        [Subtype("FunctionId", 0x27, typeof(FEditShadow))]
        [Subtype("FunctionId", 0x28, typeof(FEditEyelid))]
        [Subtype("FunctionId", 0x29, typeof(FEditEye))]
        [Subtype("FunctionId", 0x2A, typeof(FEditItem))]
        [Subtype("FunctionId", 0x2B, typeof(FEditEffect))]
        [Subtype("FunctionId", 0x2C, typeof(FEditDisp))]
        [Subtype("FunctionId", 0x2D, typeof(FEditHandAnim))]
        [Subtype("FunctionId", 0x2E, typeof(FAim))]
        [Subtype("FunctionId", 0x2F, typeof(FHandItem))]
        [Subtype("FunctionId", 0x30, typeof(FEditBlush))]
        [Subtype("FunctionId", 0x31, typeof(FNearClip))]
        [Subtype("FunctionId", 0x32, typeof(FClothWet))]
        [Subtype("FunctionId", 0x33, typeof(FLightRot))]
        [Subtype("FunctionId", 0x34, typeof(FSceneFade))]
        [Subtype("FunctionId", 0x35, typeof(FToneTrans))]
        [Subtype("FunctionId", 0x36, typeof(FSaturate))]
        [Subtype("FunctionId", 0x37, typeof(FFadeMode))]
        [Subtype("FunctionId", 0x38, typeof(FAutoBlink))]
        [Subtype("FunctionId", 0x39, typeof(FPartsDisp))]
        [Subtype("FunctionId", 0x3A, typeof(FTargetFlyingTime))]
        [Subtype("FunctionId", 0x3B, typeof(FCharacterSize))]
        [Subtype("FunctionId", 0x3C, typeof(FCharacterHeightAdjust))]
        [Subtype("FunctionId", 0x3D, typeof(FItemAnim))]
        [Subtype("FunctionId", 0x3E, typeof(FCharacterPosAdjust))]
        [Subtype("FunctionId", 0x3F, typeof(FSceneRot))]
        [Subtype("FunctionId", 0x40, typeof(FEditMotSmoothLen))]
        [Subtype("FunctionId", 0x41, typeof(FPvBranchMode))]
        [Subtype("FunctionId", 0x42, typeof(FDataCameraStart))]
        [Subtype("FunctionId", 0x43, typeof(FMoviePlay))]
        [Subtype("FunctionId", 0x44, typeof(FMovieDisplay))]
        [Subtype("FunctionId", 0x45, typeof(FWind))]
        [Subtype("FunctionId", 0x46, typeof(FOsageStep))]
        [Subtype("FunctionId", 0x47, typeof(FOsageMoveCollider))]
        [Subtype("FunctionId", 0x48, typeof(FCharacterColor))]
        [Subtype("FunctionId", 0x49, typeof(FSeEffect))]
        [Subtype("FunctionId", 0x4A, typeof(FEditMoveXYZ))]
        [Subtype("FunctionId", 0x4B, typeof(FEditEyelidAnim))]
        [Subtype("FunctionId", 0x4C, typeof(FEditInstrumentItem))]
        [Subtype("FunctionId", 0x4D, typeof(FEditMotionLoop))]
        [Subtype("FunctionId", 0x4E, typeof(FEditExpression))]
        [Subtype("FunctionId", 0x4F, typeof(FEditEyeAnim))]
        [Subtype("FunctionId", 0x50, typeof(FEditMouthAnim))]
        [Subtype("FunctionId", 0x51, typeof(FEditCamera))]
        [Subtype("FunctionId", 0x52, typeof(FEditModeSelect))]
        public DscFunctionBase Function;

        /// <summary>
        ///     The string representation of a DSC function
        /// </summary>
        public override string ToString() => $"0x{FunctionId:X2} {Function}";
    }

    /// <summary>
    ///     Generic parent class for all Dsc functions
    /// </summary>
    [Serializable]
    public class DscFunctionBase
    {
        public static implicit operator DscFunctionWrapper(DscFunctionBase func) => new DscFunctionWrapper { Function = func };
        public override string ToString() => $"{GetType().Name}:";
    }

    /// <summary>
    ///     Ends the song
    /// </summary>
    public class FEnd : DscFunctionBase
    {
        public uint unk;
    }

    /// <summary>
    ///     Makes the next function activate after set time
    /// </summary>
    public class FTime : DscFunctionBase
    {
        /// <sumary>Time in milliseconds</sumary>
        [FieldOrder(0)] [FieldScale(100)] [SerializeAs(SerializedType.UInt4)] public double TimeStamp;

        public override string ToString() => base.ToString() + $" {TimeStamp} ms";
    }

    /// <summary>
    ///     Moves the selected character to a specific position
    /// </summary>
    public class FMikuMove : DscFunctionBase
    {
        [FieldOrder(0)] public uint playerID;

        [FieldOrder(1)] public Vector3 position;
    }

    /// <summary>
    ///     Changes the selected character's orientation
    /// </summary>
    public class FMikuRotate : DscFunctionBase
    {
        /// <summary>
        ///     The character's rotation on the Z axis
        /// </summary>

        [FieldOrder(0)] public uint playerID;

        [FieldOrder(1)] public int orientation;
    }
        
    /// <summary>
    ///     Changes the selected character's display state
    /// </summary>
    public class FMikuDisplay : DscFunctionBase
    {
        [FieldOrder(0)] public uint playerID;

        [FieldOrder(1)] [SerializeAs(SerializedType.UInt4)] public bool state;
    }

    /// <summary>
    ///     Changes the selected character's shadow display state
    /// </summary>
    public class FMikuShadow : DscFunctionBase
    {
        [FieldOrder(0)] public uint playerID;

        [FieldOrder(1)] [SerializeAs(SerializedType.UInt4)] public bool state;
    }

    /// <summary>
    ///     Creates a new target with the specified paramaters
    /// </summary>
    public class FTarget : DscFunctionBase
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

        /// <summary> The maximum position you can spawn a target at </summary>
        public static readonly Vector2 MaxPosition = new Vector2(50, 15);

        /// <summary> The type of the note from <seealso cref="EType"/> </summary>
        [FieldOrder(0)] public EType Type;
        /// <summary>How long should the note be held in milliseconds </summary>
        /// <remarks>Used for the note tail calculations. Set to -1 when not a hold note</remarks>
        [FieldOrder(1)] [FieldScale(100)] [SerializeAs(SerializedType.Int4)] public double HoldLength = -1;
        [FieldOrder(2)] public int IsHoldEnd = -1;
        /// <summary>The position at which the target spawns at </summary>
        /// <remarks>The screen limits are <seealso cref="MaxPosition"/></remarks>
        [FieldOrder(3), FieldScale(10_000), SerializeAs(SerializedType.Int4)] public double PositionX = MaxPosition.X / 2;
        [FieldOrder(4), FieldScale(10_000), SerializeAs(SerializedType.Int4)] public double PositionY = MaxPosition.Y / 2;
        /// <summary> The angle that the note enters at and reaches the target </summary>
        [FieldOrder(5)] [FieldScale(100_000)] [SerializeAs(SerializedType.Int4)] public double EntryAngle;
        /// <summary> How quick does the note oscillate </summary>
        [FieldOrder(6)] public int OscillationFrequency = 2;
        /// <summary> The angle that the note oscillates at  </summary>
        [FieldOrder(7)] [FieldScale(100_000)] [SerializeAs(SerializedType.Int4)] public double OscillationAngle = 3; 
        /// <summary> The note's oscillation amplitude in ??? </summary>
        [FieldOrder(8)] public uint OscillationAmplitude = 500;
        /// <summary> The time taken by the note to reach the target </summary> 
        [FieldOrder(9)] public uint TimeOut = 1500;
        /// <summary> Used as a substitute to <seealso cref="TimeOut"/> when it doesn't have a value. </summary>
        /// <remarks> Uses the last <seealso cref="FBarTimeSet"/>. <c>(60  / BPM * (1 + TimeSignature) * 1000)</c></remarks>
        [FieldOrder(10)] public int TimeSignature = 3;
        [FieldOrder(11)] public int Pad = -1;

        public override string ToString() => base.ToString() + $" {Type} at ({PositionX:F2}, {PositionY:F2}) ";
    }

    /// <summary>
    ///     Creates a new target with the specified paramaters
    /// </summary>
    public class F2Target : DscFunctionBase
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
    }

    /// <summary> Plays an animation on the character </summary>
    public class FSetMotion : DscFunctionBase
    {
        /// <summary> Which animation to play </summary>
        [FieldOrder(1)] public uint animID;

        /// <summary> Which player to affect </summary>
        [FieldOrder(0)] public int playerID;

        /// <summary> Animation speed </summary>
        [FieldOrder(3)] public int speed;

        /// <summary> Which frame the animation should start on </summary>
        [FieldOrder(2)] [FieldScale(100)] [SerializeAs(SerializedType.Int4)] public double time;
    }

    /// <summary> Unknown </summary>
    public class FSetPlaydata : DscFunctionBase
    {
        /// <summary> ??? </summary>
        [FieldOrder(1)] public uint mode;

        /// <summary> Which player to affect </summary>
        [FieldOrder(0)] public uint playerID;
    }

    /// <summary> Displays an Effect? </summary>
    /// <remark> From old DIVA, Doesn't do anything in new games </remark>
    public class FEffect : DscFunctionBase
    {
        /// <summary> ???? </summary>
        [FieldOrder(0)] public int unk;
        /// <summary> ???? </summary>
        [FieldOrder(1)] public int unk1;
        /// <summary> ???? </summary>
        [FieldOrder(2)] public int unk2;
        /// <summary> ???? </summary>
        [FieldOrder(3)] public int unk3;
        /// <summary> ???? </summary>
        [FieldOrder(4)] public int unk4;
        /// <summary> ???? </summary>
        [FieldOrder(5)] public int unk5;
    }

    /// <summary> ??? </summary>
    /// <remark> From old DIVA, Doesn't do anything in new games </remark>
    public class FFadeinField : DscFunctionBase
    {
    }

    /// <summary> Hides an Effect? </summary>
    /// <remark> From old DIVA, Doesn't do anything in new games </remark>
    public class FEffectOff : DscFunctionBase
    {
        /// <summary> ???? </summary>
        [FieldOrder(0)] public Vector3 unk1;

        /// <summary> ???? </summary>
        [FieldOrder(1)] public Vector3 unk2;
    }

    /// <summary>  ?? </summary>
    /// <remark> Something with the cameras. Haven't looked into it </remark>
    public class FSetCamera : DscFunctionBase
    {
    }

    /// <summary> Works the same as FSetCamera? </summary>
    public class FDataCamera : DscFunctionBase
    {
        [FieldOrder(0)] public uint unk;
        [FieldOrder(1)] public uint unk1;
    }

    /// <summary> Changes the stage </summary>
    public class FChangeField : DscFunctionBase
    {
        /// <summary> New stage to set. goes by pv_db / pv_field </summary>
        [FieldOrder(0)] public uint fieldID;
    }

    /// <summary> Hides the field </summary>
    /// <remark> Haven't looked into it </remark>
    public class FHideField : DscFunctionBase
    {
        /// <summary> Visibility toggle? </summary>
        [FieldOrder(0)] public uint state;
    }

    /// <summary> Moves the field </summary>
    /// <remark> Haven't looked into it </remark>
    public class FMoveField : DscFunctionBase
    {
    }

    /// <summary> Acts the same as FFadeInField </summary>
    /// <remark> From old DIVA, Doesn't do anything in new games </remark>
    public class FFadeoutField : DscFunctionBase
    {
    }

    /// <summary> Plays an animation on the character's eyes </summary>
    /// <remark> Only does blinking? </remark>
    public class FEyeAnim : DscFunctionBase
    {
        /// <summary> Which animation to play? </summary>
        [FieldOrder(1)] public uint animID;

        /// <summary> Which player to affect </summary>
        [FieldOrder(0)] public int playerID;

        /// <summary> ???? </summary>
        [FieldOrder(2)] public uint unk;
    }

    /// <summary> Plays mouth animations </summary>
    /// <remark> Isn't completely researched </remark>
    public class FMouthAnim : DscFunctionBase
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
    }

    /// <summary> Hand motions, but haven't looked much into it </summary>
    public class FHandAnim : DscFunctionBase
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
    }

    /// <summary> Makes the character move their eyes, but haven't looked much into it </summary>
    public class FLookAnim : DscFunctionBase
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint unk;
        /// <summary> DESC </summary>
        [FieldOrder(1)] public uint unk1;
        /// <summary> DESC </summary>
        [FieldOrder(2)] public uint unk2;
        /// <summary> DESC </summary>
        [FieldOrder(3)] public uint unk3;
    }

    /// <summary> Expessions for the face, but haven't looked much into it </summary>
    public class FExpression : DscFunctionBase
    {
        /// <summary> Which expression to play </summary>
        [FieldOrder(1)] public uint expID;

        /// <summary> Which player to affect </summary>
        [FieldOrder(0)] public int playerID;

        /// <summary> The animation speed, disabled if - </summary>
        [FieldOrder(3)] public int speed;

        /// <summary> When the animation should start </summary>
        [FieldOrder(2)] public int start;
    }

    /// <summary> No idea what this is for </summary>
    public class FLookCamera : DscFunctionBase
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;
    }

    /// <summary> Displays a lyric with the specified color </summary>
    public class FLyric : DscFunctionBase
    {
        /// <summary> "Color per byte" </summary>
        [FieldOrder(1)] public uint color;

        /// <summary> "Which lyric to display, Goes by pv_db" </summary>
        [FieldOrder(0)] public uint lyricID;
    }

    /// <summary> Plays the music </summary>
    public class FMusicPlay : DscFunctionBase
    {
    }

    /// <summary> Sets the mode. e.g. Chance Time </summary>
    /// <remark> 4 in a row makes it playable in every difficulty </remark>
    public class FModeSelect : DscFunctionBase
    {
        /// <summary> DESC </summary>
        [FieldOrder(1)] public int modeID;

        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint unk;
    }

    /// <summary>
    ///     This doesn't work anymore. It was used in PSP's edit mode and in FT. However, the speed values still work.
    ///     The motion ID is unfunctional
    /// </summary>
    public class FEditMotion : DscFunctionBase
    {
        /// <summary> Which animation to play </summary>
        [FieldOrder(0)] public uint animID;

        /// <summary> Speed of the animation </summary>
        [FieldOrder(2)] public uint speed;

        /// <summary> When to start the animation </summary>
        [FieldOrder(1)] public uint start;
    }

    /// <summary>
    ///     First 4 bytes = bpm. This set the actual bpm number rather than time, like CD 00 00 00, which is 205 bpm.
    ///     This doesn't do anything in Sega's dscs because of the bpm time in the notes.
    /// </summary>
    public class FBarTimeSet : DscFunctionBase
    {
        /// <summary> Actual BPM value </summary>
        [FieldOrder(0)] public int Bpm;

        /// <summary> The speed of the notes </summary>
        [FieldOrder(1)] public int NoteSpeed;
    }

    /// <summary> ???? </summary>
    public class FShadowheight : DscFunctionBase
    {
        /// <summary> ???? </summary>
        [FieldOrder(0)] public uint unk1;

        /// <summary> ???? </summary>
        [FieldOrder(1)] public uint unk2;
    }

    /// <summary> Haven't looked into this </summary>
    public class FEditFace : DscFunctionBase
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;
    }

    /// <summary> Moves camera. Doesn't reposition around character </summary>
    /// <remark> Values Not Researched </remark>
    public class FMoveCamera : DscFunctionBase
    {
    }

    /// <summary> The point where the song ends </summary>
    public class FPvEnd : DscFunctionBase
    {
        /// <summary> Doesn't really exist, made up to please Deserializing </summary>
        [FieldOrder(0)] public uint pad;
    }

    /// <summary> ???? </summary>
    public class FShadowpos : DscFunctionBase
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;
    }

    /// <summary>   </summary>
    /// <remark> From old DIVA, Doesn't do anything in new games </remark>
    public class FEditLyric : DscFunctionBase
    {
    }

    /// <summary>   </summary>
    /// <remark>  </remark>
    public class FEditTarget : DscFunctionBase
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;
    }

    /// <summary> Edit variant of FMouthAnim </summary>
    public class FEditMouth : DscFunctionBase
    {
        /// <summary> The animation speed, disabled if -1  </summary>
        [FieldOrder(2)] public int speed;

        /// <summary> When the animation should start </summary>
        [FieldOrder(1)] public int start;

        /// <summary> ??? </summary>
        [FieldOrder(0)] public int unk;
    }

    /// <summary> Sets the character for edit functions </summary>
    public class FSetCharacter : DscFunctionBase
    {
        /// <summary> Which player to affect </summary>
        [FieldOrder(0)] public int playerID;
    }

    /// <summary> Edit variant of FMikuMove </summary>
    public class FEditMove : DscFunctionBase
    {
        /// <summary> New position </summary>
        [FieldOrder(0)] public Vector3 position;
    }

    /// <summary> Edit variant of FMikuShadow </summary>
    public class FEditShadow : DscFunctionBase
    {
        /// <summary> Visiblity toggle </summary>
        [FieldOrder(0)] public uint state;
    }

    /// <summary> Haven't looked into this </summary>
    public class FEditEyelid : DscFunctionBase
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;
    }

    /// <summary> Same as above </summary>
    public class FEditEye : DscFunctionBase
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;
    }

    /// <summary> First 4 bytes = I guess what hand it goes on for the player. For the player, it goes by SET_CHARA </summary>
    public class FEditItem : DscFunctionBase
    {
        /// <summary> Which item to use, Goes by ???? </summary>
        [FieldOrder(0)] public int itemID;
    }

    /// <summary> EDIT Function: Sets an effect </summary>
    public class FEditEffect : DscFunctionBase
    {
        /// <summary> Which effect to display </summary>
        [FieldOrder(0)] public int fxID;

        /// <summary> How fast should the effect play </summary>
        [FieldOrder(1)] public int speed;
    }

    /// <summary> Basically MIKU_DISP with 4 less bytes </summary>
    public class FEditDisp : DscFunctionBase
    {
        /// <summary> Visibility toggle </summary>
        [FieldOrder(0)] public uint state;
    }

    /// <summary> No idea </summary>
    public class FEditHandAnim : DscFunctionBase
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;
    }

    /// <summary> No idea </summary>
    public class FAim : DscFunctionBase
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;
    }

    /// <summary> I've looked into it, but I forgot what they were </summary>
    public class FHandItem : DscFunctionBase
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;
    }

    /// <summary> First 4 bytes = Blush ID </summary>
    public class FEditBlush : DscFunctionBase
    {
        /// <summary> Which blush to display </summary>
        [FieldOrder(0)] public int blushID;
    }

    /// <summary> Sets the camera's near clip </summary>
    /// <remark> Speculation! </remark>
    public class FNearClip : DscFunctionBase
    {
        /// <summary> The new near clip value </summary>
        [FieldOrder(0)] public uint value;
    }

    /// <summary> No idea </summary>
    public class FClothWet : DscFunctionBase
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;
    }

    /// <summary> No idea </summary>
    public class FLightRot : DscFunctionBase
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;
    }

    /// <summary> First 4 bytes = Fade speed </summary>
    public class FSceneFade : DscFunctionBase
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
    }

    /// <summary> This does load, but always crashes when the function starts. It was used in FT </summary>
    public class FToneTrans : DscFunctionBase
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;
    }

    /// <summary> De/saturates the screen </summary>
    public class FSaturate : DscFunctionBase
    {
        /// <summary> Saturation amount </summary>
        [FieldOrder(0)] public uint amount;
    }

    /// <summary> No idea </summary>
    public class FFadeMode : DscFunctionBase
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;
    }

    /// <summary> No idea </summary>
    public class FAutoBlink : DscFunctionBase
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;
    }

    /// <summary> No idea </summary>
    public class FPartsDisp : DscFunctionBase
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;
    }

    /// <summary> Sets the BPM for every incoming note </summary>
    /// <remark>  FT Exclusive </remark>
    public class FTargetFlyingTime : DscFunctionBase
    {
        /// <summary> New BPM value </summary>
        [FieldOrder(0)] public uint bpm;
    }

    /// <summary> Sets the character's scaling </summary>
    public class FCharacterSize : DscFunctionBase
    {
        /// <summary> Which player to affect </summary>
        [FieldOrder(0)] public int playerID;

        /// <summary> New size </summary>
        [FieldOrder(1)] public uint size;
    }

    /// <summary> No idea </summary>
    public class FCharacterHeightAdjust : DscFunctionBase
    {
        /// <summary> Which player to affect </summary>
        [FieldOrder(0)] public int playerID;

        /// <summary> New Height </summary>
        [FieldOrder(1)] public uint height;
    }

    /// <summary> No idea </summary>
    public class FItemAnim : DscFunctionBase
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;

        /// <summary> DESC </summary>
        [FieldOrder(1)] public uint var1;

        /// <summary> DESC </summary>
        [FieldOrder(2)] public uint var2;

        /// <summary> DESC </summary>
        [FieldOrder(3)] public uint var3;
    }

    /// <summary> No idea </summary>
    public class FCharacterPosAdjust : DscFunctionBase
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;
    }

    /// <summary> No idea </summary>
    public class FSceneRot : DscFunctionBase
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;
    }

    /// <summary>   </summary>
    /// <remark> Not Researched </remark>
    public class FEditMotSmoothLen : DscFunctionBase
    {
        /// <summary> ??? </summary>
        [FieldOrder(0)] public uint unk1;

        /// <summary> ??? </summary>
        [FieldOrder(1)] public uint unk2;
    }

    /// <summary> Sets the mode for the rest of the dsc </summary>
    public class FPvBranchMode : DscFunctionBase
    {
        /// <summary> Which mode to use </summary>
        [FieldOrder(0)] public uint mode;
    }

    /// <summary> I guess starts the camera </summary>
    public class FDataCameraStart : DscFunctionBase
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint unk;
        [FieldOrder(1)] public uint unk1;
    }

    /// <summary> Plays the movie, but doesn't display it </summary>
    public class FMoviePlay : DscFunctionBase
    {
        /// <summary> Play movie? </summary>
        [FieldOrder(0)] [SerializeAs(SerializedType.UInt4)] public bool state;
    }

    /// <summary> Displays the movie, but doesn't play it </summary>
    public class FMovieDisplay : DscFunctionBase
    {
        /// <summary> Display movie? </summary>
        [FieldOrder(0)] [SerializeAs(SerializedType.UInt4)] public bool state;
    }

    /// <summary> No idea </summary>
    public class FWind : DscFunctionBase
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint unk;
        [FieldOrder(1)] public uint unk1;
        [FieldOrder(2)] public uint unk2;
    }

    /// <summary> No idea </summary>
    public class FOsageStep : DscFunctionBase
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint unk;
        [FieldOrder(1)] public uint unk1;
        [FieldOrder(2)] public uint unk2;
    }

    /// <summary> Moves the hair colliders </summary>
    /// <remark> Speculation! </remark>
    public class FOsageMoveCollider : DscFunctionBase
    {
        /// <summary> New position for the colliders </summary>
        [FieldOrder(0)] public Vector3 position;
    }

    /// <summary> First 4 bytes = Player ID </summary>
    public class FCharacterColor : DscFunctionBase
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
    }

    /// <summary> Plays the bonus effect sounds, but I haven't looked into the values </summary>
    public class FSeEffect : DscFunctionBase
    {
        /// <summary> Which sound to play </summary>
        [FieldOrder(0)] public uint sfxID;
    }

    /// <summary> Moves all 3 characters </summary>
    /// <remark> Speculation! </remark>
    public class FEditMoveXYZ : DscFunctionBase
    {
        /// <summary> Player 1's new position </summary>
        [FieldOrder(0)] public Vector3 player1Position;

        /// <summary> Player 2's new position </summary>
        [FieldOrder(1)] public Vector3 player2Position;

        /// <summary> Player 3's new position </summary>
        [FieldOrder(2)] public Vector3 player3Position;
    }

    /// <summary> No idea </summary>
    public class FEditEyelidAnim : DscFunctionBase
    {
        /// <summary> ??? </summary>
        [FieldOrder(0)] public uint unk1;

        /// <summary> ??? </summary>
        [FieldOrder(1)] public uint unk2;

        /// <summary> ??? </summary>
        [FieldOrder(2)] public uint unk3;
    }

    /// <summary> Sets the instrument the character uses </summary>
    public class FEditInstrumentItem : DscFunctionBase
    {
        /// <summary> ??? </summary>
        [FieldOrder(0)] public int unk;

        /// <summary> ??? </summary>
        [FieldOrder(1)] public uint unk1;
    }

    /// <summary> Loops the motion the player uses </summary>
    /// <remark>  Has to have a FSetMotion after it </remark>
    public class FEditMotionLoop : DscFunctionBase
    {
        /// <summary> Which animation to play </summary>
        [FieldOrder(1)] public uint animID;

        /// <summary> Which player to affect </summary>
        [FieldOrder(0)] public int playerID;

        /// <summary> Animation speed </summary>
        [FieldOrder(3)] public int speed;

        /// <summary> Which frame the animation should start on </summary>
        [FieldOrder(2)] [FieldScale(100)] [SerializeAs(SerializedType.Int4)] public double time;
    }

    /// <summary> Edit variant of FExpression </summary>
    public class FEditExpression : DscFunctionBase
    {
        /// <summary> Which expression to play </summary>
        [FieldOrder(0)] public uint expID;

        /// <summary> The animation speed, disabled if - </summary>
        [FieldOrder(2)] public int speed;

        /// <summary> When the animation should start </summary>
        [FieldOrder(1)] public int start;
    }

    /// <summary> Basically EYE_ANIM with less values </summary>
    public class FEditEyeAnim : DscFunctionBase
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;
    }

    /// <summary> Edit variant of FMouthAnim </summary>
    public class FEditMouthAnim : DscFunctionBase
    {
        /// <summary> Which animation to play? </summary>
        [FieldOrder(0)] public uint animID;

        /// <summary> The animation speed, disabled if -1  </summary>
        [FieldOrder(3)] public int speed;

        /// <summary> When the animation should start </summary>
        [FieldOrder(2)] public int start;

        /// <summary> ??? </summary>
        [FieldOrder(1)] public int unk;


    }

    /// <summary> Acts like FMoveCamera but repositions around the player </summary>
    public class FEditCamera : DscFunctionBase
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
    }

    /// <summary> Edit variant of FModeSelect </summary>
    /// <remark> Does nothing </remark>
    public class FEditModeSelect : DscFunctionBase
    {
        /// <summary> DESC </summary>
        [FieldOrder(0)] public uint var;
    }
}
