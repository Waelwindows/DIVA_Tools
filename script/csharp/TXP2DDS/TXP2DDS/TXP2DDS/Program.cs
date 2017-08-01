using System;
using System.Collections.Generic;
using System.IO;
using TxpFunc;
using DdsTools;

namespace TXP2DDS
{
    class Program
    {
        static void Main(string[] args)
        {
            string ddsDir = "D:\\QuickBMS\\mikitm\\exports\\dds";
            string txpDir = "D:\\QuickBMS\\mikitm\\exports\\test_tex.bin";
            string[] ddsPath = Directory.GetFiles(ddsDir);
            //DdsFile[] ddsTex = new DdsFile[ddsPath.Length];
            List<DdsFile> ddsFiles = new List<DdsFile>();
            //uint counter = 0;
            foreach (string path in ddsPath)
            {
                FileStream file = new FileStream(path, FileMode.Open);
                DdsFile ddsTex = new DdsFile(file);
                if (ddsTex == null) { return; }
                ddsFiles.Add(ddsTex);
            }
            DdsFile[] allDds = new DdsFile[ddsFiles.Count];
            uint counter = 0;
            foreach(DdsFile done in ddsFiles)
            {
                allDds[counter] = done;
                counter++;
            }
            TxpFile txp = TxpFile.FromDds(allDds);
            FileStream save = new FileStream(txpDir, FileMode.Create);
            txp.Save(save);
            save.Close();
            Console.ReadKey();
        }
    }
}
