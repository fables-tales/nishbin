// math.cs created with MonoDevelop
// User: sam at 20:08 01/02/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using GMP;

namespace libnish.Crypto.Math
{
	
	
	public static class math
	{
		
		public static GMP.Integer randgmp(int bits){
			Random Generator = new Random();
			GMP.Integer g = new GMP.Integer(0);
			byte[] buffer = new byte[bits/8];
			Generator.NextBytes(buffer);
			
			for(int i = 0;i<(bits/8);i++){
				g += buffer[i] << (i*8);
				
			}
			return g;
			
		}
		
	}
}
