// aes.cs created with MonoDevelop
// User: sam at 21:45 03/02/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Security.Cryptography;

namespace libnish
{
	
	
	public class aes
	{
		private Rijndael handler;
		private byte[] key;
		private byte[] IV;
		public aes(byte[] pkey)
		{
			handler = RijndaelManaged.Create();
			handler.GenerateIV();
			IV = handler.IV;
			if (pkey == null){
				handler.GenerateKey();
				key = handler.Key;
			} else{
				handler.Key = pkey;
				key = pkey;
			}
		}
		public byte[] decrypt(byte[] ciphertext){
			byte[] plaintext = null;
			System.IO.MemoryStream t = new System.IO.MemoryStream();
			t.Write(ciphertext,0,ciphertext.Length);
			t.Position = 0;
			handler.Key = key;
			handler.IV = IV;
			CryptoStream Decryptor = new CryptoStream(t,handler.CreateDecryptor(),CryptoStreamMode.Read);
			System.IO.BinaryReader getByte = new System.IO.BinaryReader(Decryptor);
			plaintext = getByte.ReadBytes(ciphertext.Length);
			return plaintext;
			
		}
		public byte[] encrypt(byte[] plaintext){
			byte[] ciphertext = null;
			System.IO.MemoryStream t = new System.IO.MemoryStream();
			t.Write(plaintext,0,plaintext.Length);
			t.Position = 0;
			handler.Key = key;
			handler.IV = IV;
			CryptoStream Encryptor = new CryptoStream(t,handler.CreateEncryptor(),CryptoStreamMode.Write);
			//System.IO.StreamWriter putByte = new System.IO.StreamWriter(Encryptor);
			System.IO.BinaryWriter putByte = new System.IO.BinaryWriter(Encryptor);
			putByte.Write(plaintext,0,plaintext.Length);
			t.Read(ciphertext,0,plaintext.Length);
			

			return ciphertext;
		}
		
	}
}
