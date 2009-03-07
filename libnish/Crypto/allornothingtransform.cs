// allornothingtransform.cs created with MonoDevelop
// User: sam at 16:36 21/02/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;

namespace libnish.Crypto
{
	
	
	public class allornothingtransform
	{
		public System.Collections.Generic.List<byte[]> message = new List<byte[]>();
		public System.Collections.Generic.List<byte[]> chunks = new List<byte[]>();
		public byte[] key;
		public byte[] iv;
		
		public allornothingtransform(byte[] file){
			this.message.Clear();
			int i = 0;
			for (i=0;i*1024<file.Length-1024;i++){
				byte[] copy = new byte[1024]();
				Array.Copy(file,i*1024,copy,0,1024);
				message.Add(copy);
			}
			if (file.Length % 1024 != 0){
				byte[] copy = new byte[1024]();
				i=0;
				for(i=0;i<file.Length;i++){
					copy[i] = file[i]; 
				}
				while (i < 1024){
					copy[i] = 0;
					i++;
				}
				message.Add(copy);
			}
			
			
		}
		
	}
}
