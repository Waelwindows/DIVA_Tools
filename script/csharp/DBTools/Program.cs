using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

using DIVALib.Databases;
using DIVALib.FileBases;
using System.Globalization;

namespace DBTools
{
    class Program
    {
        static FileFormatBase GetFormat(string name)
        {
            var culture = CultureInfo.GetCultureInfo("en-US");
            name = Path.GetFileNameWithoutExtension(name).ToLower(culture);

            switch (name)
            {
                case "aet_db":
                    return new AetDatabase();

                case "obj_db":
                    return new ObjectDatabase();

                case "spr_db":
                    return new SpriteDatabase();

                case "str_array":
                case "string_array":
                    return new StringArray();

                case "tex_db":
                    return new TextureDatabase();

                default:
                    return null;
            }
        }

        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Database Tools");
                Console.WriteLine("=============");
                Console.WriteLine("This program will convert any database");
                Console.WriteLine("file from DIVA games to XML format, or");
                Console.WriteLine("XML format to database files.\n");

                Console.WriteLine("Following files are supported:");
                Console.WriteLine("    aet_db.bin");
                Console.WriteLine("    obj_db.bin");
                Console.WriteLine("    spr_db.bin");
                Console.WriteLine("    str_array.bin");
                Console.WriteLine("    string_array.bin");
                Console.WriteLine("    tex_db.bin\n");

                Console.WriteLine("Usage:");
                Console.WriteLine("    DBTools [source] [destination]");
                Console.WriteLine("        If destination is left empty, it will");
                Console.WriteLine("        be automatically set.");
                Console.ReadLine();
                return;
            }

            string source = null;
            string destination = null;

            for (int i = 0; i < args.Length; i++)
            {
                if (source == null)
                {
                    source = args[i];
                }

                else if (destination == null)
                {
                    destination = args[i];
                }
            }

            if (source.EndsWith(".bin", StringComparison.OrdinalIgnoreCase))
            {
                if (destination == null)
                {
                    destination = Path.ChangeExtension(source, "xml");
                }
                
                if (!destination.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
                {
                    destination = Path.ChangeExtension(destination, "xml");
                }

                var format = GetFormat(source);
                
                if (format == null)
                {
                    throw new ArgumentException("Invalid file format.", nameof(source));
                }

                format.Load(source);

                XmlSerializer serializer = new XmlSerializer(format.GetType());

                using (Stream xmlDestination = File.Create(destination))
                {
                    serializer.Serialize(xmlDestination, format);
                }
            }

            else if (source.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
            {
                if (destination == null)
                {
                    destination = Path.ChangeExtension(source, "bin");
                }

                if (!destination.EndsWith(".bin", StringComparison.OrdinalIgnoreCase))
                {
                    destination = Path.ChangeExtension(destination, "bin");
                }

                var format = GetFormat(source);
                if (format == null)
                {
                    format = GetFormat(destination);
                }

                if (format == null)
                {
                    throw new ArgumentException("Could not detect file format to write.", "source & destination");
                }

                XmlSerializer deserializer = new XmlSerializer(format.GetType());

                using (Stream xmlSource = File.OpenRead(source))
                {
                    format = (FileFormatBase)deserializer.Deserialize(xmlSource);
                }

                format.Save(destination);
            }
        }
    }
}
