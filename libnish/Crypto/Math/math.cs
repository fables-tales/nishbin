// math.cs created with MonoDevelop
// User: sam at 20:08 01/02/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Mono.Math;


namespace libnish.Crypto.Math
{
	
	
	public static class math
	{
		
		public static BigInteger getRandom(int bits){
			return BigInteger.GenerateRandom(bits);
		}
		public static BigInteger makePrime(int bits){
			return BigInteger.GeneratePseudoPrime(bits);
		}
		
	}
}
