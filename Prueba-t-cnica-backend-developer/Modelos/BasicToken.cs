using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prueba_t_cnica_backend_developer.Modelos
{
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    public class BasicToken
    {
        public string Vencimiento { get; set; }
        public string Usuario { get; set; }
        public string Key { get; set; }

        public string Serialize()
        {
            using (MemoryStream m = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(m))
                {
                    writer.Write(Vencimiento);
                    writer.Write(Usuario);
                    writer.Write(Key);
                }
                string sEncriptado = Encrypt(Convert.ToBase64String(m.ToArray()));
                return sEncriptado;
            }
        }

        public BasicToken Desserialize(string sEncriptado)
        {
            BasicToken result = new BasicToken();
            using (MemoryStream m = new MemoryStream(Convert.FromBase64String(Decrypt(sEncriptado))))
            {
                using (BinaryReader reader = new BinaryReader(m))
                {
                    result.Vencimiento = reader.ReadString();
                    result.Usuario = reader.ReadString();
                    result.Key = reader.ReadString();
                }
            }
            return result;
        }

        private static string Encrypt(string Text)
        {
            string EncryptionKey = "HeFu$";
            byte[] clearBytes = Encoding.Unicode.GetBytes(Text);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    Text = Convert.ToBase64String(ms.ToArray());
                }
                return Text;
            }
        }
        private static string Decrypt(string cipherText)
        {
            string EncryptionKey = "HeFu$";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

    }
}
