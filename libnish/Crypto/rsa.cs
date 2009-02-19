// rsa.cs created with MonoDevelop
// User: sam at 21:25 12/02/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Mono.Math;


namespace libnish.Crypto
{
	
	
	public class RSAKeyPair
	{
		private BigInteger p,q,n,e,d;
		// rsa can't be used for both cryptography and signing
		// these booleans determine which one it is being used for
		private bool crypt = false;
		private bool sign = false;
		public RSAKeyPair()
		{
			p = Math.math.makePrime(4096);
			q = Math.math.makePrime(4096);
			n = p*q;
			BigInteger store = (p-1)*(q-1);
			if (store.GCD(65537) == 1){
				e = 65537;
				
			} else {
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
		public BigInteger[] encrypt(byte[] input){
			if (crypt == false && sign == false){
				crypt = true;
			}
			if (crypt == true){
				BigInteger chunk,encchunk;
				//i'll bet my bottom dollar that no message is larger than 8192*n
				BigInteger[] chunks = new BigInteger[8192];
				
				BigInteger buffer = new BigInteger(0);
				int index = input.Length;
				int shift;
				int count = 0;
				while (index != 0){
					buffer = 0;
					shift = 0;
					while (buffer < n){
						buffer += input[index] << (8*shift);
						index -= 1;
						shift += 1;
					}
					chunk = buffer;
					encchunk = chunk.ModPow(e,n);
					chunks[count] = encchunk;
					count += 1;
				}
				//hex decode this shit later?
				return chunks;
			
			}
			else{
				throw new InvalidOperationException("you can't do encryption in signing mode");
			}
		}
		//this might be a bigint later
		public byte[] message(BigInteger[] cryptochunks){
			return null;
		}
		
	}
	
	
}
