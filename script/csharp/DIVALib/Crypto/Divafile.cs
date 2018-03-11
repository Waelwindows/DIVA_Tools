using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BinarySerialization;
using DIVALib.IO;

namespace DIVALib.Crypto
{
    public class DivaFile
    {
        [FieldOrder(0), FieldLength(8)] public string Magic = MagicConstant;
        [FieldOrder(1)] public int LengthPayload;
        [FieldOrder(2)] public int LengthPlainText;
        [FieldOrder(3), FieldLength("LengthPayload")] public Stream EncryptedData;

        public const int HeaderSize = 16;
        public const int BlockSize = 16;
        public const string MagicConstant = "DIVAFILE";
        public const string EncryptionKey = "file access deny";

        public DivaFile()
        {
            
        }

        public DivaFile(Stream data, bool readFromFileBegin = true)
        {
            if (readFromFileBegin) data.Position = 0;
            LengthPlainText = (int)data.Length;
            LengthPayload = LengthPlainText;

            var key = EncryptionKey.Select(c => (byte)c).ToArray();
            using (var crypto = new AesManaged())
            {
                crypto.Key = key;
                crypto.IV = new byte[16];
                crypto.Mode = CipherMode.ECB;
                crypto.Padding = PaddingMode.Zeros;

                var encryptor = crypto.CreateEncryptor(crypto.Key, crypto.IV);
                EncryptedData = new MemoryStream();

                var byteData = new byte[data.Length];
                data.Read(byteData, 0, (int) data.Length);

                var encrypt = new byte[data.Length];

                using (var enData = new MemoryStream())
                {
                    using (var cryptoData = new CryptoStream(enData, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoData.Write(byteData, 0, (int) data.Length);
                        enData.Position = 0;
                        enData.Read(encrypt, 0, (int) data.Length);
                    }
                }
                EncryptedData = new MemoryStream(encrypt);
                Console.WriteLine("tst");
            }
        }

        public static bool IsValid(Stream stream, bool fileBegin = true)
        {
            var curPos = stream.Position;

            if (fileBegin) stream.Position = 0;
            if (stream.Length < HeaderSize) return false;

            var isValidMagic = DataStream.ReadChars(stream, 8).SequenceEqual(MagicConstant);
            if (!isValidMagic) return false;
            var payload = DataStream.ReadUInt32(stream);
            var plaintext = DataStream.ReadUInt32(stream);
            if (payload < plaintext || stream.Length < payload + HeaderSize) return false;

            stream.Position = curPos;

            return true;
        }

        public List<byte> DecryptBytes()
        {
            var key = EncryptionKey.Select(c => (byte)c).ToArray();
            var decrypted = new byte[LengthPayload];
            using (var crypto = new AesManaged())
            {
                crypto.Key = key;
                crypto.IV = new byte[16];
                crypto.Mode = CipherMode.ECB;
                crypto.Padding = PaddingMode.Zeros;

                var decryptor = crypto.CreateDecryptor(crypto.Key, crypto.IV);
                var serializer = new BinarySerializer();
                var byteArray = serializer.Deserialize<byte[]>(EncryptedData);
                //var byteArray = DataStream.ReadBytes(EncryptedData, LengthPayload);
                using (var encryptedData = new MemoryStream(byteArray))
                {
                    using (var cryptoData = new CryptoStream(encryptedData, decryptor, CryptoStreamMode.Read))
                    {
                        cryptoData.Read(decrypted, 0, LengthPayload);
                    }
                }
            }
            EncryptedData.Position = 0;
            return decrypted.ToList();
        }

        public Stream DecryptStream() => new MemoryStream(DecryptBytes().ToArray());
    }
}
