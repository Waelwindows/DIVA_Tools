using BinarySerialization;
using DIVALib.Archives;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
            const string path = @"D:\QuickBMS\dt_anim\mot_PV007.bin";
            //const string path = @"D:\QuickBMS\f_anim\mot_PV626.bin";
            //const string path = @"D:\QuickBMS\f_anim\mot_PV626_tst.bin";
            using (var file = new FileStream(path, FileMode.Open))
            {
                
                var serializer = new BinarySerializer();
                //var motFile = serializer.Deserialize<MotFile>(file);
                var motFile = MotFile.Deserialize(file, false);
                Console.WriteLine(motFile.GetMotData(0, 0).FrameCount);
                //const string savePath = @"D:\QuickBMS\f_anim\mot_PV056.bin";
                const string SavePath = @"D:\QuickBMS\modify_mot\mot_PV007.bin";
                //const string SavePath = @"D:\QuickBMS\modify_mot\mot_PV007.xml";
                /*
                using (var save = new FileStream(SavePath, FileMode.Create))
                {
                    var doc = new XmlDocument();
                    var root = doc.CreateElement("animation");
                    var wrapper = doc.CreateElement("ikAnim");
                    foreach (var value in motFile.Animations[0].Data[24].Values)
                    {
                        var element = doc.CreateElement("axis");
                        element.InnerText = $"(X={value.Value},Y=0.0,Z=0.0)";
                        
                    }
                    var leftHand = motFile.Animations[0].Data[24];
                    var leftHand1 = motFile.Animations[0].Data[24];
                    var leftHand2 = motFile.Animations[0].Data[24];

                    for (var index = 0; index < leftHand.FrameCount; index++)
                    {
                        var element = doc.CreateElement("axis");
                        var frame = doc.CreateElement("frame");
                        frame.InnerText = $"{leftHand.Frames[index]}";
                        element.InnerText = $"(X={leftHand.Values[index].Value},Y={leftHand1.Values[index].Value},Z={leftHand2.Values[index].Value})";

                        wrapper.AppendChild(frame);
                        wrapper.AppendChild(element);
                    }
                    root.AppendChild(wrapper);
                    doc.AppendChild(root);
                    doc.Save(save);
                }
                */
                Console.WriteLine(motFile.GetMotData(0, 0).FrameCount);
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
                
            }
        }
    }
}