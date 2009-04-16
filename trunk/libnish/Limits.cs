using System;

namespace libnish
{
    /// <summary>
    /// hardlimits for the protocol, anything breaching these will be rejected automatically
    /// all clients that do not use libnish, should use these limits as libnish based clients
    /// will automatically disconnect and blacklist stupid people who breach these
    /// although timed limits are obviously less enforceable
    /// </summary>    
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
