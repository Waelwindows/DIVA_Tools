using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;

using DIVALib.Archives;
using DIVALib.IO;

namespace FarcPack
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("FARC Pack");
                Console.WriteLine("=========");
                Console.WriteLine("Packer/unpacker for .FARC files.\n");
                Console.WriteLine("Usage:");
                Console.WriteLine("    FarcPack [options] [source] [destination]");
                Console.WriteLine("    Source can be either FARC or directory.");
                Console.WriteLine("    Destination can be left empty.\n");
                Console.WriteLine("Options:");
                Console.WriteLine("    -a, -alignment         Set alignment for output FARC.");
                Console.WriteLine("                           16 by default.\n");
                Console.WriteLine("    -c, --compress         Compress files in output FARC.");
                Console.WriteLine("                           Disabled by default.");
                Console.ReadLine();
                return;
            }

            string sourcePath = null;
            string destinationPath = null;

            bool compress = false;
            uint alignment = 16;

            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];

                if (arg.Equals("-a", StringComparison.OrdinalIgnoreCase) ||
                    arg.Equals("-alignment", StringComparison.OrdinalIgnoreCase))
                {
                    alignment = uint.Parse(args[++i]);
                }

                else if (arg.Equals("-c", StringComparison.OrdinalIgnoreCase) ||
                    arg.Equals("-compress", StringComparison.OrdinalIgnoreCase))
                {
                    compress = true;
                }

                else if (sourcePath == null)
                {
                    sourcePath = arg;
                }

                else if (destinationPath == null)
                {
                    destinationPath = arg;
                }
            }

            if (sourcePath == null)
            {
                throw new ArgumentException("You must provide a source.", nameof(sourcePath));
            }

            if (sourcePath.EndsWith(".farc", StringComparison.OrdinalIgnoreCase))
            {
                FarcArchive archive = new FarcArchive();
                archive.Load(sourcePath);

                if (destinationPath == null)
                {
                    destinationPath = Path.ChangeExtension(sourcePath, null);
                }

                Directory.CreateDirectory(destinationPath);

                using (Stream source = File.OpenRead(sourcePath))
                {
                    foreach (FarcEntry entry in archive)
                    {
                        using (Stream entrySource = new SubStream(source, entry.Position, entry.Length))
                        using (Stream destination = File.Create(Path.Combine(destinationPath, entry.FileName)))
                        {
                            if (archive.IsEncrypted)
                            {
                                using (AesManaged aes = new AesManaged
                                {
                                    KeySize = 128,
                                    Key = FarcArchive.FarcEncryptionKeyBytes,
                                    BlockSize = 128,
                                    Mode = CipherMode.ECB,
                                    Padding = PaddingMode.Zeros,
                                    IV = new byte[16],
                                })
                                using (CryptoStream cryptoStream = new CryptoStream(
                                    entrySource,
                                    aes.CreateDecryptor(),
                                    CryptoStreamMode.Read))
                                {
                                    if (archive.IsCompressed && entry.Length != entry.CompressedLength)
                                    {
                                        using (GZipStream gzipStream = new GZipStream(cryptoStream, CompressionMode.Decompress))
                                        {
                                            gzipStream.CopyTo(destination);
                                        }
                                    }

                                    else
                                    {
                                        cryptoStream.CopyTo(destination);
                                    }
                                }
                            }

                            else if (archive.IsCompressed && entry.Length != entry.CompressedLength)
                            {
                                using (GZipStream gzipStream = new GZipStream(entrySource, CompressionMode.Decompress))
                                {
                                    gzipStream.CopyTo(destination);
                                }
                            }

                            else
                            {
                                entrySource.CopyTo(destination);
                            }
                        }
                    }
                }
            }

            else if (File.GetAttributes(sourcePath).HasFlag(FileAttributes.Directory))
            {
                FarcArchive archive = new FarcArchive();
                archive.Alignment = alignment;
                archive.IsCompressed = compress;

                if (destinationPath == null)
                {
                    destinationPath = sourcePath + ".farc";
                }

                foreach (string fileName in Directory.GetFiles(sourcePath))
                {
                    archive.Add(new FarcEntry
                    {
                        FileName = Path.GetFileName(fileName),
                        FilePath = new FileInfo(fileName)
                    });
                }

                archive.Save(destinationPath);
            }
        }
    }
}
