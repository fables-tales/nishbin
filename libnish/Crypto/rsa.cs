// rsa.cs created with MonoDevelop
// User: sam at 21:25 12/02/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Mono.Math;


namespace libnish.Crypto
{
	
	
	public class rsa
	{
		private BigInteger p,q,n,e,d;
		public rsa()
		{
			p = Math.math.makePrime(4096);
			q = Math.math.makePrime(4096);
			n = p*q;
			BigInteger store = (p-1)*(q-1);
			if (store.GCD(65537) == 1){
				e = 65537;
			} else{
				int i = 65539;
				while (store.GCD(i) != 1){
					i += 2;
				}
				e = i;
			}
			// this next bit is fucking complicated
			// this is also the slowest possible method
			// for doing this
			// de = 1 mod store
			// d.e = - q.store = 1
			// solve using euler.
			d = Math.math.modinvsolve(e,store);
			
			
		}
		
	}
}
