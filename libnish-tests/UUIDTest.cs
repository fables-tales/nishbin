using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using libnish;

namespace libnish_tests
{
    [TestFixture]
    public class UUIDTest
    {
        [Test]
        [Description("test for uuid")]
        public void simpleUUIDTest()
        {
			string s  = libnish.Crypto.UUID.getUUID();
			Assert.AreEqual(libnish.Crypto.UUID.verifyuuid(s),true);
        }

       

    }
}
