// aes.cs created with MonoDevelop
// User: sam at 21:45 03/02/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Security.Cryptography;

using System.IO;

namespace libnish.Crypto
{
	
	
	public class aes
	{
		private Rijndael handler;
        private ICryptoTransform dec;
        private ICryptoTransform enc;
		private int offset = 0;
        public aes()
        {
            handler = RijndaelManaged.Create();
			handler.Mode = CipherMode.CBC;
            handler.GenerateIV();
            // When no key is provided, we generate one.
            handler.GenerateKey();   
            dec = handler.CreateDecryptor();
            enc = handler.CreateEncryptor();
        }
        
		public aes(byte[] pkey,byte[] iv)
		{
			if (pkey.Length == 32 && iv.Length == 16){            
				handler = RijndaelManaged.Create();
				handler.Mode = CipherMode.CBC;
				handler.IV = iv;
			
				handler.Key = pkey;
				

				dec = handler.CreateDecryptor();
				enc = handler.CreateEncryptor();
			} else{
				throw new InvalidOperationException("key not long enough");
			}
			
		}

		public byte[] decrypt(byte[] ciphertext)
        {
			byte[] result = new byte[ciphertext.Length];
			
			dec.TransformBlock(ciphertext,0,ciphertext.Length+16,result,0);
			return result;
		}
        
		public byte[] encrypt(byte[] plaintext)
        {
			byte[] result = new byte[plaintext.Length];
			enc.TransformBlock(plaintext,0,plaintext.Length+16,result,0);
			return result;
		}

		public byte[] key(){
			return handler.Key;
		}
		public byte[] iv(){
			return handler.IV;
		}
		
	}
}
