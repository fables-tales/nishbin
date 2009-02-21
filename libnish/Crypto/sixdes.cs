// sixdes.cs created with MonoDevelop
// User: sam at 21:32 03/02/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Security.Cryptography;
namespace libnish.Crypto
{
	
	
	public class sixdes
	{
		public System.Security.Cryptography.Rijndael
		public sixdes(byte[] key)
		{
			handler = TripleDESCryptoServiceProvider.Create();
			if (key==null){
				handler.GenerateKey();
			}
			else{
				handler.Key = key;
			}
		}
	}
}
