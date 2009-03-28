using System;
using System.Collections.Generic;
using System.Text;

namespace libnish
{
    public enum NetEventType
    {
        FoundPeers,
        FoundPeerFailure,
        BannedPeer,
        PeerConnectFail,
        NewPeer,
        RejectedPeerBecauseFull
    }

    public struct NetEvent
    {
        public NetEventType Type;
        public object[] Data;
        public string Description;
        public object RaisedBy;
    }

    public static class NetEvents
    {
        public static List<NetEvent> Events = new List<NetEvent>();

        public static void Add(NetEventType Type, string Description, object[] Data, object RaisedBy)
        {
            NetEvent ne = new NetEvent();
            ne.Type = Type;
            ne.Description = Description;
            ne.Data = Data;
            ne.RaisedBy = RaisedBy;

            lock (Events)
                Events.Add(ne);

            // Debug only!
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(Description);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
