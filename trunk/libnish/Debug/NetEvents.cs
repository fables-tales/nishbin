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
        PeerConnectFail

    }

    public struct NetEvent
    {
        public NetEventType Type;
        public object[] Data;
        public string Description;
        public object RaisedBy;
    }
    //what the what?  drag and drop is not nice :( anyway. all the netevent stuff no longer exists in the checking funcs
    // because we rewrote them in peermanager.cs, soo.... i'll add some of that back in i reckon.
    // or leave it out till debugging becomes too hard?
    // it's only about 6 lines of code.
    // are they pretty lines of code?
    // yes. :)
    // ok
    
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
        }
    }
}
