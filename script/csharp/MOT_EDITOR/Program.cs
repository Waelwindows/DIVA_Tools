using BinarySerialization;
using DIVALib.Archives;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace MOT_EDITOR
{
    internal class Program
    {
        [STAThreadAttribute]
        private static void Main(string[] args)
        {
            const string path = @"D:\QuickBMS\modify_mot\mot_PV040_org.bin";
            //const string path = @"D:\QuickBMS\f_anim\mot_PV626_tst.bin";
            using (var file = new FileStream(path, FileMode.Open))
            {
                var serializer = new BinarySerializer();
                //var motFile = serializer.Deserialize<MotFile>(file);
                var motFile = MotFile.Deserialize(file, false);
                Console.WriteLine(motFile.GetMotData(0, 0).FrameCount);
                //const string savePath = @"D:\QuickBMS\f_anim\mot_PV056.bin";
                const string SavePath = @"D:\QuickBMS\modify_mot\mot_PV040.xml";
                using (var save = new FileStream(SavePath, FileMode.Create))
                {
                    var frames = new List<Vector3>();
                    for (int frame = 0; frame < motFile.Animations[0].Skeleton.IkLeftFoot.AnimData[0].Values.Count; frame++)
                    {
                        var xaxis = 0.0f;
                        var yaxis = 0.0f;
                        var zaxis = 0.0f;
                        try
                        {
                            xaxis = motFile.Animations[0].Skeleton.IkLeftFoot.AnimData[0].Values[frame].Value;
                        }
                        catch (Exception e) { }
                        try
                        {
                            yaxis = motFile.Animations[0].Skeleton.IkLeftFoot.AnimData[1].Values[frame].Value;
                        }
                        catch (Exception e) { }
                        try
                        {
                            zaxis = motFile.Animations[0].Skeleton.IkLeftFoot.AnimData[2].Values[frame].Value;
                        }
                        catch (Exception e) { }
                        frames.Add(new Vector3(xaxis, yaxis, zaxis));
                    }
                    var frameStrings = "(";
                    foreach (var frame in frames)
                    {
                        frameStrings += $"(X={frame.X},Y={frame.Y},Z={frame.Z}),\n";
                    }
                    frameStrings += ")";
                    Clipboard.SetText(frameStrings);
                    //serializer.Serialize(save, motFile);
                }
                /*
                var archive = new FarcArchive
                {
                    new FarcEntry
                    {
                        FileName = Path.GetFileName(SavePath),
                        FilePath = new FileInfo(SavePath)
                    }
                };

                var farcDirectory = @"D:\Emulators\RPSC3\dev_hdd0\game\NPJB00134\USRDIR\rom\rob\" + Path.ChangeExtension(archive[0].FileName, ".farc");
                //var farcDirectory = @"D:\Emulators\RPSC3\dev_hdd0\disc\BLJM60527\PS3_GAME\USRDIR\rom\rob" + Path.ChangeExtension(archive[0].FileName, ".farc");
                archive.Save(farcDirectory);
                */
            }
        }
    }
}