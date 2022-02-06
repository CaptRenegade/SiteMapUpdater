using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

public class AESEncryption
{
    public string Encrypt(string clearText)
    {
        string EncryptionKey = "LDKJasHJDKKASKash349023akjanJKASFHakj897";
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x43, 0x61, 0x70, 0x74, 0x52, 0x65, 0x6E, 0x65, 0x67, 0x61, 0x64, 0x65, 0x20, 0x46, 0x6F, 0x72, 0x65, 0x76, 0x65, 0x72, 0x21 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }

    public string Decrypt(string cipherText)
    {
        string EncryptionKey = "LDKJasHJDKKASKash349023akjanJKASFHakj897";
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x43, 0x61, 0x70, 0x74, 0x52, 0x65, 0x6E, 0x65, 0x67, 0x61, 0x64, 0x65, 0x20, 0x46, 0x6F, 0x72, 0x65, 0x76, 0x65, 0x72, 0x21 });
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
