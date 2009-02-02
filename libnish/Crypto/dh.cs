// dh.cs created with MonoDevelop
// User: sam at 18:52 26/01/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using GMP;

namespace libnish
{
	
	
	public class dh
	{
		
		private Random Generator = new Random();
		public GMP.Integer g,p,a,k1,k2,key;
		public dh()
		{
			g = 0;
			p = 0;
			a = 0;
			k1 = 0;
			k2 = 0;
			key = 0;
		}
		
	}
}
