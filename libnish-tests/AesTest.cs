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

            Console.Write("PKEY:");
            foreach (byte b in pkey)
                Console.Write(b + ",");
            Console.Write("\nIV:");
            foreach (byte c in iv)
                Console.Write(c + ",");
            Console.WriteLine();

			aes aesobj = new aes(pkey, iv);
			byte[] dave = new byte[1024*1024*256];
            new Random().NextBytes(dave);
            Console.WriteLine("Dave rng finished.");
			byte[] enc = aesobj.encrypt(dave);
            Console.WriteLine("Enc's done, yo.");
			//Console.WriteLine(dave.Length);
			//Console.WriteLine(enc.Length);
			
			byte[] dec = aesobj.decrypt(enc);
            Console.WriteLine("!!decol");
			//for (int i =0;i<dave.Length;i++){
				//Console.WriteLine("Enc: " + enc[i] + " Dec: " + dec[i] + " Dave: " + dave[i]);
			//}
            //for (int i = dave.Length; i < dec.Length; i++)
            //{
               // Console.WriteLine("Enc: " + enc[i] + " Dec: " + dec[i]);
            //}
			//Console.WriteLine(dec[0]);
			//Console.WriteLine(dave[0]);
			//Assert.AreEqual(dave,dec);
			
		}
	}
}
