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
            byte[] input = new byte[1024];
            new Random().NextBytes(input);
			libnish.Crypto.allornothingtransform trans = new allornothingtransform(input);
            Console.WriteLine("BEFORE:");
            foreach (byte b in input)
            {
                Console.Write(b + ", ");
            }
			trans.generatekeyforthismessage();
			trans.encrypt();
			trans.decrypt();
            Console.WriteLine("\ntrans.message contains " + trans.message.Count + " byte[] arrays.");
            Console.WriteLine("AFTER:");
            foreach (byte c in trans.message[0])
                Console.Write(c + ", ");
            Console.WriteLine();
            Console.WriteLine("Before length: " + input.Length.ToString());
            Console.WriteLine("After length: " + trans.message[0].Length.ToString());
			Assert.AreEqual(input, trans.message[0]);
			//Assert.Greater(trans.chunks.Count,1);
            Assert.AreEqual(1, trans.chunks.Count);
			
			
			
		}
		
	}
}
