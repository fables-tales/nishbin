// dh.cs created with MonoDevelop
// User: sam at 18:52 26/01/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using Mono.Math;



namespace libnish.Crypto
{
	
	/*
	 * The actual dh protocol:
	 * person a: generate g and p, send to person b
	 * person b: accept g and p and respond with an acknowledgement (usually a hash of a + b)
	 * person a: generate a, compute k1 = (g^a) mod p
	 * person b: generate b, compute k2 = (g^b) mod p
	 * preson a: send k1 to person b
	 * person b: send k2 to person a
	 * person a: compute key = (k2^a) mod p
	 * person b: compute key = (k1^b) mod p
	 * 
	 * due to the laws of exponentiation, (k2^a) mod p == (k1^b) mod p  
	 * */
	
	public class dh
	{
		
		//private Random Generator = new Random();
		public BigInteger a,g,p,b,k1,k2,key;
		
		public dh()
		{
			a = null;
			g = null;
			p = null;
			k1 = null;
			k2 = null;
			key = null;
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
