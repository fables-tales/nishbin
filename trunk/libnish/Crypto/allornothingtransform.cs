// allornothingtransform.cs created with MonoDevelop
// User: sam at 16:36 21/02/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;


namespace libnish.Crypto
{
	
	
	public class allornothingtransform
	{
		public byte[] message;
		public byte[,] chunks;
		public byte[] key;
		public byte[] iv;
		private byte[] hashkey;
		
		public allornothingtransform(byte[] message){
			if (message.Length % 1024 != 0){
				byte[] newmessage = new byte[((message.Length/1024)+1)*1024];
				message.CopyTo(newmessage,0);
				for (int i=message.Length;i<newmessage.Length;i++){
					newmessage[i] = 0;
				
				}
				this.message = newmessage;
			} else {
				this.message = message;
			}
			int index = 0;
			int counter = 0;
			
			chunks = new byte[1024,this.message.Length/1024];
			for (index=0;index<1024;index++){
				for (counter=0;counter<this.message.Length/1024;index++){
					chunks[index,counter] = this.message[index*1024+counter];
				}
			}
			this.generateKeyForThis();
			
		}
		public allornothingtransform(byte[,] chunks, byte[] key,byte[] iv){
			if (chunks.GetLength(1) == 1024 && chunks.GetLength(0) > 0){
				this.chunks = chunks;
				this.iv = iv;
				this.key = key;
			}
		}
		public void generateKeyForThis(){
			this.key = new byte[this.chunks.GetLength(0)*256];
			this.iv = new byte[this.chunks.GetLength(0)*256];
			for(int i = 0;i<this.key.Length;i++){
				key[i] = Math.math.getRandom(8).GetBytes()[0];
				iv[i] = Math.math.getRandom(8).GetBytes()[0];
			}
		}
		public void assembleKey(){
			System.Security.Cryptography.SHA256 hasher = new System.Security.Cryptography.SHA256Managed();
			this.hashkey = hasher.ComputeHash(this.key);
		}
		
		public byte[,] decrypt(){
			byte[,] buffer = new byte[this.chunks.GetLength(0),this.chunks.GetLength(1)];
			aes aeshandler = new aes(this.key,this.iv);
			
			for(int i=0;i<buffer.GetLength(0);i++){
					buffer[i] = aeshandler.decrypt(this.chunks[i]);
			}
			return buffer;
		}
		public byte[,] encrypt(){
			
			byte[,] buffer = new byte[this.chunks.GetLength(0),this.chunks.GetLength(1)];
			aes aeshandler = new aes(this.key,this.iv);
			
			for(int i=0;i<buffer.GetLength(0);i++){
					buffer[i] = aeshandler.encrypt(this.chunks[i]);
			}
			return buffer;
		}
		
		
	}
}
