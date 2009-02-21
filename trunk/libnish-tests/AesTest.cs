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
			aes aesobj = new aes();
			byte[] dave = new byte[32];
			for (int i = 0;i<32;i++){
				dave[i] = 0;
			}
			dave[0] = 32;
			dave[1] = 127;
			byte[] dave2 = new byte[32];
			for (int i = 0;i<32;i++){
				dave2[i] = 0;
			}
			dave2[0] = 12;
			dave2[1] = 47;
			
			byte[] enc = aesobj.encrypt(dave);
			byte[] enc2 = aesobj.encrypt(dave2);
			Console.WriteLine(dave.Length);
			Console.WriteLine(enc.Length);
			byte[] dec = aesobj.decrypt(enc);
			Console.WriteLine(dec);
			Assert.AreEqual(dave,dec);
			
		}
	}
}
