using System;
using System.Collections.Generic;
using System.IO;
using ProtoBuf;

namespace ProtobuffTest
{
    [ProtoContract]
    class BuffTest
    {
        [ProtoMember(1)] public string AuthorName;
        [ProtoMember(2)] public string SongName;
        [ProtoMember(3)] public int SongLength;
    }
    class Program
    {
        static void Main(string[] args)
        {
            var sng1 = new BuffTest() {AuthorName = "Pinocchio-P", SongName = "Common World Domination", SongLength = 3};
            var sng2 = new BuffTest() {AuthorName = "wowka", SongName = "Unknown Mothergoose", SongLength = 3};

            /*
            using (var file = File.Create("john.dat"))
            {
                Serializer.Serialize(file, sng1);
                Serializer.Serialize(file, sng2);
            }
            */

            var songs = new List<BuffTest>();
            using (var file = File.Open("john.dat", FileMode.Open))
            {
                file.Position = 0;
                while (true)
                {
                    if (file.ReadByte() <= 0) break;
                    file.Position -= 1;
                    songs.Add(Serializer.Deserialize<BuffTest>(file));
                }
            }
            Console.Read();
        }
    }
}
