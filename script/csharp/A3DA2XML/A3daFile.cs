using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;
using BinarySerialization;
using DIVALib.IO;
using DIVALib.Math;

namespace A3DA2XML
{
    public class A3DaHeader
    {
        public enum EConverterVersion
        {
            DreamyTheater2 = 20050823,
            F = 20111019,
        }

        public enum EPropertyVersion
        {
            DreamyTheater2 = 20050706,
            F = 20110526,
        }

        public const string ConversionDateFormat = @"ddd MMM dd H:mm:ss yyyy";
        public const string KeyPairDateFormat = @"yyyymmdd";

        [FieldOrder(0)] public string Magic;
        [FieldOrder(1)] public byte[] Reserved;
        [FieldOrder(2)] public DateTime ConvertsionDate;

        private A3DaKeyPairString _converterVersion;
        private A3DaKeyPairString _fileName;
        private A3DaKeyPairString _propertyVersion;

        [FieldOrder(3)] public DateTime ConverterVersion
        {
            get => DateTime.ParseExact(_converterVersion.Pair, KeyPairDateFormat, CultureInfo.InvariantCulture);
            set => _converterVersion.Pair = value.ToString();
        }
        [FieldOrder(4)] public string FileName
        {
            get => _fileName.Pair;
            set => _fileName.Pair = value;
        }
        [FieldOrder(5)] public DateTime PropertyVersion
        {
            get => DateTime.ParseExact(_propertyVersion.Pair, KeyPairDateFormat, CultureInfo.InvariantCulture);
            set => _propertyVersion.Pair = value.ToString();
        }

        public A3DaHeader()
        {
            Reserved = null;
            ConvertsionDate = DateTime.Now;
            _converterVersion = new A3DaKeyPairString("_.converter.version", ((int)EConverterVersion.DreamyTheater2).ToString());
            _fileName = new A3DaKeyPairString("_.file_name", "empty.filename");
            _propertyVersion = new A3DaKeyPairString("_.converter.version", ConverterVersion.ToString(KeyPairDateFormat));

        }

        public void Serialize(Stream stream)
        {
            DataStream.WriteCString(stream, Magic, 0xA);
            DataStream.WriteCString(stream, ConvertsionDate.ToString(ConversionDateFormat), 0xA);
            DataStream.WriteCString(stream, _converterVersion.ToString(), 0xA);
            DataStream.WriteCString(stream, _fileName.ToString(), 0xA);
            DataStream.WriteCString(stream, _propertyVersion.ToString(), 0xA);
        }

        public void Deserialize(Stream stream)
        {
            Magic = DataStream.ReadCString(stream, 0xA);
            ConvertsionDate = DateTime.ParseExact(DataStream.ReadCString(stream, 0xA).Substring(1), ConversionDateFormat, CultureInfo.InvariantCulture);
            _converterVersion = new A3DaKeyPairString(DataStream.ReadCString(stream, 0xA).Split('='));
            FileName = DataStream.ReadCString(stream, 0xA).Split('=')[1];
            _propertyVersion = new A3DaKeyPairString(DataStream.ReadCString(stream, 0xA).Split('='));
        }

        public override string ToString() => $"{FileName}: Converted at {ConvertsionDate}, version {ConverterVersion.ToShortDateString()}";
    }
    /*
    public class Dta3DaHeader : A3DaHeader
    {
        public Dta3DaHeader(string name = "no_name.bin")
        {
            ConverterVersion = EConverterVersion.DreamyTheater2;
            FileName = name;
            PropertyVersion = EPropertyVersion.DreamyTheater2;
        }
    }

    public class Fa3DaHeader : A3DaHeader
    {
        public Fa3DaHeader(string name = "no_name.bin")
        {
            Magic = "#A3DC__________";
            
            ConverterVersion = EConverterVersion.F;
            FileName = name;
            PropertyVersion = EPropertyVersion.F;
        }
    }
    */
    [XmlInclude(typeof(A3DaKeyPair))]
    [XmlInclude(typeof(A3DaKeyPair<>))]
    [XmlInclude(typeof(A3DaKeyPairString))]
    [XmlInclude(typeof(A3DaKeyPairNested))]
    public class A3DaFile : IBinarySerializable
    {
        public const int Filestart = 0x85;
        [FieldOrder(0)] public A3DaHeader Header;
        [FieldOrder(1)] public List<A3DaKeyPair> Keypairs;

        public void Serialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
        {
            var serializer = new BinarySerializer();
            serializer.Serialize(stream, Header);
            foreach (var a3DaKeyPair in Keypairs)
            {
                DataStream.WriteCString(stream, a3DaKeyPair.ToString(), 0xA);
            }
        }

        public void Deserialize(Stream stream, Endianness endianness, BinarySerializationContext serializationContext)
        {
            stream.Position = Filestart;
            var lines = DataStream.ReadString(stream, (int)(stream.Length - Filestart)).Split('\n');
            Keypairs = new List<A3DaKeyPair>();
            foreach (var line in lines)
            {
                Keypairs.Add(A3DaKeyPair.Parse(line));
            }
        }
    }

    public class A3DaKeyPair
    {
        public string Key;
        public object Pair;

        public A3DaKeyPair LastElement;

        public A3DaKeyPair() => Key = "empty_key";

        public A3DaKeyPair(string key) => Key = key;

        public override string ToString() => $"{Key} = {Pair}";

        public static A3DaKeyPair Parse(string line)
        {

            var keypair = line.Split('=');
            //Console.WriteLine(keypair[0]);
            var currnetKeyPair = new A3DaKeyPair();
            if (keypair.Length > 1)
            {
                var tree = keypair[0].Split('.');
                for (var i = tree.Length - 1; i >= 0; --i)
                    if (i == tree.Length - 1)
                    {
                        currnetKeyPair = new A3DaKeyPairString(tree[i - 1], keypair[1]);
                        if (keypair[1].Contains("("))
                        {
                            var componentCount = keypair[1].Split(',').Length;
                            switch (componentCount)
                            {
                                case 2:
                                    currnetKeyPair =
                                        new A3DaKeyPair<Vector2>(tree[i - 1], Vector2.Parse(keypair[1]));
                                    break;
                                case 3:
                                    currnetKeyPair =
                                        new A3DaKeyPair<Vector3>(tree[i - 1], Vector3.Parse(keypair[1]));
                                    break;
                            }
                        }
                    }
                    else if (i != 0)
                    {
                        currnetKeyPair = new A3DaKeyPairNested(tree[i - 1], currnetKeyPair);
                    }
            }
            return currnetKeyPair;
        }
    }

    public class A3DaKeyPair<T> : A3DaKeyPair
    {
        public new T Pair;

        public A3DaKeyPair() {}

        public A3DaKeyPair(string key, T pair) : base(key) => Pair = pair;

        public override string ToString() => $"{Key} = {Pair}";
    }

    public class A3DaKeyPairString : A3DaKeyPair
    {
        public new string Pair;

        public A3DaKeyPairString() {}

        public A3DaKeyPairString(string keyPair)
        {
            var stringSplit = keyPair.Split('=');
            Key = stringSplit[0];
            Pair = stringSplit[1];
        }

        public A3DaKeyPairString(string key, string pair) : base(key) => Pair = pair;

        public A3DaKeyPairString(IList<string> keyPair)
        {
            Key = keyPair[0];
            Pair = keyPair[1];
        }

        public override string ToString() => $"{Key} = {Pair}";
    }

    public class A3DaKeyPairNested : A3DaKeyPair
    {
        public A3DaKeyPair Nest;

        public new A3DaKeyPair LastElement
        {
            get
            {
                A3DaKeyPairNested currentNest = Nest as A3DaKeyPairNested;
                A3DaKeyPair keyPair;
                while (true)
                {
                    try
                    {
                        if ((A3DaKeyPairNested)currentNest.Nest != null)
                            currentNest = (A3DaKeyPairNested)currentNest.Nest;
                    }
                    catch (InvalidCastException)
                    {
                        keyPair = currentNest.Nest;
                        break;
                    }
                }
                return keyPair;
            }
            set {
                A3DaKeyPairNested currentNest = Nest as A3DaKeyPairNested;
                A3DaKeyPair keyPair;
                while (true)
                {
                    try
                    {
                        if ((A3DaKeyPairNested)currentNest.Nest != null)
                            currentNest = (A3DaKeyPairNested)currentNest.Nest;
                    }
                    catch (InvalidCastException)
                    {
                        keyPair = currentNest.Nest;
                        break;
                    }
                }
                keyPair = value;
            }
        }

        public A3DaKeyPairNested()
        {
        }

        public A3DaKeyPairNested(string key, A3DaKeyPair nest) : base(key)
        {
            Nest = nest;
        }

        public override string ToString()
        {
            return $"{Key}.{Nest}";
        }
    }
}