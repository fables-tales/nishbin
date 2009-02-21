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
        private ICryptoTransform dec;
        private ICryptoTransform enc;

        public aes()
        {
            handler = RijndaelManaged.Create();
            handler.GenerateIV();
            // When no key is provided, we generate one.
            handler.GenerateKey();   
			handler.Mode = CipherMode.CBC;

            dec = handler.CreateDecryptor();
            enc = handler.CreateEncryptor();
        }
        
		public aes(byte[] pkey)
		{
			if (pkey.Length == 32){            
				handler = RijndaelManaged.Create();
				handler.GenerateIV();
			
				handler.Key = pkey;
				handler.Mode = CipherMode.CBC;

				dec = handler.CreateDecryptor();
				enc = handler.CreateEncryptor();
			} else{
				throw new InvalidOperationException("key not long enough");
			}
			
		}

		public byte[] decrypt(byte[] ciphertext)
        {

            // Use the provided byte[] buffer as a backing store to read out of
			MemoryStream t = new MemoryStream(ciphertext);
            // Stream automatically decrypts as we read data out of it.
			CryptoStream Decryptor = new CryptoStream(t, dec, CryptoStreamMode.Read);
            // Read data back out of the CryptoStream.
            byte[] plaintext = new byte[Decryptor.Length];
            Decryptor.Read(plaintext, 0, (int) Decryptor.Length); // if it doesn't fit in an int, its > 2gb anyway. deserves to crash...

			return plaintext;
		}
        
		public byte[] encrypt(byte[] plaintext)
        {
			MemoryStream t = new MemoryStream(plaintext);
			CryptoStream Encryptor = new CryptoStream(t, enc, CryptoStreamMode.Read);
            byte[] ciphertext = new byte[Encryptor.Length];
            Encryptor.Read(ciphertext, 0, (int) Encryptor.Length);

			return ciphertext;
		}
		
	}
}
