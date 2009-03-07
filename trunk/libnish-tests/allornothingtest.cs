// RSATest.cs created with MonoDevelop
// User: sam at 14:10 21/02/2009
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using libnish.Crypto;


namespace libnish_tests
{
	
	[TestFixture]
	public class allornothingtest
	{
		
		[Test]
        [Description("bees")]
		public void allornothing_a_barrel_roll(){
			libnish.Crypto.allornothingtransform trans = new allornothingtransform(new byte[] {14,12,41,23});
			trans.generatekeyforthismessage();
			trans.encrypt();
			trans.decrypt();
			Assert.AreEqual(14,trans.message[0][0]);
			
			
		}
		
	}
}
