// AesTest.cs created with MonoDevelop
// User: sam at 20:40 04/02/2009
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
	public class AesTest
	{
		[Test]
		[Description("Test encryption")]
		public void EncTest(){
            byte[] pkey = new byte[32];
            byte[] iv = new byte[16];
            new Random().NextBytes(pkey);
            new Random().NextBytes(iv);
			aes aesobj = new aes(pkey, iv);
			byte[] dave = new byte[1024*1024];
            new Random().NextBytes(dave);
			byte[] enc = aesobj.encrypt(dave);
			//Console.WriteLine(dave.Length);
			//Console.WriteLine(enc.Length);
			byte[] dec = aesobj.decrypt(enc);
			Assert.AreEqual(dave,dec);
		}
	}
}
