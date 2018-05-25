using System;
using System.Collections.Generic;
using System.IO;
using BinarySerialization;
using DIVALib.ImageUtils;

namespace TXP2DDS
{
    class Program
    {
        static void Main(string[] args)
        {
#if !DEBUG
            if (args.Length == 0)
            {
                var helpstr = @"TXP2DDS
==================
Converter for all .txp/_tex.bin virtua fighter engine games.
It can export to a .dds file with the equivilant pixel format.

Supports:
    - All Project DIVA games before F2nd (tested on F)
    - Batch importing / exporting

Usage:
    Drag'n'drop your .txp/.dds/directory into the application.
    or
    TXP2DDS [source]";
                Console.WriteLine(helpstr);
                Console.ReadLine();
                return;

        }
#endif
#if !DEBUG
            var dir = args[0];
            
            if (!File.Exists(args[0]) && !File.GetAttributes(dir).HasFlag(FileAttributes.Directory))
                throw new IOException("file doesn't exist.");
#endif
#if DEBUG
            var dir = @"D:\QuickBMS\nez_txp";
            dir = @"D:\QuickBMS\nez_txp\test1\acf15p3_tex.bin";
#endif

            if (File.GetAttributes(dir).HasFlag(FileAttributes.Directory))
            {
                var ddsPaths = new List<string>();
                foreach (var filePath in Directory.EnumerateFiles(dir))
                {
                    using (var file = new FileStream(filePath, FileMode.Open))
                    {
                        switch (Path.GetExtension(filePath))
                        {
                            case ".bin":
                            case ".txp":
                                TxpToDds(file, filePath);
                                break;
                            case ".dds":
                                ddsPaths.Add(filePath);
                                break;
                        }
                    }
                }
                if (ddsPaths.Count > 0) DdsToTxpAtlas(ddsPaths); 
            }
            else
            {
                using (var file = new FileStream(dir, FileMode.Open))
                {
                    switch (Path.GetExtension(dir))
                    {
                        case ".bin":
                        case ".txp":
                            TxpToDds(file, dir);
                            break;
                        case ".dds":
                            DdsToTxp(file, dir);
                            break;
                    }
                }
            }
        }

        private static void OnMemberDeserialized(object sender, MemberSerializedEventArgs e)
        {
            Console.CursorLeft = e.Context.Depth * 4;
            var value = e.Value ?? "null";
            Console.WriteLine("D-End: {0} ({1}) @ {2}", e.MemberName, value, e.Offset);
        }

        public static void TxpToDds(Stream s, string path)
		{
		    var serializer = new BinarySerializer();
#if DEBUG
            serializer.MemberDeserialized += OnMemberDeserialized;
#endif
            var atlas = serializer.Deserialize<TxpTextureAtlas>(s);
            atlas.SetTextures(s);
		    var ddsTex = (List<DdsFile>) atlas;
		    var texPath = $"{path.Substring(0, path.Length - 4)}_tex";
		    for (var i = 0; i < ddsTex.Count; ++i)
		    {
		        using (var save = new FileStream($"{texPath}{i:00}.dds", FileMode.Create)) serializer.Serialize(save, ddsTex[i]);
		    }       
		}

        public static void DdsToTxp(Stream s, string path)
        {
            var serializer = new BinarySerializer();
            var dds = serializer.Deserialize<DdsFile>(s);
            dds.SetMipMaps();
            using (var file = new FileStream(Path.ChangeExtension(path, "dds"), FileMode.Create)) serializer.Serialize(file, dds.ToTxp());
        }

        public static void DdsToTxpAtlas(List<string> ddsFiles)
        {
            var atlas = new TxpTextureAtlas();
            var serializer = new BinarySerializer();
            foreach (var path in ddsFiles)
            {
                using (var ddsFile = new FileStream(path, FileMode.Open))
                {
                    var dds = serializer.Deserialize<DdsFile>(ddsFile);
                    dds.SetMipMaps();
                    atlas.Textures.Add(dds.ToTxp());
                }
            }
            atlas.TextureCount = atlas.Textures.Count;
            atlas.Textures.ForEach(tex => tex.SetMipOffsets());
            atlas.SetTextureOffsets();
            var savePath = ddsFiles[0].Length > 10 ? $"{ddsFiles[0].Slice(end: -10)}.txp" : Path.ChangeExtension(ddsFiles[0], "txp");
            using (var txpFile = File.Create(savePath)) serializer.Serialize(txpFile, atlas); 
        }
    }
}