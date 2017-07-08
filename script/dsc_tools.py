from bitstring import *
from script.binclass import *

bs = BitStream(filename="/Users/waelwindows/Documents/DIVA_files/dsc/pv_717_normal.dsc")

class dsc_header(BinClass):
    def __init__(self, bs):
        s = self
        BinClass.__init__(s, bs)
        s.magic = s.read_str(4)
        assert s.magic == 'PVSC', "Loaded file is not valid F2nd DSC file"
        s.content_length = s.read_uint8()
        s.shift(s_uint, 6)
        s.unk1 = s.read_uint8()
        s.shift(s_uint, 7)

class dsc_note(BinClass):
    def __init__(self, bs, encoding="le"):
        s = self
        BinClass.__init__(s, bs)
        s.unk1 = s.read_uint8(encoding)
        s.time_stamp = s.read_uint8(encoding)
        s.note_opcode = s.read_uint8(encoding)
        s.note_type = s.read_uint8(encoding)
        s.hold_length = s.read_int8(encoding)
        s.is_hold_end = s.read_int8(encoding)
        s.note_xcoord = s.read_int8(encoding)
        s.note_ycoord = s.read_int8(encoding)
        s.curve_angle1 = s.read_int8(encoding)
        s.curve_angle2 = s.read_int8(encoding)
        s.unk2 = s.read_uint8(encoding)
        s.unk3 = s.read_uint8(encoding)
        s.note_timeout = s.read_uint8(encoding) #in ms
        s.unk4 = s.read_uint8(encoding)
        s.unk5 = s.read_int8(encoding)

class create_dsc_note():
    def __init__(self):
        s = self
        BinClass.__init__(s, bs)
        s.unk1 = 0
        s.time_stamp = 0
        s.note_opcode = 0
        s.note_type = 0
        s.hold_length = 0
        s.is_hold_end = 0
        s.note_xcoord = 0
        s.note_ycoord = 0
        s.curve_angle1 = 0
        s.curve_angle2 = 0
        s.unk2 = 0
        s.unk3 = 0
        s.note_timeout = 0 #in ms
        s.unk4 = 0
        s.unk5 = 0

    def get_note_type(self):
        s = self
        if s.note_type == 0:
            return "Triangle"
        elif s.note_type == 1:
            return "Circle"
        elif s.note_type == 2:
            return "Cross"
        elif s.note_type == 3:
            return "Square"
        elif s.note_type == 4:
            return "Triangle Arrow"
        elif s.note_type == 5:
            return "Circle Arrow"
        elif s.note_type == 6:
            return "Cross Arrow"
        elif s.note_type == 7:
            return "Square Arrow"
        elif s.note_type == 8:
            return "Triangle Hold"
        elif s.note_type == 9:
            return "Circle Hold"
        elif s.note_type == 10:
            return "Cross Hold"
        elif s.note_type == 11:
            return "Square Hold"
        elif s.note_type == 12:
            return "Star"
        elif s.note_type == 14:
            return "Double Star"
        elif s.note_type == 15:
            return "Chance Star"
        elif s.note_type == 22:
            return "Linked Star"
        elif s.note_type == 23:
            return "Linked Star End"
        else:
            #raise ValueError("Invalid note type")
            return "WTF"
        # TODO: Complete the if statement

    def __str__(self):
        return "Note: {0} note at {1} ms".format(self.get_note_type(), self.time_stamp)

class dsc_file(BinClass):
    def __init__(self, bs):
        s = self
        BinClass.__init__(s, bs)
        header = dsc_header(bs)
        s.shift(dword)
        counter = 8
        s.notes = []
        while counter <= header.content_length-60:
            self.notes.append(dsc_note(bs, "be"))
            counter+=60


tst = dsc_file(bs)
print(tst.notes[0].to_BArray)
