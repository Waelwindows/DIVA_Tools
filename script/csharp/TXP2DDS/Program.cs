using System;
using System.Collections.Generic;
using System.IO;
using DIVALib.ImageUtils;
using TxpFunc;

namespace TXP2DDS
{
    class Program
    {
        static void Main(string[] args)
        {
            string dir = "D:\\QuickBMS\\mikitm\\exports\\dds";
            dir = Console.ReadLine();
            //dir.Replace("\", "\\");
            if (dir.Contains("."))
            {
                Console.Write("TXP to DDS mode");
                FileStream file = new FileStream(dir, FileMode.Open);
                TxpToDds(file, dir.Substring(0, dir.Length-8));
            } else 
            {
                Console.Write("DDS to TXP mode");
                DdsToTxp(dir);
            }
            Console.ReadKey();
        }

		public static void TxpToDds(Stream s, string path)
		{
            TxpFile txp = new TxpFile(s);
            uint counter = 0;
            foreach(TxpTexture tex in txp.textures)
            {
                FileStream saveFile = new FileStream(string.Format("{0}_{1}.dds", path, ++counter), FileMode.Create);
                tex.ToDds().Save(saveFile);
                saveFile.Close();
            }
		}

        public static void DdsToTxp(string directory)
        {
            string[] ddsPaths = Directory.GetFiles(directory);
            DdsFile[] dfiles = new DdsFile[ddsPaths.Length];
            uint counter = 0;
            foreach(string path in ddsPaths)
            {
                ++counter;
                FileStream file = new FileStream(path, FileMode.Open);
                dfiles[counter] = new DdsFile(file);
                if (dfiles == null) {Console.Write("Oh no, the newly created DDS object is null");}
            }
            TxpFile txp = TxpFile.FromDds(dfiles);
            FileStream save = new FileStream(ddsPaths[0].Substring(0, ddsPaths[0].Length - 5) + "tex.bin", FileMode.Create);
            txp.Save(save);
            save.Close();
        }
    }
}
