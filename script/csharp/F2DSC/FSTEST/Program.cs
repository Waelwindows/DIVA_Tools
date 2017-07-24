using System;
using System.IO;
using System.Xml;
using DscFunc;

class Program
{
    static void Main(string[] args)
    {
        Console.Title = "Project Diva F2nd DSC Converter";
        string userInput = "";
        //Console.Write(args.Length + "\n");
        if (args.Length == 0)
        {
            Console.Write("==== Project Diva F2nd .DSC Converter ==== \n");
            Console.Write(">The program converts dsc files into xml files and vice versa\n");
            Console.Write(">This program can be run from a command line interface with the file path as the first argument\n");
            Console.Write("Please enter a file path: ");
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
        string extention = userInput.Substring(userInput.Length - 3);
        if (extention != "dsc" && extention != "xml")
        {
            Console.Write("Invalid file extention ." + extention);
            Console.ReadLine();
            return;
        }
        userInput = userInput.Replace("/", "//");
        switch (extention)
        {
            case "dsc": DscToXml(userInput); break;
            case "xml": XmlToDsc(userInput); break;
        }
        Console.Title = "Project Diva F2nd .DSC Converter : Status: Done";
        Console.Write("Successfully created ." + (extention == "dsc" ? "xml" : "dsc") + " file");
        Console.ReadKey();
    }

    static void DscToXml(string path)
    {
        FileStream file = new FileStream(path, FileMode.Open);
        FileStream saveFile = new FileStream(path.Substring(0, path.Length-3) + "xml", FileMode.CreateNew);
        XmlDocument doc = new XmlDocument();
        DscFile dsc = new DscFile(file);
        dsc.OutputToXml(doc);
        doc.Save(saveFile);
    }

    static void XmlToDsc(string path)
    {
        XmlDocument doc = new XmlDocument(); doc.Load(path);
        if (doc.DocumentElement.Name != "f2nd_dsc")
        {
            Console.Write("Invalid XML file");
            return;
        }
        FileStream dscFile = new FileStream(path.Substring(0, path.Length - 3) + "dsc", FileMode.CreateNew);
        DscFile dsc = new DscFile();
        dsc.CreateNotesFromXml(doc);
        dsc.SaveToFile(dscFile);
    }
}