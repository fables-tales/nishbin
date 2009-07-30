// rsa.cs created with MonoDevelop
// User: sam at 21:25 12/02/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Collections.Generic;
using Mono.Math;


namespace libnish.Crypto
{
	
	//this probably shouldn't be used yet.
    //this is also highly incomplete
	public class RSAKeyPair
	{
		private BigInteger p,q,n,e,d;
		// rsa can't be used for both cryptography and signing
		// these booleans determine which one it is being used for
		private bool crypt = false;
		private bool sign = false;
		public RSAKeyPair()
		{
			System.Console.WriteLine("output");
			q = Math.makePrime(2048);
			System.Console.WriteLine("p done");
			p = Math.makePrime(2048);
			System.Console.WriteLine("q done");
			n = p*q;
			System.Console.WriteLine("n done");
			BigInteger store = (p-1)*(q-1);
			System.Console.WriteLine("store done");
			if (store.GCD(65537) == 1){
				e = 65537;
			} else {
				int i = 65539;
				while (store.GCD(i) != 1){
					i += 2;
				}
				e = i;
			}
			System.Console.WriteLine("e done");
			
			d = e.ModInverse(n);
			System.Console.WriteLine("d done");
			
		}
		public RSAKeyPair(BigInteger p, BigInteger q, BigInteger e, BigInteger n, BigInteger d){
			this.p = p;
			this.q = q;
			this.e = e;
			this.n = n;
			this.d = d;
		}
		public string[] exportcryptoblob(){
			List<string> Blob = new List<string>();
			
			foreach (BigInteger b in new BigInteger[] { p, q, e, n, d })
				Blob.Add(b.ToString());
			
			return Blob.ToArray();
		}
		public BigInteger encrypt(byte[] input){
			if (crypt == false && sign == false){
				crypt = true;
			}
			if (crypt == true){
				if (input.Length > 32){
					throw new InvalidOperationException("you can't use aes with more than 256 bits");
				} else {
					BigInteger buffer = new BigInteger(0);
					
					for (int i = (input.Length-1);i>=0;i--){
						buffer += input[i] << i*8;
					}
                    //wat?
					buffer = buffer << (8192 - 256);
					buffer += (Math.getRandom(8192) >> 256);
					buffer = buffer.ModPow(e,n);
					return buffer;
				}
					
				//hex decode this shit later?
				
			
			}
			else{
				throw new InvalidOperationException("you can't do encryption in signing mode");
			}
		}
		//this might be a bigint later
		public byte[] decrypt(BigInteger cryptochunks){
			if (crypt == false && sign == false){
				crypt = true;
			} 
			if (crypt){
				
				BigInteger result = cryptochunks.ModPow(d,n);
				result = result >> (8192-256);
				byte[] decrypt = new byte[32];
				int  counter = 0;
				while (result != 0){
					decrypt[counter] = (byte)(result % (1 << 8));
					result = result >> 8;
					counter += 1;
				}
				
				return decrypt;	
				
			} else {
				throw new InvalidOperationException("you can't do decryption in signing mode");
			}
		}
		
	}
	
	
}
