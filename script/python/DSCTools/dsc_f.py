from bitstring import *
from script.python.binclass import *

fileloc = "/Users/waelwindows/Documents/DIVA_Tools/test_files/dsc/tst_dsc.dsc"
#bs = BitStream(filename=fileloc)
bs = BitArray()
dsc_file = open(fileloc, "w+b")

def read_funcid(bs, func_id, func_desp="Standard Function"):
    file_funcid = s.read_uint8()
    assert file_funcid == func_id, "Invalid func_id, {0}".format(file_funcid)
    return file_funcid

class FFunc(BinClass):
    func_id = 0x00
    def __init__(self, bs, create):
        s = self
        BinClass.__init__(s, bs)
        if not create:
            s._read(bs)
        else:
            s._create()

    def _read(self, bs):
        file_funcid = self.read_uint8()
        assert self.func_id == file_funcid, "Invalid func_id for {0} func, {1} func_id found".format(self.__class__.__name__, file_funcid)

    def _create(self):
        pass

class dsc_header(BinClass):
    def __init__(self, bs, create=0):
        s = self
        BinClass.__init__(s, bs)
        if not create:
            s.magic = s.read("bytes",4, True)
            assert s.magic == b' \x02\x02\x12', "Invalid F .dsc file"
        else:
            s.magic = b' \x02\x02\x12'

class end(FFunc):
    def __init__(self, bs, create=0):
        FFunc.__init__(self, bs, create)
    def _read(self, bs):
        self.unk = self.read_uint8()
    def _create(self):
        self.unk = 0

class time(FFunc):
    def __init__(self, bs, create=0):
        FFunc.__init__(self, bs, create)
    def _read(self, bs):
        self.timestamp = read_uint8
    def _create(self):
        self.timestamp = 0
    def set_time(self, new_time):
        if isinstance(new_time, str):
            if new_time[-1] == "s":
                try: self.timestamp = int(new_time[:-1]) * 100_000
                except Exception:
                    raise TypeError("Invalid input for time")
        elif isinstance(new_time, int):
            self.timestamp = abs(new_time) * 1000

header = dsc_header(bs, 1)
bs.append(header.to_BArray)
time1 = time(bs, 1)
time1.set_time("10s")
bs.append(time1.to_BArray)
print(time1.to_BArray)
print(bs)
#bs.tofile(dsc_file)
