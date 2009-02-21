// uuid.cs created with MonoDevelop
// User: sam at 22:36 25/01/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace libnish.Crypto
{
	
	
	public static class UUID
	{
		
		 		
		public static string  getUUID(){
			string suuid;
			System.Guid t1 = System.Guid.NewGuid();
			System.Guid t2 = System.Guid.NewGuid();
			suuid = string.Format("%s-%s",t1.ToString(),t2.ToString());
			return suuid;
		}
		public static bool verifyuuid(string auuid){
			string[] split1;
			string[] split2;
			if (auuid.Length == getUUID().Length){
				split1 = auuid.Split('-');
				split2 = getUUID().Split('-');
				if (split1.Length == split2.Length){
					return true;
				}
			}
			return false;
		}
	}
}
