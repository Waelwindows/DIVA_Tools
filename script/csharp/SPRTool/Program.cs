using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using BinarySerialization;
using DIVALib.ImageUtils;
using DIVALib.IO;

namespace SPRTool
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = @"D:\QuickBMS\mdl_thmb\pv701\spr_gam_pv701.spr";
            path = @"D:\QuickBMS\mdl_thmb\thmb\spr_win_shop.spr";
            path = @"D:\QuickBMS\mdl_thmb\pv701\spr_gam_pv701.spr";
            var file = new FileStream(path, FileMode.OpenOrCreate);
            var serializer = new BinarySerializer();
#if DEBUG
            serializer.MemberDeserialized += OnMemberDeserialized;
#endif

            var tex = serializer.Deserialize<SpriteFile>(file);

            /*
            var parentPath = Path.GetDirectoryName(path);
            var ddsTXP = (DdsFile)tex.Atlas.Textures[0];
            var tst = 0;

            foreach (var txpTexture in tex.Atlas.Textures)
            {
                using (var save = new FileStream($"{parentPath}\\{tex.Header.TextureNames[tst++]}.dds", FileMode.Create))
                {
                    serializer.Serialize(save, (DdsFile) txpTexture);
                }
            }
            */
            file.Close();
            /**/

            var ddsFiles = Directory.EnumerateFiles(Path.GetDirectoryName(path)).Where(f => Path.GetExtension(f) == ".dds");
            var counter = 0;

            foreach (var ddsFile in ddsFiles)
            {
                using (var ddsEdit = new FileStream(ddsFile, FileMode.Open))
                {
                    var dds = serializer.Deserialize<DdsFile>(ddsEdit);
                    tex.Atlas.Textures[counter++].Mipmaps[0] = dds.ToTxpMip();
                }
            }

            file = new FileStream(path, FileMode.Create);
            serializer.Serialize(file, tex);
            
        }

        private static void OnMemberDeserialized(object sender, MemberSerializedEventArgs e)
        {
            Console.CursorLeft = e.Context.Depth * 4;
            var value = e.Value ?? "null";
            Console.WriteLine("D-End: {0} ({1}) @ {2}", e.MemberName, value, e.Offset);
        }
    }
}
