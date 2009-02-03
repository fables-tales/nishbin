// dh.cs created with MonoDevelop
// User: sam at 18:52 26/01/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using GMP;


namespace libnish.Crypto
{
	
	
	public class dh
	{
		
		//private Random Generator = new Random();
		public GMP.Integer g,p,a,k1,k2,key;
		
		public dh()
		{
			g = new GMP.Integer(0);
			p = new GMP.Integer(0);
			a = new GMP.Integer(0);
			k1 = new GMP.Integer(0);
			k2 = new GMP.Integer(0);
			key = new GMP.Integer(0);
		}
		//G and P are the variables that everyone knows.
		public void generateGP(){
			g = Math.math.randgmp(256);
			p = Math.math.randgmp(256);
		}
		
		//A is the private exponent of person a, (g^a) mod p is calculated
		public void generateA(){
			GMP.Integer t = new GMP.Integer(0);
			
		}
		
	}
}
