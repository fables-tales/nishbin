// aes.cs created with MonoDevelop
// User: sam at 21:45 03/02/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Security.Cryptography;

using System.IO;

namespace libnish
{
	
	
	public class aes
	{
		private Rijndael handler;
		

        public aes()
        {
            handler = RijndaelManaged.Create();
            handler.GenerateIV();
            // When no key is provided, we generate one.
            handler.GenerateKey();   
			handler.Mode = CipherMode.CBC;
        }

		public aes(byte[] pkey)
		{
            handler = RijndaelManaged.Create();
            handler.GenerateIV();
            handler.Key = pkey;
			handler.Mode = CipherMode.CBC;
			
			
		}

		public byte[] decrypt(byte[] ciphertext)
        {
            // Use the provided byte[] buffer as a backing store to read out of
			MemoryStream t = new MemoryStream(ciphertext);
            // Stream automatically decrypts as we read data out of it.
			CryptoStream Decryptor = new CryptoStream(t, handler.CreateDecryptor(), CryptoStreamMode.Read);
            // Read data back out of the CryptoStream.
            byte[] plaintext = new byte[Decryptor.Length];
            Decryptor.Read(plaintext, 0, (int) Decryptor.Length); // if it doesn't fit in an int, its > 2gb anyway. deserves to crash...

			return plaintext;
		}

		public byte[] encrypt(byte[] plaintext)
        {
			MemoryStream t = new MemoryStream(plaintext);
			CryptoStream Encryptor = new CryptoStream(t, handler.CreateEncryptor(), CryptoStreamMode.Write);
            byte[] ciphertext = new byte[Encryptor.Length];
            Encryptor.Read(ciphertext, 0, (int) Encryptor.Length);

			return ciphertext;
		}
		
	}
}
