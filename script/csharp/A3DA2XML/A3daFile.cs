using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Collections.Generic;
using DIVALib.IO;

namespace A3daFunc
{
	public class A3daFile
	{
		const int fileStart = 0xBB;
		public List<string> keys;
		public List<string> values;
		//byte[] byteData;

		public A3daFile() { }
		public A3daFile(Stream s)
		{
			s.Position = fileStart;
			string fileTXT = DataStream.ReadCString(s);
			string[] keyPair = fileTXT.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
			List<string> kPValue = new List<string>();
			keys = new List<string>();
			values  = new List<string>();
			s.Position -= 2;
			SubStream data = new SubStream(s, s.Position);
			foreach(string kp in keyPair)
			{                
				string[] kpSplit = kp.Split('=');
				if (kpSplit.Length == 1) { break; }
				string[] keyNames = kpSplit[0].Split('.');
				if (keyNames[keyNames.Length-1] == "bin_offset")
				{
					string keyName = "";
					keyName = string.Join(".", keyNames);
					keyName = keyName.Substring(0, keyName.Length - 6);
					keyName += "value";
					keys.Add(keyName);
					data.Position = int.Parse(kpSplit[1]);
					values.Add(DataStream.ReadByte(data).ToString());
				}
				keys.Add(kpSplit[0]);
				values.Add(kpSplit[1]);
			 }
		}

		public void ToXml(XmlDocument doc)
		{
			XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
			XmlNode root = doc.CreateElement("a3da_file");
			doc.AppendChild(dec);
			doc.AppendChild(root);
			for (int i = 0; i < keys.Count; ++i)
			{
				List<string> keyNames = keys[i].Split('.').ToList();
				XmlNode parentNode = doc.CreateElement(keyNames[0]);
				List<XmlNode> childNodes = new List<XmlNode>();
				//Child node loop
				for (int x = 1; x < keyNames.Count; ++x)
				{
					//Checks if the name is a number and sets it as an ID if it is
					if (char.IsDigit(keyNames[x][0]))
					{
						XmlAttribute id = doc.CreateAttribute("id");
						id.Value = keyNames[x];
						if (childNodes.Count == 0)
						{
							parentNode.Attributes.Append(id);
						}else
						{
							childNodes[x - 1].Attributes.Append(id);
						}
						continue;
					}
					XmlNode childNode = doc.CreateElement(keyNames[x]);
					if (x == keyNames.Count - 1)
					{
						childNode.InnerText = values[i];
					}
					childNodes.Add(childNode);
				}
				//Append nodes
				for (int nC = childNodes.Count-1; nC >= 0; --nC)
				{
					if (nC == 0)
					{
						parentNode.AppendChild(childNodes[nC]);
					}
					else
					{
						childNodes[nC - 1].AppendChild(childNodes[nC]);
					}
				}
				XmlNode foundNode = doc.DocumentElement.SelectSingleNode(keyNames[0]);

				List<XmlNode> docNodes = new List<XmlNode>();
				foreach(XmlNode dN in doc.DocumentElement)
				{
					docNodes.Add(dN);
				}

				if (foundNode == null)
				{
					doc.DocumentElement.AppendChild(parentNode);
				} else 
				{
					//doc.DocumentElement.AppendChild(parentNode);
					List<string> attr = new List<string>();

					foreach (XmlNode node in docNodes)
					{
						Console.Write("Node name: " + node.Name + " Parent Name: " + parentNode.Name + "\n");
						if (node.Attributes.Count != 0 && node.Name == parentNode.Name)
						{
							attr.Add(node.Attributes[0].Value);
						}
					}
					docNodes.RemoveRange(0, attr.Count);

					Console.Write(" \nNew iter \n\n");

					if (parentNode.Attributes.Count > 0)
					{
						if (attr.Contains(parentNode.Attributes[0].Value))
						{
							foundNode.AppendChild(childNodes[0]);
						}else
						{
							doc.DocumentElement.AppendChild(parentNode);
						}
					}else
					{
						foundNode.AppendChild(childNodes[0]);
					}
				}
			}
		}
	}
}
