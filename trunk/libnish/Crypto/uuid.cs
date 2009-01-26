// uuid.cs created with MonoDevelop
// User: sam at 22:36 25/01/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace libnish.Crypto
{
	
	
	public class uuid
	{
		
		public string GenerateUUID(){
			
			System.Guid t1 = new Guid();
			System.Guid t2 = new Guid();
			return t1.ToString() + "-" + t2.ToString();
		}
	}
}
