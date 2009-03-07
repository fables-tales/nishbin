using System;

namespace libnish
{
    public struct Limits
    {
        public int PushPacketSizeHardLimit;

        //
        // More to come!
        //

        public Limits Default
        {
            get
            {
                Limits l = new Limits();

                l.PushPacketSizeHardLimit = 256 * 1024; // 256kb.. and even that's way too big!

                return l;
            }
        }

    }

}