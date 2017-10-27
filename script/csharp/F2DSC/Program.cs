using System;
using System.IO;
using System.Xml;
using DscFunc;

class Program
{
    static void Main(string[] args)
    {
        Console.Title = "Project Diva F2nd DSC Converter";
        Console.Clear();
        string userInput = "";
        //Console.Write(args.Length + "\n");
        if (args.Length == 0)
        {
            Console.Write("==== Project Diva F2nd .DSC Converter ==== \n");
            Console.Write("#> The program converts dsc files into xml files and vice versa\n");
            Console.Write("#> Please use absolute paths as to not cause confusion\n");
            Console.Write("#> This program can be run from a command line interface with the file path as the first argument\n");
            Console.Write("Please enter a file path or help: ");
            userInput = Console.ReadLine();
        } else
        {
            userInput = args[0];
        }
        if (userInput.Length == 0 || userInput == string.Empty)
        {
            Console.Write("Please enter a valid path");
            Console.ReadLine();
            return;
        }
        if (!userInput.Contains(".") && userInput != "help")
        {
            Console.Write("Please enter a valid path");
            Console.ReadLine();
            return;
        }
        string extention = userInput.Substring(userInput.Length - 3);
        if (!userInput.Contains(".dsc") && !userInput.Contains(".xml") && !userInput.Contains("help"))
        {
            Console.Write("Invalid file extention ." + extention);
            Console.ReadLine();
            return;
        }
        userInput = userInput.Replace("/", "//");
        Console.Clear();
        bool success = false;
        switch (extention)
        {
            case "dsc": DscToXml(userInput, ref success); break;
            case "xml": XmlToDsc(userInput, ref success); break;
            case "elp": Help(); break;
        }
        if (success)
        {
            Console.Title = "Project Diva F2nd .DSC Converter : Status: Done";
            Console.Write($"Successfully created . { extention == "dsc" ? "xml" : "dsc" } file \n");
            Console.Write("Don't forget to exit the program before editing, Press any key...");
        } else if (!success && extention == "elp")
        {
            Console.Write("\n Press any key to exit...");
        }
        else
        {
            Console.Title = "Project Diva F2nd .DSC Converter : Status: Fail";
            Console.Write("Couldn't create ." + (extention == "dsc" ? "xml" : "dsc") + " file due to an error");
        }
        Console.ReadKey();
    }

    static void Help()
    {
        Console.Clear();
        Console.Write("====== Help ======\n");
        Console.Write("Position units: A note is 0.5x0.5 units wide, Meaning the screen is 100x60 notes wide \n \n");
        Console.Write("How to get real-time: To make notes appear at the correct time, you need to \n" +
            "add the timestamp (ms) and the timeOut (ms) to get the time when the player will hit the note.  \n \n ");
        //Console.Write("Rotation units: BLANK");
    }

    static void DscToXml(string path, ref bool success)
    {
        FileStream file = new FileStream(path, FileMode.Open);
        XmlDocument doc = new XmlDocument();
        DscFile dsc = new DscFile(file);
        if (dsc.header.magic == "DIVA")
        {
            Console.Write("ERROR: DIVAFILE encrypted DSC, Please decrypt this DSC first before conversion. \n");
            success = false;
            return;
        }
        else
        {
            FileStream saveFile = new FileStream(path.Substring(0, path.Length - 3) + "xml", FileMode.CreateNew);
            dsc.OutputToXml(doc);
            doc.Save(saveFile);
            saveFile.Close();
        }
        success = true;
    }

    static void XmlToDsc(string path, ref bool success)
    {
        XmlDocument doc = new XmlDocument(); doc.Load(path);
        if (doc.DocumentElement.Name != "f2nd_dsc")
        {
            Console.Write("Invalid XML file");
            success = false;
            return;
        }
        FileStream dscFile = new FileStream(path.Substring(0, path.Length - 3) + "dsc", FileMode.Create);
        DscFile dsc = new DscFile();
        dsc.CreateNotesFromXml(doc);
        dsc.SaveToFile(dscFile);
        dscFile.Close();
        success = true;
    }
}