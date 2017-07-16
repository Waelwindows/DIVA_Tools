from bitstring import *
from script.python.binclass import *
from enum import IntEnum

bs = BitStream(filename=fileloc)
file = open(fileloc, "r+b")

class NOTE(IntEnum):
    TRIANGLE        = 0
    CIRCLE          = 1
    CROSS           = 2
    SQUARE          = 3
    ARROW_TRIANGLE  = 4
    ARROW_CIRCLE    = 5
    ARROW_CROSS     = 6
    ARROW_SQUARE    = 7
    HOLD_TRIANGLE   = 8
    HOLD_CIRCLE     = 9
    HOLD_CROSS      = 10
    HOLD_SQUARE     = 11
    STAR            = 12
    DOUBLE_STAR     = 14
    CHANCE_STAR     = 15
    LINKED_STAR     = 22
    LINKED_STAR_END = 23

class dsc_header(BinClass):
    def __init__(self, bs):
        s = self
        BinClass.__init__(s, bs)
        s.magic = s.read_str(4)
        assert s.magic == 'PVSC', "Loaded file is not valid F2nd DSC file"
        s.cl_tell = bs.pos
        s.content_length = s.read_uint8("le")
        s.shift(s_uint, 6)
        s.unk1 = s.read_uint8("le")
        s.shift(s_uint, 7)

class dsc_note(BinClass):
    def __init__(self, bs, create=False):
        static_encode = "be"
        s = self
        BinClass.__init__(s, bs)
        s.note_pos = 0
        if not create:
            s.note_pos = bs.pos
            s.unk1 = s.read_uint8(static_encode)
            s.time_stamp = s.read_uint8(static_encode)
            s.note_opcode = s.read_uint8(static_encode)
            s.note_type = s.read_uint8(static_encode)
            s.hold_length = s.read_int8(static_encode)
            s.is_hold_end = s.read_int8(static_encode)
            s.note_xcoord = s.read_int8(static_encode)
            s.note_ycoord = s.read_int8(static_encode)
            s.curve_angle1 = s.read_int8(static_encode)
            s.curve_angle2 = s.read_int8(static_encode)
            s.unk2 = s.read_uint8(static_encode)
            s.unk3 = s.read_uint8(static_encode)
            s.note_timeout = s.read_uint8(static_encode) #in ms
            s.unk4 = s.read_uint8(static_encode)
            s.unk5 = s.read_int8(static_encode)
        else:
            s.unk1 = 0
            s.time_stamp = 0 # / 10 (e.g. 12345 = 0.12345)
            s.note_opcode = 0
            s.note_type = 0 # Add enum ?
            s.hold_length = 0
            s.is_hold_end = 0 # is Bool
            s.note_xcoord = 0
            s.note_ycoord = 0
            s.curve_angle1 = 0
            s.curve_angle2 = 0
            s.unk2 = 0
            s.unk3 = 0
            s.note_timeout = 0 #in ms
            s.unk4 = 0
            s.unk5 = 0


    def __str__(self):
        return "Note: {0} note at {1} ms".format(self.get_note_type(), self.time_stamp)

    @property
    def to_BArray(self):
        allvar = vars(self)
        bs = BitArray("")
        try: allvar.pop("bs")
        except KeyError: print("Hmm, no bs")
        try: allvar.pop("note_pos")
        except KeyError: print("Hmm, no note_pos")
        counter = 0
        formatstr = ""
        uint_list = [1, 2, 3, 4, 11, 12, 13, 14]
        int_list = [5, 6, 7, 8, 9, 10, 15]
        for value in allvar.values():
            counter += 1
            if counter in uint_list:
                formatstr = "uintbe:32="
            elif counter in int_list:
                formatstr = "intbe:32="
            bs += formatstr + str(value)
        return bs

        def get_note_type(self):
            s = self
            if s.note_type == 0: return "Triangle"
            elif s.note_type == 1: return "Circle"
            elif s.note_type == 2: return "Cross"
            elif s.note_type == 3: return "Square"
            elif s.note_type == 4: return "Triangle Arrow"
            elif s.note_type == 5: return "Circle Arrow"
            elif s.note_type == 6: return "Cross Arrow"
            elif s.note_type == 7: return "Square Arrow"
            elif s.note_type == 8: return "Triangle Hold"
            elif s.note_type == 9: return "Circle Hold"
            elif s.note_type == 10: return "Cross Hold"
            elif s.note_type == 11: return "Square Hold"
            elif s.note_type == 12: return "Star"
            elif s.note_type == 14: return "Double Star"
            elif s.note_type == 15: return "Chance Star"
            elif s.note_type == 22: return "Linked Star"
            elif s.note_type == 23: return "Linked Star End"
            else:
                #raise ValueError("Invalid note type")
                return "WTF"

class dsc_file(BinClass):
    def __init__(self, bs):
        s = self
        BinClass.__init__(s, bs)
        s.header = dsc_header(bs)
        s.shift(dword)
        counter = 8
        s.notes = []
        while counter <= s.header.content_length-60:
            self.notes.append(dsc_note(bs))
            counter+=60

    def overwrite_note(self, note, pos=0):
        note_offset = self.notes[pos].note_pos
        self.bs.overwrite(note.to_BArray, note_offset)

    def update_note(self, pos=0):
        note_offset = self.notes[pos].note_pos
        self.bs.overwrite(self.notes[pos].to_BArray, note_offset)

    def add_note(self, note):
        note_offset = self.notes[-1].note_pos
        self.bs.insert(note.to_BArray, note_offset + 480)
        new_cl = BitArray("uintle:32="+str(self.header.content_length+60))
        self.bs.overwrite(new_cl, self.header.cl_tell)

    def save_changes(self, file):
        self.bs.tofile(file)
