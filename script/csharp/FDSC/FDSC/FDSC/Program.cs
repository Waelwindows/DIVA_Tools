using System;
using System.IO;
using System.Collections.Generic;
using DscOp;

namespace FDSC
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = "D:\\DIVATools\\test_files\\dsc\\pv_609_extreme.dsc";
            path = "//Users//waelwindows//Documents//DIVA_Tools//test_files//dsc//f_tst.dsc";
            FileStream file = new FileStream(path, FileMode.Open);
            DscFile dsc = new DscFile(file);
            Console.Write(dsc.funcs);
            Console.ReadKey();
        }
    }
}
