// allornothingtransform.cs created with MonoDevelop
// User: sam at 16:36 21/02/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace libnish.Crypto
{
	
	/// <summary>
	/// this ought to do all or nothing transformations, although security isn't information theoretic yet
	/// oooooooh, idea, instead of creating a massive key for a series of chunks, xor the stuff together
	/// does c# have symbolic rational number representation?
	/// </summary>
	public class AllOrNothingTransform
	{
		public List<byte[]> message = new List<byte[]>();
		public List<byte[]> chunks = new List<byte[]>();
		public List<byte[]> keychunks = new List<byte[]>();
		public byte[] key;
		public byte[] iv;
		private AES aes = new AES();

        // FIXME: Officialy define chunk size in protocol/allow changes
		/// <summary>
		/// this constructor is used when you have a plaintext file and want to encrypt stuff
		/// </summary>
		/// <param name="file">
		/// A <see cref="System.Byte"/>
		/// </param>
		public AllOrNothingTransform(byte[] file){
			this.message.Clear();
			int i = 0;
			for (i=0;i*1024<file.Length-1024;i++){
				byte[] copy = new byte[1024];
				Array.Copy(file,i*1024,copy,0,1024);
				message.Add(copy);
			}
            if (file.Length % 1024 != 0)
            {
                byte[] copy = new byte[1024];
                i = 0;
                for (i = 0; i < file.Length; i++)
                {
                    copy[i] = file[i];
                }
                while (i < 1024)
                {
                    copy[i] = 0;
                    i++;
                }
                message.Add(copy);
            }
            else
            {
                byte[] copy = new byte[1024];
                Array.Copy(file, ((file.Length) - 1024), copy, 0, 1024);
                message.Add(copy);
            }
			
			
		}
        /// <summary>
        /// This constructor is used when you have all the chunks and want to decrypt the file
        /// </summary>
        /// <param name="chunks">
        /// A <see cref="System.Collections.Generic.List`1"/>
        /// </param>
        /// <param name="keychunks">
        /// A <see cref="System.Collections.Generic.List`1"/>
        /// </param>
        /// <param name="iv">
        /// A <see cref="System.Byte"/>
        /// </param>
		public AllOrNothingTransform(System.Collections.Generic.List<byte[]> chunks,System.Collections.Generic.List<byte[]> keychunks,byte[] iv){
			this.chunks = chunks;
			this.keychunks = keychunks;
			this.iv = iv;
		}
		public void derivekey(){
			SHA256 hasher = new SHA256Managed();
			List<byte> toblob = new List<byte>();
			for (int i=0;i<this.keychunks.Count;i++){
				toblob.AddRange(this.keychunks[i]);
			}
			this.key = hasher.ComputeHash(toblob.ToArray());

		}
		public void generatekeyforthismessage(){
			for(int i=0;i<this.message.Count;i++){
				byte[] thischunk = new byte[32];
				thischunk = Math.getRandom(256).GetBytes();
				this.keychunks.Add(thischunk);
			}
			this.derivekey();
			this.iv = Math.getRandom(128).GetBytes();
		}
		public void Encrypt(){
			AES aes = new AES(this.key,this.iv);
			this.chunks.Clear();
			for(int i=0 ;i<this.message.Count;i++){
				this.chunks.Add(aes.Encrypt(this.message[i]));
			}			
			
		}
		public void Decrypt(){
			AES aes = new AES(this.key,this.iv);
			this.message.Clear();
			for(int i=0;i<this.chunks.Count;i++){
				this.message.Add(aes.Decrypt(this.chunks[i]));
			}
			
		}
		
		
	}
}
