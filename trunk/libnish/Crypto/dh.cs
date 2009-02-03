// dh.cs created with MonoDevelop
// User: sam at 18:52 26/01/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using GMP;
using Mono.Math;



namespace libnish.Crypto
{
	
	
	public class dh
	{
		
		//private Random Generator = new Random();
		public BigInteger a,g,p,b,k1,k2,key;
		
		public dh()
		{
			a = new BigInteger(0);
			g = new BigInteger(0);
			p = new BigInteger(0);
			k1 = new BigInteger(0);
			k2 = new BigInteger(0);
			key = new BigInteger(0);
		}
		//G and P are the variables that everyone knows.
		public void generateGP(){
			g = Math.math.makePrime(256);
			p = Math.math.makePrime(256);
		}
		
		//a is the private exponent of person a, k1 =  (g^a) mod p is calculated and sent to person b
		public void generateA(){
			BigInteger t = Math.math.makePrime(256);
			while ((g*a) < (t)){
				t = Math.math.makePrime(256);
				
			}
			a = t;
			
		}
		//this calculates k1
		public void computeK1(){
			k1 = g.ModPow(a,p);
		}
		//this recieves k2 from a remote user and sets it
		public void setk2(BigInteger B){
			k2 = B;
		}
		//this computes the shared key from (k2^a) mod p
		public void computeKey(){
			key = k2.ModPow(a,p);
		}
		
	}
}
