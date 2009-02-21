using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using libnish;

// fix this
namespace libnish_tests
{
    [TestFixture]
    public class PacketsTest
    {
        [Test]
        [Description("Simple tests for MetaNotifyPacket.")]
        public void MetaNotifyPacketSimpleTest()
        {
            MetaNotifyPacket mnp1 = new MetaNotifyPacket(libnish.Crypto.UUID.getUUID());

            Assert.That(mnp1.ContainingUUID == libnish.Crypto.UUID.getUUID());
            Assert.That(mnp1.Type == PacketType.MetaNotify);
            
            byte[] ExpectedUneArray = System.Text.Encoding.ASCII.GetBytes("META " + libnish.Crypto.UUID.getUUID());
            byte[] ActualUneArray = mnp1.ToUnencryptedByteArray();

            Assert.That(ActualUneArray.Length == ExpectedUneArray.Length);

            for (int i = 0; i < ExpectedUneArray.Length; i++)
                Assert.AreEqual(ExpectedUneArray[i], ActualUneArray[i]);
        }

        [Test]
        [Description("Simple tests for MetaNotifyPacket.")]
        public void MetaNotifyPacketSimpleTest2()
        {
            MetaNotifyPacket mnp1 = new MetaNotifyPacket(libnish.Crypto.UUID.getUUID());
            
            mnp1.ContainingUUID = libnish.Crypto.UUID.getUUID();

            Assert.That(mnp1.ContainingUUID == libnish.Crypto.UUID.getUUID());
            Assert.That(mnp1.Type == PacketType.MetaNotify);

            byte[] ExpectedUneArray = System.Text.Encoding.ASCII.GetBytes("META F47AC10B-58CC-4372-A567-0E02B2C3D479");
            byte[] ActualUneArray = mnp1.ToUnencryptedByteArray();

            Assert.That(ActualUneArray.Length == ExpectedUneArray.Length);

            for (int i = 0; i < ExpectedUneArray.Length; i++)
                Assert.AreEqual(ExpectedUneArray[i], ActualUneArray[i]);
        }



    }
}
