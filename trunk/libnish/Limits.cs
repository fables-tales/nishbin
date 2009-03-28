using System;

namespace libnish
{
    public struct Limits
    {
        public int PushPacketSizeHardLimit;
        public int MsBetweenConnectionAttempts;

        //
        // More to come!
        //

        public static Limits Default
        {
            get
            {
                Limits l = new Limits();

                l.PushPacketSizeHardLimit = 256 * 1024; // 256kb.. and even that's way too big!
                l.MsBetweenConnectionAttempts = 5000; // Note this means 5 secs after all connection attempts have succeeded/failed.

                return l;
            }
        }

    }

}
