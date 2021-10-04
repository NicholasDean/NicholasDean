using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;



public static class Helper
{

    private static string hash = "C8DE445!!";


    /* Serialise and Deserialise saved data*/
    public static string Serialize<T>(this T toSerialize)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T)); //This process is used to covnert objects into a form that can be readily transported
        StringWriter writer = new StringWriter();
        serializer.Serialize(writer, toSerialize);
        return writer.ToString();
    }

    public static T Deserialize<T>(this string toDeserialize)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        StringReader reader = new StringReader(toDeserialize);
        return (T)serializer.Deserialize(reader);
    }

    
    /* Encyption and Decryption of the users save data */
    public static string Encrypt(string input) //Data Encryption for the saving game
    {
        byte[] utf = UTF8Encoding.UTF8.GetBytes(input); //Unicode Transformation Format -8bit
        //This encoder can translate any Unicode char, and match it with a unique binary string
        //Turning code points into a UTF-8 binary encoder, which is made up of 1 byte

        using (MD5CryptoServiceProvider crypto = new MD5CryptoServiceProvider()) { //Computes an MD5 hash value for input data
            byte[] key = crypto.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash)); //Computes the hash through UTF8 encoding using the given hash variable above.
            using (TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider()
            {
                Key = key,
                Mode = CipherMode.ECB, //Electronic Codebook encrypts each block individually 
                Padding = PaddingMode.PKCS7 //A cryptographic Message Syntax, allows encrypted data to be stored.
            })
            {
                ICryptoTransform transform = DES.CreateEncryptor();
                byte[] results = transform.TransformFinalBlock(utf, 0, utf.Length);
                return Convert.ToBase64String(results, 0, results.Length);
            }  
        }
    }

    
    public static string Decrypt(string input) //Data Decryption for the loading game
    {
        byte[] utf = Convert.FromBase64String(input);

        using (MD5CryptoServiceProvider crypto = new MD5CryptoServiceProvider())
        {
            byte[] key = crypto.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
            using (TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider()
            {
                Key = key,
                Mode = CipherMode.ECB, //Electronic Codebook encrypts each block individually 
                Padding = PaddingMode.PKCS7
            })
            {
                ICryptoTransform transform = DES.CreateDecryptor();
                byte[] results = transform.TransformFinalBlock(utf, 0, utf.Length);
                return UTF8Encoding.UTF8.GetString(results);
            }
        }
    }
}
