from bitstring import *

#Setting common format sizes for seeking / shifting
word = 4
dword = word * 2
s_short = s_ushort =s_hfloat = 2
s_int = s_uint = s_float = word
s_int64 = s_uint64 = s_double = dword

class BinClass():
    def __init__(self, bs):
        self.bs = bs

    def read(self, string, count=1, useBits=False):
        multipler = 4 if not useBits else 1
        if ":" in string:
            return self.bs.read(string)
        else:
            if count == 1:
                return self.bs.read(string)
            else:
                return self.bs.read(string+":{0}".format(count*multipler))

    @property
    def to_BArray(self):
        var = vars(self)
        var.pop("bs")
        byteStr = ""
        for value in var.values():
            byteStr+=str(byteStr)
        print(byteStr)
        barray = BitArray(byteStr)
        return barray

    def read_bool(self, count=1):
        #return self.read("bool", count)
        if count == 1:
            return self.read("bool")
        else:
            bool_list = []
            for i in range(count):
                bool_list.append(self.read("bool"))
            return bool_list

    def read_str(self, charcount, encoding="utf-8"):
        if charcount == 0:
            pass
        else:
            byteStr = self.read("bytes", charcount, True)
            return byteStr.decode(encoding)

    def read_int8(self, endian="le"):
        return self.read("int"+endian, 8)

    def read_uint8(self, endian="le"):
        return self.read("uint"+endian, 8)

    def read_int16(self, endian="le"):
        return self.read("int"+endian, 16)

    def read_uint16(self, endian="le"):
        return self.read("uint"+endian, 16)

    def seek(self, offset):
        self.bs.bytepos = offset

    def shift(self, size, count=1):
        self.bs.bytepos += (size * count)

    def __str__(self):
        allclassvars = vars(self)
        allclassvars.pop("bs")
        var = ""
        for key, item in allclassvars.items():
            var += "{0}={1}, ".format(key, item)
        return "{0}: ".format(self.__class__.__name__) + var[:-2]
