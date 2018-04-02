using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BinarySerialization;
using DIVALib.ImageUtils;
using DIVALib.IO;

namespace SPRTool
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var path = "";
#if !DEBUG
            if (args.Length == 0) return;
            path = args[0];
#endif
#if DEBUG
            path = @"D:\QuickBMS\mdl_thmb\pv701\spr_gam_pv701.spr";
            path = @"D:\QuickBMS\mdl_thmb\thmb\spr_win_shop.spr";
            path = @"D:\QuickBMS\mdl_thmb\pv701\spr_gam_pv701.spr";
#endif
            if (Directory.Exists(path))
            {
                await Dds2Spr(path);
            } else if (Path.GetExtension(path) == ".spr")
            {
                await Spr2Dds(path);
            }
            else
            {
                return;
            }
            
        }

        private static void OnMemberDeserialized(object sender, MemberSerializedEventArgs e)
        {
            Console.CursorLeft = e.Context.Depth * 4;
            var value = e.Value ?? "null";
            Console.WriteLine("D-End: {0} ({1}) @ {2}", e.MemberName, value, e.Offset);
        }

        public static async Task Spr2Dds(string path)
        {
            var file = File.Open(path, FileMode.Open);
            var parentPath = Path.GetDirectoryName(path);
            var serializer = new BinarySerializer();
#if DEBUG
            serializer.MemberDeserialized += OnMemberDeserialized;   
#endif
            var tex = await serializer.DeserializeAsync<SpriteFile>(file);
            file.Close();
            var tst = 0;

            foreach (var txpTexture in tex.Atlas.Textures)
            {
                using (var save = new FileStream($"{parentPath}\\{tex.Header.TextureNames[tst++]}.dds", FileMode.Create))
                {
                    await serializer.SerializeAsync(save, (DdsFile)txpTexture);
                }
            }
        }

        public static async Task Dds2Spr(string path)
        {
            var file = File.Open(path, FileMode.Open);
            var serializer = new BinarySerializer();
#if DEBUG
            serializer.MemberDeserialized += OnMemberDeserialized;
#endif
            var tex = await serializer.DeserializeAsync<SpriteFile>(file);
            var ddsFiles = Directory.EnumerateFiles(Path.GetDirectoryName(path)).Where(f => Path.GetExtension(f) == ".dds");
            var counter = 0;

            foreach (var ddsFile in ddsFiles)
            {
                using (var ddsEdit = new FileStream(ddsFile, FileMode.Open))
                {
                    var dds = await serializer.DeserializeAsync<DdsFile>(ddsEdit);
                    tex.Atlas.Textures[counter++].Mipmaps[0] = dds.ToTxpMip();
                }
            }

            file = new FileStream(path, FileMode.Create);
    
            await serializer.SerializeAsync(file, tex);
        }
    }
}

