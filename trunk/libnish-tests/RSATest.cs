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
	public class RSATest
	{
		
		[Test]
        [Description("Simple tests for Rsa")]
		public void SimpleRSA(){
			DateTime f = DateTime.Now;
			RSAKeyPair k = new RSAKeyPair();
			DateTime p = DateTime.Now;
			System.Console.WriteLine((p-f).TotalSeconds);
			Assert.That((p - f).Minutes <= 2);
			
		}
	}
}
