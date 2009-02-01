// uuid.cs created with MonoDevelop
// User: sam at 22:36 25/01/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace libnish.Crypto
{
	
	
	public class UUID
	{
		
		private string suuid; 		
		public UUID(){
			Random RandomGenerator = new System.Random();
			string seed1 = RandomGenerator.Next().ToString();
			string seed2 = RandomGenerator.Next().ToString();	
			System.Guid t1 = new Guid(seed1);
			System.Guid t2 = new Guid(seed2);
			suuid = string.Format("%s-%s",t1.ToString(),t2.ToString());
		}
		public string uuid{
			get { return suuid; }
		}
	}
}
