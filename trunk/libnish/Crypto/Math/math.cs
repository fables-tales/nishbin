// math.cs created with MonoDevelop
// User: sam at 20:08 01/02/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Mono.Math;
using System.Threading;


namespace libnish.Crypto
{
	
	public static class Math
	{
		/// <summary>
		/// Generates a random biginteger
		/// </summary>
		/// <param name="bits">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <returns>
		/// A <see cref="BigInteger"/>
		/// </returns>
        
        //TODO: entropy testing, seed blowing up
        public static BigInteger getRandom(int bits){
            
			return BigInteger.GenerateRandom(bits);
		}
		
		/// <summary>
		/// generates a prime, relatively good confidence factor
		/// </summary>
		/// <param name="bits">
		/// A <see cref="System.Int32"/>
		/// </param>
		/// <returns>
		/// A <see cref="BigInteger"/>
		/// </returns>
        
		public static BigInteger makePrime(int bits){
            //needs testing			
			bool passed = false;
			BigInteger pseudoprime = 0; 
			while (passed != true){
				pseudoprime = BigInteger.GeneratePseudoPrime(bits);
				passed = Mono.Math.Prime.PrimalityTests.RabinMillerTest(pseudoprime,Mono.Math.Prime.ConfidenceFactor.ExtraHigh);
			}
			return pseudoprime;
			
		}
	
		//might be useful later, don't remove
		public static BigInteger toitient(BigInteger n){
			BigInteger toit = 0;
			for(BigInteger i = 0;i<n;i+=1){
				if (n.GCD(i) == 1){
					toit += 1;
				}
			}
			return toit;
		}
        //mostly pointless, biginteger does this already
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
