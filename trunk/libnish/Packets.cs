using System;
using System.Collections.Generic;

using System.Text;

namespace libnish
{
    public class Packet
    {
        byte[] Content;

        public byte[] ToUnencryptedByteArray()
        {
            return Content;
        }
    }
}
