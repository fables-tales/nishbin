using System;
using System.Collections.Generic;

using System.Text;
using libnish.PeerFinders;
using System.Threading;

namespace libnish
{
    public static class PeerManager
    {
        static List<Peer> Peers = new List<Peer>();
        static List<PeerFinder> PeerFinders = new List<PeerFinder>();

        static Thread P2PThread;

        public static void Initialise(Peer[] InitialPeers, PeerFinder[] InitialPeerFinders)
        {
            Peers.AddRange(InitialPeers);
            PeerFinders.AddRange(InitialPeerFinders);
            
            //P2PThread = new Thread(new ThreadStart(
        }
    }
}
