// math.cs created with MonoDevelop
// User: sam at 20:08 01/02/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Mono.Math;
using System.Threading;


namespace libnish.Crypto.Math
{
	
	public static class math
	{
		
		public static BigInteger getRandom(int bits){
			return BigInteger.GenerateRandom(bits);
		}
		
		
		public static BigInteger makePrime(int bits){
			//TODO: make this more bullet proof
			return BigInteger.GeneratePseudoPrime(bits);
		}
	
		
		public static BigInteger toitient(BigInteger n){
			BigInteger toit = 0;
			for(BigInteger i = 0;i<n;i+=1){
				if (n.GCD(i) == 1){
					toit += 1;
				}
			}
			return toit;
		}
		public static BigInteger modinvsolve(BigInteger a,BigInteger b){
			BigInteger x = new BigInteger(0);
			BigInteger lastx = new BigInteger(1);
			BigInteger y = new BigInteger(0);
			BigInteger lasty = new BigInteger(1);
			BigInteger temp;
			BigInteger q;
			while (b != 0){
				temp = b;
				q = a / b;
				b = a % q;
				a = temp;
				
				temp = x;
				x = lastx-(q*x);
				lastx = temp;
				
				temp = y;
				y = lasty-(q*y);
				lasty = temp;
				
					
			}
			return lastx;
		}
	}
}
